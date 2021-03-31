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
    }
}
