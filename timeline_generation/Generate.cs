using System;
using Godot;
using Godot.Collections;

public partial class TimelineGenerator : Node
{
  public Array<Person> Persons = new Array<Person> {
    GD.Load<Person>("res://resources/persons/dr_isaac_zhou.tres"),
    GD.Load<Person>("res://resources/persons/major_thomas_oleary.tres"),
    GD.Load<Person>("res://resources/persons/sam_beauregard.tres"),
    GD.Load<Person>("res://resources/persons/sir_geoffrey_bradford.tres"),
  };

  public Array<Location> Locations = new Array<Location> {
    GD.Load<Location>("res://resources/locations/dining_room.tres"),
    GD.Load<Location>("res://resources/locations/foyer.tres"),
    GD.Load<Location>("res://resources/locations/garden.tres"),
    GD.Load<Location>("res://resources/locations/kitchen.tres"),
    GD.Load<Location>("res://resources/locations/library.tres"),
    GD.Load<Location>("res://resources/locations/living_room.tres"),
  };

  public Array<Clue> Weapons = new Array<Clue> {
    GD.Load<Clue>("res://resources/clues/bust.tres"),
    GD.Load<Clue>("res://resources/clues/candelabra.tres"),
    GD.Load<Clue>("res://resources/clues/cleaver.tres"),
    GD.Load<Clue>("res://resources/clues/fire_poker.tres"),
    GD.Load<Clue>("res://resources/clues/paper_weight.tres"),
    GD.Load<Clue>("res://resources/clues/shears.tres"),
  };

  public CrimeLine Generate()
  {
    CrimeLine crimeLine = new CrimeLine();
    Random rand = new Random();

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
    Clue weapon = Weapons[weaponIndex];

    //  TODO: the weapon should be it's own `Clue` sub-class
    //        with a corresponding `KillActionType`
    KillAction killAction = new KillAction()
    {
      Actor = killer,
      Victim = victim,
      Type = KillActionType.Bludgeon  //  weapon.KillType
    };

    int startHour = rand.Next(1, 13);
    int endHour = rand.Next(startHour, 13);
    int startMinutes = rand.Next(12) * 5;
    int endMinutes = rand.Next(12) * 5;
    TimeRange killTime = new TimeRange()
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

    Occurence incitingIncident = new Occurence()
    {
      When = killTime,
      Where = primeScene,
      Action = killAction,
    };

    crimeLine.RootNode = new TimelineNode()
    {
      Occurence = incitingIncident,
    };

    //  next, the other persons must be accounted for during the kill time


    return crimeLine;
  }
}