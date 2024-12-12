using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3;
    public GameObject dino;
    [SerializeField]
    private int _offset = 2;

    
    void Update()
    {
        transform.position += Vector3.left * _speed * Time.deltaTime;
        if (dino.transform.position.x - _offset >= transform.position.x)
        {
            Destroy(gameObject);
            
        }                                 // si il dépasse le x du joueur, il se détruit 
    }
}
