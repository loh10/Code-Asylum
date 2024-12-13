using UnityEngine;
using System.Collections;

public static class ScreenShakeUtility
{
    /// <summary>
    /// Triggers a screen shake effect on the specified transform.
    /// </summary>
    /// <param name="targetTransform">The transform to shake (e.g., the camera).</param>
    /// <param name="duration">Duration of the shake effect.</param>
    /// <param name="magnitude">Magnitude of the shake effect.</param>
    /// <param name="onComplete">Optional callback when the shake effect is complete.</param>
    public static IEnumerator TriggerScreenShake(Transform targetTransform, float duration, float magnitude, System.Action onComplete = null)
    {
        if (targetTransform == null)
        {
            Debug.LogWarning("ScreenShakeUtility: No targetTransform provided for screen shake.");
            yield break;
        }

        Vector3 originalPosition = targetTransform.localPosition;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            Vector3 randomOffset = Random.insideUnitSphere * magnitude;
            targetTransform.localPosition = originalPosition + randomOffset;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        targetTransform.localPosition = originalPosition; // Reset position after shaking

        onComplete?.Invoke(); // Trigger the callback if provided
    }
}


 /* Triggers a screen shake effect on a given Transform (e.g., a camera).
 
 The screen shake creates a randomized shaking motion for the specified duration and magnitude,
 providing a visual impact effect commonly used in games for events like explosions, incorrect
 actions, or intense moments. Once the shaking effect is complete, an optional `onComplete`
 callback can be invoked to perform additional actions or cleanup logic.
 
 Parameters:
 - `target`: The Transform to apply the screen shake effect (usually the camera's Transform).
 - `duration`: The length of time the screen shake effect lasts, in seconds.
 - `magnitude`: The intensity of the shaking effect.
 - `onComplete`: (Optional) A callback that executes after the screen shake effect finishes.
   This is useful for chaining actions, resetting states, or triggering follow-up effects.
 
 Example Usage:
 ```
 // Trigger a screen shake on the main camera for 0.5 seconds with a magnitude of 0.2.
 StartCoroutine(ScreenShakeUtility.TriggerScreenShake(
     Camera.main.transform, 0.5f, 0.2f, 
     () => Debug.Log("Screen shake complete!")));
 
 // Alternative: Use a named method for the callback.
 private void OnShakeComplete() {
     Debug.Log("Shake finished! Re-enable input.");
 }
 StartCoroutine(ScreenShakeUtility.TriggerScreenShake(Camera.main.transform, 0.5f, 0.2f, OnShakeComplete));
 ```
 
 Notes:
 - The `onComplete` callback is optional and can be null if no post-shake action is required.
 - The screen shake will restore the Transform's position to its original value after completion.
 - Designed as a utility for easy reuse across different scripts without requiring a MonoBehaviour
   instance or component-specific logic.
*/

