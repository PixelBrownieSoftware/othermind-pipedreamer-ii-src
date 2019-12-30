using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MagnumFoudation;

public class u_leemminigame : s_utility
{
    public float timer = 60f;
    public s_object boundary;
    bool activeEvent = false;
    public enum state
    {
        INACTIVE,
        ONGOING,
        FAIL,
        WIN,
        REWARD
    }
    public int labelToCall;

    private new void Start()
    {
        base.Start();
        eventState = -1;
    }
    public new void Update()
    {
        if (eventState == (int)state.ONGOING)
        {
            if (!activeEvent)
            {
                timer = 60f;
                activeEvent = true;
            }
            timer -= Time.deltaTime;
            s_gui.DisplayNotificationText("Time: " + timer, -1);
            if (timer <= 0)
            {
                eventState = (int)state.FAIL;
                s_gui.HideNotificationText();
            }
        }
        if (eventState == (int)state.FAIL)
        {
            if (activeEvent)
            {
                s_gui.HideNotificationText();
                activeEvent = false;
            }
        }
        if (eventState == (int)state.WIN)
        {
            s_gui.DisplayNotificationText("You won! Go back to the host!", 0.8f);
            boundary.DespawnObject();
            eventState = (int)state.REWARD;
        }
    }
    
}
