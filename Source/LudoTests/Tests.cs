using GameEngine;
using GameEngine.DataAccess;
using System;
using Xunit;

namespace LudoTests
{
    public class Tests
    {
        [Fact]
        public void ExpectNullWhenEnteringNonExistingUser()
        {
            LudoEngine engine = new LudoEngine();
            var user = engine.GetUserByName("nonexistenuser");
            Assert.Null(user);
        }

        [Fact]
        public void ExpectNumber1to6WhenRollDice()
        {
            LudoEngine engine = new LudoEngine();
            var diceRoll = engine.ThrowDice();

            Assert.True(diceRoll >= 1 &&  diceRoll <= 6);
            
        }
    }
}
