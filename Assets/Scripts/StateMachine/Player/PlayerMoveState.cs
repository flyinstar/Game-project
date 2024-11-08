using Character.Player;
using UnityEngine;

namespace StateMachine.Player
{
    public class PlayerMoveState : IState
    {
        private static readonly int IsWalk = Animator.StringToHash("isWalk");
        readonly Character.Player.Player player;

        public PlayerMoveState(Character.Player.Player player)
        {
            this.player = player;
        }

        public void OnEnter()
        {
            player.anim.SetBool(IsWalk, true);
        }

        public void OnUpdate()
        {
            //待机
            if (player.speed == 0)
            {
                player.TransitionState(PlayerStateType.Idle);
            }
            //攻击
            if(player.isAttack)
            {
                player .TransitionState(PlayerStateType.Attack);
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

        public void OnFixedUpdate()
        {
            player.Move();
        }

        public void OnExit()
        {
            player.anim.SetBool(IsWalk, false);
        }

    }
}
