using DesktopUI;
using GameEngine;
using System.IO;
using GameEngine.Enums;
using Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace ConsoleUI
{
    class Program
    {
        //Initialize the required objects
        public static LevelSet levelSet;
        public static Level level;
        public static ConsoleDrawing draw;
        //ToDO: Menu without field
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("To move Dragger,use your arrows .The goal is to push all boxes to the X.Please  enjoy!!!");
            Console.WriteLine("Press Esc to exit \n");
            Console.WriteLine("Press R to Restart \n");
            Console.WriteLine("Press Enter to Start \n");

            Console.ForegroundColor = ConsoleColor.Red;
            Console.BackgroundColor = ConsoleColor.Black;

            StartLevel(false);

            ConsoleKeyInfo keyInfo;
            if ((keyInfo = Console.ReadKey(true)).Key == ConsoleKey.Enter)
            {
                Console.Clear();
                draw.DrawLevel();

                while ((keyInfo = Console.ReadKey(true)).Key != ConsoleKey.Escape && !level.IsFinished())
                {
                    OnKeyPress(keyInfo);
                }
            }            
        }
        private static void MoveDragger(MoveDirection direction)
        {
            if (direction == MoveDirection.Up)
                level.MoveDragger(MoveDirection.Up);
            else if (direction == MoveDirection.Down)
                level.MoveDragger(MoveDirection.Down);
            else if (direction == MoveDirection.Right)
                level.MoveDragger(MoveDirection.Right);
            else if (direction == MoveDirection.Left)
                level.MoveDragger(MoveDirection.Left);

            // Draw the changes of the level
            DrawChanges();
            if (level.IsFinished())
            {
                Console.WriteLine("You did it in {0} and {1} !!!", level.Moves.ToString(), level.Pushes.ToString());

                if (levelSet.CurrentLevel < levelSet.NrOfLevelsInSet)
                {
                    Console.WriteLine("Good job");
                    levelSet.CurrentLevel++;
                    level = (Level)levelSet[levelSet.CurrentLevel - 1];
                    draw = new ConsoleDrawing(level);
                    draw.DrawLevel();
                }
                else
                {
                    Console.WriteLine("That was the last level!Well done");
                }
            }
        }
        private static void DrawChanges()
        {
            string[,] res = draw.DrawChanges();
            draw.DrawMap(res);

            // Update labels with number of moves and pushes
            Console.WriteLine("Moves : {0}", level.Moves.ToString());
            Console.WriteLine("Pushes : {0}", level.Pushes.ToString());
        }

        private static void StartLevel(bool redraw)
        {
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\levels\boxworld.xml";

            levelSet = new LevelSet();            
            levelSet.SetLevelSet(path);
            levelSet.CurrentLevel = 1;
            levelSet.SetLevelsInLevelSet(path);

            level = (Level)levelSet[levelSet.CurrentLevel - 1];
            draw = new ConsoleDrawing(level);
            
            if (redraw)
            {
                Console.Clear();
                draw.DrawLevel();
            }
        }
        private static void OnKeyPress(ConsoleKeyInfo info)
        {
            switch (info.Key)
            {
                case ConsoleKey.UpArrow:
                    MoveDragger(MoveDirection.Left);
                    break;

                case ConsoleKey.RightArrow:
                    MoveDragger(MoveDirection.Down);
                    break;

                case ConsoleKey.DownArrow:
                    MoveDragger(MoveDirection.Right);
                    break;

                case ConsoleKey.LeftArrow:
                    MoveDragger(MoveDirection.Up);
                    break;

                case ConsoleKey.R:
                    StartLevel(true);
                    break;
            }
        }
    }
}

