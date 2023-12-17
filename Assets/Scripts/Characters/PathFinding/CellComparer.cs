using System.Collections.Generic;

namespace Characters.PathFinding
{
    public class CellComparer : IComparer<Node>
    {
        public int Compare(Node x, Node y)
        {
            if ((int) ((x.H - y.H) * 1000f) != 0)
            {
                return (int) ((x.H - y.H) * 1000f);
            }
            else if (x.G - y.G != 0)
            {
                return (int) ((x.G - y.G) * 1000);
            }
            else if (x.Position.x - y.Position.x != 0)
            {
                return (int)(x.Position.x - y.Position.x);
            }
            else
            {
                return (int)(x.Position.y - y.Position.y);
            }
        }
    }
}