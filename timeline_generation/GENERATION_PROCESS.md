# Generation Process
This is a step by step walkthrough to better understand how the timeline generation works.

## Notes
- In order to generate `Occurence`s which make sense, we must have a set list of `Action`s performed upon `Clue`s, some being `Location` bound
  - EX: player can "burry" a `Clue` in the "garden", or "burn" a `Clue` in the "living room" fireplace

## Steps
- [ ] start with a root node: 
  - [ ] someone murdered someone
  - [ ] somewhere
  - [ ] with some item
  - [ ] everyone else should be accounted for during that time range
- [ ] add primary nodes (everyone was somewhere - "dinner", "lounging", "found the body"... for a long duration of time)
- [ ] add minor nodes
  - [ ] go backwards from root node, then forwards from root node
  - [ ] switch between people, allowing a chance for an `Occurence` should a person's path cross another
    - [ ] "meeting"/"seeing"
    - [ ] "observing"/"overhearing"
  - [ ] these should connect with the primary nodes when appropriate
    - [ ] if they need to change `Location`, then insert the appropriate `Location` changes from one place to another
- [ ] once someone "raises the alarm", everyone should either:
  - [ ] go to the "body"
  - [ ] take advantage of the "panic"
- [ ] a `Suspect`'s `Location` and `Action` will be drive by their `Motive`
