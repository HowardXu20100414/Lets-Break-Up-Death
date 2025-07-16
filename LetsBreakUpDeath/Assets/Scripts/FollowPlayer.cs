using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject player;
    public GameObject portal;

    public float verticalOffset = -2f;  // How much space below the player to show
    public float upperBound = 15f;
    public float lowerBound = 0f;
    public float leftBound = -1000;
    public float rightBound = 1000;


    void Update()
    {
        Vector3 newPos = transform.position;

        float targetX = player.transform.position.x;
        float targetY = player.transform.position.y + verticalOffset;
        targetX = Mathf.Clamp(targetX, leftBound, rightBound);
        targetY = Mathf.Clamp(targetY, lowerBound, upperBound);
        newPos.x = targetX;
        newPos.y = targetY;

        transform.position = newPos;
    }
}
