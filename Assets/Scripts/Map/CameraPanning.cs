using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPanning : MonoBehaviour
{
    private GameManager GM;
    float minFov = 50f;
    float maxFov = 90f;
    float sensitivity = 20f;
    Vector3 LastPosition;

    Vector3 MapSizeInMeters;

    float MouseSensitivity = 2f;
    float KeySensitivity = 1f;

    void Start()
    {
        GM = FindObjectOfType<GameManager>();
    }

    public void Initialize()
    {
        MapSizeInMeters = GM.MapGenerator.GetMapSizeInMeters();

        Debug.Log("MapSizeInMeters: " + MapSizeInMeters);
    }

    void Update()
    {
        if(!GM.IsGameStarted || GM.IsInUI)
            return;

        float fov = Camera.main.fieldOfView;
        fov -= Input.GetAxis("Mouse ScrollWheel") * sensitivity;
        fov = Mathf.Clamp(fov, minFov, maxFov);
        Camera.main.fieldOfView = fov;

        if(Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
        {
            LastPosition = transform.position;
        }

        if(!Input.GetMouseButton(1) && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.D))
            return;

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
            cameraPositionX = 0f;

        if(cameraPositionY < -5f)
            cameraPositionY = -5f;

        if(cameraPositionX > MapSizeInMeters.x)
            cameraPositionX = MapSizeInMeters.x;

        if(cameraPositionY > MapSizeInMeters.y - 100f)
            cameraPositionY = MapSizeInMeters.y - 100f;

        transform.position = new Vector3(cameraPositionX, 50f, cameraPositionY);
        LastPosition = transform.position;
    }
}
