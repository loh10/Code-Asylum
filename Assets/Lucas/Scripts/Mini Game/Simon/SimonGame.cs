using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class SimonGame : MonoBehaviour
{
    [SerializeField] private List<Image> _listImages = new List<Image>();
    [SerializeField] private int _round;
    [SerializeField] private float _timeToChangeColor;
    [SerializeField] private float _timeStayColor;
    
    private readonly List<int> _sequence = new List<int>();
    private int _sequenceIndex;
    private int _roundIndex;
    private Button _startButton;
    private readonly List<Color> _initialColor = new List<Color>();
    private MiniGameManager _miniGame;

    private void Awake()
    {
        foreach (Image image in _listImages)
        {
            _initialColor.Add(image.color);
        }
        _miniGame = GetComponentInParent<MiniGameManager>();
    }
    private void Start()
    {
        AddColor();
    }

    public void StartGame(Button button)
    {
        InteractableButton(false);
        StartCoroutine(StartColor());
        button.interactable = false;
        _startButton = button;
    }
    
    private IEnumerator StartColor(int indexColor = 0)
    {
        yield return new WaitForSeconds(_timeToChangeColor);
        Color initialColor = _listImages[_sequence[indexColor]].color;
        _listImages[_sequence[indexColor]].color *= 2f;


        AudioManager.Instance.PlaySound(AudioType.simon);


        yield return new WaitForSeconds(_timeStayColor);
        _listImages[_sequence[indexColor]].color = initialColor;
        indexColor++;
        if (indexColor != _sequence.Count)
        {
            StartCoroutine(StartColor(indexColor));
        }
        else
        {
            InteractableButton(true);
        }
    }
    private void InteractableButton(bool value)
    {
        foreach (Image image in _listImages)
        {
            image.GetComponent<Button>().interactable = value;
        }
    }

    private void AddColor()
    {
        int indexColor = Random.Range(0, _listImages.Count);
        _sequence.Add(indexColor);
    }

    public void OnClick(int index)
    {
        _listImages[index].color = _initialColor[index];

        AudioManager.Instance.PlaySound(AudioType.simon);


        if (index != _sequence[_sequenceIndex])
        {
            ResetGame();
            AddColor();
            StartCoroutine(StartColor());
            return;
        }
        
        _sequenceIndex++;
        if (_sequenceIndex != _sequence.Count) return;
        
        _sequenceIndex = 0;
        _roundIndex++;
        
        if (_roundIndex == _round)
            _miniGame.Solve();
        else
        {
            InteractableButton(false);
            AddColor();
            StartCoroutine(StartColor());
        }
    }
    
    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        PlayerController.freezeInput = true;
        if (_startButton != null)
            _startButton.interactable = true;
    }
    public void OnDisable()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        PlayerController.freezeInput = false;
        StopAllCoroutines();
        InteractableButton(false);
    }
    private void ResetGame()
    {
        _roundIndex = 0;
        _sequenceIndex = 0;
        _sequence.Clear();
        InteractableButton(false);
    }

    private void Update()
    {
        if (!Input.GetKeyUp(KeyCode.Escape)) return;
        
        gameObject.SetActive(false);
    }
    
}
