using UnityEngine;

/// <summary>
/// A basic test script for a generator that visually indicates power state using material color.
/// </summary>
public class TestGenerator : MonoBehaviour
{
    [Header("Generator Configuration")]
    [Tooltip("The lock component that restricts access to the generator.")]
    [SerializeField] private Lock generatorLock;

    [Header("Visual Indicator")]
    [Tooltip("The sphere representing the generator's power state.")]
    [SerializeField] private Renderer powerIndicatorSphere;

    [Tooltip("Material for the 'off' state.")]
    [SerializeField] private Material offMaterial;

    [Tooltip("Material for the 'on' state.")]
    [SerializeField] private Material onMaterial;

    private bool isPowerOn = false;

    private void Awake()
    {
        if (generatorLock == null)
        {
            generatorLock = GetComponent<Lock>();
        }

        if (generatorLock != null)
        {
            generatorLock.OnUnlock += OnGeneratorUnlocked;
        }
        else
        {
            Debug.LogWarning($"No Lock component assigned or found on the GameObject '{gameObject.name}'. Power will not be controllable.");
        }

        if (powerIndicatorSphere == null)
        {
            Debug.LogWarning($"No sphere assigned for visual power indicator on '{gameObject.name}'.");
        }

        UpdatePowerIndicator();
    }

    private void OnDestroy()
    {
        if (generatorLock != null)
        {
            generatorLock.OnUnlock -= OnGeneratorUnlocked;
        }
    }

    private void OnGeneratorUnlocked()
    {
        if (isPowerOn)
        {
            Debug.Log("Power is already on.");
            return;
        }

        TurnOnPower();
    }

    private void TurnOnPower()
    {
        isPowerOn = true;
        Debug.Log("Power has been turned on!");
        UpdatePowerIndicator();
    }

    /// <summary>
    /// Updates the sphere's material to indicate the current power state.
    /// </summary>
    private void UpdatePowerIndicator()
    {
        if (powerIndicatorSphere != null)
        {
            powerIndicatorSphere.material = isPowerOn ? onMaterial : offMaterial;
        }
    }
}
