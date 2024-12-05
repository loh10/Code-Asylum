using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Play(string SceneToLoad)
    {
        SceneManager.LoadScene(SceneToLoad);
    }

    public void Settings()
    {

    }

    public void Credits(GameObject creditsBackground)
    {
        creditsBackground.SetActive(!creditsBackground.activeSelf);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
