using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarFallTrigger : MonoBehaviour
{
    public PillarFall pillar; // assign in Inspector

void OnTriggerEnter2D(Collider2D other)
{
    if (other.CompareTag("Player"))
    {
            print("hi");
        pillar.StartFall();
    }
}

}
