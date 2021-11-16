using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoDashTutorial : MonoBehaviour
{
    public TutorialNotifier notifier;
    public AbilityBag AbilityBag;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (AbilityBag.abilitesInResetRoom.Contains(Ability.Dash)) return;
        notifier.Notify("Not every wise step is taken forward.\n Press R to Reset", 3.0f);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        notifier.Notify("", 0.1f);
    }
}
