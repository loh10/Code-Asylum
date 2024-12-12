using System.Collections;
using UnityEngine;

public class FlashingLight : MonoBehaviour
{
    private Light _light;
    [SerializeField, Range(1, 10)] private float _flashingLightSpeed = 1.0f;
    [SerializeField, Range(0, 3000)] private float _flashingLightMaxIntensity =  1500;
    [SerializeField, Range(0, 3000)] private float _flashingLightMinIntensity= 1000;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _light = GetComponent<Light>();
        StartCoroutine(StartFlashing());
    }
    IEnumerator StartFlashing()
    {
        yield return StartCoroutine(ChangeIntensity(_flashingLightMinIntensity, _flashingLightMaxIntensity));
        yield return StartCoroutine(ChangeIntensity(_flashingLightMaxIntensity, _flashingLightMinIntensity));
        StartCoroutine(StartFlashing());
    }

    IEnumerator ChangeIntensity(float fromIntensity, float toIntensity)
    {
        float elapsedTime = 0f;
        while (elapsedTime < _flashingLightSpeed)
        {
            _light.intensity = Mathf.Lerp(fromIntensity, toIntensity, elapsedTime / _flashingLightSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        _light.intensity = toIntensity;
    }
}