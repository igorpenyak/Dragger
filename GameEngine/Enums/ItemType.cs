using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Enums
{
        // Enum bor every possible item in a level
        public enum ItemType
        {
            Wall,
            Floor,
            Package,
            Goal,
            Dragger,
            PackageOnGoal,
            DraggerOnGoal,
            Space
        }
}
