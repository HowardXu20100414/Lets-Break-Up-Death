using UnityEngine;
// No need for System.Collections and System.Collections.Generic unless you use them elsewhere

public class ButtonAction : MonoBehaviour
{
    // No need for initialTime or startTime here if GameManager handles the timing

    public void Button()
    {
        // Option 1: Log or use the current timer value from GameManager
        //Debug.Log("Time taken for this level: " + GameManager.instance.timer.ToString());

        // Option 2: If this button signifies the end of a timed segment
        // and you want to reset the timer for the *next* segment/scene:
        GameManager.instance.timer = 0f; // Reset the timer here for the next scene

        // Load the next scene
        GameManager.instance.LoadScene(GameManager.instance.cutScene);
    }
}