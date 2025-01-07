using System;
using System.Collections.Generic;
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
    int victimIndex = rand.Next(Persons.Count);
    Person victim = Persons[victimIndex];

    int killerIndex;
    do
    {
      killerIndex = rand.Next(Persons.Count);
    } while (killerIndex == victimIndex);
    Person killer = Persons[killerIndex];

    int primeSceneIndex = rand.Next(Locations.Count);
    Location primeScene = Locations[primeSceneIndex];

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
    System.Collections.Generic.Dictionary<Location, List<Tuple<Person, TimeRange>>> alibiLocations = GenerateAlibis(rand, innocents, killTime, primeSceneIndex);

    //  create occurences for the innocents


    return crimeLine;
  }

  public static TimeRange GenerateKillTime(Random rand)
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

  public System.Collections.Generic.Dictionary<Location, List<Tuple<Person, TimeRange>>> GenerateAlibis(Random rand, Array<Person> innocents, TimeRange killTime, int primeSceneIndex)
  {
    System.Collections.Generic.Dictionary<Location, List<Tuple<Person, TimeRange>>> alibiLocations = new();

    foreach (Person innocent in innocents)
    {
      //  set a time that intersects the killTime
      int startHour = rand.Next(
        killTime.End.Hour == 12 ? 12 : killTime.End.Hour + 1
      );
      if (startHour == killTime.Start.Hour && killTime.Start.Minute == 0)
      {
        startHour--;
      }

      int endHour = rand.Next(startHour, 13);
      if (endHour == killTime.End.Hour && killTime.End.Minute == 55)
      {
        endHour++;
      }

      int startMinutes = startHour == killTime.Start.Hour
        ? rand.Next(0, killTime.Start.Minute)
        : rand.Next(60)
      ;
      int endMinutes = endHour == killTime.End.Hour
        ? rand.Next(killTime.End.Minute, 60)
        : rand.Next(60)
      ;

      TimeRange timeRange = new()
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

      //  give each person a location
      int locationIndex;
      do
      {
        locationIndex = rand.Next(Locations.Count);
      } while (locationIndex == primeSceneIndex);
      Location alibiLocation = Locations[locationIndex];

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

    return alibiLocations;
  }
}