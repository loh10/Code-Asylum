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
    Rigidbody r;

    void Start()
    {
        distanceFromCamera = Vector3.Distance(sphere.position,cam.transform.position);
        r = sphere.GetComponent<Rigidbody>();
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
            r.linearVelocity = (pos - sphere.position) * 10;
        }
    }
}
