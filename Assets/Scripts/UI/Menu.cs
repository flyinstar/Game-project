using Character.Player;
using Input;
using UnityEngine;

namespace UI
{
    public class Menu : MonoBehaviour
    {
        public PlayerInput playerInput;
        
        public GameObject menu;
    
        private bool menuOpen;

        private void Update()
        {
            if (!menuOpen)
            {
                if (UnityEngine.Input.GetKeyDown(KeyCode.Escape))
                {
                    menu.SetActive(true);
                    menuOpen = true;
                    Time.timeScale = 0;
                    // playerInput.EnableUIInput();
                }
            }
            else if (menuOpen)
            {
                if (UnityEngine.Input.GetKeyDown(KeyCode.Escape))
                {
                    menu.SetActive(false);
                    menuOpen = false;
                    Time.timeScale = 1;
                    // playerInput.EnablePlayerInput();
                }
            }
        }

        public void Continue()
        {
            menu.SetActive(false);
            menuOpen = false;
            Time.timeScale = 1;
        }

        public void Restart()
        {
            menu.SetActive(false);
            menuOpen = false;
        }
    
        public void Exit()
        {
        
        }
    }
}
