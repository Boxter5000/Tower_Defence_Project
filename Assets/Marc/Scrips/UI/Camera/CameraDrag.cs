using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDrag : MonoBehaviour
{
    public float zoomSpeed = 1;
    public float targetOrtho;
    public float smoothSpeed = 2.0f;
    public float minOrtho = 1.0f;
    public float maxOrtho = 20.0f;
    [SerializeField] private LayerMask inputLayerMask;

    private Vector3 ResetCamera;
    private Vector3 Origin;
    private Vector3 Diference;
    private bool Drag = false;
    private Camera camera;
    void Start()
    {
        camera = Camera.main;
        ResetCamera = camera.transform.position;
        targetOrtho = camera.orthographicSize;
        camera.eventMask = inputLayerMask;
    }
    private void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0.0f)
        {
            targetOrtho -= scroll * zoomSpeed;
            targetOrtho = Mathf.Clamp(targetOrtho, minOrtho, maxOrtho);
        }

        camera.orthographicSize = Mathf.MoveTowards(camera.orthographicSize, targetOrtho, smoothSpeed * Time.deltaTime);
    }
    void LateUpdate()
    {
        if (Input.GetMouseButton(1))
        {
            Diference = (camera.ScreenToWorldPoint(Input.mousePosition)) - camera.transform.position;
            if (Drag == false)
            {
                Drag = true;
                Origin = camera.ScreenToWorldPoint(Input.mousePosition);
            }
        }
        else
        {
            Drag = false;
        }
        if (Drag == true)
        {
            camera.transform.position = Origin - Diference;
        }
        //RESET CAMERA TO STARTING POSITION WITH RIGHT CLICK
        if (Input.GetMouseButton(2))
        {
            camera.transform.position = ResetCamera;
        }
    }
}