using UnityEngine;

public class SaveReload : MonoBehaviour
{
    public Vector3 respawnPoint;
    public GameObject ennemi;
    private Vector3 _startEnnemiPos;

    private void Awake()
    {
        respawnPoint = transform.position;
        _startEnnemiPos = ennemi.transform.position;
    }

    public void IsDead()
    {
        transform.position = respawnPoint;
        ennemi.transform.position = _startEnnemiPos;
    }
}
