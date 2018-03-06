# Full Week Project: Wanderer - The RPG game

Build a hero based walking on tiles and killing monsters type of game. Heroes and monsters have levels and stats depending on their levels.

### Workshop: Plan your works
#### 1. Fork this repository under your own account, clone the forked repository to your computer
#### 2. Add .gitignore
#### 3. Add link to the README.md of your usual repository of this new forked repo
#### 4. Add tag to your new, forked repository with the name of your class.
#### 5. Go through the [language specific](cs.md) notes first
#### 6. Go through the [project specification](specification.md).
#### 7. Go through the [project stories](stories.md).
#### 8. Form groups and plan your application together.
Plan your architecture. In your architecture you should consider the following components:
- Models
    - GameObject
        - Character
            - Monster
            - Hero
            - types
        - Area
        - Tile
            - EmptyTile
            - NotEmptyTile
- GameLogic
    - current hero
    - current area
- MainLoop
    - handling events
    - current game

#### 9. Think about task breakdown in Kanban together
Now that you see the big picture, **go through the stories together** and think about how to break them down into tasks:
  - To classes
  - To methods
  - To data and actions
  - Extend the story cards with some of these points as a reminder

#### 10. Start working on your first task!
