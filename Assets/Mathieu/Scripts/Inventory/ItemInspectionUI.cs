using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Serialization;

/// <summary>
/// Displays the selected item's model, name, and description using a dedicated ModelPreviewRoot and a separate camera.
/// Allows the player to rotate the model by dragging the mouse.
/// </summary>
public class ItemInspectionUI : MonoBehaviour
{
    [FormerlySerializedAs("inspectionPanel")]
    [Header("UI References")]
    [SerializeField] private GameObject inspectionWindow;
    [SerializeField] private TMP_Text itemNameText;
    [SerializeField] private TMP_Text itemDescriptionText;

    [Header("3D Preview Setup")]
    [Tooltip("Assign the Transform of the ModelPreviewRoot in the world.")]
    [SerializeField] private Transform modelPreviewRoot;

    [Tooltip("RawImage that displays the RenderTexture from the ModelPreviewCamera.")]
    [SerializeField] private RawImage previewRawImage;

    [Header("Model Rotation")]
    [SerializeField] private float rotationSpeed = 100f;

    private GameObject currentModelInstance;
    private Vector3 lastMousePos;

    private void Start()
    {
        HideItem();
    }

    public void ShowItem(ItemConfig item)
    {
        if (item == null) return;

        // Show UI window
        if (inspectionWindow != null)
            inspectionWindow.SetActive(true);

        // Update text
        if (itemNameText != null)
            itemNameText.text = item.itemName;
        if (itemDescriptionText != null)
            itemDescriptionText.text = item.description;

        // Destroy old model if any
        if (currentModelInstance != null)
        {
            Destroy(currentModelInstance);
        }

        // Instantiate the new model at the ModelPreviewRoot
        if (item.modelPrefab != null && modelPreviewRoot != null)
        {
            currentModelInstance = Instantiate(item.modelPrefab, modelPreviewRoot);
            currentModelInstance.transform.localPosition = Vector3.zero;
            currentModelInstance.transform.localRotation = Quaternion.identity;

            // Optional: Adjust scale if needed
            // currentModelInstance.transform.localScale = Vector3.one * someScaleFactor;
        }
        else
        {
            Debug.LogWarning($"No model assigned for item {item.itemName} or modelPreviewRoot not set.");
        }

        // Ensure the previewRawImage is showing the RenderTexture from the ModelPreviewCamera
        // This should be set up in the inspector.
    }

    public void HideItem()
    {
        if (inspectionWindow != null)
            inspectionWindow.SetActive(false);

        if (currentModelInstance != null)
        {
            Destroy(currentModelInstance);
            currentModelInstance = null;
        }
    }

    private void Update()
    {
        // Only allow rotation if window is open and there's a model
        if (inspectionWindow != null && inspectionWindow.activeSelf && currentModelInstance != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                lastMousePos = Input.mousePosition;
            }
            else if (Input.GetMouseButton(0))
            {
                Vector3 delta = Input.mousePosition - lastMousePos;
                lastMousePos = Input.mousePosition;

                // Rotate model based on mouse movement
                currentModelInstance.transform.Rotate(Vector3.up, -delta.x * rotationSpeed * Time.deltaTime, Space.World);
                currentModelInstance.transform.Rotate(Vector3.right, delta.y * rotationSpeed * Time.deltaTime, Space.World);
            }
        }
    }
}
