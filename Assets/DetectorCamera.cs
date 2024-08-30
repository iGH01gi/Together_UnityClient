using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[ExecuteInEditMode]
public class DetectorCamera : MonoBehaviour
{
    public Camera targetCamera;
    public Transform mainCamera;
    public Shader replacementShader; // The replacement shader that will render the specific GameObject

    private int originalLayer; // Store the original layer of the GameObject
    public bool _isDetecting = false;

    void OnEnable()
    {
        if (!Managers.Player.IsMyDediPlayerKiller())
        {
            enabled = false;
        }
        mainCamera = GameObject.Find("Main Camera").transform;
        if (targetCamera == null)
        {
            targetCamera = transform.GetComponent<Camera>();
        }
        if (replacementShader == null)
        {
            Debug.LogError("Replacement shader not assigned! Please assign a shader.");
            return;
        }
        targetCamera.SetReplacementShader(replacementShader, "");
    }

    void Update()
    {
        if (_isDetecting)
        {
            Quaternion relativeRotation = mainCamera.rotation * Quaternion.Inverse(transform.rotation);
            transform.rotation = relativeRotation;
            Vector3 relativePosition = mainCamera.position - transform.position;
            transform.position += relativePosition;
        }
        else
        {
            transform.rotation = mainCamera.rotation;
            transform.position = mainCamera.position;
        }
    }
    
    void OnDisable()
    {
        // Reset the camera's shader replacement when the script is disabled
        if (targetCamera != null)
        {
            targetCamera.ResetReplacementShader();
        }
    }
}
