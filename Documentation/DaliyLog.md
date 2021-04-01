# 2021-03-31

## We started the day with finishing the planning we started yesterday.

## We now have three scenarios/user stories.

## We started coding with Anton as designated driver.

## Working our way through Scenario 1 step by step.
- We now have some basic gameEngine functions
- User can choose between: New game, load saved game, go to statistics.
- User can start a new game.
- Enter amount of players.
- Choose names for each color.
- "Start" game.
- Roll dice, get options based on diceResults.(move out of nest, move piece on table...)
- Pieces are moved correctly.

### Time:16:30

## Remade logic for choosing and moving pieces
* Added two methods:
  * ChoosePiece - Let's the user choose between the list of pieces given in the parameter and returns that object.
  * MovePiece - Uses the parameters to move a piece a certain number of steps. Also makes sure to only move one step if the piece is moving out of the nest.
* Edited the code in Run to make use of the methods above.

___
**2021-04-01**

## 
-We used the time before lunch for refactoring.
- Reason: We wanted to separate all Console-code from the GameEngine.  
- We continued to develope the gametable and how the pieces moves.
___
