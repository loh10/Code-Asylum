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
        ennemi.enabled = false;
        
        transform.position = respawnPoint;
        ennemi.transform.position = _startEnnemiPos;
        
        ennemi.enabled = true;
        PlayerController.freezeInput = false;
        if (MiniGameManager.currentMiniGame != null)
            MiniGameManager.currentMiniGame.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
