using UnityEngine;

public class EndGame : MonoBehaviour
{
    [SerializeField] private GameObject _uiEnd;
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        
        // Show UI
        _uiEnd.SetActive(true);
        MiniGameManager.currentMiniGame = _uiEnd;
        
        // Hide active dialogue box
        DialogueMessageBoxUI.Instance.HideImmediately();
        
        // Handle cursor
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        
        // Freeze time
        Time.timeScale = 0.0f;
    }
}
