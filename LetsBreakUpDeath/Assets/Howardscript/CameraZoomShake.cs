using UnityEngine;
using System.Collections;

public class CameraZoomShake : MonoBehaviour
{
    public static CameraZoomShake instance;

    private Camera cam;
    private float originalSize;
    private Coroutine currentShake;

    [Header("Zoom Shake Settings")]
    public float zoomOutAmount = 3f;     // How far to zoom out
    public float zoomDuration = 0.6f;    // How long total zoom effect lasts

    void Awake()
    {
        instance = this;
        cam = GetComponent<Camera>();
        originalSize = cam.orthographicSize;
    }

    public void TriggerZoomShake()
    {
        if (currentShake != null)
            StopCoroutine(currentShake);

        currentShake = StartCoroutine(ZoomRoutine(zoomOutAmount, zoomDuration));
    }

    IEnumerator ZoomRoutine(float intensity, float duration)
    {
        float targetSize = originalSize + intensity;
        float timer = 0f;

        // Zoom Out
        while (timer < duration / 2f)
        {
            cam.orthographicSize = Mathf.Lerp(originalSize, targetSize, timer / (duration / 2f));
            timer += Time.deltaTime;
            yield return null;
        }

        // Zoom Back In
        timer = 0f;
        while (timer < duration / 2f)
        {
            cam.orthographicSize = Mathf.Lerp(targetSize, originalSize, timer / (duration / 2f));
            timer += Time.deltaTime;
            yield return null;
        }

        cam.orthographicSize = originalSize;
        currentShake = null;
    }
}
