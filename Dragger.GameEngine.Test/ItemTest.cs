using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameEngine;
using GameEngine.Enums;


namespace Dragger.GameEngine.Test
{
    [TestClass]
    public class ItemTest
    {
        [TestMethod]        
        public void Test_ItemConstructor()
        {
            Item item = new Item(global::GameEngine.Enums.ItemType.Floor, 5, 4);
            Assert.IsTrue(item.XPos == 5, "item.XPos!=5");
            Assert.IsTrue(item.YPos == 4, "item.YPos!=4");
            Assert.IsTrue(item.ItemType == ItemType.Floor, "item.ItemType!=Floor");        
        }

    }
}
