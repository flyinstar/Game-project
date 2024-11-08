using Character.Player;
using UnityEngine;

namespace StateMachine.Player
{
    public class PlayerHurtState : IState
    {
        private static readonly int Hurt = Animator.StringToHash("Hurt");
        private AnimatorStateInfo info;

        readonly Character.Player.Player player;

        public PlayerHurtState(Character.Player.Player player)
        {
            this.player = player;
        }

        public void OnEnter()
        {
            player.anim.SetTrigger(Hurt);
        }

        public void OnUpdate()
        {
            info = player.anim.GetCurrentAnimatorStateInfo(0);
            //获取动画信息
            if (info.normalizedTime >= .95f)//当动画播放到百分之95时切换到待机状态
            {
                player.TransitionState(PlayerStateType.Idle);
            }
        }

        public void OnFixedUpdate()
        {
            player.Move();
        }

        public void OnExit()
        {
            player.isHurt = false;
        }
    }
}
