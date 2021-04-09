using GameEngine;
using GameEngine.DataAccess;
using GameEngine.GameModels;
using GameEngine.Models;
using Moq;
using Moq.EntityFrameworkCore;
using System.Collections.Generic;
using Xunit;

namespace LudoTests
{
    public class Tests
    {

        [Fact]
        public void ExpectNumber1to6WhenRollDice()
        {
            var contextMock = new Mock<LudoDbContext>();
            List<Game> gameToReturn = new List<Game>();
            contextMock.Setup(x => x.Games).ReturnsDbSet(gameToReturn);
            LudoEngine game = new LudoEngine(contextMock.Object, "testgame1");
            var diceRoll = game.ThrowDice();

            Assert.True(diceRoll >= 1 &&  diceRoll <= 6);
            
        }

        [Fact]
        public void WhenAddingNewPlayer_ExpectItToHave4Pieces()
        {
            var contextMock = new Mock<LudoDbContext>();
            List<Game> gameToReturn = new List<Game>();
            List<User> usersToReturn = new List<User>();

            contextMock.Setup(x => x.Games).ReturnsDbSet(gameToReturn);
            contextMock.Setup(x => x.Users).ReturnsDbSet(usersToReturn);

            LudoEngine game = new LudoEngine(contextMock.Object, "testgame1");

            game.AddPlayer(typeof(RedPiece), "playerName");

            Assert.Equal(4, game.Players[0].Pieces.Count);
        }

        [Fact]
        public void WhenAddingNewPlayer_ExpectItToHavePiecesOfTheCorrectType()
        {
            var contextMock = new Mock<LudoDbContext>();
            List<Game> gameToReturn = new List<Game>();
            List<User> usersToReturn = new List<User>();

            contextMock.Setup(x => x.Games).ReturnsDbSet(gameToReturn);
            contextMock.Setup(x => x.Users).ReturnsDbSet(usersToReturn);

            LudoEngine game = new LudoEngine(contextMock.Object, "testgame1");

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
            List<Game> gameToReturn = new List<Game>();
            List<User> usersToReturn = new List<User>();

            contextMock.Setup(x => x.Games).ReturnsDbSet(gameToReturn);
            contextMock.Setup(x => x.Users).ReturnsDbSet(usersToReturn);

            LudoEngine game = new LudoEngine(contextMock.Object, "testgame1");

            game.AddPlayer(typeof(RedPiece), "playerName");
            var moveablePieces = game.GetMoveablePieces(5);

            Assert.Empty(moveablePieces);
        }

        //[Fact]

    }
}
