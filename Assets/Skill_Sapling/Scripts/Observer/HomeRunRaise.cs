using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HomeRunRaise
{
    public static Ability HomeRunAbility;
    
    public static void RaiseHomeRun(GameEvent e, Ability homeRun)
    {
        HomeRunAbility = homeRun;
        e.Raise();
    }
}
