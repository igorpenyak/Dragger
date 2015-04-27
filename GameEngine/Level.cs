using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using GameEngine.Enums;

namespace GameEngine
{
    public class Level
    {
        #region Private fields
        public const int ItemSize = 40;// ITEM_SIZE is the size of an item in the level

        private string _name = string.Empty;
        private int _nrOfGoals = 0;          
        private int _levelNr = 0;
        private int _width = 0;             
        private int _height = 0;               
        private string _levelSetName = string.Empty;
        private bool _isUndoable = false;
        #endregion        

        #region Properties
        public ItemType[,] LevelMap { get; private set;}        
        public string Name
        {
            get { return _name; }
        }
        public int NumOfGoals
        {
            get { return _nrOfGoals; }
        }
        public int LevelNr
        {
            get { return _levelNr; }
        }
        public int Width
        {
            get { return _width; }
        }
        public int Height
        {
            get { return _height; }
        }        
        public int Moves { get; set; }            
        public int Pushes { get; set; }             
        public int SokoPosX { get; set; }              
        public int SokoPosY { get; set; }
        public string LevelSetName
        {
            get { return _levelSetName; }
        }
        public bool IsUndoable
        {
            get { return _isUndoable; }
            set { _isUndoable = value; }
        }        
        public MoveDirection SokoDirection { get; private set; }

        // changedItems is updated every time Dragger moves or pushes a box.
        // Max. 3 items can be changed each push, 2 for each move. I keep
        // track of these change so we don't have to redraw the whole level
        // after each move/push.    
        //public Item item1;
        public Item Item1 { get; set; }
        public Item Item2 { get; set; }
        public Item Item3 { get; set; }

        // Here are 3 items to keep the undo information
        public Item Item1U { get; private set; }
        public Item Item2U { get; private set; }
        public Item Item3U { get; private set; }

        public int MovesBeforeUndo { get; set; }    
        public int PushesBeforeUndo { get; set; }
        #endregion

        #region Constructor
        public Level(string aName, ItemType[,] aLevelMap, int aWidth,
            int aHeight, int aNrOfGoals, int aLevelNr, string aLevelSetName)
        {

            _name = aName;
            _width = aWidth;
            _height = aHeight;
            LevelMap = aLevelMap;
            _nrOfGoals = aNrOfGoals;
            _levelNr = aLevelNr;
            _levelSetName = aLevelSetName;
            SokoDirection = MoveDirection.Right;
        }
        #endregion      

