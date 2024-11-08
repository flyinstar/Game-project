using UnityEngine;

namespace StateMachine.Player
{
    public class PlayerDeathState : IState
    {
        private static readonly int IsDeath = Animator.StringToHash("isDeath");
        readonly Character.Player.Player player;

        public PlayerDeathState(Character.Player.Player player)
        {
            this.player = player;
        }

        public void OnEnter()
        {
            player.anim.SetBool(IsDeath, player.isDeath);
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
