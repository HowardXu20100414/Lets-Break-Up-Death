using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartLevelOnFall : MonoBehaviour
{
    public float fallThresholdY = -10f;

    void Update()
    {
        if (transform.position.y < fallThresholdY)
        {
            RestartLevel();
        }
    }

    void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
