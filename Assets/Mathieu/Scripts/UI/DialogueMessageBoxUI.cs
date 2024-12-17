using UnityEngine;
using TMPro;
using System.Collections;

public class DialogueMessageBoxUI : MonoBehaviour
{
    public static DialogueMessageBoxUI Instance;

    [Header("UI References")]
    [SerializeField] private GameObject boxPanel;      // A panel or image that backgrounds the text
    [SerializeField] private TMP_Text messageText;     // Text field to show the message
    [SerializeField] private float defaultDuration = 3f; // Default time before the message hides automatically

    private Coroutine hideRoutine;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (boxPanel != null)
            boxPanel.SetActive(false);
    }

    /// <summary>
    /// Show a message for a certain duration, if duration <= 0, uses defaultDuration.
    /// </summary>
    public void ShowMessage(string message, float duration = -1f)
    {
        // Stop any previous hide routine if it exists
        if (hideRoutine != null) StopCoroutine(hideRoutine);

        if (messageText != null)
            messageText.text = message;

        if (boxPanel != null)
            boxPanel.SetActive(true);

        if (duration <= 0f)
            duration = defaultDuration;

        hideRoutine = StartCoroutine(HideAfterSeconds(duration));
    }

    public void HideImmediately()
    {
        if (hideRoutine != null)
        {
            StopCoroutine(hideRoutine);
            hideRoutine = null;
        }

        if (boxPanel != null)
            boxPanel.SetActive(false);
    }

    private IEnumerator HideAfterSeconds(float sec)
    {
        yield return new WaitForSeconds(sec);
        if (boxPanel != null)
            boxPanel.SetActive(false);
        hideRoutine = null;
    }
}