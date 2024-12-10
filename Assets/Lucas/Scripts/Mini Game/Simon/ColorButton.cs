using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ColorButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Button _button;
    private Color _initialColor;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _initialColor = _button.image.color;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!_button.IsInteractable()) return;
        
        _button.image.color *= 2f;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (!_button.IsInteractable()) return;
        
        _button.image.color = _initialColor;
    }
}
