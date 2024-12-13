using UnityEngine;
using UnityEngine.SceneManagement;

public class AreYouSurePopUp : MonoBehaviour
{
    public void Yes()
    {
        SceneManager.LoadScene(0);
    }

    public void Cancel(GameObject popUp)
    {
        popUp.SetActive(false);
    }
}
