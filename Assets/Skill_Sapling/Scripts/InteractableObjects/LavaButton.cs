using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaButton : MonoBehaviour
{
    [SerializeField] GameEvent buttonpressed;
    [SerializeField] LayerMask player;
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (anim.GetBool("playerNear"))
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                Debug.Log("PRESSED");
                buttonpressed.Raise();
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        anim.SetBool("playerNear", true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        anim.SetBool("playerNear", false);
    }
    }
