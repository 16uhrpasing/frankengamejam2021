using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashRightSlowmotion : MonoBehaviour
{
    public TutorialNotifier Notifier;
    public AbilityBag AbilityBag;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (AbilityBag.abilitesInResetRoom.Contains(Ability.Dash)) return;
        Notifier.Notify("Press D + Shift to Dash Right", 1.0f);
        Time.timeScale = .3f;
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (AbilityBag.abilitesInResetRoom.Contains(Ability.Dash)) return;
        Notifier.Notify("", 0.1f);
        Time.timeScale = 1.0f;
    }
}
