using UnityEngine;


public class ReplayLabyrinth : MonoBehaviour
{
    private MiniGameManager _miniGame;

    private void Start()
    {
        _miniGame = GetComponentInParent<MiniGameManager>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        _miniGame.Solve();
    }
}
