using UnityEngine;

public class ReplayLabyrinth : MonoBehaviour
{
    [SerializeField] private GameObject _labyrinth;
    [SerializeField] private GameObject _panelVictory;
    private void OnCollisionEnter(Collision collision)
    {
        _panelVictory.SetActive(true);
        _labyrinth.layer = LayerMask.NameToLayer("Default");
    }
}
