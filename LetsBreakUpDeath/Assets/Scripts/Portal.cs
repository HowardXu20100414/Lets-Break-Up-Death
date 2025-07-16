using System.Collections;
using System.Collections.Generic;
// using JetBrains.Annotations; // Not strictly needed for Unity functionality, can remove.
using UnityEngine;

public class Portal : MonoBehaviour
{
    bool triggered = false; // Public for debugging, but typically private
    public bool startBobbing = false;

    public GameObject death;
    public float fadeDuration = 1.5f; // How long it takes for Death to fully fade in
    public float moveDuration = 2f; // How long it takes for Death to move up
    public float targetY = 9.3f; // The target Y position for Death
    public Animator anim; // Existing Animator for the Portal

    [Header("Camera Panning & Zoom")]
    public Camera mainCamera; // Assign your Main Camera here
    public GameObject player; // Assign your Player GameObject here
    public FollowPlayer followPlayerScript; // Assign the FollowPlayer script component from the Main Camera

    public float cameraPanDuration = 2f; // How long the camera takes to pan to Death
    public float cameraZoomOutTargetSize = 10f; // Target orthographic size for zoom out (to fit both)

    public Vector3 cameraOffset = new Vector3(0, 0, -10); // Offset from Death's position for the camera

    [Header("Camera Shake")]
    public float shakeDuration = 2f; // How long the camera shakes
    public float shakeMagnitude = 0.1f; // How much the camera shakes
    public float shakeRoughness = 0.05f; // How frequently the shake position updates

    private SpriteRenderer deathSpriteRenderer; // To control the fade
    private float initialCameraOrthographicSize = 7.5f; // Stores camera's initial zoom level (will be set dynamically)

    // --- NEW: Dialogue Data Reference ---
    [Header("Dialogue")]
    public DialogueData deathDialogue; // Assign the DialogueData asset for Death's dialogue here

    void Awake()
    {
        // Initialize the Portal's Animator if it exists
        anim = GetComponent<Animator>();
        if (anim != null)
        {
            anim.Play("PortalMovement");
        }

        // Get the SpriteRenderer component from the Death GameObject
        deathSpriteRenderer = death.GetComponent<SpriteRenderer>();

        // Ensure Death is initially inactive
        death.SetActive(false);

        // Set Death's initial position
        Transform deathTransform = death.GetComponent<Transform>();
        deathTransform.position = new Vector3(8.33f, -4.7f, 0f);

        // Set initial alpha to 0 for fading in
        Color color = deathSpriteRenderer.color;
        color.a = 0f;
        deathSpriteRenderer.color = color;

        // If camera is not assigned, try to find the Main Camera
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        // Get the FollowPlayer script from the main camera
        if (mainCamera != null && followPlayerScript == null)
        {
            followPlayerScript = mainCamera.GetComponent<FollowPlayer>();
        }

        // Store the camera's initial orthographic size for zooming
        //if (mainCamera != null && mainCamera.orthographic)
        //{
        //    initialCameraOrthographicSize = mainCamera.orthographicSize;
        //}
        //else if (mainCamera != null && !mainCamera.orthographic)
        //{
        //    Debug.LogWarning("Camera is not orthographic. Zooming will not work as expected.");
        //}
    }

    void Update()
    {
        // No update logic needed for this animation
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the colliding object is the player AND if the animation hasn't been triggered yet
        if (collision.gameObject == player && !triggered)
        {
            triggered = true; // Set triggered to true to prevent re-triggering

            // Get PlayerController and set floating state, passing portal's position
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
            if (playerController != null)
            {
                // Pass the portal's current position as the center for the player to move to
                playerController.SetFloatingInPortal(true, transform.position);
            }

            // Stop any existing coroutines on this script to prevent conflicts
            StopAllCoroutines();

            // Disable the FollowPlayer script BEFORE starting our custom camera animation
            if (followPlayerScript != null)
            {
                followPlayerScript.enabled = false;
            }

            StartCoroutine(DeathAppearAndMoveAnimation());
            // Start camera shake concurrently with the main animation
            StartCoroutine(CameraShake(shakeDuration, shakeMagnitude, shakeRoughness));
        }
    }

