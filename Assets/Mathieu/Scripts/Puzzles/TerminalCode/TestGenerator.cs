using UnityEngine;

/// <summary>
/// A basic test script for a generator that can be interacted with after being unlocked.
/// </summary>
public class TestGenerator : MonoBehaviour, IInteractable
{
    [Header("Generator Configuration")]
    [Tooltip("The lock component that restricts access to the generator.")]
    [SerializeField] private Lock generatorLock;

    public bool IsInteractable => generatorLock != null && !generatorLock.isLocked;
    public string InteractionHint => IsInteractable ? "Press 'E' to turn on the power" : "Locked";

    private bool isPowerOn = false;

    private void Awake()
    {
        if (generatorLock == null)
        {
            Debug.LogWarning("No Lock component assigned to the generator. Assign one in the Inspector.");
        }
    }

    public void Interact(GameObject interactor)
    {
        if (!IsInteractable)
        {
            Debug.Log("Generator is locked. Solve the puzzle to unlock it.");
            return;
        }

        if (isPowerOn)
        {
            Debug.Log("The power is already on.");
            return;
        }

        TurnOnPower();
    }

    private void TurnOnPower()
    {
        isPowerOn = true;
        Debug.Log("Power has been turned on!");
    }
}