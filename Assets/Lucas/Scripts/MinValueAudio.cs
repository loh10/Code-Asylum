using UnityEngine;
using UnityEngine.UI;

public class MinValueSlider : MonoBehaviour
{
    private Slider slider;
    private void Start()
    {
        slider = GetComponent<Slider>();
    }

    private void Update()
    {
        if (Mathf.Approximately(slider.value, -30))
        {
            slider.minValue = -80;
            slider.value = slider.minValue;
        }
        else if (Mathf.Approximately(slider.minValue, -80) && !Mathf.Approximately(slider.value, -80))
        {
            slider.minValue = -30;
            slider.value = slider.minValue + 1;
        }
    }
}
