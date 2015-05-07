using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameEngine;
using GameEngine.Enums;
using System.IO;

namespace Dragger.GameEngine.Test
{
    [TestClass]
    public class LevelTest
    {
        private TestContext m_testContext;
        public TestContext TestContext
        {
            get { return m_testContext; }
            set { m_testContext = value; }
        }
        [TestMethod]
        public void Test_LevelConstructor()
        {
            // Review remark from IP:
            // проситься використання конфігураційних файлів
            string path = Path.GetDirectoryName(Path.GetDirectoryName(TestContext.TestDir)) + @"\Dragger.GameEngine.Test\Resources\boxworld.xml";

            LevelSet levelSet = new LevelSet();
            levelSet = new LevelSet();
            levelSet.SetLevelSet(path);
            levelSet.CurrentLevel = 1;
            levelSet.SetLevelsInLevelSet(path);

            Level level = (Level)levelSet[levelSet.CurrentLevel - 1];

            Assert.IsNotNull(level);
            Assert.IsTrue(level.Name == "BW1", "level.Name!=BW1");
            Assert.IsTrue(level.Width == 9, "level.Width!=9");
            Assert.IsTrue(level.Height == 9, "level.Height!=9");
            Assert.IsTrue(level.NumOfGoals == 3, "level.NumOfGoals!=3");
            Assert.IsTrue(level.LevelNr == 1, "level.LevelNr!=1");
        }
        [TestMethod]
        public void Test_IsFinishedFalse()
        {
            string path = Path.GetDirectoryName(Path.GetDirectoryName(TestContext.TestDir)) + @"\Dragger.GameEngine.Test\Resources\boxworld.xml";

            LevelSet levelSet = new LevelSet();
            levelSet = new LevelSet();
            levelSet.SetLevelSet(path);
            levelSet.CurrentLevel = 1;
            levelSet.SetLevelsInLevelSet(path);

            Level level = (Level)levelSet[levelSet.CurrentLevel - 1];

            Assert.IsTrue(level.IsFinished() == false, "level.IsFinished() != false");
        }
        [TestMethod]
        public void Test_MoveDragger()
        {
            string path = Path.GetDirectoryName(Path.GetDirectoryName(TestContext.TestDir)) + @"\Dragger.GameEngine.Test\Resources\boxworld.xml";

            LevelSet levelSet = new LevelSet();
            levelSet = new LevelSet();
            levelSet.SetLevelSet(path);
            levelSet.CurrentLevel = 1;
            levelSet.SetLevelsInLevelSet(path);

            Level level = (Level)levelSet[levelSet.CurrentLevel - 1];
            level.SokoPosX = 1;
            level.SokoPosY = 1;

            level.MoveDragger(MoveDirection.Right);
            Assert.IsTrue(level.SokoPosX == 2);
            Assert.IsTrue(level.SokoPosY == 1);
            level.MoveDragger(MoveDirection.Right);
            Assert.IsTrue(level.SokoPosX == 3);
            Assert.IsTrue(level.SokoPosY == 1);
            level.MoveDragger(MoveDirection.Right);
            Assert.IsTrue(level.SokoPosX == 3);
            Assert.IsTrue(level.SokoPosY == 1);
            level.MoveDragger(MoveDirection.Left);
            Assert.IsTrue(level.SokoPosX == 2);
            Assert.IsTrue(level.SokoPosY == 1);
            level.MoveDragger(MoveDirection.Down);
            Assert.IsTrue(level.SokoPosX == 2);
            Assert.IsTrue(level.SokoPosY == 1);
            level.MoveDragger(MoveDirection.Right);
            Assert.IsTrue(level.SokoPosX == 3);
            Assert.IsTrue(level.SokoPosY == 1);
            level.MoveDragger(MoveDirection.Down);
            Assert.IsTrue(level.SokoPosX == 3);
            Assert.IsTrue(level.SokoPosY == 2);
            level.MoveDragger(MoveDirection.Up);
            Assert.IsTrue(level.SokoPosX == 3);
            Assert.IsTrue(level.SokoPosY == 1);


            Assert.IsTrue(level.LevelMap[2, 2] == ItemType.Package, "First package");
            Assert.IsTrue(level.LevelMap[2, 3] == ItemType.Package, "Second package");
            Assert.IsTrue(level.LevelMap[3, 3] == ItemType.Package, "Third package");
        }
        [TestMethod]
        public void Test_MoveDraggerIsFinished()
        {
            string path = Path.GetDirectoryName(Path.GetDirectoryName(TestContext.TestDir)) + @"\Dragger.GameEngine.Test\Resources\boxworldPer.xml";

            LevelSet levelSet = new LevelSet();
            levelSet = new LevelSet();
            levelSet.SetLevelSet(path);
            levelSet.CurrentLevel = 1;
            levelSet.SetLevelsInLevelSet(path);

            Level level = (Level)levelSet[levelSet.CurrentLevel - 1];
            level.IsFinished();
            level.SokoPosX = 5;
            level.SokoPosY = 4;

            level.MoveDragger(MoveDirection.Right);
            Assert.IsTrue(level.SokoPosX == 6);
            Assert.IsTrue(level.SokoPosY == 4);

            Assert.IsTrue(level.LevelMap[7, 3] == ItemType.Goal, "Not goal");
            Assert.IsTrue(level.LevelMap[7, 4] == ItemType.PackageOnGoal, "Package not on goal");
            Assert.IsTrue(level.LevelMap[7, 5] == ItemType.Goal, "Not goal");

            level.MoveDragger(MoveDirection.Left);
            Assert.IsTrue(level.SokoPosX == 5);
            Assert.IsTrue(level.SokoPosY == 4);

            level.MoveDragger(MoveDirection.Up);
            Assert.IsTrue(level.SokoPosX == 5);
            Assert.IsTrue(level.SokoPosY == 3);

            level.MoveDragger(MoveDirection.Right);
            Assert.IsTrue(level.SokoPosX == 6);
            Assert.IsTrue(level.SokoPosY == 3);
            Assert.IsTrue(level.LevelMap[7, 3] == ItemType.PackageOnGoal, "Package not on goal");
            Assert.IsTrue(level.LevelMap[7, 4] == ItemType.PackageOnGoal, "Not Package2");
            Assert.IsTrue(level.LevelMap[7, 5] == ItemType.Goal, "Package not on goal");

            level.MoveDragger(MoveDirection.Left);
            Assert.IsTrue(level.SokoPosX == 5);
            Assert.IsTrue(level.SokoPosY == 3);

            level.MoveDragger(MoveDirection.Down);
            Assert.IsTrue(level.SokoPosX == 5);
            Assert.IsTrue(level.SokoPosY == 4);

            level.MoveDragger(MoveDirection.Down);
            Assert.IsTrue(level.SokoPosX == 5);
            Assert.IsTrue(level.SokoPosY == 5);

            level.MoveDragger(MoveDirection.Right);
            Assert.IsTrue(level.SokoPosX == 6);
            Assert.IsTrue(level.SokoPosY == 5);
            Assert.IsTrue(level.LevelMap[7, 3] == ItemType.PackageOnGoal, "Package not on goal");
            Assert.IsTrue(level.LevelMap[7, 4] == ItemType.PackageOnGoal, "Package not on goal");
            Assert.IsTrue(level.LevelMap[7, 5] == ItemType.PackageOnGoal, "Package not on goal");
            level.IsFinished();
            Assert.IsTrue(level.IsFinished() == true); ;
        }
        [TestMethod]
        public void Test_MoveDraggerOnGoal()
        {
            string path = Path.GetDirectoryName(Path.GetDirectoryName(TestContext.TestDir)) + @"\Dragger.GameEngine.Test\Resources\boxworldPer.xml";

            LevelSet levelSet = new LevelSet();
            levelSet = new LevelSet();
            levelSet.SetLevelSet(path);
            levelSet.CurrentLevel = 1;
            levelSet.SetLevelsInLevelSet(path);

            Level level = (Level)levelSet[levelSet.CurrentLevel - 1];
            level.SokoPosX = 5;
            level.SokoPosY = 4;

            level.MoveDragger(MoveDirection.Down);
            Assert.IsTrue(level.SokoPosX == 5);
            Assert.IsTrue(level.SokoPosY == 5);

            level.MoveDragger(MoveDirection.Down);
            Assert.IsTrue(level.SokoPosX == 5);
            Assert.IsTrue(level.SokoPosY == 6);

            level.MoveDragger(MoveDirection.Right);
            Assert.IsTrue(level.SokoPosX == 6);
            Assert.IsTrue(level.SokoPosY == 6);

            level.MoveDragger(MoveDirection.Right);
            Assert.IsTrue(level.SokoPosX == 7);
            Assert.IsTrue(level.SokoPosY == 6);

            level.MoveDragger(MoveDirection.Right);
            Assert.IsTrue(level.SokoPosX == 7);
            Assert.IsTrue(level.SokoPosY == 6);
            Assert.IsTrue(level.LevelMap[8, 6] == ItemType.Wall, " not  Wall");

            level.MoveDragger(MoveDirection.Up);
            Assert.IsTrue(level.SokoPosX == 7);
            Assert.IsTrue(level.SokoPosY == 5);
            Assert.IsTrue(level.LevelMap[7, 5] == ItemType.DraggerOnGoal, " not on goal");
        }
        [TestMethod]
        public void Test_MoveLeft()
        {
            string path = Path.GetDirectoryName(Path.GetDirectoryName(TestContext.TestDir)) + @"\Dragger.GameEngine.Test\Resources\boxworldPer.xml";

            LevelSet levelSet = new LevelSet();
            levelSet = new LevelSet();
            levelSet.SetLevelSet(path);
            levelSet.CurrentLevel = 1;
            levelSet.SetLevelsInLevelSet(path);

            Level level = (Level)levelSet[levelSet.CurrentLevel - 1];
            level.SokoPosX = 3;
            level.SokoPosY = 1;

            level.MoveDragger(MoveDirection.Left);
            Assert.IsTrue(level.SokoPosX == 2);
            Assert.IsTrue(level.SokoPosY == 1);
        }
        [TestMethod]
        public void Test_MoveLeftPackageOnFloor()
        {
            string path = Path.GetDirectoryName(Path.GetDirectoryName(TestContext.TestDir)) + @"\Dragger.GameEngine.Test\Resources\boxworldPer.xml";

            LevelSet levelSet = new LevelSet();
            levelSet = new LevelSet();
            levelSet.SetLevelSet(path);
            levelSet.CurrentLevel = 1;
            levelSet.SetLevelsInLevelSet(path);

            Level level = (Level)levelSet[levelSet.CurrentLevel - 1];
            level.SokoPosX = 5;
            level.SokoPosY = 4;

            level.MoveDragger(MoveDirection.Down);

            level.MoveDragger(MoveDirection.Left);
            Assert.IsTrue(level.SokoPosX == 4);
            Assert.IsTrue(level.SokoPosY == 5);
        }
        [TestMethod]
        public void Test_MoveLeftPackageOnGoal()
        {
            string path = Path.GetDirectoryName(Path.GetDirectoryName(TestContext.TestDir)) + @"\Dragger.GameEngine.Test\Resources\boxworldPerAlt.xml";

            LevelSet levelSet = new LevelSet();
            levelSet = new LevelSet();
            levelSet.SetLevelSet(path);
            levelSet.CurrentLevel = 1;
            levelSet.SetLevelsInLevelSet(path);

            Level level = (Level)levelSet[levelSet.CurrentLevel - 1];
            level.SokoPosX = 5;
            level.SokoPosY = 4;

            level.MoveDragger(MoveDirection.Down);
            level.MoveDragger(MoveDirection.Down);
            level.MoveDragger(MoveDirection.Left);
            Assert.IsTrue(level.SokoPosX == 4);
            Assert.IsTrue(level.SokoPosY == 6);
        }
        [TestMethod]
        public void Test_MoveLeftPackageAlreadyOnGoal()
        {
            string path = Path.GetDirectoryName(Path.GetDirectoryName(TestContext.TestDir)) + @"\Dragger.GameEngine.Test\Resources\boxworldPerAlt.xml";

            LevelSet levelSet = new LevelSet();
            levelSet = new LevelSet();
            levelSet.SetLevelSet(path);
            levelSet.CurrentLevel = 1;
            levelSet.SetLevelsInLevelSet(path);

            Level level = (Level)levelSet[levelSet.CurrentLevel - 1];
            level.SokoPosX = 5;
            level.SokoPosY = 4;

            level.MoveDragger(MoveDirection.Up);
            level.MoveDragger(MoveDirection.Left);
            Assert.IsTrue(level.SokoPosX == 4);
            Assert.IsTrue(level.SokoPosY == 3);
        }
        [TestMethod]
        public void Test_MoveLeftOnGoal()
        {
            string path = Path.GetDirectoryName(Path.GetDirectoryName(TestContext.TestDir)) + @"\Dragger.GameEngine.Test\Resources\boxworldPerAlt.xml";

            LevelSet levelSet = new LevelSet();
            levelSet = new LevelSet();
            levelSet.SetLevelSet(path);
            levelSet.CurrentLevel = 1;
            levelSet.SetLevelsInLevelSet(path);

            Level level = (Level)levelSet[levelSet.CurrentLevel - 1];
            level.SokoPosX = 5;
            level.SokoPosY = 4;

            level.MoveDragger(MoveDirection.Left);
            level.MoveDragger(MoveDirection.Left);
            Assert.IsTrue(level.SokoPosX == 3);
            Assert.IsTrue(level.SokoPosY == 4);
        }
        [TestMethod]
        public void Test_MovePackageOnFloorUp()
        {
            string path = Path.GetDirectoryName(Path.GetDirectoryName(TestContext.TestDir)) + @"\Dragger.GameEngine.Test\Resources\boxworldMoveUp.xml";

            LevelSet levelSet = new LevelSet();
            levelSet = new LevelSet();
            levelSet.SetLevelSet(path);
            levelSet.CurrentLevel = 1;
            levelSet.SetLevelsInLevelSet(path);

            Level level = (Level)levelSet[levelSet.CurrentLevel - 1];
            level.SokoPosX = 5;
            level.SokoPosY = 5;

            level.MoveDragger(MoveDirection.Up);
            Assert.IsTrue(level.SokoPosX == 5);
            Assert.IsTrue(level.SokoPosY == 4);
        }
        [TestMethod]
        public void Test_MovePackageOnGoalUp()
        {
            string path = Path.GetDirectoryName(Path.GetDirectoryName(TestContext.TestDir)) + @"\Dragger.GameEngine.Test\Resources\boxworldMoveUp.xml";

            LevelSet levelSet = new LevelSet();
            levelSet = new LevelSet();
            levelSet.SetLevelSet(path);
            levelSet.CurrentLevel = 1;
            levelSet.SetLevelsInLevelSet(path);

            Level level = (Level)levelSet[levelSet.CurrentLevel - 1];
            level.SokoPosX = 5;
            level.SokoPosY = 5;

            level.MoveDragger(MoveDirection.Left);
            level.MoveDragger(MoveDirection.Up);
            Assert.IsTrue(level.SokoPosX == 4);
            Assert.IsTrue(level.SokoPosY == 4);
        }
        [TestMethod]
        public void Test_MoveDraggerOnGoalUp()
        {
            string path = Path.GetDirectoryName(Path.GetDirectoryName(TestContext.TestDir)) + @"\Dragger.GameEngine.Test\Resources\boxworldMoveUp.xml";

            LevelSet levelSet = new LevelSet();
            levelSet = new LevelSet();
            levelSet.SetLevelSet(path);
            levelSet.CurrentLevel = 1;
            levelSet.SetLevelsInLevelSet(path);

            Level level = (Level)levelSet[levelSet.CurrentLevel - 1];
            level.SokoPosX = 5;
            level.SokoPosY = 5;

            level.MoveDragger(MoveDirection.Left);

            level.MoveDragger(MoveDirection.Up);
            level.MoveDragger(MoveDirection.Up);
            Assert.IsTrue(level.SokoPosX == 4);
            Assert.IsTrue(level.SokoPosY == 3);
        }
        [TestMethod]
        public void Test_DraggerOnGoal()
        {
            string path = Path.GetDirectoryName(Path.GetDirectoryName(TestContext.TestDir)) + @"\Dragger.GameEngine.Test\Resources\boxworldOnG.xml";

            LevelSet levelSet = new LevelSet();
            levelSet = new LevelSet();
            levelSet.SetLevelSet(path);
            levelSet.CurrentLevel = 1;
            levelSet.SetLevelsInLevelSet(path);

            Level level = (Level)levelSet[levelSet.CurrentLevel - 1];
            level.SokoPosX = 5;
            level.SokoPosY = 7;

            level.MoveDragger(MoveDirection.Up);
            Assert.IsTrue(level.SokoPosX == 5);
            Assert.IsTrue(level.SokoPosY == 6);
        }
        [TestMethod]
        public void Test_MovePackageOnFloorDown()
        {
            string path = Path.GetDirectoryName(Path.GetDirectoryName(TestContext.TestDir)) + @"\Dragger.GameEngine.Test\Resources\boxworldMoveDown.xml";

            LevelSet levelSet = new LevelSet();
            levelSet = new LevelSet();
            levelSet.SetLevelSet(path);
            levelSet.CurrentLevel = 1;
            levelSet.SetLevelsInLevelSet(path);

            Level level = (Level)levelSet[levelSet.CurrentLevel - 1];
            level.SokoPosX = 4;
            level.SokoPosY = 5;

            level.MoveDragger(MoveDirection.Down);
            Assert.IsTrue(level.SokoPosX == 4);
            Assert.IsTrue(level.SokoPosY == 6);
        }
        [TestMethod]
        public void Test_MovePackageOnGoalDown()
        {
            string path = Path.GetDirectoryName(Path.GetDirectoryName(TestContext.TestDir)) + @"\Dragger.GameEngine.Test\Resources\boxworldMoveDown.xml";

            LevelSet levelSet = new LevelSet();
            levelSet = new LevelSet();
            levelSet.SetLevelSet(path);
            levelSet.CurrentLevel = 1;
            levelSet.SetLevelsInLevelSet(path);

            Level level = (Level)levelSet[levelSet.CurrentLevel - 1];
            level.SokoPosX = 4;
            level.SokoPosY = 5;

            level.MoveDragger(MoveDirection.Left);
            level.MoveDragger(MoveDirection.Down);
            Assert.IsTrue(level.SokoPosX == 3);
            Assert.IsTrue(level.SokoPosY == 6);
        }
        [TestMethod]
        public void Test_MoveDraggerOnGoalDown()
        {
            string path = Path.GetDirectoryName(Path.GetDirectoryName(TestContext.TestDir)) + @"\Dragger.GameEngine.Test\Resources\boxworldMoveDown.xml";

            LevelSet levelSet = new LevelSet();
            levelSet = new LevelSet();
            levelSet.SetLevelSet(path);
            levelSet.CurrentLevel = 1;
            levelSet.SetLevelsInLevelSet(path);

            Level level = (Level)levelSet[levelSet.CurrentLevel - 1];
            level.SokoPosX = 4;
            level.SokoPosY = 5;

            level.MoveDragger(MoveDirection.Left);
            level.MoveDragger(MoveDirection.Down);
            level.MoveDragger(MoveDirection.Down);
            Assert.IsTrue(level.SokoPosX == 3);
            Assert.IsTrue(level.SokoPosY == 7);
        }
        [TestMethod]
        public void Test_MoveDraggerOnGoalWithoutPackageDown()
        {
            string path = Path.GetDirectoryName(Path.GetDirectoryName(TestContext.TestDir)) + @"\Dragger.GameEngine.Test\Resources\boxworldMoveDown.xml";

            LevelSet levelSet = new LevelSet();
            levelSet = new LevelSet();
            levelSet.SetLevelSet(path);
            levelSet.CurrentLevel = 1;
            levelSet.SetLevelsInLevelSet(path);

            Level level = (Level)levelSet[levelSet.CurrentLevel - 1];
            level.SokoPosX = 4;
            level.SokoPosY = 5;

            level.MoveDragger(MoveDirection.Right);
            level.MoveDragger(MoveDirection.Down);

            Assert.AreEqual(level.SokoPosX, 5);
            Assert.AreEqual(level.SokoPosY, 6);
        }
        [TestMethod]
        public void Test_MoveDraggerOnGoalDownToWall()
        {
            string path = Path.GetDirectoryName(Path.GetDirectoryName(TestContext.TestDir)) + @"\Dragger.GameEngine.Test\Resources\boxworldMoveDown.xml";

            LevelSet levelSet = new LevelSet();
            levelSet = new LevelSet();
            levelSet.SetLevelSet(path);
            levelSet.CurrentLevel = 1;
            levelSet.SetLevelsInLevelSet(path);

            Level level = (Level)levelSet[levelSet.CurrentLevel - 1];
            level.SokoPosX = 5;
            level.SokoPosY = 4;

            level.MoveDragger(MoveDirection.Left);
            level.MoveDragger(MoveDirection.Down);
            level.MoveDragger(MoveDirection.Down);
            level.MoveDragger(MoveDirection.Down);
            Assert.IsTrue(level.SokoPosX == 4);
            Assert.IsTrue(level.SokoPosY == 6);
        }
        [TestMethod]
        public void Test_MoveIsUndoable()
        {
            string path = Path.GetDirectoryName(Path.GetDirectoryName(TestContext.TestDir)) + @"\Dragger.GameEngine.Test\Resources\boxworldMoveDown.xml";

            LevelSet levelSet = new LevelSet();
            levelSet = new LevelSet();
            levelSet.SetLevelSet(path);
            levelSet.CurrentLevel = 1;
            levelSet.SetLevelsInLevelSet(path);

            Level level = (Level)levelSet[levelSet.CurrentLevel - 1];
            level.SokoPosX = 5;
            level.SokoPosY = 4;

            level.MoveDragger(MoveDirection.Left);
            level.MoveDragger(MoveDirection.Down);
            Assert.IsTrue(level.IsUndoable);
            level.IsUndoable = false;
            Assert.IsFalse(level.IsUndoable);
        }

