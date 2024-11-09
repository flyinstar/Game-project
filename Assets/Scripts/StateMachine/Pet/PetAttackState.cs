using Character.Pet;
using UnityEngine;

namespace StateMachine.Pet
{
    public class PetAttackState : IState
    {
        private AnimatorStateInfo info;
        
        Character.Pet.Pet pet;

        public PetAttackState(Character.Pet.Pet pet)
        {
            this.pet = pet;
        }

        public void OnEnter()
        {
            pet.anim.Play("Attack");
        }

        public void OnUpdate()
        {
            //禁止移动
            pet.rb.velocity = Vector2.zero;
            //角色翻转
            if (pet.attackTarget != null)
            {
                float x = pet.transform.position.x - pet.attackTarget.transform.position.x;
                if (x > 0)
                {
                    pet.sr.flipX = true;
                }
                if (x < 0)
                {
                    pet.sr.flipX = false;
                }
            }
           
            //获取当前动画信息
            info = pet.anim.GetCurrentAnimatorStateInfo(0);

            if (info.normalizedTime >= 1f)
            {
                pet.TransitionState(PetStateType.Idle);
            }
        }

        public void OnFixedUpdate()
        {

        }

        public void OnExit()
        {

        }
    }
}
