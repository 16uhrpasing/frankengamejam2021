using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode()]
public class ProgressBar : MonoBehaviour
{
#if UNITY_EDITOR
    [MenuItem("GameObject/UI/Progress Bars/Linear Progress Bar")]
    public static void AddLinearProgressBar()
    {
        GameObject obj = Instantiate(Resources.Load<GameObject>("UI/LinearProgressBar"));
        obj.transform.SetParent(Selection.activeGameObject.transform, false);
    }
    [MenuItem("GameObject/UI/Progress Bars/Radial Progress Bar")]
    public static void AddRadialProgressBar()
    {
        GameObject obj = Instantiate(Resources.Load<GameObject>("UI/RadialProgressBar"));
        obj.transform.SetParent(Selection.activeGameObject.transform, false);
    }
#endif

    public int minimum;
    public int maximum;
    public int current;
    public Image mask;
    public Image fill;
    public Color color;

    public void Update()
    {
        GetCurrentFill();
    }

    public void ChangeValue()
    {
        current = (int)(maximum / VariableRaise.publicCooldown * VariableRaise.publicFloat);
    }

    public void ResetBarToFull()
    {
        Debug.Log("Reset ability bar after change!");
        current = 0;
    }

    void GetCurrentFill()
    {
        float currentOffset = current - minimum;
        float maximumOffset = maximum - minimum;
        float fillAmount = currentOffset / maximum;
        mask.fillAmount = fillAmount;
        fill.color = color;
    }
}
