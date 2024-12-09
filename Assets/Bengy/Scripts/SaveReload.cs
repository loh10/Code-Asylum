using UnityEngine;

public class SaveReload : MonoBehaviour
{
    public Vector3 respawnPoint;

    private void Awake()
    {
        respawnPoint = transform.position;
    }

    public void IsDead()
    {
        transform.position = respawnPoint;
    }
}
