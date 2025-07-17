using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TruckTrigger : MonoBehaviour
{
    public GameObject mainCamera;
    Camera cameraComponent;

    [Header("Zoom Settings")]
    public float targetZoomOutSize = 10f; // The orthographic size for "zoomed out" state
    public float targetZoomInSize = 5f;    // The orthographic size for "zoomed in" state
    public float zoomSpeed = 2f;           // How fast the camera zooms (higher value = faster zoom)

    [Tooltip("If checked, the camera will zoom IN (to targetZoomInSize) when triggered. If unchecked, it will zoom OUT (to targetZoomOutSize).")]
    public bool zoomIn = false; // Control the zoom direction via Inspector

    // Start is called before the first frame update
    void Start()
    {
        // Get the Camera component from the mainCamera GameObject
        if (mainCamera != null)
        {
            cameraComponent = mainCamera.GetComponent<Camera>();
            if (cameraComponent == null)
            {
                Debug.LogError("Main Camera GameObject does not have a Camera component assigned!", mainCamera);
            }
        }
        else
        {
            Debug.LogError("Main Camera GameObject is not assigned in the Inspector for TruckTrigger!");
        }
    }

    // This method is called when another Collider2D enters this trigger's Collider2D
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the colliding object has the "Player" tag
        if (collision.CompareTag("Player"))
        {
            StopAllCoroutines(); // Stop any existing zoom coroutine

            if (zoomIn)
            {
                Debug.Log("Player entered trigger! Zooming IN...");
                // Start a zoom from the current size towards the 'zoomed in' target
                StartCoroutine(SmoothZoomCoroutine(targetZoomInSize));
            }
            else
            {
                Debug.Log("Player entered trigger! Zooming OUT...");
                // Start a zoom from the current size towards the 'zoomed out' target
                StartCoroutine(SmoothZoomCoroutine(targetZoomOutSize));
            }
        }
    }

    /// <summary>
    /// Coroutine to smoothly change the camera's orthographic size to a target size.
    /// </summary>
    /// <param name="targetSize">The final orthographic size the camera should reach.</param>
    IEnumerator SmoothZoomCoroutine(float targetSize)
    {
        if (cameraComponent == null)
        {
            Debug.LogError("Cannot zoom: Camera component is not assigned or found.");
            yield break;
        }

        float startZoomSize = cameraComponent.orthographicSize;
        float timer = 0f;

        // Loop until the camera is very close to the target size or max duration is reached
        while (Mathf.Abs(cameraComponent.orthographicSize - targetSize) > 0.01f && timer < 1f) // Changed max timer to 1f, as zoomSpeed handles the actual duration
        {
            timer += Time.deltaTime * zoomSpeed;
            cameraComponent.orthographicSize = Mathf.Lerp(startZoomSize, targetSize, timer);
            yield return null;
        }

        // Ensure the camera hits the exact target size at the end
        cameraComponent.orthographicSize = targetSize;
        Debug.Log("Camera zoom completed to size: " + targetSize);
    }
}