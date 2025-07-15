using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public float speed;
    Animator anim;
    public string carType;
    void Start()
    {
        speed = Random.Range(6f, 12f);
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime);
        anim.Play(carType + "Drive");
    }
}
