using System.Collections.Generic;

namespace Characters.PathFinding.Nodes
{
    public class NodeComparer : IComparer<Node>
    {
        public int Compare(Node x, Node y)
        {
            if ((int)((x.distNodeToTarget - y.distNodeToTarget) * 1000f) != 0)
                return (int)((x.distNodeToTarget - y.distNodeToTarget) * 1000f);

            if (x.distStartToNode - y.distStartToNode != 0)
                return (int)(x.distStartToNode - y.distStartToNode) * 1000;

            if (x.currentPosition.x - y.currentPosition.x != 0)
                return (int)(x.currentPosition.x - y.currentPosition.x);

            return (int)(x.currentPosition.y - y.currentPosition.y);
        }
    }
}