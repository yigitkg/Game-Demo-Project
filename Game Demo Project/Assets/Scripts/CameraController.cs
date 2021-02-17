using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //Variables
    [SerializeField] private float mouseSensitivity;

    //References
    private Transform parent;

    private void Start()
    {
        parent = transform.parent;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate()
    {
        Rotate();
    }

    private void Rotate()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        //Rotate around Y axis
        parent.Rotate(Vector3.up, mouseX);
    }

}
