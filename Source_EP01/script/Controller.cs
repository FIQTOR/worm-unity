using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public float sensitivityRotation;
    public float movementSpeed;

    private Vector3 mousePos;

    private void Start()
    {
        myCam = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }

    private void FixedUpdate()
    {
        RotateToMouse();
        Movement();
        CameraFollow();
    }

    private void RotateToMouse()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Quaternion direction = Quaternion.LookRotation(Vector3.forward, mousePos - transform.position);

        transform.rotation = Quaternion.Lerp(transform.rotation, direction, sensitivityRotation * Time.deltaTime);
    }

    private void Movement()
    {
        transform.position += transform.up * movementSpeed * Time.deltaTime;
    }

    private Transform myCam;
    [Range(0f, 1f)]
    public float cameraFollowSpeed = 0.5f;
    private void CameraFollow()
    {
        Vector3 velocity = Vector3.zero;
        myCam.position = Vector3.SmoothDamp(myCam.position,new Vector3(transform.position.x, transform.position.y, -10), ref velocity, cameraFollowSpeed);
    }
}
