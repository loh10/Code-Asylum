using UnityEngine;
using UnityEngine.SceneManagement;

public class AreYouSurePopUp : MonoBehaviour
{
    public void Yes()
    {
        SceneManager.LoadScene(0);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        PlayerController.freezeInput = false;
        Time.timeScale = 1;
    }

    public void Cancel(GameObject popUp)
    {
        popUp.SetActive(false);
    }
}
