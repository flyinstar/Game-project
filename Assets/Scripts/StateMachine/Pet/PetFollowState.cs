using Character.Pet;
using UnityEngine;

namespace StateMachine.Pet
{
    public class PetFollowState : IState
    {
        Character.Pet.Pet pet;

        public PetFollowState(Character.Pet.Pet pet)
        {
            this.pet = pet;
        }

        public void OnEnter()
        {
            pet.anim.Play("Move");
            pet.speed = pet.defaultSpeed;
        }

        public void OnUpdate()
        {
            pet.GetTarget();
            if (pet.attackTarget != null)
            {
                pet.TransitionState(PetStateType.Chase);
            }
            
            pet.GetDistance();
            pet.AutoPath(pet.player.transform.position);

            if (pet.pathPointList.Count > 0)
            {
                if (pet.distance <= pet.idleDistance)
                {
                    pet.TransitionState(PetStateType.Idle);
                }
                else if (pet.distance >= pet.followDistance)
                {
                    pet.transform.position = pet.player.transform.position - new Vector3(1f,1f,0f);
                }
                else
                {
                    pet.moveDirection = pet.pathPointList[pet.currentIndex] - pet.transform.position;
                }
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
