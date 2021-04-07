using GameEngine;
using GameEngine.DataAccess;
using GameEngine.GameModels;
using GameEngine.Models;
using Moq;
using Moq.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace LudoTests
{
    public class Tests
    {

        [Fact]
        public void ExpectNumber1to6WhenRollDice()
        {
            var contextMock = new Mock<LudoDbContext>();
            LudoEngine game = new LudoEngine(contextMock.Object, "");
            var diceRoll = game.ThrowDice();

            Assert.True(diceRoll >= 1 &&  diceRoll <= 6);
            
        }

        [Fact]
        public void WhenAddingNewPlayer_ExpectItToHave4Pieces()
        {
            var contextMock = new Mock<LudoDbContext>();
            LudoEngine game = new LudoEngine(contextMock.Object, "");

            game.AddPlayer(typeof(RedPiece), "playerName");

            Assert.Equal(4, game.Players[0].Pieces.Count);
        }

        [Fact]
        public void WhenAddingNewPlayer_ExpectItToHavePiecesOfTheCorrectType()
        {
            var contextMock = new Mock<LudoDbContext>();
            LudoEngine game = new LudoEngine(contextMock.Object, "");

            game.AddPlayer(typeof(RedPiece), "playerName");
            var pieces = game.Players[0].Pieces;

            Assert.IsType<RedPiece>(pieces[0]);
            Assert.IsType<RedPiece>(pieces[1]);
            Assert.IsType<RedPiece>(pieces[2]);
            Assert.IsType<RedPiece>(pieces[3]);
        }

        [Fact]
        public void GivenAPlayerHasAllPiecesInNest_WhenDiceRollResultsInLessThan6_Expect0MoveablePieces()
        {
            var contextMock = new Mock<LudoDbContext>();
            LudoEngine game = new LudoEngine(contextMock.Object, "");

            game.AddPlayer(typeof(RedPiece), "playerName");
            var moveablePieces = game.GetMoveablePieces(5);

            Assert.Empty(moveablePieces);
        }

        [Fact]
        public void WhenTwoPlayersAddedGame_ExpectItToBeAddedToDatabase()
        {
            var contextMock = new Mock<LudoDbContext>();
            List<Game> games = new List<Game>() { new Game() { GameId = 1, Active = true, Name = "game1" } };
            List<User> users = new List<User>() { new User() { UserId = 1, Name = "userExample" } };
            contextMock.SetupSequence(x => x.Set<Game>()).ReturnsDbSet(new List<Game>()).ReturnsDbSet(games);
            contextMock.Setup(x => x.Users).ReturnsDbSet(users);

            LudoEngine game = new LudoEngine(contextMock.Object, "testName");

            game.AddPlayer(typeof(RedPiece), "player1");
            game.AddPlayer(typeof(BluePiece), "player2");

            Game gameEntity = null;
            try
            {
                gameEntity = contextMock.Object.Games.Where(g => g.Name == "testName").Single();
            }catch
            {
                gameEntity = null;
            }

            Assert.NotNull(gameEntity);

        }


    }
}
