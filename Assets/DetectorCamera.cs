using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectorCamera : MonoBehaviour
{
    public Camera targetCamera; // The camera that should render the specific GameObject
    public string layerName = "RenderOnCamera"; // The layer name to use

    private int originalLayer; // Store the original layer of the GameObject

    public void ShowGameObjectOnCamera(GameObject obj)
    {
        // Store the original layer of the GameObject
        originalLayer = obj.layer;
        
        // Set the GameObject to the new layer
        obj.layer = LayerMask.NameToLayer(layerName);
        
        // Enable the camera
        targetCamera.enabled = true;
    }

    public void ResetGameObjectLayer(GameObject obj)
    {
        // Reset the GameObject back to its original layer
        obj.layer = originalLayer;
    }
}
