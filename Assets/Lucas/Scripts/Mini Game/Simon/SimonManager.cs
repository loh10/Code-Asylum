using UnityEngine;

public class SimonManager : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject _simonGame;


    public bool IsInteractable { get; }
    public string InteractionHint { get; }
    public void Interact(GameObject interactor)
    {
        _simonGame.SetActive(true);
    }
}
