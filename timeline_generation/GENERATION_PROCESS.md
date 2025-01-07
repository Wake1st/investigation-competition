# Generation Process
This is a step by step walkthrough to better understand how the timeline generation works.

## Notes
- In order to generate `Occurence`s which make sense
  - there must be a set list of `Action`s performed upon `Clue`s, some being `Location` bound
    - EX: player can "burry" a `Clue` in the "garden", or "burn" a `Clue` in the "living room" fireplace
  - `Suspect`s will change `Location` and take `Action` based on their `Motive`
    - EX: "Zhou" will falsify an "alibi" `Clue` (`Testimony`?) for "O'Leary" out of "love"
    - EX: "Zhou" will plant a fake "item" `Clue` near the body out of "revenage" against "Beuregard"
  - it needs to be easy and logical to understand when all the pieces are in place
  - it would be best to start with something hand crafted to see how the pieces might be automated best


## Steps
- [x] start with a root node: 
  - [x] someone murdered someone
  - [x] somewhere
  - [x] with some item
  - [ ] everyone else should be accounted for during that time range
    - [x] give each person a location
    - [x] set a time that intersects the murder time
    - [ ] create occurences (possibly shared) for the timeline
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
