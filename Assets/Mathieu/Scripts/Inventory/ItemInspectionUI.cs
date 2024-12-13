using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Serialization;

/// <summary>
/// Displays the selected item's model, name, and description.
/// Allows the player to rotate the model by dragging the mouse.
/// </summary>
public class ItemInspectionUI : MonoBehaviour
{
    [FormerlySerializedAs("inspectionPanel")]
    [Header("UI References")]
    [SerializeField] private GameObject inspectionWindow;
    [SerializeField] private TMP_Text itemNameText;
    [SerializeField] private TMP_Text itemDescriptionText;
    [SerializeField] private Button closeButton;

    [Header("Model Display")]
    [SerializeField] private Transform modelParent;
    [SerializeField] private float rotationSpeed = 100f;

    private GameObject currentModelInstance;
    private Vector3 lastMousePos;

    private void Start()
    {
        if (closeButton != null)
        {
            closeButton.onClick.AddListener(HideItem);
        }
        HideItem();
    }

    public void ShowItem(ItemConfig item)
    {
        if (item == null) return;

        if (inspectionWindow != null)
            inspectionWindow.SetActive(true);

        itemNameText.text = item.itemName;
        itemDescriptionText.text = item.description;

        // Clear existing model
        if (currentModelInstance != null)
            Destroy(currentModelInstance);

        if (item.modelPrefab != null)
        {
            currentModelInstance = Instantiate(item.modelPrefab, modelParent);
            currentModelInstance.transform.localPosition = Vector3.zero;
            currentModelInstance.transform.localRotation = Quaternion.identity;
        }
        else
        {
            Debug.LogWarning($"No model assigned for item {item.itemName}");
        }
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

                // Rotate model
                currentModelInstance.transform.Rotate(Vector3.up, -delta.x * rotationSpeed * Time.deltaTime, Space.World);
                currentModelInstance.transform.Rotate(Vector3.right, delta.y * rotationSpeed * Time.deltaTime, Space.World);
            }
        }
    }
}
