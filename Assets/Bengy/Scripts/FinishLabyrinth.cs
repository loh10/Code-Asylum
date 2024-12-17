using UnityEngine;


public class FinishLabyrinth : MonoBehaviour
{
    [SerializeField] private MiniGameManager _miniGame;
    
    private void OnCollisionEnter(Collision collision)
    {
        _miniGame.Solve();
    }
}
