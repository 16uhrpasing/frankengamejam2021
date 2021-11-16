using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeRunEmitter : MonoBehaviour
{
    public AbilityBag _abilityBag;
    public GameEvent homeRunEvent;
    public GameEvent resetAbilityPositionsEvent;
    public GameEvent abilityChangedEvent;
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (_abilityBag.currentAbility == Ability.None) return;
        if (!_abilityBag.abilitesInResetRoom.Contains(_abilityBag.currentAbility))
        {
            Debug.Log("Home Run!: " + _abilityBag.currentAbility.ToString());
            HomeRunRaise.RaiseHomeRun(homeRunEvent, _abilityBag.currentAbility);
            _abilityBag.BringAbilityToResetRoom();
        }
        _abilityBag.currentAbility = Ability.None;
        _abilityBag.UpdateGUISprite();
        resetAbilityPositionsEvent.Raise();
        abilityChangedEvent.Raise();
    }
}
