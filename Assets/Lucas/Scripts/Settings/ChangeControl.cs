using System.Collections.Generic;
using System.Linq;
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

        if (changeControls.Where((t, i) => i != _indexBinding).Where(t => 
        t._actionReference.action.bindings[t._indexBinding].ToDisplayString().Length > 1).Any(t => 
        control == t._actionReference.action.bindings[t._indexBinding].ToDisplayString()[1..^1]))
        {
            return false;
        }

        _actionReference.action.ChangeBinding(_indexBinding).WithPath("<Keyboard>/#(" + control + ")");
        _textControl.text = control;
        _control = control;
        return true;
    }
}
