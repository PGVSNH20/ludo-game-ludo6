# Documentation

## User Stories

### Scenario 1
1. Choose:
   1. Start new game
   2. Load game
   3. Show user statistics
   4. Show history of all games
   9. Exit
2. Player chooses 1.
3. What should the game be called?
4. Player enters 'testgame'.
5. Enter number of players (between 2 and 4):
6. Player enters 2.
7. Enter username for  Blue? > Lasse
8. Enter username for Green? > Nils
9. Lasse, it's your turn. Enter 'r' to roll the dice > r
10. You got a 2, still in your nest.
11. Nils, it's your turn. Enter 'r' to roll the dice > r
12. Lasse got a 2!
13. Nils, it's your turn. Enter 'r' to roll the dice > r
14. Nils got a 6! 
15. Which piece do you want to move:
    0: Piece at position 0.
    1: Piece at position 0.
    0
16. Nils moved a piece to position 11.
17. Lasse, it's your turn. Enter 'r' to roll the dice > r
18. You got a 5. Which piece do you want to move:
    1: Piece at position 23.
    2: Piece at position 43.
    2
19. Lasse entered goal with a piece!
20. [...]
21. Nils won the game!

### Scenario 2
1. Choose:
   1. Start new game
   2. Load game
   3. Show user statistics
   4. Show history of all games
   9. Exit
2. Player chooses 2.
2. Write the name of the game: 'testgame'
3. Nils, it's your turn. Enter 'r' to roll the dice > r
4. [...]

### Scenario 3
1. Choose:
   1. Start new game
   2. Load game
   3. Show user statistics
   4. Show history of all games
   9. Exit
2. Player chooses 3.
3. Username: Nils
4. You've won 4 games and lost 33.

### Scenario 4
1. Choose:
   1. Start new game
   2. Load game
   3. Show user statistics
   4. Show history of all games
   9. Exit
2. Player chooses 4.
3. 
testgame - Winner: Lisa
testgame2 - Winner: Nils

## LudoEngine usage
### New game
* Use GameExists to check that the game name is available.
* Instantiate a LudoEngine, passing in a DB context and name for the game.

### Load a game
* Use Load and pass in the game name and a DB context to instantiate an ongoing game.

### Gameplay
* Use GetPieceTypes to get a list of all available piece types.
* Use AddPlayer to create a player and send in the wanted piece type.
* Use SaveGame to save the new game.
* Use ThrowDice to get a random number between 1 and 6.
* Use GetMoveablePieces to get a list of all the current players moveable pieces.
* Use MovePiece to move one of the moveable pieces the correct number of steps.
* Collided and CollidingPiece can be used to check if there was a collision.
* Use PieceIsEnemy to check if the colliding piece is an enemy piece.
* Use PieceIsInGoal to check if the piece entered goal.
* Use FindWinner to see if anyone won the game.
* Use SwitchPlayer to switch the current player.

### Stats
* Use GetUserByName to get a user, with GamesWon & GamesLost.
* Use GetAllGames to get all games.

## LudoEngine Classes
### In DataAccess
Class for DB context to communicate with database.
### In DbModels
Models used in the database.
### In GameModels
Models used in the game.
### LudoEngine.cs
Contains all logic for the game.

## Database

### Table: User
Columns:
* UserID
* Name
* GamesWon
* GamesLost


### Table: Game
Columns:
* GameID
* Name
* Active
* NextToRollDice

### Table: GameMembers
Columns:
* GameID
* UserID
* PieceID

### Table: Pieces
Columns:
* PieceID
* Color

### Table: GamePositions
Columns:
* ID
* GameID
* UserID
* Position

