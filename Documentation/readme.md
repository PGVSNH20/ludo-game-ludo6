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

### Table: GamePositions
Columns:
* ID
* GameID
* UserID
* Color
* Position

