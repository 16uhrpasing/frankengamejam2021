using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AbilityChangeRaise
{
    public static Ability publicAbility;
    
    public static Ability hover;
    

    public static void RaiseAbilityChange(GameEvent e, Ability a)
    {
        publicAbility = a;
        e.Raise();
    }
    
    
}