        #region Moving Dragger
        // When Dragger moves or pushes we only draws these changes instead of
        // redrawing the whole level again. Great performance improvement.
        // returns the 'level' image that will be drawn to screen
        public bool IsFinished()
        {
            int nrOfPackagesOnGoal = 0;

            for (int i = 0; i < _width; i++)
                for (int j = 0; j < _height; j++)
                    if (LevelMap[i, j] == ItemType.PackageOnGoal)
                        nrOfPackagesOnGoal++;

            return nrOfPackagesOnGoal == _nrOfGoals ? true : false;
        }
        public void MoveDragger(MoveDirection direction)
        {
            SokoDirection = direction;

            switch (direction)
            {
                case MoveDirection.Up:
                    MoveUp();
                    break;
                case MoveDirection.Down:
                    MoveDown();
                    break;
                case MoveDirection.Right:
                    MoveRight();
                    break;
                case MoveDirection.Left:
                    MoveLeft();
                    break;
            }
        }
        private void MoveUp()
        {
            if ((LevelMap[SokoPosX, SokoPosY - 1] == ItemType.Package ||
                LevelMap[SokoPosX, SokoPosY - 1] == ItemType.PackageOnGoal) &&
                (LevelMap[SokoPosX, SokoPosY - 2] == ItemType.Floor ||
                LevelMap[SokoPosX, SokoPosY - 2] == ItemType.Goal))
            {
                Item3U = new Item(LevelMap[SokoPosX, SokoPosY - 2], SokoPosX, SokoPosY - 2);
                Item2U = new Item(LevelMap[SokoPosX, SokoPosY - 1], SokoPosX, SokoPosY - 1);
                Item1U = new Item(LevelMap[SokoPosX, SokoPosY], SokoPosX, SokoPosY);

                if (LevelMap[SokoPosX, SokoPosY - 2] == ItemType.Floor)
                {
                    LevelMap[SokoPosX, SokoPosY - 2] = ItemType.Package;
                    Item3 = new Item(ItemType.Package, SokoPosX, SokoPosY - 2);
                }
                else if (LevelMap[SokoPosX, SokoPosY - 2] == ItemType.Goal)
                {
                    LevelMap[SokoPosX, SokoPosY - 2] = ItemType.PackageOnGoal;
                    Item3 = new Item(ItemType.PackageOnGoal, SokoPosX, SokoPosY - 2);
                }
                if (LevelMap[SokoPosX, SokoPosY - 1] == ItemType.Package)
                {
                    LevelMap[SokoPosX, SokoPosY - 1] = ItemType.Dragger;
                    Item2 = new Item(ItemType.Dragger, SokoPosX, SokoPosY - 1);
                }
                else if (LevelMap[SokoPosX, SokoPosY - 1] == ItemType.PackageOnGoal)
                {
                    LevelMap[SokoPosX, SokoPosY - 1] = ItemType.DraggerOnGoal;
                    Item2 = new Item(ItemType.DraggerOnGoal, SokoPosX, SokoPosY - 1);
                }

                _isUndoable = true;
                UpdateCurrentDraggerPosition();
                MovesBeforeUndo = Moves;
                PushesBeforeUndo = Pushes;
                Moves++;
                Pushes++;
                SokoPosY--;
            }
            else if (LevelMap[SokoPosX, SokoPosY - 1] == ItemType.Floor ||
                LevelMap[SokoPosX, SokoPosY - 1] == ItemType.Goal)
            {
                if (LevelMap[SokoPosX, SokoPosY - 1] == ItemType.Floor)
                {
                    LevelMap[SokoPosX, SokoPosY - 1] = ItemType.Dragger;
                    Item2 = new Item(ItemType.Dragger, SokoPosX, SokoPosY - 1);
                }
                else if (LevelMap[SokoPosX, SokoPosY - 1] == ItemType.Goal)
                {
                    LevelMap[SokoPosX, SokoPosY - 1] = ItemType.DraggerOnGoal;
                    Item2 = new Item(ItemType.DraggerOnGoal, SokoPosX, SokoPosY - 1);
                }

                Item3 = null;
                UpdateCurrentDraggerPosition();
                Moves++;
                SokoPosY--;
            }
        }
        private void MoveDown()
        {
            if ((LevelMap[SokoPosX, SokoPosY + 1] == ItemType.Package ||
                LevelMap[SokoPosX, SokoPosY + 1] == ItemType.PackageOnGoal) &&
                (LevelMap[SokoPosX, SokoPosY + 2] == ItemType.Floor ||
                LevelMap[SokoPosX, SokoPosY + 2] == ItemType.Goal))
            {
                Item3U = new Item(LevelMap[SokoPosX, SokoPosY + 2], SokoPosX, SokoPosY + 2);
                Item2U = new Item(LevelMap[SokoPosX, SokoPosY + 1], SokoPosX, SokoPosY + 1);
                Item1U = new Item(LevelMap[SokoPosX, SokoPosY], SokoPosX, SokoPosY);

                if (LevelMap[SokoPosX, SokoPosY + 2] == ItemType.Floor)
                {
                    LevelMap[SokoPosX, SokoPosY + 2] = ItemType.Package;
                    Item3 = new Item(ItemType.Package, SokoPosX, SokoPosY + 2);
                }
                else if (LevelMap[SokoPosX, SokoPosY + 2] == ItemType.Goal)
                {
                    LevelMap[SokoPosX, SokoPosY + 2] = ItemType.PackageOnGoal;
                    Item3 = new Item(ItemType.PackageOnGoal, SokoPosX, SokoPosY + 2);
                }

                if (LevelMap[SokoPosX, SokoPosY + 1] == ItemType.Package)
                {
                    LevelMap[SokoPosX, SokoPosY + 1] = ItemType.Dragger;
                    Item2 = new Item(ItemType.Dragger, SokoPosX, SokoPosY + 1);
                }
                else if (LevelMap[SokoPosX, SokoPosY + 1] == ItemType.PackageOnGoal)
                {
                    LevelMap[SokoPosX, SokoPosY + 1] = ItemType.DraggerOnGoal;
                    Item2 = new Item(ItemType.DraggerOnGoal, SokoPosX, SokoPosY + 1);
                }

                _isUndoable = true;
                UpdateCurrentDraggerPosition();
                MovesBeforeUndo = Moves;
                PushesBeforeUndo = Pushes;
                Moves++;
                Pushes++;
                SokoPosY++;
            }
            else if (LevelMap[SokoPosX, SokoPosY + 1] == ItemType.Floor ||
                LevelMap[SokoPosX, SokoPosY + 1] == ItemType.Goal)
            {
                if (LevelMap[SokoPosX, SokoPosY + 1] == ItemType.Floor)
                {
                    LevelMap[SokoPosX, SokoPosY + 1] = ItemType.Dragger;
                    Item2 = new Item(ItemType.Dragger, SokoPosX, SokoPosY + 1);
                }
                else if (LevelMap[SokoPosX, SokoPosY + 1] == ItemType.Goal)
                {
                    LevelMap[SokoPosX, SokoPosY + 1] = ItemType.DraggerOnGoal;
                    Item2 = new Item(ItemType.DraggerOnGoal, SokoPosX, SokoPosY + 1);
                }

                Item3 = null;
                UpdateCurrentDraggerPosition();
                Moves++;
                SokoPosY++;
            }
        }
        private void MoveRight()
        {
            if ((LevelMap[SokoPosX + 1, SokoPosY] == ItemType.Package ||
                LevelMap[SokoPosX + 1, SokoPosY] == ItemType.PackageOnGoal) &&
                (LevelMap[SokoPosX + 2, SokoPosY] == ItemType.Floor ||
                LevelMap[SokoPosX + 2, SokoPosY] == ItemType.Goal))
            {
                Item3U = new Item(LevelMap[SokoPosX + 2, SokoPosY], SokoPosX + 2, SokoPosY);
                Item2U = new Item(LevelMap[SokoPosX + 1, SokoPosY], SokoPosX + 1, SokoPosY);
                Item1U = new Item(LevelMap[SokoPosX, SokoPosY], SokoPosX, SokoPosY);

                if (LevelMap[SokoPosX + 2, SokoPosY] == ItemType.Floor)
                {
                    LevelMap[SokoPosX + 2, SokoPosY] = ItemType.Package;
                    Item3 = new Item(ItemType.Package, SokoPosX + 2, SokoPosY);
                }
                else if (LevelMap[SokoPosX + 2, SokoPosY] == ItemType.Goal)
                {
                    LevelMap[SokoPosX + 2, SokoPosY] = ItemType.PackageOnGoal;
                    Item3 = new Item(ItemType.PackageOnGoal, SokoPosX + 2, SokoPosY);
                }
                if (LevelMap[SokoPosX + 1, SokoPosY] == ItemType.Package)
                {
                    LevelMap[SokoPosX + 1, SokoPosY] = ItemType.Dragger;
                    Item2 = new Item(ItemType.Dragger, SokoPosX + 1, SokoPosY);
                }
                else if (LevelMap[SokoPosX + 1, SokoPosY] == ItemType.PackageOnGoal)
                {
                    LevelMap[SokoPosX + 1, SokoPosY] = ItemType.DraggerOnGoal;
                    Item2 = new Item(ItemType.DraggerOnGoal, SokoPosX + 1, SokoPosY);
                }

                _isUndoable = true;
                UpdateCurrentDraggerPosition();
                MovesBeforeUndo = Moves;
                PushesBeforeUndo = Pushes;
                Moves++;
                Pushes++;
                SokoPosX++;
            }
            else if (LevelMap[SokoPosX + 1, SokoPosY] == ItemType.Floor ||
                LevelMap[SokoPosX + 1, SokoPosY] == ItemType.Goal)
            {
                if (LevelMap[SokoPosX + 1, SokoPosY] == ItemType.Floor)
                {
                    LevelMap[SokoPosX + 1, SokoPosY] = ItemType.Dragger;
                    Item2 = new Item(ItemType.Dragger, SokoPosX + 1, SokoPosY);
                }
                else if (LevelMap[SokoPosX + 1, SokoPosY] == ItemType.Goal)
                {
                    LevelMap[SokoPosX + 1, SokoPosY] = ItemType.DraggerOnGoal;
                    Item2 = new Item(ItemType.DraggerOnGoal, SokoPosX + 1, SokoPosY);
                }

                Item3 = null;
                UpdateCurrentDraggerPosition();
                Moves++;
                SokoPosX++;
            }
        }
        private void MoveLeft()
        {
            if ((LevelMap[SokoPosX - 1, SokoPosY] == ItemType.Package ||
                LevelMap[SokoPosX - 1, SokoPosY] == ItemType.PackageOnGoal) &&
                (LevelMap[SokoPosX - 2, SokoPosY] == ItemType.Floor ||
                LevelMap[SokoPosX - 2, SokoPosY] == ItemType.Goal))
            {
                Item3U = new Item(LevelMap[SokoPosX - 2, SokoPosY], SokoPosX - 2, SokoPosY);
                Item2U = new Item(LevelMap[SokoPosX - 1, SokoPosY], SokoPosX - 1, SokoPosY);
                Item1U = new Item(LevelMap[SokoPosX, SokoPosY], SokoPosX, SokoPosY);

                if (LevelMap[SokoPosX - 2, SokoPosY] == ItemType.Floor)
                {
                    LevelMap[SokoPosX - 2, SokoPosY] = ItemType.Package;
                    Item3 = new Item(ItemType.Package, SokoPosX - 2, SokoPosY);
                }
                else if (LevelMap[SokoPosX - 2, SokoPosY] == ItemType.Goal)
                {
                    LevelMap[SokoPosX - 2, SokoPosY] = ItemType.PackageOnGoal;
                    Item3 = new Item(ItemType.PackageOnGoal, SokoPosX - 2, SokoPosY);
                }
                if (LevelMap[SokoPosX - 1, SokoPosY] == ItemType.Package)
                {
                    LevelMap[SokoPosX - 1, SokoPosY] = ItemType.Dragger;
                    Item2 = new Item(ItemType.Dragger, SokoPosX - 1, SokoPosY);
                }
                else if (LevelMap[SokoPosX - 1, SokoPosY] == ItemType.PackageOnGoal)
                {
                    LevelMap[SokoPosX - 1, SokoPosY] = ItemType.DraggerOnGoal;
                    Item2 = new Item(ItemType.DraggerOnGoal, SokoPosX - 1, SokoPosY);
                }

                _isUndoable = true;
                UpdateCurrentDraggerPosition();
                MovesBeforeUndo = Moves;
                PushesBeforeUndo = Pushes;
                Moves++;
                Pushes++;
                SokoPosX--;
            }
            else if (LevelMap[SokoPosX - 1, SokoPosY] == ItemType.Floor ||
                LevelMap[SokoPosX - 1, SokoPosY] == ItemType.Goal)
            {
                if (LevelMap[SokoPosX - 1, SokoPosY] == ItemType.Floor)
                {
                    LevelMap[SokoPosX - 1, SokoPosY] = ItemType.Dragger;
                    Item2 = new Item(ItemType.Dragger, SokoPosX - 1, SokoPosY);
                }
                else if (LevelMap[SokoPosX - 1, SokoPosY] == ItemType.Goal)
                {
                    LevelMap[SokoPosX - 1, SokoPosY] = ItemType.DraggerOnGoal;
                    Item2 = new Item(ItemType.DraggerOnGoal, SokoPosX - 1, SokoPosY);
                }

                Item3 = null;
                UpdateCurrentDraggerPosition();
                Moves++;
                SokoPosX--;
            }
        }
        private void UpdateCurrentDraggerPosition()
        {
            // Updates Dragger's position. This code is used in all the Move
            // methods, so I put it in a separate method.
            if (LevelMap[SokoPosX, SokoPosY] == ItemType.Dragger)
            {
                LevelMap[SokoPosX, SokoPosY] = ItemType.Floor;
                Item1 = new Item(ItemType.Floor, SokoPosX, SokoPosY);
            }
            else if (LevelMap[SokoPosX, SokoPosY] == ItemType.DraggerOnGoal)
            {
                LevelMap[SokoPosX, SokoPosY] = ItemType.Goal;
                Item1 = new Item(ItemType.Goal, SokoPosX, SokoPosY);
            }
        }
        #endregion
    }
}
