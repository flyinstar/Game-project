using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Atsar
{
    public class AstarSeeker : MonoBehaviour
    {
        public int width;
        public int height;
        public int originX;
        public int originY;
        public float nodeSize;
        public LayerMask obstacleMask;
        
        private GridNode gridNode;
        
        private void Start()
        {
            gridNode = new GridNode(width - originX, height - originY, nodeSize, obstacleMask);
        }

        //计算路径
        public List<Vector3> pathFind(Vector3 start, Vector3 target)
        {
            var toSearch = new List<Node>();
            var processed = new List<Node>();
            
            Node startNode = new Node
            {
                position = start
            };
            Node targetNode = new Node
            {
                position = target
            };
            startNode.setParent(null);
            
            //找到第一个格点
            for (int x = 0; x < width - originX; x++)
            {
                for (int y = 0; y < height - originY; y++)
                {
                    Node neighbor = gridNode.GetNode(x, y);
                    if (ManhattanDistance(neighbor,startNode) <= nodeSize)
                    {
                        startNode.Neighbors.Add(neighbor);
                    }
                }
            }
            
            toSearch.Add(startNode);
            
            //只要有待处理的节点就继续
            while (toSearch.Any())
            {
                // Debug.Log("A");
                
                //在搜索列表中找到最有价值的节点
                var currentNode = toSearch[0];
                
                // Debug.Log(currentNode.position);
                
                foreach (var t in toSearch)
                {
                    if (t.FCost < currentNode.FCost || Mathf.Approximately(t.FCost, currentNode.FCost) && t.HCost < currentNode.HCost)
                    {
                        currentNode = t;
                    }
                }
                
                // Debug.Log(currentNode.position);
                
                // Debug.Log("B");
                
                processed.Add(currentNode);
                toSearch.Remove(currentNode);

                //到达终点时返回路径
                if (ManhattanDistance(currentNode, targetNode) <= nodeSize)
                {
                    var path = new List<Vector3>();
                    targetNode.setParent(currentNode);
                    var curentPathTile = targetNode.position;
                    while (currentNode.position != startNode.position)
                    {
                        path.Add(currentNode.position);
                        currentNode = currentNode.parent;
                    }

                    path.Reverse();

                    // Debug.Log("YYY");
                    
                    return path;
                }

                // Debug.Log("C");
                
                gridNode.GetNeighbours(currentNode,currentNode.intPosition.x,currentNode.intPosition.y);
                
                //在邻居节点中可行走且未处理的对象
                foreach (var neighbor in currentNode.Neighbors.Where(t => !t.isObstacle && !processed.Contains(t)))
                {
                    // Debug.Log(neighbor.position);
                    var inSearch = toSearch.Contains(neighbor);
                    
                    var costToNeighbor = currentNode.GCost + ManhattanDistance(currentNode, neighbor);

                    if (!inSearch || costToNeighbor < neighbor.GCost)
                    {
                        neighbor.setGCost(costToNeighbor);
                        neighbor.setParent(currentNode);

                        if (!inSearch)
                        {
                            neighbor.setHCost(ManhattanDistance(neighbor,targetNode));
                            toSearch.Add(neighbor);
                        }
                    }
                    // Debug.Log("E");
                }
                
                // Debug.Log("D");
            }

            // Debug.Log("XXX");
            return null;
        }
        
        private float ManhattanDistance(Node node1, Node node2)
        {
            return Mathf.Abs(node1.position.x - node2.position.x) + Mathf.Abs(node1.position.y - node2.position.y);
        }
    }
}