        [TestMethod]
        public void Test_MoveRight()
        {
            string path = Path.GetDirectoryName(Path.GetDirectoryName(TestContext.TestDir)) + @"\Dragger.GameEngine.Test\Resources\boxworldMoveRight.xml";

            LevelSet levelSet = new LevelSet();
            levelSet = new LevelSet();
            levelSet.SetLevelSet(path);
            levelSet.CurrentLevel = 1;
            levelSet.SetLevelsInLevelSet(path);

            Level level = (Level)levelSet[levelSet.CurrentLevel - 1];
            level.SokoPosX = 2;
            level.SokoPosY = 4;

            level.MoveDragger(MoveDirection.Right);
            Assert.IsTrue(level.SokoPosX == 3);
            Assert.IsTrue(level.SokoPosY == 4);
        }
        
        [TestMethod]
        public void Test_MoveRightPackageOnFloor()
        {
            string path = Path.GetDirectoryName(Path.GetDirectoryName(TestContext.TestDir)) + @"\Dragger.GameEngine.Test\Resources\boxworldMoveRight.xml";

            LevelSet levelSet = new LevelSet();
            levelSet = new LevelSet();
            levelSet.SetLevelSet(path);
            levelSet.CurrentLevel = 1;
            levelSet.SetLevelsInLevelSet(path);

            Level level = (Level)levelSet[levelSet.CurrentLevel - 1];
            level.SokoPosX = 2;
            level.SokoPosY = 4;

            level.MoveDragger(MoveDirection.Down);
            level.MoveDragger(MoveDirection.Right);
            level.MoveDragger(MoveDirection.Right);
            level.MoveDragger(MoveDirection.Right);
            Assert.IsTrue(level.SokoPosX == 5);
            Assert.IsTrue(level.SokoPosY == 5);
        }

