using Character.Enemy;
using UnityEngine;

namespace StateMachine.Enemy
{
    public class EnemyIdleState : IState
    {
        private float timer;

        readonly Character.Enemy.Enemy enemy;

        public EnemyIdleState(Character.Enemy.Enemy enemy)
        {
            this.enemy = enemy;
        }

        public void OnEnter()
        {
            enemy.anim.Play("Idle");
            enemy.rb.velocity = Vector2.zero;
        }

        public void OnUpdate()
        {
            if (enemy.isHurt)
            {
                enemy.TransitionState(EnemyStateType.Hurt);
            }

            //判断玩家是否在追击范围内
            enemy.GetPlayerTransfrom();

            //玩家在追击范围内
            if (enemy.player != null) 
            {
                //玩家在攻击范围外，切换到追击状态
                if (enemy.distance > enemy.attackDistance)
                {
                    enemy.TransitionState(EnemyStateType.Chase);
                }
                //玩家在攻击范围内，切换到攻击状态
                if (enemy.distance <= enemy.attackDistance)
                {
                    enemy.TransitionState(EnemyStateType.Attack);
                }
            }
            //玩家在追击范围外，一段时间后进入巡逻状态
            else
            {
                if(timer <= enemy.idleDuration)
                {
                    timer += Time.deltaTime;
                }
                else
                {
                    timer = 0f;
                    enemy.TransitionState(EnemyStateType.Patrol);
                }
            }
        }
    
        public void OnFixedUpdate()
        {
        
        }
    
        public void OnExit()
        {
        
        }
    }
}
