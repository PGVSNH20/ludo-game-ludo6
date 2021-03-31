# Documentation

Use this file to fill in your documentation

## Scenario 1
1. Choose 1 (start new) or 2 (load saved game) > 1
2. Enter number of players > 2
3. Who will be player 1? > Lasse
4. Who will be player 2? > Nils
5. Lasse, it's your turn. Enter 'r' to roll the dice > r
6. You got a 2, still in your nest.
7. Nils, it's your turn. Enter 'r' to roll the dice > r
8. You got a 6, you're out of your nest!
9. [...]
10. Nils, it's your turn. Enter 'r' to roll the dice > r
11. You got a 6. Move a piece out of your nest (n), or move a piece on the field (f)? > f
12. Which piece do you want to move:
	1: 31 from safe ground.
	2: 5 from safe ground.
	2 
13. Moved a piece into safe ground!
14. [...]
15. Nils, it's your turn. Enter 'r' to roll the dice > r
16. You got a 5. Which piece do you want to move:
	1: 19 from safe ground.
	2: 4 from goal.
	2
17. Moved a piece into goal!
18. [...]
19. Nils, moved his last piece into the goal and won the game!

## Scenario 2
1. Choose 1 (start new) or 2 (load saved game) > 2
2. Write the name of the game: spel1
3. Loading spel1...
4. Nils, it's your turn. Enter 'r' to roll the dice > r
5. [...]

## Scenario 3
1. Choose 1 (start new), 2 (load saved game) or 3 (show user statistics) > 3
2. Username: Nils
3. You've won 4 games and lost 33.


## Database

### Table: User
Columns:
* UserID
* Name
* GamesWon
* GamesLost

### Table: Pieces
Columns:
* PieceID
* Color

### Table: Game
Columns:
* GameID
* Name
* Active
* NextToRollDice

### Table: GamePositions
Columns:
* ID
* GameID
* UserID
* PieceID
* Position

