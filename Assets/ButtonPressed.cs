using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonPressed : MonoBehaviour
{
    public void LoadScene(int i)
    {
        SceneManager.LoadScene(i);
    }
}
