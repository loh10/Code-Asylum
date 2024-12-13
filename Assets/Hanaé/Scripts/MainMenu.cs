using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private bool _isPaused;
    [SerializeField] private GameObject _pauseMenu;
    
    public void Escape(InputAction.CallbackContext ctx)
    {
        if (!ctx.canceled) return;
        
        if (_isPaused)
        {
            Close(_pauseMenu);
        }
        else if (!Cursor.visible)
        {
            Open(_pauseMenu);
        }
    }

    private void Open(GameObject menu)
    {
        menu.SetActive(true);
        _isPaused = true;
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        PlayerController.freezeInput = true;
    }

    public void Close(GameObject menu)
    {
        _isPaused = false;
        menu.SetActive(false);
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        PlayerController.freezeInput = false;
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

    public void Quit()
    {
        Application.Quit();
    }
}
