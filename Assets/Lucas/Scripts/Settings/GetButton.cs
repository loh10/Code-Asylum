using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class GetButton : MonoBehaviour
{
    [SerializeField] private List<ChangeControl> _changeControls;
    [SerializeField] private GameObject _popUp;
    [SerializeField] private InputActionAsset _actionAsset;
    
    private int _indexButton;
    private string _text = "99";
    
    public void Any(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed)
            return;
        
        InputSystem.onAnyButtonPress.Call(currentAction => { _text = currentAction.displayName; });
        if (_text.Length > 1) return;

        _popUp.SetActive(!_changeControls[_indexButton].Change(_text, _changeControls));
    }
    public void OnClick(int index)
    {
        _indexButton = index;
        _popUp.SetActive(false);
    }
    private void OnEnable()
    {
        _actionAsset.actionMaps[0].Disable();
        _actionAsset.actionMaps[1].Enable();
    }
    private void OnDisable()
    {
        _actionAsset.actionMaps[0].Enable();
        _actionAsset.actionMaps[1].Disable();
    }
}
