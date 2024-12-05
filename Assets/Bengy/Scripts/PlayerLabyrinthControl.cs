using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLabyrinthControl : MonoBehaviour
{
    public Rigidbody rb;
    public float moveSpeed = 10f;
    private float _xInput;
    private float _zInput;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInputs();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void ProcessInputs()
    { 
        if(Input.GetMouseButton(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, 3))

            {

                // Obtenir la position de la souris dans l'espace écran
                Vector3 mousePosition = Input.mousePosition;

                // Convertir la position de la souris en coordonnées du monde
                mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, Camera.main.transform.position.y));

                // Déplacer la boule vers la position de la souris
                transform.position = new Vector3(mousePosition.x, 3, mousePosition.z);
            }
        }
    }


    private void Move()
    {
        rb.AddForce(new Vector3(_xInput, 0f, _zInput) * moveSpeed);
    }
}
