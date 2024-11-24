using System.Collections;
using System.Collections.Generic;
using Atsar;
using UnityEngine;

public class DrawGrid : MonoBehaviour
{
    public int width;
    public int height;
    public int originX;
    public int originY;
    public float nodeSize;
    public LayerMask obstacleMask;
    
    #region  网格可视化

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
        
        // 可以在Unity编辑器中可视化网格
        void OnDrawGizmos()
        {
            for (int x = originX; x < width; x++)
            {
                for (int y = originY; y < height; y++)
                {
                    if (IsObstacle((x+0.5f) * nodeSize, (y+0.5f) * nodeSize))
                    {
                        Gizmos.color = Color.red;
                        Gizmos.DrawWireCube(new Vector3((x+0.5f) * nodeSize, (y+0.5f) * nodeSize), new Vector3(nodeSize, nodeSize));
                    }
                    else if (IsNeighbourObstacle((x+0.5f) * nodeSize, (y+0.5f) * nodeSize, nodeSize))
                    {
                        //障碍物周围也设置为不可行走
                        //便于绘图及保证移动时不被卡住
                    }
                    else
                    {
                        Gizmos.color = Color.white;
                        Gizmos.DrawWireCube(new Vector3((x+0.5f) * nodeSize, (y+0.5f) * nodeSize), new Vector3(nodeSize, nodeSize));
                    }
                }
            }
        }

        #endregion
        
}
