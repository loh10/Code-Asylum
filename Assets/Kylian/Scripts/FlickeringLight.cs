using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class FlickeringLight : MonoBehaviour
{
    private Light _flickeringLights;

    Light _light;
    public float minFlickerTime = 0.1f;
    public float maxFlickerTime = 0.4f;

    void Start()
    {
        _flickeringLights = GetComponent<Light>();
        _light = _flickeringLights.GetComponent<Light>();
        StartCoroutine(Flicker(_light));
    }

    IEnumerator Flicker(Light _light)
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minFlickerTime, maxFlickerTime));
            _light.enabled = !_light.enabled;
        }
    }
}