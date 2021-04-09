**2021-03-31**

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

**2021-04-06**

## 
* We agreed on a change in the modell by creating classes for each color of Pieces.
* And also adding an IPiece interface. 
* We are currently working on getting the pieces to finish at the correct position. 
___

**2021-04-07**

Removed a test that was testing against DB
Added some new test:
- WhenAddingNewPlayer_ExpectItToHave4Pieces()
- WhenAddingNewPlayer_ExpectItToHavePiecesOfTheCorrectType()
- GivenAPlayerHasAllPiecesInNest_WhenDiceRollResultsInLessThan6_Expect0MoveablePieces()
- //-WhenTwoPlayersAddedGame_ExpectItToBeAddedToDatabase()\\ Not completed

Started working on the Db connection.
Methods for:
- Save
- Update
- Load

___

**2021-04-08**

- Continued to develop the methods talking to the Db.
- Added AddedPiece
- Added AddedPieceDb
- AskForGameName : Checking if name allready exist in Db
- SwitchPlayer() : Updated with an Db update/save for "NextToRollDice" in Games table.
- Added logic to update the 'WinnerUserId' and 'Active' columns in the Games table.
- Added logic to be able to load an existing game from the database using its name.
