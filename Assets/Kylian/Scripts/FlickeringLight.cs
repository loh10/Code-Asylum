using System.Collections;
using UnityEngine;

public class FlickeringLight : MonoBehaviour
{
    public GameObject[] flickeringLights;

    Light _light;
    public float minFlickerTime = 0.1f;
    public float maxFlickerTime = 0.4f;

    void Start()
    {
        flickeringLights = GameObject.FindGameObjectsWithTag("FlickeringLight");
        for (int i = 0; i < flickeringLights.Length; i++)
        {
            _light = flickeringLights[i].GetComponent<Light>();
            StartCoroutine(Flicker(_light));
        }
    }
    IEnumerator Flicker(Light _light)
    {
        while(true)
        {
            yield return new WaitForSeconds(Random.Range(minFlickerTime, maxFlickerTime));
            _light.enabled = !_light.enabled;
        }

        
    }
}