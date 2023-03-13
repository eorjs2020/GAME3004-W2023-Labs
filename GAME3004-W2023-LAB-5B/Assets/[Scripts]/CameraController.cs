using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float mouseSensitivity = 100.0f;
    public Transform playerBody;
    public Joystick rightStick;
    private float XRotation = 0.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        rightStick = GameObject.Find("RightStick").GetComponent<Joystick>();
        //Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        /* float mouseX = (Application.isMobilePlatform) ? rightStick.Horizontal * mouseSensitivity : Input.GetAxis("Mouse X") * mouseSensitivity ;
         float mouseY = (Application.isMobilePlatform) ? rightStick.Vertical * mouseSensitivity : Input.GetAxis("Mouse Y") * mouseSensitivity;*/
        float mouseX = rightStick.Horizontal * mouseSensitivity;
        float mouseY = rightStick.Vertical * mouseSensitivity;


         XRotation -= mouseY;
        XRotation = Mathf.Clamp(XRotation, -90.0f, 90.0f);

        transform.localRotation = Quaternion.Euler(XRotation, 0.0f, 0.0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
