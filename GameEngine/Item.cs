using GameEngine.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public class Item
    {
        #region Private fields
        private ItemType _itemType;  // Type: wall, floor, etc..
        private int _xPos;           // X position in the level
        private int _yPos;           // Y position in the level
        #endregion

        #region Constructor
        public Item(ItemType aItemType, int aXPos, int aYPos)
        {
            _itemType = aItemType;
            _xPos = aXPos;
            _yPos = aYPos;
        }
        #endregion

        #region Properties
        public ItemType ItemType
        {
            get { return _itemType; }
        }

        public int XPos
        {
            get { return _xPos; }
        }

        public int YPos
        {
            get { return _yPos; }
        }
        #endregion
    }
}
