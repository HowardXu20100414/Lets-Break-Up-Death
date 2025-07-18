using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public float timer;
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
    public string level3 = "3rdLevelChase";
    public void LoadScene(string name)
    {
        print(name);
        SceneManager.LoadScene(name);
    }

    void Update()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        if (currentSceneName != menuScene) // Exclude menu and cutscene
        {
            timer += Time.deltaTime;
        }
        if (currentSceneName == cutScene)
        {
            Time.timeScale = 1f;
        }
    }
}
