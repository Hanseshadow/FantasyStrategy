using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class CameraMapMovement : MonoBehaviour
{
    private GameManager GM;
    float MinFov = 50f;
    float MaxFov = 75f;
    float Sensitivity = 20f;
    Vector3 LastPosition;
    public Vector3 MinimumZoom = new Vector3(0f, 50f, 0f);
    public Vector3 MaximumZoom = new Vector3(0f, 100f, 0f);

    Vector3 MapSizeInMeters;

    float MouseSensitivity = 2f;
    float KeySensitivity = 1f;

    RaycastHit Hit;
    Transform MainCameraTransform;

    void Start()
    {
        GM = FindObjectOfType<GameManager>();
    }

    public void Initialize()
    {
        MapSizeInMeters = GM.MapGenerator.GetMapSizeInMeters();

        Debug.Log("MapSizeInMeters: " + MapSizeInMeters);

        MainCameraTransform = Camera.main.transform;

        CameraUpdate();
    }

    void Update()
    {
        if(!GM.IsGameStarted || GM.IsInUI)
            return;

        float fov = Camera.main.fieldOfView;
        fov -= Input.GetAxis("Mouse ScrollWheel") * Sensitivity;
        fov = Mathf.Clamp(fov, MinFov, MaxFov);
        Camera.main.fieldOfView = fov;

        // If you don't press a key to pan, then don't do anything.
        if(!Input.GetMouseButton(1) && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.D))
            return;

        if(Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
        {
            LastPosition = transform.position;
        }

        float cameraPositionX = LastPosition.x;
        float cameraPositionY = LastPosition.z;

        if(Input.GetKey(KeyCode.W))
        {
            cameraPositionY += KeySensitivity;
        }

        if(Input.GetKey(KeyCode.A))
        {
            cameraPositionX -= KeySensitivity;
        }

        if(Input.GetKey(KeyCode.S))
        {
            cameraPositionY -= KeySensitivity;
        }

        if(Input.GetKey(KeyCode.D))
        {
            cameraPositionX += KeySensitivity;
        }

        if(Input.GetMouseButton(1))
        {
            cameraPositionX = LastPosition.x - (Input.GetAxis("Mouse X") * MouseSensitivity);
            cameraPositionY = LastPosition.z - (Input.GetAxis("Mouse Y") * MouseSensitivity);
        }

        if(cameraPositionX < 0f)
            cameraPositionX = MapSizeInMeters.x;

        if(cameraPositionY < -5f)
            cameraPositionY = -5f;

        if(cameraPositionX > MapSizeInMeters.x)
            cameraPositionX = 0;

        if(cameraPositionY > MapSizeInMeters.y - 50f)
            cameraPositionY = MapSizeInMeters.y - 50f;

        transform.position = new Vector3(cameraPositionX, 50f, cameraPositionY);
        LastPosition = transform.position;

        CameraUpdate();
    }

    public void CameraUpdate()
    {
        Physics.Raycast(MainCameraTransform.position, MainCameraTransform.forward, out Hit, 100.0f);

        if(Hit.collider != null && Vector3.Distance(Hit.point, Camera.main.transform.position) > 50f)
        {
            GM.CameraUpdate(Hit.point);
        }
    }
}
