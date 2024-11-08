using Character.Enemy;
using UnityEngine;

namespace StateMachine.Enemy
{
    public class EnemyPatrolState : IState
    {
        readonly Character.Enemy.Enemy enemy;

        public EnemyPatrolState(Character.Enemy.Enemy enemy)
        {
            this.enemy = enemy;
        }

        public void OnEnter()
        {
            enemy.GeneratePatrolPoint();
            enemy.anim.Play("Walk");
        }

        public void OnUpdate()
        {
            if (enemy.isHurt)
            {
                enemy.TransitionState(EnemyStateType.Hurt);
            }
            
            enemy.GetPlayerTransfrom();
            if (enemy.player != null)
            {
                enemy.TransitionState(EnemyStateType.Chase);
            }

            //路径为空时计算巡逻路径
            if (enemy.pathPointList == null || enemy.pathPointList.Count <= 0) 
            {
                enemy.GeneratePatrolPoint();
            }
            else
            {
                //敌人到达当前路径点
                if (Vector2.Distance(enemy.transform.position, enemy.pathPointList[enemy.currentIndex]) <= 0.1f)
                {
                    //索引指向下一个路径点
                    enemy.currentIndex++;

                    //到达巡逻点时切换到待机状态
                    if (enemy.currentIndex >= enemy.pathPointList.Count)
                    {
                        enemy.TransitionState(EnemyStateType.Idle);
                    }
                    //将移动方向传递给敌人
                    else
                    {
                        enemy.moveDirection = (enemy.pathPointList[enemy.currentIndex] - enemy.transform.position).normalized;
                    }
                }
                //相撞处理
                else
                {
                    //敌人速度小于默认速度且未到达巡逻点
                    if (enemy.rb.velocity.magnitude < enemy.speed && enemy.currentIndex < enemy.pathPointList.Count)
                    {
                        //敌人不动（在寻路范围外）
                        if (enemy.rb.velocity.magnitude == 0f)
                        {
                            //将移动方向传递给敌人
                            enemy.moveDirection = (enemy.pathPointList[enemy.currentIndex] - enemy.transform.position).normalized;
                        }
                        //敌人相撞
                        else
                        {
                            enemy.TransitionState(EnemyStateType.Idle);
                        }
                    }
                }
            }
        }

        public void OnFixedUpdate()
        {
            enemy.Move();
        }

        public void OnExit()
        {
        
        }  
    }
}
