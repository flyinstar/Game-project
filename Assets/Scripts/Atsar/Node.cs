using System.Collections.Generic;
using UnityEngine;

namespace Atsar
{
    public class Node
    {
        public Vector3 position;
        public Vector2Int intPosition;
        public Node parent{get;private set;}
        public List<Node> Neighbors = new List<Node>();
        public float GCost{get;private set;}
        public float HCost{get;private set;}
        public float FCost => GCost + HCost;

        public bool isObstacle;

        public void setParent(Node _parent) => parent = _parent;
        public void setGCost(float gCost) => GCost = gCost;
        public void setHCost(float hCost) => HCost = hCost;
    }
}
