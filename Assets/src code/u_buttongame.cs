using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MagnumFoudation;

public class u_buttongame : s_utility
{

    public bool redMode = false;

    public List<o_button> redButtons = new List<o_button>();
    public List<o_button> blueButtons = new List<o_button>();
    public List<o_generic> redBounds = new List<o_generic>();
    public List<o_generic> blueBounds = new List<o_generic>();

    public enum MODE
    {
        BLUE,
        RED
    }
    public MODE mod;

    new void Start()
    {
        
    }

    public void CheckAllButtons()
    {
        switch (mod)
        {
            case MODE.BLUE:

                foreach (o_button b in redButtons)
                {
                    //if one of the buttons is on set that one to off
                    if (b.isOn)
                    {
                        foreach (o_generic g in redBounds)
                        {
                            g.collision.isTrigger = true;
                        }
                        foreach (o_generic g in blueBounds)
                        {
                            g.collision.isTrigger = false;
                        }
                        b.isOn = false;
                        mod = MODE.RED;
                    }
                }
                break;
        }
    }
    
    new void Update()
    {
        
    }
}
