using Character.Enemy;

namespace StateMachine.Enemy
{
    public class EnemyChaseState : IState
    {
        readonly Character.Enemy.Enemy enemy;

        public EnemyChaseState(Character.Enemy.Enemy enemy)
        {
            this.enemy = enemy;
        }

        public void OnEnter()
        {
            enemy.anim.Play("Walk");
        }

        public void OnUpdate()
        {
            if (enemy.isHurt)
            {
                enemy.TransitionState(EnemyStateType.Hurt);
            }

            enemy.GetPlayerTransfrom();

            enemy.AutoPath();

            if (enemy.player != null)
            {

                if (enemy.pathPointList == null || enemy.pathPointList.Count <= 0)
                {
                    return;
                }

                if (enemy.distance > enemy.attackDistance)
                {
                    enemy.moveDirection = (enemy.pathPointList[enemy.currentIndex] - enemy.transform.position).normalized;
                }
                if(enemy.distance <= enemy.attackDistance)
                {
                    enemy.TransitionState(EnemyStateType.Attack);
                }
            }
            else
            {
                enemy.TransitionState(EnemyStateType.Idle);
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
