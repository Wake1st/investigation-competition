# Investigation Competition

Play with (or against) others to solve the case first!

Collect clues, make deductions, and close your case before the time runs out! The player with the highest score wins.

## User Stories

- [x] player can collect `Clue`
- [x] player can inspect `Clue`
- [ ] player can plant "fake" `Clue`
- [ ] player can steal `Clue`
- [ ] dev can adjust a visible focal point for each location (to align the walls properly)
- [ ] player can interogate `Suspects`
  - [ ] `Suspects` can lie
  - [ ] players can "pressure" `Suspects` to tell the truth
  - [ ] players can "coerce" `Suspects` to lie to the other players
- [ ] player has `CaseFile`
  - [ ] `Timeline`: a branching network of `Occurence`
  - [ ] `Evidence`: a list of `Clue`
  - [ ] `Suspects`: a list of `Suspect`
- [ ] levels are a `CrimeScene`
  - [ ] `Room`: a `Location`, connected together with neighboring `Room`s
  - [ ] `Suspects`: a set of `Suspect` (they've already been round up to be interogated)
  - [ ] `Clue`: a set of `Clue` in each `Room`
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
- [ ] `Clue`: has an "id", a "term", and a "description"; is `Evidence` or an observation
- [ ] `Motivation`: has a strength and a desire (money, power, love, revenge)
- [ ] `Level` data should be auto-generated and scrambled (don't rely on steriotypes - it's unrealistic and easliy predictable)