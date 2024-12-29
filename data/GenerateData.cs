using System;
using System.Collections.Generic;

public static class GenerateData
{
  public static List<Location> Locations()
  {
    return new List<Location>() {
      new Location { Term = "Dining Room", Description = "It's a Dining Room." },
      new Location { Term = "Foyer", Description = "It's a Foyer." },
      new Location { Term = "Living Room", Description = "It's a Living Room." },
      new Location { Term = "Library", Description = "It's a Library." },
      new Location { Term = "Kitchen", Description = "It's a Kitchen." },
    };
  }

  public static List<Suspect> Suspects()
  {
    return new List<Suspect>() {
      new Suspect { Name = "Dr. Zhao", Motive = new Motive { Strength = MotiveStrength.Weak, Desire = MotiveDesire.Power } },
      new Suspect { Name = "Sam Beauregard", Motive = new Motive { Strength = MotiveStrength.Strong, Desire = MotiveDesire.Love } },
      new Suspect { Name = "Major O'Leary", Motive = new Motive { Strength = MotiveStrength.Fair, Desire = MotiveDesire.Revenge } },
      new Suspect { Name = "Sir Bradford", Motive = new Motive { Strength = MotiveStrength.Strong, Desire = MotiveDesire.Money } },
    };
  }

  public static List<Clue> Clues()
  {
    return new List<Clue>() {
      new Clue { Term = "Candle Stick", Description = "It's a Candle Stick." },
      new Clue { Term = "Bust", Description = "It's a Bust." },
      new Clue { Term = "Bread Knife", Description = "It's a Bread Knife." },
      new Clue { Term = "Fire Poker", Description = "It's a Fire Poker." },
      new Clue { Term = "Paper Weight", Description = "It's a Paper Weight." },
      new Clue { Term = "Wallet", Description = "It's a Wallet." },
      new Clue { Term = "Lipstick", Description = "It's a Lipstick." },
      new Clue { Term = "Monocle", Description = "It's a Monocle." },
      new Clue { Term = "War Medal", Description = "It's a War Medal." },
    };
  }
}