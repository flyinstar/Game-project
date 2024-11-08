using Character.Player;

namespace StateMachine.Player
{
    public class PlayerIdleState : IState
    {
        readonly Character.Player.Player player;

        public PlayerIdleState(Character.Player.Player player)
        {
            this.player = player;
        }

        public void OnEnter()
        {
            player.anim.Play("PlayerIdle");
        }

        public void OnFixedUpdate()
        {
            player.Move();
        }

        public void OnUpdate()
        {
            //移动
            if (player.speed != 0)  
            {
                player.TransitionState(PlayerStateType.Move);
            }
            //攻击
            if (player.isAttack) 
            {
                player.TransitionState(PlayerStateType.Attack);
            }
            //闪避
            if (player.isDodge) 
            {
                player.TransitionState(PlayerStateType.Dodge);
            }
            //受伤
            if (player.isHurt) 
            {
                player.TransitionState(PlayerStateType.Hurt);
            }
        }

        public void OnExit()
        {
        
        }
    }
}
