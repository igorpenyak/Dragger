using GameEngine;
using GameEngine.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Handlers
{
    public class ConsoleDrawing
    {
        // Console controls to display information on the screen
        #region Private fields
        private Level level;
        private string item;
        private static string[,] Map;
        #endregion

        #region Constructors
        public ConsoleDrawing(Level level)
        {
            this.level = level;
        }
        #endregion

        #region Draw
        public void DrawLevel()
        {
            int levelWidth = (level.Width);
            int levelHeight = (level.Height);
            Map = new string[level.Height, level.Width];
            //Draw the level
            for (int i = 0; i < level.LevelMap.GetLength(0); i++)
            {
                for (int j = 0; j < level.LevelMap.GetLength(1); j++)
                {
                    string image = GetLevelImage(level.LevelMap[i, j], level.SokoDirection);
                    Map[i, j] = image;

                    Console.Write(image);
                    
                    if (level.LevelMap[i, j] == ItemType.Dragger ||
                        level.LevelMap[i, j] == ItemType.DraggerOnGoal)
                    {
                        level.SokoPosX = i;
                        level.SokoPosY = j;
                    }
                }
                Console.WriteLine();
            }
        }
        public string[,] DrawChanges()
        {
            //TODO: забрати
            string image1 = GetLevelImage(level.Item1.ItemType, level.SokoDirection);
            //g.DrawImage(image1, Level.ItemSize + level.Item1.XPos * Level.ItemSize,
            //    Level.ItemSize + level.Item1.YPos * Level.ItemSize, Level.ItemSize, Level.ItemSize);
            Map[level.Item1.XPos, level.Item1.YPos] = image1;

            string image2 = GetLevelImage(level.Item2.ItemType, level.SokoDirection);
            //g.DrawImage(image2, Level.ItemSize + level.Item2.XPos * Level.ItemSize,
            //   Level.ItemSize + level.Item2.YPos * Level.ItemSize, Level.ItemSize, Level.ItemSize);
            Map[level.Item2.XPos, level.Item2.YPos] = image2;
            //g.DrawImage();


            if (level.Item3 != null)
            {
                string image3 = GetLevelImage(level.Item3.ItemType, level.SokoDirection);
                // g.DrawImage(image3, Level.ItemSize + level.Item3.XPos * Level.ItemSize,
                //Level.ItemSize + level.Item3.YPos * Level.ItemSize, Level.ItemSize, Level.ItemSize);
                Map[level.Item3.XPos, level.Item3.YPos] = image3;
            }

            return Map;
        }
        public string GetLevelImage(ItemType itemType, MoveDirection direction)
        {
            if (itemType == ItemType.Wall)
            {
                item = "#";
            }
            else if (itemType == ItemType.Floor)
            {
                item = " ";
            }
            else if (itemType == ItemType.Package)
            {
                item = "@";
            }
            else if (itemType == ItemType.Goal)
            {
                item = "X";
            }
            else if (itemType == ItemType.Dragger)
            {
                if (direction == MoveDirection.Up)
                {
                    item = "Y";
                }
                else if (direction == MoveDirection.Down)
                {
                    item = "Y";
                }
                else if (direction == MoveDirection.Right)
                {
                    item = "Y";
                }
                else
                {
                    item = "Y";
                }
            }
            else if (itemType == ItemType.PackageOnGoal)
            {
                item = "@";
            }
            else if (itemType == ItemType.DraggerOnGoal)
            {
                if (direction == MoveDirection.Up)
                {
                    item = "Y";
                }
                else if (direction == MoveDirection.Down)
                {
                    item = "Y";
                }
                else if (direction == MoveDirection.Right)
                {
                    item = "Y";
                }
                else
                {
                    item = "Y";
                }
            }
            else
            {
                item = " ";
            }
            return item;
        }
        public void DrawMap(string[,] map)
        {
            Console.Clear();
            for (int i = 0; i < map.GetLength(0); i++)                        
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    if (map[i, j] == "#")
                    {
                        Console.Write("#");
                    }
                    if (map[i, j] == "X")
                    {
                        Console.Write("X");
                    }
                    if (map[i, j] == "@")
                    {
                        Console.Write("@");
                    }
                    if (map[i, j] == " ")
                    {
                        Console.Write(" ");
                    }
                    if (map[i, j] == "Y")
                    {
                        Console.Write("Y");
                    }
                }
                Console.WriteLine();
            }
        }
        #endregion
    }
}

