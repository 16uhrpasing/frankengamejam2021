using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour
{
    [SerializeField] private GameEvent respawnEvent;
    private float step = 1;
    private bool sink = false;
    Vector2 newpos = new Vector2(0, -24.81f);

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 7)
        {
            respawnEvent.Raise();
        }
    }

    public void SinkLava()
    {
        
        sink = true;
    }

    private void Update()
    {
        float step = 1* Time.deltaTime;
        if (sink)
        transform.position = Vector2.MoveTowards(transform.position, newpos, step);
    }
}
