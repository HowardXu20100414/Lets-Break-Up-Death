using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateDeath : MonoBehaviour
{
    public Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (DialogueManager.Instance.currentLineIndex == 1)
        {
            anim.Play("Idle");
        }
        if (DialogueManager.Instance.currentLineIndex == 2)
        {
            anim.Play("Talking");
        }
        if (DialogueManager.Instance.currentLineIndex == 3)
        {
            anim.Play("Angry");
        }
        if (DialogueManager.Instance.isEnded)
        {
            GameManager.instance.LoadScene(GameManager.instance.levelScene);
        }
    }
}
