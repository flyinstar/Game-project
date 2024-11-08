using Character.Enemy;
using UnityEngine;

namespace StateMachine.Enemy
{
    public class EnemyHurtState : IState
    {
        private Vector2 direction;//被击退方向
        private float timer;

        readonly Character.Enemy.Enemy enemy;

        public EnemyHurtState(Character.Enemy.Enemy enemy)
        {
            this.enemy = enemy;
        }

        public void OnEnter()
        {
            enemy.anim.Play("Hurt");
        }

        public void OnUpdate()
        {
            if (enemy.isKnockback)
            {
                if (enemy.player != null)
                {
                    direction = enemy.transform.position - enemy.player.position;
                }
                else
                {
                    Transform player = GameObject.FindWithTag("Player").transform;
                    direction = enemy.transform.position - player.position;
                }
            } 
        }

        public void OnFixedUpdate()
        {
            if (timer <= enemy.knockbackDuration)
            {
                enemy.rb.AddForce(direction.normalized * enemy.knockbackForce,ForceMode2D.Impulse);
                timer += Time.fixedDeltaTime;
            }
            else
            {
                timer = 0;
                enemy.isHurt = false;
                enemy.TransitionState(EnemyStateType.Idle);
            }
        }

        public void OnExit()
        {
            enemy.isHurt = false;
        }
    }
}
