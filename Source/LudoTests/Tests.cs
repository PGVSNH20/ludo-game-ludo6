using GameEngine;
using GameEngine.DataAccess;
using GameEngine.GameModels;
using System;
using Xunit;

namespace LudoTests
{
    public class Tests
    {

        [Fact]
        public void ExpectNumber1to6WhenRollDice()
        {
            LudoEngine engine = new LudoEngine();
            var diceRoll = engine.ThrowDice();

            Assert.True(diceRoll >= 1 &&  diceRoll <= 6);
            
        }

        [Fact]
        public void WhenAddingNewPlayer_ExpectItToHave4Pieces()
        {
            LudoEngine game = new LudoEngine();

            game.AddPlayer(typeof(RedPiece), "playerName");

            Assert.Equal(4, game.Players[0].Pieces.Count);
        }

        [Fact]
        public void WhenAddingNewPlayer_ExpectItToHavePiecesOfTheCorrectType()
        {
            LudoEngine game = new LudoEngine();

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
            LudoEngine game = new LudoEngine();

            game.AddPlayer(typeof(RedPiece), "playerName");
            var moveablePieces = game.GetMoveablePieces(5);

            Assert.Empty(moveablePieces);
        }


    }
}