        [TestMethod]
        public void Test_MoveRightPackageOnGoal()
        {
            string path = Path.GetDirectoryName(Path.GetDirectoryName(TestContext.TestDir)) + @"\Dragger.GameEngine.Test\Resources\boxworldMoveRight.xml";

            LevelSet levelSet = new LevelSet();
            levelSet = new LevelSet();
            levelSet.SetLevelSet(path);
            levelSet.CurrentLevel = 1;
            levelSet.SetLevelsInLevelSet(path);

            Level level = (Level)levelSet[levelSet.CurrentLevel - 1];
            level.SokoPosX = 2;
            level.SokoPosY = 4;

            level.MoveDragger(MoveDirection.Up);
            level.MoveDragger(MoveDirection.Right);
            level.MoveDragger(MoveDirection.Right);
            level.MoveDragger(MoveDirection.Right);
            Assert.IsTrue(level.SokoPosX == 5);
            Assert.IsTrue(level.SokoPosY == 3);
        }
        [TestMethod]
        public void Test_MoveRightPackageAlreadyOnGoal()
        {
            string path = Path.GetDirectoryName(Path.GetDirectoryName(TestContext.TestDir)) + @"\Dragger.GameEngine.Test\Resources\boxworldMoveRight.xml";

            LevelSet levelSet = new LevelSet();
            levelSet = new LevelSet();
            levelSet.SetLevelSet(path);
            levelSet.CurrentLevel = 1;
            levelSet.SetLevelsInLevelSet(path);

            Level level = (Level)levelSet[levelSet.CurrentLevel - 1];
            level.SokoPosX = 2;
            level.SokoPosY = 4;

            level.MoveDragger(MoveDirection.Up);
            level.MoveDragger(MoveDirection.Up);
            level.MoveDragger(MoveDirection.Right);
            Assert.IsTrue(level.SokoPosX == 3);
            Assert.IsTrue(level.SokoPosY == 2);
        }
    }
}
