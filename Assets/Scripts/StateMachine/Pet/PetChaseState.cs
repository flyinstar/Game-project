using Character.Pet;
using UnityEngine;

namespace StateMachine.Pet
{
    public class PetChaseState : IState
    {
        Character.Pet.Pet pet;

        public PetChaseState(Character.Pet.Pet pet)
        {
            this.pet = pet;
        }

        public void OnEnter()
        {
            pet.anim.Play("Move");
        }

        public void OnUpdate()
        {
            if (pet.attackTarget != null)
            {
                pet.AutoPath(pet.attackTarget.transform.position);
                if (Vector2.Distance(pet.attackTarget.transform.position, pet.transform.position) <= pet.attackDistance)
                {
                    pet.TransitionState(PetStateType.Attack);
                }
                else
                {
                    pet.moveDirection = pet.pathPointList[pet.currentIndex] - pet.transform.position;
                }
            }
            else
            {
                pet.TransitionState(PetStateType.Idle);
            }
        }

        public void OnFixedUpdate()
        {
            pet.Move();
        }

        public void OnExit()
        {

        }
    }
}
