using Character.Enemy;
using UnityEngine;

namespace StateMachine.Enemy
{
    public class EnemyAttackState : IState
    {
        private AnimatorStateInfo info;

        readonly Character.Enemy.Enemy enemy;

        public EnemyAttackState(Character.Enemy.Enemy enemy)
        {
            this.enemy = enemy;
        }

        public void OnEnter()
        {
            if (enemy.isAttack)
            {
                enemy.anim.Play("Attack");
                enemy.isAttack = false;
                enemy.AttackCoolDown();
            }
        }

        public void OnUpdate()
        {
            if (enemy.isHurt)
            {
                enemy.TransitionState(EnemyStateType.Hurt);
            }

            //攻击时禁止移动
            enemy.rb.velocity = Vector2.zero;
            //角色翻转
            float x = enemy.transform.position.x - enemy.player.transform.position.x;
            if (x > 0)
            {
                enemy.sr.flipX = true;
            }
            if (x < 0)
            {
                enemy.sr.flipX = false;
            }
            //获取当前动画信息
            info = enemy.anim.GetCurrentAnimatorStateInfo(0);

            if (info.normalizedTime >= 1f)
            {
                enemy.TransitionState(EnemyStateType.Idle);
            }
        }

        public void OnFixedUpdate()
        {

        }

        public void OnExit()
        {
            enemy.isAttack = true;
        }
    }
}
