using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollectDirt : MonoBehaviour
{
    private bool blockShowed = false;
    [SerializeField] private GameObject light, text;
    private void Start()
    {
        //ShowBlock();
    }

    public void ShowBlock()
    {
        GetComponent<SpriteRenderer>().color = new Vector4(1, 1, 1, 1);
        blockShowed = true;
        light.SetActive(true);
    }

    private void Update()
    {
        if (blockShowed)
        {
            Collider2D[] playerNear = Physics2D.OverlapCircleAll(transform.position, 2f);
            bool near = false;
            foreach (Collider2D coll in playerNear)
            {
                if (coll.gameObject.layer == 7)
                {
                    near = true;
                    text.SetActive(true);
                }
            }

            if (!near)
            {
                text.SetActive(false);
            }
            else
            {
                if (Input.GetKeyDown("e"))
                {
                    SceneManager.LoadScene(2);
                }
            }
        }

    }
}
