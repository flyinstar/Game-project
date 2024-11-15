using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public void StartGameMenu()
    {
        SceneManager.LoadScene("Scenes/SampleScene");
    }
}
