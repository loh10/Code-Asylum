using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public AudioMixer _audioMixer;
    
    [SerializeField] private Toggle _toggleFullScreen;
    [Header("Sliders")]
    [SerializeField] private Slider _sliderGeneral;
    [SerializeField] private TextMeshProUGUI _textValueGeneral;
    [SerializeField] private Slider _sliderSFX;
    [SerializeField] private TextMeshProUGUI _textValueSFX;
    [SerializeField] private Slider _sliderMusic;
    [SerializeField] private TextMeshProUGUI _textValueMusic;
    [SerializeField] private Slider _sliderSensitivity;
    [SerializeField] private TextMeshProUGUI _textValueSensitivity;
    
    [Header("Resolution")]
    [SerializeField] private TMP_Dropdown _dropdownResolution;
    
    private Resolution[] _resolutions;
    
    private void Start()
    {
        if (_toggleFullScreen != null)
            _toggleFullScreen.isOn = Screen.fullScreen;

        if (PlayerPrefs.HasKey("General") && _sliderGeneral != null)
            _sliderGeneral.value = PlayerPrefs.GetFloat("General");

        if (PlayerPrefs.HasKey("SFX") && _sliderSFX != null)
            _sliderSFX.value = PlayerPrefs.GetFloat("SFX");

        if (PlayerPrefs.HasKey("Music") && _sliderMusic != null)
            _sliderMusic.value = PlayerPrefs.GetFloat("Music");

        if (PlayerPrefs.HasKey("Sensitivity") && _sliderSensitivity != null)
            _sliderSensitivity.value = PlayerPrefs.GetFloat("Sensitivity");
        
        SetTextValueSlider(_sliderGeneral, _textValueGeneral);
        SetTextValueSlider(_sliderSFX, _textValueSFX);
        SetTextValueSlider(_sliderMusic, _textValueMusic);
        SetTextValueSlider(_sliderSensitivity, _textValueSensitivity);
        
        if (_dropdownResolution != null)
            GetResolution();
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }
    
    public void SetVolumeGeneral(float volume)
    {
        if (_audioMixer == null) return;
        
        _audioMixer.SetFloat("General", volume);
        PlayerPrefs.SetFloat("General", _sliderGeneral.value);
        SetTextValueSlider(_sliderGeneral, _textValueGeneral);
    }
    
    public void SetVolumeSFX(float volume)
    {
        if (_audioMixer == null) return;
        
        _audioMixer.SetFloat("SFX", volume);
        PlayerPrefs.SetFloat("SFX", _sliderSFX.value);
        SetTextValueSlider(_sliderSFX, _textValueSFX);
    }
    
    public void SetVolumeMusic(float volume)
    {
        if (_audioMixer == null) return;
        
        _audioMixer.SetFloat("Music", volume);
        PlayerPrefs.SetFloat("Music", _sliderMusic.value);
        SetTextValueSlider(_sliderMusic, _textValueMusic);
    }
    
    public void SetSensitivity(float sensitivity)
    {
        PlayerPrefs.SetFloat("Sensitivity", _sliderSensitivity.value);
        SetTextValueSlider(_sliderSensitivity, _textValueSensitivity);
    }
    
    private Resolution[] GetResolution()
    {
        _resolutions = Screen.resolutions.Select(resolution => 
            new Resolution { width = resolution.width, height = resolution.height }).Distinct().ToArray();
        _dropdownResolution.ClearOptions();
        List<string> options = new List<string>();
        int currentResolution = 0;
        for (int i = 0; i < _resolutions.Length; i++)
        {
            string option = _resolutions[i].width + "x" + _resolutions[i].height;
            options.Add(option);
            
            if (_resolutions[i].width == Screen.width && _resolutions[i].height == Screen.height)
            {
                currentResolution = i;
            }
        }
        
        _dropdownResolution.AddOptions(options);
        _dropdownResolution.value = currentResolution;
        _dropdownResolution.RefreshShownValue();
        
        return _resolutions;
    }
    
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = _resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
    private void SetTextValueSlider(Slider slider, TextMeshProUGUI textValue)
    {
        int mappedValue = Mathf.RoundToInt((slider.value - slider.minValue) / -(slider.minValue - slider.maxValue) * 100);
        textValue.text = mappedValue.ToString("0");
    }
}
