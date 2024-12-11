using UnityEngine;

public class MiniGameManager : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject _miniGame;


    public bool IsInteractable { get; }
    public string InteractionHint { get; }
    public void Interact(GameObject interactor)
    {
        _miniGame.SetActive(true);
    }
}
