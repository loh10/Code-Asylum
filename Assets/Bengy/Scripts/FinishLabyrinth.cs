using UnityEngine;

public class ReplayLabyrinth : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Time.timeScale = 1;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Time.timeScale = 0;
    }
}
