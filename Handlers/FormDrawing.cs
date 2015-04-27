using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using GameEngine;
using GameEngine.Enums;
//using GameEngine.Enums;


namespace Handlers
{
    public class FormDrawing
    {
        // Form controls to display information on the screen
        #region Private fields
        private Level _level;
        private Bitmap _img;
        private Graphics _g;
        #endregion

        #region Constructors
        public FormDrawing(Level level)
        {
            this._level = level;
        }
        #endregion

        #region DRAW
        public Image DrawLevel()
        {
            int levelWidth = (_level.Width + 2) * Level.ItemSize;
            int levelHeight = (_level.Height + 2) * Level.ItemSize;

            _img = new Bitmap(levelWidth, levelHeight);

            _g = Graphics.FromImage(_img);
            _g.Clear(Color.FromArgb(2, 33, 61));

            Font statusText = new Font("Tahoma", 10, FontStyle.Bold);

            // Draw the border around the level
            for (int i = 0; i < _level.Width + 2; i++)
            {
                _g.DrawImage(ImgSpace, Level.ItemSize * i, 0,
                    Level.ItemSize, Level.ItemSize);
                _g.DrawImage(ImgSpace, Level.ItemSize * i,
                    (_level.Height + 1) * Level.ItemSize, Level.ItemSize, Level.ItemSize);
            }
            for (int i = 1; i < _level.Height + 1; i++)
            { 
                _g.DrawImage(ImgSpace, 0, Level.ItemSize * i,
                    Level.ItemSize, Level.ItemSize);
            }
            for (int i = 1; i < Level.ItemSize + 1; i++)
            { 
                _g.DrawImage(ImgSpace, (_level.Width + 1) * Level.ItemSize,
                    Level.ItemSize * i, Level.ItemSize, Level.ItemSize);
            }

            // Draw the level
            for (int i = 0; i < _level.Width; i++)
            {
                for (int j = 0; j < _level.Height; j++)
                {
                    Image image = GetLevelImage(_level.LevelMap[i, j], _level.SokoDirection);

                    _g.DrawImage(image, Level.ItemSize + i * Level.ItemSize,
                        Level.ItemSize + j * Level.ItemSize, Level.ItemSize, Level.ItemSize);

                    // Set Dragger's position
                    if (_level.LevelMap[i, j] == ItemType.Dragger ||
                        _level.LevelMap[i, j] == ItemType.DraggerOnGoal)
                    {
                        _level.SokoPosX = i;
                        _level.SokoPosY = j;
                    }
                }
            }
            return _img;
        }
        public Image DrawChanges()
        {
            Image image1 = GetLevelImage(_level.Item1.ItemType, _level.SokoDirection);
            _g.DrawImage(image1, Level.ItemSize + _level.Item1.XPos * Level.ItemSize,
                Level.ItemSize + _level.Item1.YPos * Level.ItemSize, Level.ItemSize, Level.ItemSize);

            Image image2 = GetLevelImage(_level.Item2.ItemType, _level.SokoDirection);
            _g.DrawImage(image2, Level.ItemSize + _level.Item2.XPos * Level.ItemSize,
               Level.ItemSize + _level.Item2.YPos * Level.ItemSize, Level.ItemSize, Level.ItemSize);
            //g.DrawImage();


            if (_level.Item3 != null)
            {
                Image image3 = GetLevelImage(_level.Item3.ItemType, _level.SokoDirection);
                _g.DrawImage(image3, Level.ItemSize + _level.Item3.XPos * Level.ItemSize,
               Level.ItemSize + _level.Item3.YPos * Level.ItemSize, Level.ItemSize, Level.ItemSize);
            }

            return _img;
        }
        public Image GetLevelImage(ItemType itemType, MoveDirection direction)
        {
            Image image;

            if (itemType == ItemType.Wall)
                image = ImgWall;
            else if (itemType == ItemType.Floor)
                image = ImgFloor;
            else if (itemType == ItemType.Package)
                image = ImgPackage;
            else if (itemType == ItemType.Goal)
                image = ImgGoal;
            else if (itemType == ItemType.Dragger)
            {
                if (direction == MoveDirection.Up)
                    image = ImgSokoUp;
                else if (direction == MoveDirection.Down)
                    image = ImgSokoDown;
                else if (direction == MoveDirection.Right)
                    image = ImgSokoRight;
                else
                    image = ImgSokoLeft;
            }
            else if (itemType == ItemType.PackageOnGoal)
                image = ImgPackageGoal;
            else if (itemType == ItemType.DraggerOnGoal)
            {
                if (direction == MoveDirection.Up)
                    image = ImgSokoUpGoal;
                else if (direction == MoveDirection.Down)
                    image = ImgSokoDownGoal;
                else if (direction == MoveDirection.Right)
                    image = ImgSokoRightGoal;
                else
                    image = ImgSokoLeftGoal;
            }
            else
                image = ImgSpace;

            return image;
        }
        public Image Undo()
        {
            // item1U, item2U and item3U contains the ItemTypes and their
            // positions at the time of just before the last push.
            Image image1 = GetLevelImage(_level.Item1U.ItemType, _level.SokoDirection);
            _g.DrawImage(image1, Level.ItemSize + _level.Item1U.XPos * Level.ItemSize,
                Level.ItemSize + _level.Item1U.YPos * Level.ItemSize, Level.ItemSize, Level.ItemSize);
            _level.LevelMap[_level.Item1U.XPos, _level.Item1U.YPos] = _level.Item1U.ItemType;

            Image image2 = GetLevelImage(_level.Item2U.ItemType, _level.SokoDirection);
            _g.DrawImage(image2, Level.ItemSize + _level.Item2U.XPos * Level.ItemSize,
                Level.ItemSize + _level.Item2U.YPos * Level.ItemSize, Level.ItemSize, Level.ItemSize);
            _level.LevelMap[_level.Item2U.XPos, _level.Item2U.YPos] = _level.Item2U.ItemType;

            Image image3 = GetLevelImage(_level.Item3U.ItemType, _level.SokoDirection);
            _g.DrawImage(image3, Level.ItemSize + _level.Item3U.XPos * Level.ItemSize,
                Level.ItemSize + _level.Item3U.YPos * Level.ItemSize, Level.ItemSize, Level.ItemSize);
            _level.LevelMap[_level.Item3U.XPos, _level.Item3U.YPos] = _level.Item3U.ItemType;

            // Here we remove Dragger from his current position and replace it
            // with a floor or goal (depending on where he was standing on).
            // If Dragger was already standing on the same place as he was just
            // before the last push, we can skip this step.
            if (!(_level.SokoPosX == _level.Item1U.XPos && _level.SokoPosY == _level.Item1U.YPos))
            {
                if (_level.LevelMap[_level.SokoPosX, _level.SokoPosY] == ItemType.Dragger)
                {
                    //TODO: винести в функціію
                    _level.LevelMap[_level.SokoPosX, _level.SokoPosY] = ItemType.Floor;
                    _g.DrawImage(GetLevelImage(ItemType.Floor, MoveDirection.Up),
                        Level.ItemSize + _level.SokoPosX * Level.ItemSize, Level.ItemSize +
                        _level.SokoPosY * Level.ItemSize, Level.ItemSize, Level.ItemSize);
                }
                else if (_level.LevelMap[_level.SokoPosX, _level.SokoPosY] == ItemType.DraggerOnGoal)
                {
                    _level.LevelMap[_level.SokoPosX, _level.SokoPosY] = ItemType.Goal;
                    _g.DrawImage(GetLevelImage(ItemType.Goal, MoveDirection.Up),
                        Level.ItemSize + _level.SokoPosX * Level.ItemSize, Level.ItemSize +
                        _level.SokoPosY * Level.ItemSize, Level.ItemSize, Level.ItemSize);
                }
            }

            // Update Dragger's position
            _level.SokoPosX = _level.Item1U.XPos;
            _level.SokoPosY = _level.Item1U.YPos;

            // Restore the number of moves and pushes
            _level.Moves = _level.MovesBeforeUndo;
            _level.Pushes = _level.PushesBeforeUndo;

            _level.IsUndoable = false;

            return _img;
        }
        #endregion
        
