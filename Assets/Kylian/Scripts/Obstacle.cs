using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3;
    public GameObject dino;
    [SerializeField]
    private int _offset = 2;

    private Transform _transform;

    private void Start()
    {
        _transform = transform;
    }
    
    private void Update()
    {
        _transform.position += Vector3.left * (_speed * Time.deltaTime);
        if (dino.transform.position.x - _offset >= _transform.position.x)
        {
            Destroy(gameObject);
        }                                 // If it exceeds the X position of the player, it destroys itself.
    }
}
