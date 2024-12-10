using Unity.VisualScripting;
using UnityEngine;

public class Dino : MonoBehaviour
{
    private bool _isUp;
    [SerializeField]
    private float speed = 1.0f;
    [SerializeField]
    private Transform _targetUp, _targetDown;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            
        }
    }

    private void ChangePosition()
    {
        if (_isUp)
        {
            transform.position = Vector3.MoveTowards(_targetUp.position, _targetDown.position, speed * Time.deltaTime);
        }
    }
}
