using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.EventSystems;

public class ChangeControl : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textControl;
    [SerializeField] private int _indexBinding;
    [SerializeField] private InputActionReference _actionReference;
    
    private string _control;
    private void Start()
    {
        _control = _actionReference.action.bindings[_indexBinding].ToDisplayString();
        if (_control.Length > 1)
            _control = _control[1..^1];
        _textControl.text = _control;
    }
    public bool Change(string control, List<ChangeControl> changeControls)
    {
        if (EventSystem.current.currentSelectedGameObject != gameObject || control == _control)
            return true;

        foreach (ChangeControl changeControl in changeControls)
        {
            string currentControl = changeControl._actionReference.action.bindings[changeControl._indexBinding].ToDisplayString();

            if (currentControl.Length > 1)
                currentControl = currentControl[1..^1];

            if (currentControl == control)
                return false;
        }

        _actionReference.action.ChangeBinding(_indexBinding).WithPath("<Keyboard>/#(" + control + ")");
        _textControl.text = control;
        _control = control;
        return true;
    }
}
