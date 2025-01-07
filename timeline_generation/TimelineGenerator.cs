using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Godot.Collections;

public partial class TimelineGenerator : Node
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

  public CrimeLine Generate()
  {
    CrimeLine crimeLine = new();
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

    crimeLine.RootNode = new()
    {
      Occurence = incitingIncident,
    };

    //  next, the other persons must be accounted for during the kill time
    Array<Person> innocents = Persons.Duplicate();
    innocents.Remove(victim);
    innocents.Remove(killer);
    Array<Occurence> alibiOccurences = GenerateAlibis(rand, innocents, killTime, primeSceneIndex);

    //  what do we fuckin do now?

    return crimeLine;
  }

  private TimeRange GenerateKillTime(Random rand)
  {
    int startHour = rand.Next(1, 13);
    int endHour = rand.Next(startHour, 13);
    int startMinutes = rand.Next(12) * 5;
    int endMinutes = startMinutes + 5 % 60;

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

  private Array<Occurence> GenerateAlibis(Random rand, Array<Person> innocents, TimeRange killTime, int primeSceneIndex)
  {
    System.Collections.Generic.Dictionary<Location, List<Tuple<Person, TimeRange>>> alibiLocations = new();

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
    }

        //  create occurences for the innocents
    Array<Occurence> occurences = new();
    foreach (Location location in alibiLocations.Keys)
    {
      var list = alibiLocations[location];
      if (list.Count > 1) {
        //  multiple people involved
        var randItem = list[rand.Next(list.Count)];
        list.Remove(randItem);
        GroupAction groupAction = new() {
          Actor = randItem.Item1,
          Type = GetRandEnum<GroupActionType>(),
          Others = list.Select(tuple => tuple.Item1).AsArray()
        };
        foreach (var personTimeRange in list) {
          occurences.Add(new Occurence {
            When = personTimeRange.Item2,
            Where = location,
            Action = groupAction
          });
        }
      } else {
        //  just one person
        var personTimeRange = list.First();
        occurences.Add(new Occurence {
          When = personTimeRange.Item2,
          Where = location,
          Action = new LoneAction {
            Actor = personTimeRange.Item1,
            Type = GetRandEnum<LoneActionType>(),
          }
        });
      }
    }

    return occurences;
  }

  public T GetRandEnum<[MustBeVariant] T>() {
    Random rand = new();
    System.Array values = Enum.GetValues(typeof(T));
    return (T)values.GetValue(rand.Next(values.Length));
  }
}

public static class GeneratorExtensions {
  public static T GetRandFromArray<[MustBeVariant] T>(this Array<T> items, int? exceptionIndex = null) {
    Random rand = new();

    if (exceptionIndex != null) {
      int itemIndex;
      do
      {
        itemIndex = rand.Next(items.Count);
      } while (itemIndex == exceptionIndex);

      return items[itemIndex];
    } else {
      return items[rand.Next(items.Count)];
    }
  }

  public static Array<T> AsArray<[MustBeVariant] T>(this IEnumerable<T> items) {
    Array<T> arrayItems = new();
    foreach (T item in items) {
      arrayItems.Add(item);
    }
    return arrayItems;
  }
}