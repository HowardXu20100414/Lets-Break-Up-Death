using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeEnd : MonoBehaviour
{

    public GameObject player;
    public GameObject carPrefab;
    public GameObject sceneReload;
    RestartLevelOnFall playerRestart;
    RestartLevelOnFall sceneReloader;
    KillPlayerOnHi killPlayer;
    PlayerFreeze playerFreeze;
    // Start is called before the first frame update
    void Start()
    {
        playerRestart = player.GetComponent<RestartLevelOnFall>();
        sceneReloader = sceneReload.GetComponent<RestartLevelOnFall>();
        killPlayer = carPrefab.GetComponent<KillPlayerOnHi>();
        playerFreeze = FindObjectOfType<PlayerFreeze>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            print("disabled scripts");
            playerRestart.enabled = false;
            sceneReloader.enabled = false;
            killPlayer.enabled = false;
            playerFreeze.Freeze();
        }
    }

}
