using Character.Player;
using UnityEngine;

namespace StateMachine.Player
{
    public class PlayerDodgeState : IState
    {
        private static readonly int IsDodge = Animator.StringToHash("isDodge");
        private float dodgeTimer;

        readonly Character.Player.Player player;

        public PlayerDodgeState(Character.Player.Player player)
        {
            this.player = player;
        }

        public void OnEnter()
        {
        
        }

        public void OnUpdate()
        {
            player.anim.SetBool(IsDodge, player.isDodge);

            if (player.isHurt)
            {
                player.TransitionState(PlayerStateType.Hurt);
            }

            if (player.isDodge == false) 
            {
                player.TransitionState(PlayerStateType.Idle);
            }
        }

        public void OnFixedUpdate()
        {
            player.Move();
            Dodge();
        }

        public void OnExit()
        {
       
        }   
    
        void Dodge()
        {
            if(!player.isDodgeOnCoolDown)
            {
                if(dodgeTimer<= player.dodgeDuration)
                {
                    player.rb.AddForce(player.inputDirection * player.dodgeForce,ForceMode2D.Impulse);
                    dodgeTimer += Time.fixedDeltaTime;
                }
                else
                {
                    //闪避结束
                    player.isDodge = false;
                    player.isDodgeOnCoolDown = true;
                    player.DodgeOnCoolDown();
                    dodgeTimer = 0f;
                }
            }
        }
    }
}
