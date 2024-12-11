using UnityEngine;

public class ReplayLabyrinth : MonoBehaviour
{
    [SerializeField] private PlayerLabyrinthControl _playerLabyrinthControl;
    private void OnCollisionEnter(Collision collision)
    {
        _playerLabyrinthControl.Solve();
    }
}
