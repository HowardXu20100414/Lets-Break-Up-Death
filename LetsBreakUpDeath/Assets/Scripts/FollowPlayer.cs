using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject player;
    float upperBound = 7.8f;
    float lowerBound = 0f;
    void Update()
    {
        transform.position = new Vector3(player.transform.position.x, GetCameraY(), transform.position.z);
    }


    float GetCameraY()
    {
        float cameraModified = player.transform.position.y / 3 + 2;

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
