using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HairControl : MonoBehaviour
{

    private Animator anim;
    private Animator animPlayer;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        animPlayer = GetComponentInParent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //int state = animPlayer.GetInteger("state");

        Ability current = GameObject.Find("Player").GetComponent<AbilityBag>().currentAbility;
        int temp = (int)current;
        anim.SetInteger("style", temp);

        }
    }
