using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class EffectVignette : MonoBehaviour
{
    [SerializeField] private GameObject _ai;
    [SerializeField] private GameObject _player;
    [SerializeField] private Volume _volumeComponent;
    [SerializeField] private float _distanceToAi;

    private float _maxValue = 0.7f;
    private float _minValue = 0f;
    
    private Vignette _vignette;

    private void Start()
    {
        _volumeComponent.profile.TryGet(out _vignette);
    }

    private void Update()
    {
        if (_vignette == null || _player == null || _ai == null) return;
        
        float distance = Vector3.Distance(_player.transform.position, _ai.transform.position);

        float normalizedDistance = Mathf.InverseLerp(_distanceToAi, _minValue, distance); // 0 (far) to 1 (close)
        _vignette.intensity.value = Mathf.Lerp(0f, _maxValue, normalizedDistance);
    }
}
