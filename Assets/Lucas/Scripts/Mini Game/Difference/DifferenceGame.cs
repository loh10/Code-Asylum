using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class DifferenceGame : MonoBehaviour
{
    [SerializeField] private List<DifferenceNumber> _numbers = new List<DifferenceNumber>();
    [SerializeField] private GameObject _cross;
    
    private int _index;
    private int _numberOfDifferences;
    private int _numberOfClicks = 2;
    private bool _isEnd;
    private GameObject _crossObject;
    private MiniGameManager _miniGame;

    private void Start()
    {
        _crossObject = Instantiate(_cross, Vector3.zero, Quaternion.identity, _numbers[0].transform.parent);
        _crossObject.SetActive(false);
        _miniGame = GetComponentInParent<MiniGameManager>();
    }
    public void OnClick(Button button)
    {
        if (_isEnd) return;
        
        button.interactable = false;
        button.image.color = Color.white;
        
        _numberOfDifferences--;
        if (_numberOfDifferences != 0)
            return;
        
        StartCoroutine(EndGame(0.5f));
    }
    public void BadClick()
    {
        if (_isEnd) return;
        
        _numberOfClicks--;
        _crossObject.SetActive(true);
        _crossObject.transform.position = Input.mousePosition;
        
        if (_numberOfClicks > 0) return;
        
        StartCoroutine(ResetGame(0.5f));
    }

    private IEnumerator ResetGame(float time)
    {
        _isEnd = true;
        yield return new WaitForSeconds(time);
        DifferenceNumber number = _numbers[_index];
        foreach (Button button in number.listButtons)
        {
            button.interactable = true;
            button.image.color = Color.white - new Color { a = 1f };
        }

        if (_numbers.Count > 1)
        {
            number.gameObject.SetActive(false);
            _numbers.Remove(_numbers[_index]);
            _index = Random.Range(0, _numbers.Count);
            _numberOfDifferences = _numbers[_index].listButtons.Count;
            _numbers[_index].gameObject.SetActive(true);
            _numbers.Add(number);
        }
        else
        {
            _numberOfDifferences = _numbers[0].listButtons.Count;
        }
        _numberOfClicks = 2;
        _isEnd = false;
        _crossObject.SetActive(false);
    }

    private IEnumerator EndGame(float time)
    {
        _isEnd = true;
        yield return new WaitForSeconds(time);
        _miniGame.Solve();
    }
    private void OnEnable()
    {
        _index = Random.Range(0, _numbers.Count);
        _numberOfDifferences = _numbers[_index].listButtons.Count;
        _numbers[_index].gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        PlayerController.freezeInput = true;
    }
    public void OnDisable()
    {
        //StartCoroutine(ResetGame(0));
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        PlayerController.freezeInput = false;
    }
}
