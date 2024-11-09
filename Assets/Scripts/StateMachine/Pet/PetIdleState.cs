using Character.Pet;
using UnityEngine;

namespace StateMachine.Pet
{
    public class PetIdleState : IState
    {
        Character.Pet.Pet pet;

        public PetIdleState(Character.Pet.Pet pet)
        {
            this.pet = pet; 
        }

        public void OnEnter()
        {
            pet.anim.Play("Idle");
            pet.rb.velocity = Vector2.zero;
        }

        public void OnUpdate()
        {
            pet.GetTarget();
            pet.GetDistance();
            if (pet.attackTarget != null)
            {
                pet.TransitionState(PetStateType.Chase);
            }
            else  if(pet.distance > pet.idleDistance)
            {
                pet.TransitionState(PetStateType.Follow);
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
