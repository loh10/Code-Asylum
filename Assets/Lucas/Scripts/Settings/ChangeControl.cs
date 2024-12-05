using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Linq;

public class ChangeControl : MonoBehaviour
{
    private string _control = "";
    private InputActionAsset _actionAsset;
    public int _indexActionMap;
    public int _indexAction;
    public int _indexBinding;
    private void Start()
    {
        _actionAsset = GetComponentInParent<GetButton>()._actionAsset;
        _control = _actionAsset.actionMaps[_indexActionMap].actions[_indexAction].bindings[_indexBinding].path.ToString();
        _control = _control[11..];
        ChangeQwerty();
        GetComponentInChildren<TextMeshProUGUI>().text = _control.ToUpper();
    }
    public void Change()
    {
        _control = GetButton._text;
        if (EventSystem.current.currentSelectedGameObject != gameObject)
            return;
        List<string> list = GetComponentInParent<GetButton>()._listControl;
        if (list.Any(t => t == "<Keyboard>/" + _control))
        {
            return;
        }
        _actionAsset.actionMaps[_indexActionMap].actions[_indexAction].ChangeBinding(_indexBinding).WithPath("<Keyboard>/" + _control);
        ChangeQwerty();
        GetComponentInChildren<TextMeshProUGUI>().text = _control.ToUpper();
        GetComponentInParent<GetButton>().SetListControl();
    }
    private void ChangeQwerty()
    {
        _control = _control switch
        {
            "q" => "a",
            "a" => "q",
            "w" => "z",
            "z" => "w",
            "semicolon" => "m",
            _ => _control
        };
    }
}
