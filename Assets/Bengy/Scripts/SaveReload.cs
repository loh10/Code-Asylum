using UnityEngine;
using UnityEngine.AI;

public class SaveReload : MonoBehaviour
{
    [SerializeField] private NavMeshAgent ennemi;
    private Vector3 _startEnnemiPos;
    private Vector3 respawnPoint;
    
    private void Awake()
    {
        respawnPoint = transform.position;
        _startEnnemiPos = ennemi.transform.position;
    }
    
    public void IsDead()
    {
        string message = DialogueManager.GetDialogue("Player", "Dead");
        DialogueMessageBoxUI.Instance.ShowMessage(message, 3f);
        
        ennemi.enabled = false;
        
        transform.position = respawnPoint;
        ennemi.transform.position = _startEnnemiPos;
        
        ennemi.enabled = true;
        PlayerController.freezeInput = false;
        if (MiniGameManager.currentMiniGame != null)
        {
            MiniGameManager.currentMiniGame.SetActive(false);
            InventoryManager.Instance.ToggleInventoryDisplay(false);
        }
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
