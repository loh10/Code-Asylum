using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Open(GameObject menu)
    {
        menu.SetActive(true);
        Time.timeScale = 0;
    }

    void Close(GameObject menu)
    {
        Time.timeScale = 1;
        menu.SetActive(false);
    }

    public void Play(string SceneToLoad)
    {
        SceneManager.LoadScene(SceneToLoad);
    }

    public void Settings(GameObject settingsMenu)
    {
        settingsMenu.SetActive(true);
    }

    public void Credits(GameObject creditsBackground)
    {
        creditsBackground.SetActive(!creditsBackground.activeSelf);
    }

    public void TitleScreen(GameObject popUp)
    {
        popUp.SetActive(true);
    }

    public void GoBackToStartingRoom(GameObject ActiveMenu)
    {
        //to complete !!!
        Close(ActiveMenu);
    }

    public void Back(GameObject toClose)
    {
        Close(toClose);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
