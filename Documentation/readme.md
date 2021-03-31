# Documentation

Use this file to fill in your documentation

## 
1. Choose to start new game or continue > Start new
2. Enter number of players > 2
3. Who will be green? > Lasse
4. Who will be red? > Nils
5. Green, it's your turn. Enter 'r' to roll the dice or 's' to save the game.
6. [info]


## Database

### Table: User
Columns:
* UserID
* Name
* GamesWon
* GamesLost

#### Table: Pieces
Columns:
* PieceID
* Color
* StartPosition
* EndPosition

#### Table: Game
Columns:
* GameID
* Active
* NextToRollDice

### Table: GamePositions
Columns:
* ID
* GameID
* UserID
* PieceID
* Position

