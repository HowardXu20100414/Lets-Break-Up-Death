using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTrigger : MonoBehaviour
{
    public GameObject mainCamera;
    // We don't need to declare cameraComponent here, it's better to get it once in Awake or Start
    bool triggered = false;

    public float zoomTarget = 6.5f; // The target orthographic size
    public float zoomDuration = 1.0f; // How long the zoom takes (in seconds)

    private Camera cameraComponentRef; // A reference to the Camera component

    // Start is called before the first frame update
    void Awake() // Use Awake to get component reference before OnTriggerEnter2D might be called
    {
        if (mainCamera != null)
        {
            cameraComponentRef = mainCamera.GetComponent<Camera>();
            if (cameraComponentRef == null)
            {
                Debug.LogError("Main Camera GameObject does not have a Camera component!", this);
            }
        }
        else
        {
            Debug.LogError("mainCamera GameObject is not assigned in the Inspector!", this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // No update logic needed for this script
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !triggered) // Use CompareTag for efficiency
        {
            if (cameraComponentRef != null && cameraComponentRef.orthographic)
            {
                triggered = true;
                // Start the coroutine for smooth zooming
                StartCoroutine(SmoothZoom(cameraComponentRef.orthographicSize, zoomTarget, zoomDuration));
            }
            else if (cameraComponentRef != null && !cameraComponentRef.orthographic)
            {
                Debug.LogWarning("Camera is not orthographic. Smooth zoom will not work as expected.", this);
            }
        }
    }

    // Coroutine for smooth zooming
    IEnumerator SmoothZoom(float startSize, float endSize, float duration)
    {
        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float progress = timer / duration;
            cameraComponentRef.orthographicSize = Mathf.Lerp(startSize, endSize, progress);
            yield return null; // Wait for the next frame
        }
        // Ensure the camera reaches the exact target size at the end
        cameraComponentRef.orthographicSize = endSize;
    }
}