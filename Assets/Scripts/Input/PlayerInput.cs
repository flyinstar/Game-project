using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Input
{
    [CreateAssetMenu(fileName ="MyPlayerInput")]

    public class PlayerInput : ScriptableObject, InputActions.IPlayerActions
    {
        public event UnityAction<Vector2> onMove;
        public event UnityAction onMoveStop;
        public event UnityAction onAttack;
        // public event UnityAction onFire; 
        public event UnityAction onDodge;

        InputActions inputActions;

        private void OnEnable()
        {
            inputActions = new InputActions();

            //PlayerInput�̳�������ӿ�,���Դ���this����playinputע��Ϊ�ص������Ľ����ߡ�
            inputActions.Player.SetCallbacks(this);
        }

        public void EnablePlayerInput()
        {
            SwitchActionMap(inputActions.Player);
        }

        //切换动作表
        void SwitchActionMap(InputActionMap actionMap)
        {
            inputActions.Disable();
            actionMap.Enable();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            if(context.performed)
            {
                onMove?.Invoke(context.ReadValue<Vector2>());
            }
            if(context.canceled)
            {
                onMoveStop?.Invoke();
            }

            //switch (context.phase)
            //{
            //    case InputActionPhase.Started://���µ���һ��
            //        Debug.Log("Started");
            //        break;
            //    case InputActionPhase.Performed://��������
            //        Debug.Log("Performed");
            //        break;
            //    case InputActionPhase.Canceled://�ɿ�
            //        Debug.Log("Canceled");
            //        break;
            //}
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            if(context.started)
            {
                onAttack?.Invoke();
            }

            // if (context.performed)
            // {
            //     onFire?.Invoke();
            // }
        }

        public void OnDodge(InputAction.CallbackContext context)
        {
            if (context.started) 
            {
                onDodge?.Invoke();
            }
        }

        public void OnInteraction(InputAction.CallbackContext context)
        {
            //todo:交互设置
        }

        public void OnCallPet(InputAction.CallbackContext context)
        {
            //todo:召唤宠物
        }
    }
}