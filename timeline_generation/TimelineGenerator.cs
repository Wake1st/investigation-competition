using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Godot.Collections;

public partial class TimelineGenerator
{
  public Array<Person> Persons = new()
  {
    GD.Load<Person>("res://resources/persons/dr_isaac_zhou.tres"),
    GD.Load<Person>("res://resources/persons/major_thomas_oleary.tres"),
    GD.Load<Person>("res://resources/persons/sam_beauregard.tres"),
    GD.Load<Person>("res://resources/persons/sir_geoffrey_bradford.tres"),
  };

  public Array<Location> Locations = new()
  {
    GD.Load<Location>("res://resources/locations/dining_room.tres"),
    GD.Load<Location>("res://resources/locations/foyer.tres"),
    GD.Load<Location>("res://resources/locations/garden.tres"),
    GD.Load<Location>("res://resources/locations/kitchen.tres"),
    GD.Load<Location>("res://resources/locations/library.tres"),
    GD.Load<Location>("res://resources/locations/living_room.tres"),
  };

  public Array<Weapon> Weapons = new()
  {
    GD.Load<Weapon>("res://resources/clues/weapons/bust.tres"),
    GD.Load<Weapon>("res://resources/clues/weapons/candelabra.tres"),
    GD.Load<Weapon>("res://resources/clues/weapons/cleaver.tres"),
    GD.Load<Weapon>("res://resources/clues/weapons/fire_poker.tres"),
    GD.Load<Weapon>("res://resources/clues/weapons/paper_weight.tres"),
    GD.Load<Weapon>("res://resources/clues/weapons/shears.tres"),
  };

  public TimePoint StartTime { get; set; } = new();
  public TimePoint EndTime { get; set; } = new();

  public CrimeLine Generate()
  {
    //  first, create the inciting incident
    (
      Occurence incitingIncident,
      Person victim,
      Person killer,
      TimeRange killTime,
      int primeSceneIndex
    ) = GenerateIncitingIncident();

    //  next, the other persons must be accounted for during the kill time
    Array<Person> innocents = Persons.Duplicate();
    innocents.Remove(victim);
    innocents.Remove(killer);
    Array<Occurence> alibiOccurences = GenerateAlibis(innocents, killTime, primeSceneIndex);

    //  next, primary nodes (dinner)
    Occurence primaryIncident = GeneratePrimaryIncident(killTime, victim);
    StartTime = primaryIncident.When.Start;

    // fill in nodes for each person
    return new CrimeLine
    {
      RootNode = incitingIncident,
      SuspectTimelines = GenerateSuspectTimelines(
        primaryIncident,
        incitingIncident,
        alibiOccurences,
        victim,
        killer,
        primeSceneIndex
      )
    };
  }

  private (Occurence, Person, Person, TimeRange, int) GenerateIncitingIncident()
  {
    Random rand = new();

    //  first, there must be an inciting incident
    Person victim = Persons.GetRandFromArray();
    Person killer = Persons.GetRandFromArray(Persons.IndexOf(victim));

    Location primeScene = Locations.GetRandFromArray();
    int primeSceneIndex = Locations.IndexOf(primeScene);

    int weaponIndex = rand.Next(Weapons.Count);
    Weapon weapon = Weapons[weaponIndex];

    KillAction killAction = new()
    {
      Actor = killer,
      Victim = victim,
      Type = weapon.KillType
    };

    TimeRange killTime = GenerateKillTime(rand);

    Occurence incitingIncident = new()
    {
      When = killTime,
      Where = primeScene,
      Action = killAction,
    };

    return (incitingIncident, victim, killer, killTime, primeSceneIndex);
  }

  private TimeRange GenerateKillTime(Random rand)
  {
    int startHour = rand.Next(1, 25);
    int endHour = rand.Next(startHour, 25);
    int startMinutes = rand.Next(12) * 5;
    int endMinutes = startMinutes + 5;

    return new TimeRange()
    {
      Start = new TimePoint()
      {
        Hour = startHour,
        Minute = startMinutes
      },
      End = new TimePoint()
      {
        Hour = endHour,
        Minute = endMinutes
      }
    };
  }

