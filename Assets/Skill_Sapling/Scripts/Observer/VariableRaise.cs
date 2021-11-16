using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VariableRaise
{
    public static float publicFloat;
    public static float publicCooldown;

    public static void RaiseFloat(GameEvent e, float i)
    {
        VariableRaise.publicFloat = i;
        e.Raise();
    }
}
