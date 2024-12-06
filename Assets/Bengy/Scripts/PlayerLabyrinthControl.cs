using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerLabyrinthControl : MonoBehaviour
{
    public Camera cam;
    public Transform sphere;
    public float distanceFromCamera;
    private Rigidbody _r;

    void Start()
    {
        distanceFromCamera = Vector3.Distance(sphere.position,cam.transform.position);
        _r = sphere.GetComponent<Rigidbody>();
    }


    // Update is called once per frame
    void Update()
    {
        ProcessInputs();
    }

    private void ProcessInputs()
    { 
        if(Input.GetMouseButton(0))
        {
            Vector3 pos = Input.mousePosition;
            pos.z = distanceFromCamera;
            pos = cam.ScreenToWorldPoint(pos);
            _r.linearVelocity = (pos - sphere.position) * 10;
        }
    }
}