  private Array<Occurence> GenerateAlibis(Array<Person> innocents, TimeRange killTime, int primeSceneIndex)
  {
    System.Collections.Generic.Dictionary<Location, Godot.Collections.Dictionary<TimeRange, Person>> alibiLocations = new();
    Random rand = new();
    Array<Occurence> occurences = new();

    foreach (Person innocent in innocents)
    {
      //  set a time that intersects the killTime
      TimeRange timeRange = killTime.GenerateIntersectingRange();

      //  give each person a location
      Location alibiLocation = Locations.GetRandFromArray(primeSceneIndex);

      //  add the person doing something alone
      occurences.Add(new Occurence
      {
        When = timeRange,
        Where = alibiLocation,
        Action = new LoneAction
        {
          Actor = innocent,
          Type = GetRandEnum<LoneActionType>(),
        }
      });
    }

    return occurences;
  }

  private T GetRandEnum<[MustBeVariant] T>()
  {
    Random rand = new();
    System.Array values = Enum.GetValues(typeof(T));
    return (T)values.GetValue(rand.Next(values.Length));
  }

  private Occurence GeneratePrimaryIncident(TimeRange killTime, Person victim)
  {
    Array<Person> others = Persons.Duplicate();
    others.Remove(victim);

    return new Occurence
    {
      When = killTime.GenerateDisconnectingRange(),
      Where = Locations.GetRandFromArray(),
      Action = new GroupAction
      {
        Actor = victim,
        Type = GetRandEnum<GroupActionType>(),
        Others = others,
      }
    };
  }

  private Godot.Collections.Dictionary<Person, Timeline> GenerateSuspectTimelines(
    Occurence primaryIncident,
    Occurence incitingIncident,
    Array<Occurence> alibiOccurences,
    Person victim,
    Person killer,
    int primeSceneIndex)
  {
    Random rand = new();
    Godot.Collections.Dictionary<Person, Timeline> timelines = new();

    foreach (Person person in Persons)
    {
      Timeline timeline = new();
      Occurence killingOccurence;
      if (person == victim || person == killer)
      {
        killingOccurence = incitingIncident;
      }
      else
      {
        killingOccurence = alibiOccurences.Single(o => person == o.Action.Actor);
      }

      //  generate minor occurences between primary and inciting incidents
      Array<Occurence> beforeNodes = new();
      Array<TimeRange> timeRanges = primaryIncident.When.GenerateInsertingRanges(killingOccurence.When);
      foreach (TimeRange range in timeRanges)
      {
        beforeNodes.Add(new Occurence
        {
          When = range,
          Where = Locations.GetRandFromArray(primeSceneIndex),
          Action = new LoneAction
          {
            Actor = person,
            Type = GetRandEnum<LoneActionType>()
          }
        });
      }

      //  add an after occurence
      Occurence afterNode = new()
      {
        When = killingOccurence.When.GenerateDisconnectingRange(before: false),
        Where = Locations.GetRandFromArray(primeSceneIndex),
        Action = new LoneAction
        {
          Actor = person,
          Type = GetRandEnum<LoneActionType>()
        }
      };

      //  check for the latest time
      if (afterNode.When.End.GetTimeInMinutes() > EndTime.GetTimeInMinutes())
      {
        EndTime = afterNode.When.End;
      }

      //  add the occurences
      timeline.Nodes = new Array<Occurence>
      {
        primaryIncident,
      };
      timeline.Nodes.AddRange(beforeNodes);
      timeline.Nodes.Add(killingOccurence);
      timeline.Nodes.Add(afterNode);

      timelines.Add(person, timeline);
    }

    return timelines;
  }
}

public static class GeneratorExtensions
{
  public static T GetRandFromArray<[MustBeVariant] T>(this Array<T> items, int? exceptionIndex = null)
  {
    Random rand = new();

    if (exceptionIndex != null)
    {
      int itemIndex;
      do
      {
        itemIndex = rand.Next(items.Count);
      } while (itemIndex == exceptionIndex);

      return items[itemIndex];
    }
    else
    {
      return items[rand.Next(items.Count)];
    }
  }

  public static Array<T> AsArray<[MustBeVariant] T>(this IEnumerable<T> items)
  {
    Array<T> arrayItems = new();
    foreach (T item in items)
    {
      arrayItems.Add(item);
    }
    return arrayItems;
  }
}