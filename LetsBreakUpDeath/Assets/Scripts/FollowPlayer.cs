using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject player;
    public float upperBound = 15f;
    public float lowerBound = 0f;
    public float pushUp; // how far to push the camera up
    public float divideBy = 3; // what do divide the camera y by
    void Update()
    {
        transform.position = new Vector3(player.transform.position.x, GetCameraY(), transform.position.z);
    }


    float GetCameraY()
    {
        float cameraModified = player.transform.position.y / divideBy + pushUp;

        if (cameraModified <= lowerBound)
        {
            return(lowerBound);
        }
        else if (cameraModified >= upperBound)
        {
            return (upperBound);
        }
        else
        {
            return (cameraModified);
        }
    }
}
