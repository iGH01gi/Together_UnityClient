using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class DetectorCamera : MonoBehaviour
{
    public Camera targetCamera; // The camera that should render the specific GameObject
    public Shader replacementShader; // The replacement shader that will render the specific GameObject

    private int originalLayer; // Store the original layer of the GameObject

    private void Start()
    {
        if (targetCamera == null){
            targetCamera = transform.GetComponent<Camera>();
        }
        if (replacementShader == null)
        {
            Debug.LogError("Replacement shader not assigned! Please assign a shader.");
            return;
        }
        targetCamera.SetReplacementShader(replacementShader, "");
    }
    
    void OnDisable()
    {
        // Reset the camera's shader replacement when the script is disabled
        if (targetCamera != null)
            targetCamera.ResetReplacementShader();
    }
}
