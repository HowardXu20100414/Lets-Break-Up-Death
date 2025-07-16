using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject player;

    public float verticalOffset = -2f;  // How much space below the player to show
    public float upperBound = 15f;
    public float lowerBound = 0f;

    void Update()
    {
        Vector3 newPos = transform.position;
        newPos.x = player.transform.position.x;

        float targetY = player.transform.position.y + verticalOffset;
        targetY = Mathf.Clamp(targetY, lowerBound, upperBound);
        newPos.y = targetY;

        transform.position = newPos;
    }
}
