using UnityEngine;
using UnityEngine.AI;

public class SaveReload : MonoBehaviour
{
    public Vector3 respawnPoint;
    public NavMeshAgent ennemi;
    private Vector3 _startEnnemiPos;
    
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
    }
}
