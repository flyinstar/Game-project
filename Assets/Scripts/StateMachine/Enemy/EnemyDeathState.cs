namespace StateMachine.Enemy
{
    public class EnemyDeathState : IState
    {
        readonly Character.Enemy.Enemy enemy;

        public EnemyDeathState(Character.Enemy.Enemy enemy)
        {
            this.enemy = enemy;
        }

        public void OnEnter()
        {
            enemy.anim.Play("Death");
        }

        public void OnUpdate()
        {

        }

        public void OnFixedUpdate()
        {

        }

        public void OnExit()
        {

        }
    }
}
