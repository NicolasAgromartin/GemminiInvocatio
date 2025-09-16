using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToHome : MonoBehaviour
{
    public void HomeScene()
    {
        SceneManager.LoadSceneAsync("MainMenu");
    }
}