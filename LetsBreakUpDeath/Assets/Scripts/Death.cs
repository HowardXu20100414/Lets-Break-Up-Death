using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : MonoBehaviour
{
    private float bobHeight = 0.03f; // How much the player bobs up and down
    private float bobSpeed = 3f;    // How fast the player bobs
    Rigidbody2D rb;
    public GameObject portal;
    Portal portalScript;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        portalScript = portal.GetComponent<Portal>();
    }

    // Update is called once per frame
    void Update()
    {
        if (portalScript.startBobbing)
        {
            Bob();
        }

    }

    public void Bob()
    {
        float bobbingOffset = Mathf.Sin(Time.time * bobSpeed) * bobHeight;
        // The player's X position remains fixed at the portal's center X
        // The player's Y position bobs around the portal's center Y
        rb.position = new Vector2(transform.position.x, transform.position.y + bobbingOffset);

        rb.velocity = Vector2.zero;
    }
}
