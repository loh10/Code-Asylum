using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifferenceNumber : MonoBehaviour
{
    [HideInInspector] public int numberOfDifferences;
    public List<Button> listButtons = new List<Button>();

    private void Awake()
    {
        numberOfDifferences = listButtons.Count;
    }
}
