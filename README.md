# CMP6206 - AI For Games Repo

Project Name: Farming Simulator

Overview:
    A farm scene where the player can interact with growing plants and animals that each have different behaviours and can interact with each other.

- Sheep:
  - Behaviour:
    - Flocking/Roaming - main state of sheep occupying pen
    - Eating - chance to do this while roaming in pen
    - Ramming fence - chance to do this while roaming pen, will not do this while wolf near, potentiall will add feature to learn to not do this if it results in wolf attack
    - Fleeing - response to being in navigatable range of wolf
    - Dying - response ot being attacked by wolf
    - Guided - movement dictated by interaction with sheepog
  - Implementation method: Needs researching
- Wolves:
  - Behaviour:
    - Pack moving - main state of wolves around scene
    - Stalking - will follow sheep when in navigatable range
    - Attacking - when sheep is in range
    - Eating - when in range of dead sheep
    - Fleeing - if in range of player while holding gun
  - Implementation method: Needs researching
- Sheepdog:
  - Behaviour:
    - Wandering - main state of dog around scene
    - Commanded by player - player will have option of different commands which will dictate the sheepdog's movement
    - Sleeping - if not interacted with by player for certain time, while sleep somewhere in scene
  - Implementation: Needs researching
- Bird:
  - Behaviour:
    - Flying in flock - main state of birds in scene
    - Eating crops - will be triggered as birds hunger stat decreases over time
    - Returning to sky: once bird completes eating acyion, hunger stat will be replenished and bird will return to the sky
    - Fleeing: if bird flies down to eat crops and is within range/ line of sight of scarecrow/player they will fly away
  - Implementation method: Needs researching
- Crops:
  - Behaviour
    - Growing: main state of crops in state
    - Eaten: if crow successfully eats crop object, crop will lose height
    - Dying: if player has not watered crop for certain time, crop will enter dying state and start losing height
  - Implementation method: Needs researching

Target Game Engine

Unity (appropriate .gitignore included)

Research - background to the project such as inspiration and what you need to find out to achieve the AI features.  If real behaviour is to be implemented then examples such as flocking or using Artificial Life (ALife) approaches such as cellular automata should be used to support your project and provide a means of comparison of how successful your implementation is.

Methodology / Planning:

Development:
This project's timeline is being planned across two Kanban boards. I am using one in Notion to track for generic topics listed in the assessment brief that I want to look into throughout the semester. I am then also using a Github Project board for more detailed breakdowns of features I have decided and planned to work on following the general topics set out in my Notion board. This will define specifics for the implementations of features and the timeline in which I expect to complete them within.

Quality Assurance:

Progress Documentation

This will be documented in my Github commits to track what was completed in each commit, anycurrent issues with the implementation and what date this was completed on. My Github project board will also be updated with the progress from these commmits and if any features are completed before/after the expected time in the initial plan.

References to Sources Used

Assets:

Tutorials:

Research:
