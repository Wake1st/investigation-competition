# Investigation Competition

Play with (or against) others to solve the case first!

Collect clues, make deductions, and close your case before the time runs out! The player with the highest score wins.

## User Stories

- [x] player can collect `clues`
- [x] player can inspect `clues`
- [ ] dev can adjust a visible focal point for each location (to align the walls properly)
- [ ] player can plant `foux-clues` (a `bool` in `clue`) 
  - [ ] a player's `expertise` allows them to `detect` `foux-clues`
- [ ] some abilities allow players to steal `clues` from other players
- [ ] player can interogate suspects
- [ ] player has a `CaseFile`
  - [ ] `Timeline`: a branching network of `Occurence`
  - [ ] "Evidence": a list of `Clues`
  - [ ] "Suspects": a list of `Suspect`
- [ ] player can visit different `CrimeScene`
  - [ ] "Rooms": a set of connected `Location`
  - [ ] "Suspects": a set of `Suspect`, hanging around different "Room(s)"
  - [ ] "Clues": a set of `Clue` in each "Room"
- [ ] `Suspect` has a personal `Timeline` and a `Motive`
  - [ ] `Suspect` has a "true" timeline
  - [ ] `Suspect` can omit/change/fabricate details of their `Timeline`
- [ ] `Timeline` UI
  - [ ] `Time` axis - horizontal
  - [ ] `Location` axis - vertical
  - [ ] `Occurence` nodes in the grid, connected together
  - [ ] `Inconsistency`: a discontenuity (a person is in two locations at the same time)
- [ ] `Occurence`: has 4 parts
  - [ ] "Where": `Location`
  - [ ] "When": `Time`
  - [ ] "Cause": `Action`
  - [ ] "Effect": `Action`
- [ ] `Action`: has an "id", a "term", and a "description"
- [ ] `Clue`: has an "id", a "term", and a "description"; is evidence or an observation
- [ ] `Motivation`: has a strength and a desire (money, power, love, revenge)