using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public float sensitivityRotation;
    public float movementSpeed;

    private Vector3 mousePos;

    public List<Transform> bodyList = new List<Transform>();

    private void Start()
    {
        myCam = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }

    private void FixedUpdate()
    {
        RotateToMouse();
        Movement();
        CameraFollow();
        BodyMovement();

        if(Input.GetKey(KeyCode.F))
            AddBodyPart();
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

    public GameObject bodyPrefab;
    private void AddBodyPart()
    {
        if(bodyList.Count != 0)
        {
            GameObject newBody = Instantiate(bodyPrefab, bodyList[bodyList.Count - 1].position, bodyList[bodyList.Count - 1].rotation);
            bodyList.Add(newBody.transform);
        }
        else
        {
            GameObject newBody = Instantiate(bodyPrefab, transform.position, transform.rotation);
            bodyList.Add(newBody.transform);
        }
    }

    [Range(0f, 1f)]
    public float bodyDistance;
    private Vector3 bodyVelocity;
    private void BodyMovement()
    {
        for(int i = 0; i < bodyList.Count; i++)
        {
            bodyVelocity = Vector3.zero;
            if(i != 0)
            {
                bodyList[i].position = Vector3.SmoothDamp(bodyList[i].position, bodyList[i - 1].position, ref bodyVelocity, bodyDistance);
                bodyList[i].up = new Vector2(bodyList[i - 1].position.x - bodyList[i].position.x, bodyList[i - 1].position.y - bodyList[i].position.y);
            }
            else
            {
                bodyList[i].position = Vector3.SmoothDamp(bodyList[i].position, transform.position, ref bodyVelocity, bodyDistance);
                bodyList[i].up = new Vector2(transform.position.x - bodyList[i].position.x, transform.position.y - bodyList[i].position.y);
            }
        }
    }
}
