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
    return new CrimeLine {
      RootNode = incitingIncident,
      SuspectTimelines = GenerateSuspectTimelines(primaryIncident, alibiOccurences, primeSceneIndex)
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
    System.Collections.Generic.Dictionary<Location, List<Tuple<Person, TimeRange>>> alibiLocations = new();
    Random rand = new();

    foreach (Person innocent in innocents)
    {
      //  set a time that intersects the killTime
      TimeRange timeRange = killTime.GenerateIntersectingRange();

      //  give each person a location
      Location alibiLocation = Locations.GetRandFromArray(primeSceneIndex);

      //  add the person to the dictionary
      if (alibiLocations.ContainsKey(alibiLocation))
      {
        alibiLocations[alibiLocation].Add(
          new Tuple<Person, TimeRange>(innocent, timeRange)
        );
      }
      else
      {
        alibiLocations.Add(
          alibiLocation,
          new List<Tuple<Person, TimeRange>>() { new(innocent, timeRange) }
        );
      }

      GD.Print($"{innocent.Name} was in {alibiLocation.Term} during {timeRange.Print()}");
    }

    //  create occurences for the innocents
    Array<Occurence> occurences = new();
    foreach (Location location in alibiLocations.Keys)
    {
      // //  check if people were in the same place and time
      // Array<TimeRange> times = list.Select((item) => item.Item2).AsArray();
      // foreach ((Person person, TimeRange range) in list) {
      //   var otherTimes = times.Duplicate();
      //   otherTimes.Remove(range);
      //   foreach (TimeRange other in otherTimes) {
      //     if (range.DoesIntersect(other)) {

      //     }
      //   }
      // }

      var list = alibiLocations[location];
      if (list.Count > 1)
      {
        //  multiple people were in a location,
        //  but we must check if the times overlap
        //  maybe use dictionaries, tuples suck
        var orderedList = list.OrderBy(
          (Tuple<Person, TimeRange> a, Tuple<Person, TimeRange> b) => 
          a.Item2.Start.GetTimeInMinutes() < b.Item2.Start.GetTimeInMinutes()
        );

        var randItem = list[rand.Next(list.Count)];
        list.Remove(randItem);
        GroupAction groupAction = new()
        {
          Actor = randItem.Item1,
          Type = GetRandEnum<GroupActionType>(),
          Others = list.Select(tuple => tuple.Item1).AsArray()
        };
        foreach (var personTimeRange in list)
        {
          occurences.Add(new Occurence
          {
            When = personTimeRange.Item2,
            Where = location,
            Action = groupAction
          });
        }
      }
      else
      {
        //  just one person
        var personTimeRange = list.First();
        occurences.Add(new Occurence
        {
          When = personTimeRange.Item2,
          Where = location,
          Action = new LoneAction
          {
            Actor = personTimeRange.Item1,
            Type = GetRandEnum<LoneActionType>(),
          }
        });
      }
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
    Array<Occurence> alibiOccurences, 
    int primeSceneIndex)
  {
    Random rand = new();
    Godot.Collections.Dictionary<Person, Timeline> timelines = new();

    foreach (Person person in Persons)
    {
      Timeline timeline = new();
      #nullable enable
      Occurence? alibi = alibiOccurences.FirstOrDefault(o => person == o.Action.Actor);
      alibi ??= alibiOccurences.FirstOrDefault(o => {
        if (o.Action.GetType() == typeof(GroupAction)) {
          var names = String.Join(", ", ((GroupAction)o.Action).Others.Select(p => p.Name));
          GD.Print($"people in group: {names}");
          return ((GroupAction)o.Action).Others.Contains(person);
        } else {
          return false;
        }
      });
      GD.Print($"alibi {alibi} found for {person.Name}");

      //  generate minor occurences (person is alone)
      Array<Occurence> beforeNodes = new();
      Array<TimeRange> timeRanges = primaryIncident.When.GenerateInsertingRanges(alibi.When);
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
        When = alibi.When.GenerateDisconnectingRange(before: false),
        Where = Locations.GetRandFromArray(primeSceneIndex),
        Action = new LoneAction
        {
          Actor = person,
          Type = GetRandEnum<LoneActionType>()
        }
      };

      //  check for the latest time
      if (afterNode.When.End.GetTimeInMinutes() > EndTime.GetTimeInMinutes()) {
        EndTime = afterNode.When.End;
      }

      //  add the occurences
      timeline.Nodes = new Array<Occurence>
      {
        primaryIncident,
      };
      timeline.Nodes.AddRange(beforeNodes);
      timeline.Nodes.Add(alibi);
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