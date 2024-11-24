using UnityEngine;

namespace Atsar
{
    public class GridNode
    {
        private int width;
        private int height;

        private Node[,] nodes;
        private float nodeSize;
        private LayerMask obstacleMask;

        public GridNode(int width, int height, float nodeSize, LayerMask obstacleMask)
        {
            this.width = width;
            this.height = height;
            this.nodeSize = nodeSize;
            this.obstacleMask = obstacleMask;
            
            //在网格节点位置生成节点，设置邻居并设置是否为障碍物
            nodes = new Node[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    nodes[x, y] = new Node
                    {
                        position = new Vector3((x+0.5f) * nodeSize, (y+0.5f) * nodeSize,0),
                        intPosition = new Vector2Int(x,y)
                    };
                    
                    nodes[x,y].isObstacle = IsObstacle(x,y);
                    if (IsNeighbourObstacle((x + 0.5f) * nodeSize, (y + 0.5f) * nodeSize, nodeSize))
                    {
                        nodes[x,y].isObstacle = true;
                    }
                    //Debug.Log(nodes[x, y].position);
                }
            }
        }

        public  Node GetNode(int x, int y)
        {
            if (x < width && y < height)
            {
                return nodes[x, y];
            }

            return null;
        }

        public void GetNeighbours(Node node,int x, int y)
        {
            if (x > 0 && x < width - 1 && y > 0 && y < height - 1)
            {
                node.Neighbors.Add(nodes[x, y - 1]);
                node.Neighbors.Add(nodes[x, y + 1]);
                node.Neighbors.Add(nodes[x - 1, y]);
                node.Neighbors.Add(nodes[x + 1, y]);
            }

            if (x == 0)
            {
                if (y > 0 && y < height - 1)
                {
                    node.Neighbors.Add(nodes[x, y - 1]);
                    node.Neighbors.Add(nodes[x, y + 1]);
                    node.Neighbors.Add(nodes[1, y]);
                }

                if (y == 0)
                {
                    node.Neighbors.Add(nodes[x, 1]);
                    node.Neighbors.Add(nodes[1, y]);
                }

                if (y == height - 1)
                {
                    node.Neighbors.Add(nodes[x, y - 1]);
                    node.Neighbors.Add(nodes[1, y]);
                }
            }

            if (x == width - 1)
            {
                if (y > 0 && y < height - 1)
                {
                    node.Neighbors.Add(nodes[x, y - 1]);
                    node.Neighbors.Add(nodes[x, y + 1]);
                    node.Neighbors.Add(nodes[x - 1, y]);
                }

                if (y == 0)
                {
                    node.Neighbors.Add(nodes[x - 1, y]);
                    node.Neighbors.Add(nodes[x, 1]);
                }

                if (y == height - 1)
                {
                    node.Neighbors.Add(nodes[x - 1, y]);
                    node.Neighbors.Add(nodes[x, y - 1]);
                }
            }

            if (y == 0 && x > 0 && x < width - 1) 
            {
                node.Neighbors.Add(nodes[x, 1]);
                node.Neighbors.Add(nodes[x - 1, y]);
                node.Neighbors.Add(nodes[x + 1, y]);
            }

            if (y == height - 1 && x > 0 && x < width - 1)
            {
                node.Neighbors.Add(nodes[x, y - 1]);
                node.Neighbors.Add(nodes[x - 1, y]);
                node.Neighbors.Add(nodes[x + 1, y]);
            }
        }

        //射线检测是否有障碍物
        public bool IsObstacle(float x, float y)
        {
            Vector3 origin = new Vector3(x,y,-0.5f*nodeSize);
            Vector3 direction = Vector3.forward;
            // Debug.DrawRay(origin,direction,Color.red);
            RaycastHit2D hit = Physics2D.Raycast(origin,direction,nodeSize,obstacleMask);
            return hit;
        }

        //判断邻居格点是否为障碍
        private bool IsNeighbourObstacle(float x, float y, float size)
        {
            if (IsObstacle(x - size, y - size)) return true;
            if (IsObstacle(x - size, y))        return true;
            if (IsObstacle(x - size, y + size)) return true;
            if (IsObstacle(x,y + size))         return true;
            if (IsObstacle(x + size, y + size)) return true;
            if (IsObstacle(x + size, y))        return true;
            if (IsObstacle(x + size, y - size)) return true;
            if (IsObstacle(x, y - size))        return true;
        
            return false;
        }
    }
}