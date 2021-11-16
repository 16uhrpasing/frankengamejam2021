using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchedMe : MonoBehaviour
{
    enum TouchType
    {
        Mushroom,
        Platform,
        Other
    }
    [SerializeField] private TouchType type;
    [SerializeField] private GameObject lightObj;

    [SerializeField] private SpriteRenderer sp;
    [SerializeField] private BoxCollider2D bc;

    [SerializeField] LayerMask shrooms;

    private bool active = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        

        if (GameObject.Find("Player").GetComponent<AbilityBag>().currentAbility == Ability.Shroom)
        {
            if (type == TouchType.Mushroom)
            {
                if (collision.gameObject.layer == 7)
                {
                    lightObj.SetActive(true);
                    sp.color = Color.white;
                    gameObject.layer = 12;
                }
            }
            if (type == TouchType.Platform)
            {
                sp.color = Color.white;
                bc.enabled = true;
                /*if (collision.gameObject.layer != 7)
                {
                    active = true;
                }*/
            }
        }
    }

    private void Update()
    {
        //Collider2D[] results = new Collider2D[2];
        //Collider2D shroom;
        active = false;


        if (type == TouchType.Platform)
        {
            active = Physics2D.OverlapCircle(transform.position, 3, shrooms);
            if (active)
            {
                sp.color = Color.white;
                bc.enabled = true;
            }
        }
        //shroom = OverlapCircle(transform.position, 5, 3, -Mathf.Infinity, Mathf.Infinity);


    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (type == TouchType.Platform && !active)
        {
            Color invisible = Color.white;
            invisible.a = 0;
            sp.color = invisible;
            bc.enabled = false;

        }
    }
}