        #region FABULOUS IMAGES     
        public Image ImgWall { get { return Handlers.Properties.Resources.wall; } }
        public Image ImgFloor { get { return Handlers.Properties.Resources.floor; } }
        public Image ImgPackage { get { return Handlers.Properties.Resources.package; } }
        public Image ImgPackageGoal { get { return Handlers.Properties.Resources.package_goal; } }
        public Image ImgGoal { get { return Handlers.Properties.Resources.goal; } }
        public Image ImgSokoUp { get { return Handlers.Properties.Resources.man; } }
        public Image ImgSokoDown { get { return Handlers.Properties.Resources.man; } }
        public Image ImgSokoRight { get { return Handlers.Properties.Resources.man; } }
        public Image ImgSokoLeft { get { return Handlers.Properties.Resources.man; } }
        public Image ImgSokoUpGoal { get { return Handlers.Properties.Resources.man; } }
        public Image ImgSokoDownGoal { get { return Handlers.Properties.Resources.man; } }
        public Image ImgSokoRightGoal { get { return Handlers.Properties.Resources.man; } }
        public Image ImgSokoLeftGoal { get { return Handlers.Properties.Resources.man; } }
        public Image ImgSpace { get { return Handlers.Properties.Resources.space; } }
        #endregion
    }
}
