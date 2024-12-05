using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class GetButton : MonoBehaviour
{
    [SerializeField] private List<GameObject> _listButton;
    public InputActionAsset _actionAsset;
    [HideInInspector] public List<string> _listControl = new List<string>();
    private readonly List<ChangeControl> _changeControls = new List<ChangeControl>();
    private int _indexButton;
    public static string _text = "99";
    private void Start()
    {
        foreach (GameObject t in _listButton)
        {
            _listControl.Add("");
            _changeControls.Add(t.GetComponent<ChangeControl>());
        }
        SetListControl();
    }
    public void SetListControl()
    {
        for (int i = 0; i < _changeControls.Count; i++)
        {
            _listControl[i] = _actionAsset.actionMaps[_changeControls[i]._indexActionMap].actions[_changeControls[i]._indexAction].bindings[_changeControls[i]._indexBinding].path.ToString();
        }
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
    public void Any(InputAction.CallbackContext ctx)
    {
        InputSystem.onAnyButtonPress.CallOnce(ctrl => _text = ctrl.name);
        if ((_text.Length < 2 || _text == "space" || _text == "leftShift" || _text == "semicolon" || _text == "tab") && _text != "escape")
        {
            _listButton[_indexButton].GetComponent<ChangeControl>().Change();
        }
    }
    public void OnClick(int index)
    {
        _indexButton = index;
    }
}
