using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        //LoadScene("MainMenu");
    }

    public string menuScene = "MainMenu";
    public string levelScene = "Level";
    public string cutScene = "Cutscene";
    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }
}