    IEnumerator DeathAppearAndMoveAnimation()
    {
        death.SetActive(true); // Make Death visible

        // Store initial states for Death
        Color startColor = deathSpriteRenderer.color; // Should be alpha 0
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 1f); // Fully opaque
        Vector3 deathStartPosition = death.transform.position;
        Vector3 deathEndPosition = new Vector3(deathStartPosition.x, targetY, deathStartPosition.z);

        // Store camera initial and target positions/zoom
        Vector3 cameraStartPosition = mainCamera.transform.position;

        // Corrected declaration for midPointX
        float midPointX = (player.transform.position.x + deathEndPosition.x) / 2f;
        Vector3 cameraTargetPosition = new Vector3(midPointX, deathEndPosition.y, cameraOffset.z);

        // Determine the overall duration for the concurrent animation
        float overallAnimationDuration = Mathf.Max(fadeDuration, moveDuration, cameraPanDuration, shakeDuration);
        float timer = 0f;

        while (timer < overallAnimationDuration)
        {
            timer += Time.deltaTime;

            // --- Death Fade ---
            float fadeProgress = Mathf.Clamp01(timer / fadeDuration);
            deathSpriteRenderer.color = Color.Lerp(startColor, endColor, fadeProgress);

            // --- Death Movement ---
            float deathMoveProgress = Mathf.Clamp01(timer / moveDuration);
            death.transform.position = Vector3.Lerp(deathStartPosition, deathEndPosition, deathMoveProgress);

            // --- Camera Pan to Death & Zoom Out ---
            float cameraProgress = Mathf.Clamp01(timer / cameraPanDuration);
            mainCamera.transform.position = Vector3.Lerp(cameraStartPosition, cameraTargetPosition, cameraProgress);

            if (mainCamera.orthographic)
            {
                // Smoothly interpolate from the camera's *actual* initial size to the target zoom out size
                mainCamera.orthographicSize = Mathf.Lerp(initialCameraOrthographicSize, cameraZoomOutTargetSize, cameraProgress);
            }

            yield return null; // Wait for the next frame
        }

        // Ensure final states are exactly reached after the loop
        deathSpriteRenderer.color = endColor;
        death.transform.position = deathEndPosition;
        mainCamera.transform.position = cameraTargetPosition;
        if (mainCamera.orthographic)
        {
            mainCamera.orthographicSize = cameraZoomOutTargetSize; // Keep zoomed out
        }

        // --- Important: Re-enable the FollowPlayer script here! ---
        //if (followPlayerScript != null)
        //{
        //    followPlayerScript.enabled = true;
        //    // The FollowPlayer script will now take over and ensure the camera
        //    // stays on the player's X position, while our custom zoom-out
        //    // ensures both are in view. You might need to adjust bounds in FollowPlayer
        //    // for the new zoom level.
        //}

        startBobbing = true; // Death is now in its final position and ready to bob

        // --- NEW: Trigger Dialogue ---
        if (DialogueManager.Instance != null && deathDialogue != null)
        {
            DialogueManager.Instance.StartDialogue(deathDialogue);
        }
        else
        {
            if (DialogueManager.Instance == null) Debug.LogWarning("DialogueManager.Instance is null. Make sure DialogueManager is in the scene.");
            if (deathDialogue == null) Debug.LogWarning("Death Dialogue Data is not assigned in the Portal script.");
        }
    }

    // Coroutine for Camera Shake
    IEnumerator CameraShake(float duration, float magnitude, float roughness)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            // Get the camera's current position (which might be panning)
            Vector3 currentCameraPos = mainCamera.transform.position;

            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            // Apply shake offset relative to the current panning position
            // Ensure Z is not affected by shake for 2D
            mainCamera.transform.position = new Vector3(currentCameraPos.x + x, currentCameraPos.y + y, currentCameraPos.z);

            yield return new WaitForSeconds(roughness);

            elapsed += roughness; // Increment elapsed time by the wait duration
        }
    }
}