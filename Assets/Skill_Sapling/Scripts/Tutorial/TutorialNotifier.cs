using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialNotifier : MonoBehaviour
{

    public TMP_Text tutorialTMP;
    public float remainingTime = 0.0f;
    public bool running = false;
    
    public void Notify(string notification, float duration)
    {
        tutorialTMP.text = notification;
        remainingTime = duration;
        running = true;
    }
    // Start is called before the first frame update
    void Start()
    {
        Notify("WELCOME <3", 0.5f); 
    }

    public void welcomeHomeNotify()
    {
        Notify("HOME SWEET HOME <3", 0.5f); 
    }

    // Update is called once per frame
    void Update()
    {
        if (running)
        {
            remainingTime -= Time.deltaTime;
            if (remainingTime <= 0)
            {
                running = false;
                remainingTime = 0.0f;
                tutorialTMP.text = "";
            } 
        }

       
    }
}
