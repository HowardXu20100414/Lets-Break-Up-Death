using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class End : MonoBehaviour
{
    public GameObject EndText;
    public TMP_Text EndTextText;
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        anim.Play("End");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            EndText.SetActive(true);
            EndTextText.text = "You Won! Time: " + Time.time;
            
            Time.timeScale = 0;
        }
    }
}
