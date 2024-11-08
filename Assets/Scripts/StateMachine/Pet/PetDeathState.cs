namespace StateMachine.Pet
{
    public class PetDeathState : IState
    {
        Character.Pet.Pet pet;

        public PetDeathState(Character.Pet.Pet pet)
        {
            this.pet = pet;
        }

        public void OnEnter()
        {

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
