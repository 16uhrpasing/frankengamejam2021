using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 7)
        {
            if (collision.gameObject.GetComponent<Player_Movement>().currentDashTimer > 0)
            {
                collision.gameObject.GetComponent<Rigidbody2D>().velocity *= Vector2.up;
                collision.gameObject.GetComponent<Player_Movement>().currentDashTimer = 0;
                Destroy(gameObject);
            }
        }
    }

}
