using UnityEngine;
using UnityEngine.UI;

public class DifferenceGame : MonoBehaviour
{
    [SerializeField] private int _numberOfDifferences;
    public void OnClick(Button button)
    {
        button.interactable = false;
        button.image.color = Color.white;
        
        _numberOfDifferences--;
        if (_numberOfDifferences == 0)
            gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        PlayerController.freezeInput = true;
    }
    public void OnDisable()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        PlayerController.freezeInput = false;
    }
}
