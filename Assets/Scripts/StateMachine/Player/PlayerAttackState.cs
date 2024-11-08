using Character.Player;
using UnityEngine;

namespace StateMachine.Player
{
    public class PlayerAttackState : IState
    {
        private static readonly int IsAttack = Animator.StringToHash("isAttack");
        readonly Character.Player.Player player;

        public PlayerAttackState(Character.Player.Player player)
        {
            this.player = player;
        }

        public void OnEnter()
        {
            player.anim.SetBool(IsAttack, player.isAttack);
        }

        public void OnUpdate()
        {
            //受伤
            if(player.isHurt)
            {
                player.TransitionState(PlayerStateType.Hurt);
            }
            //攻击结束
            if(player.isAttack == false)
            {
                player.TransitionState(PlayerStateType.Idle);
            }
        }

        public void OnFixedUpdate()
        {

        }

        public void OnExit()
        {
            player.anim.SetBool(IsAttack, player.isAttack);
        }
    }
}
