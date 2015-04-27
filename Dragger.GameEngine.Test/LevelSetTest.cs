using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameEngine;

namespace Dragger.GameEngine.Test
{
    [TestClass]
    public class LevelSetTest
    {
        [TestMethod]
        public void Test_LevelSetConstructor()
        {
            LevelSet levelset = new LevelSet("Levels 1-2", "The levels from the Drager game", 1, "boxworld");
            Assert.IsTrue(levelset.Title == "Levels 1-2", "levelset.Title!=Title");
            Assert.IsTrue(levelset.Description == "The levels from the Drager game", "levelset.Description!=Description");
            Assert.IsTrue(levelset.NrOfLevelsInSet == 1, "levelset.NrOfLevelsInSet != 1");
            Assert.IsTrue(levelset.Filename == "boxworld", "levelset.FileName != File Name");
        }
    }
}
