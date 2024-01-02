using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    //minView to limit amount of Y-axis view. Probably should be moved elsewhere later
    public float minViewDistance = 15f;
    
    //this is to be fetched from player settings
    public float mouseSensitivity = 100f;
    
    private Vector3 _playerPos;
    private float rotationX = 0f;
    
    private void Start()
    {
        
    }

    private void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        
        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -70f, minViewDistance);
        
        transform.GetChild(0).transform.localRotation = Quaternion.Euler(rotationX,0f,0f);
        transform.Rotate(3f*mouseX*Vector3.up);
    }
    
    /*
     _playerPos = transform.parent.GetChild(1).transform.position;
     .
     .
     .
     Using RotateAround
     transform.RotateAround(_playerPos,Vector3.up, mouseX);
        Vector3 newPos = new Vector3(_playerPos.x-transform.position.x,transform.position.y,_playerPos.z-transform.position.z);
        Vector3 q = Quaternion.LookRotation(newPos).eulerAngles;
        transform.rotation = Quaternion.Euler(rotationX, q.y, 0);
     */
}
