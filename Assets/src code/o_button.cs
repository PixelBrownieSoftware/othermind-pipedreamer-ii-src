using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MagnumFoudation;

public class o_button : o_character
{
    public bool isOn;
    Collider2D lastBx;

    new void Start()
    {
        base.Start();
    }

    new void Update()
    {
        base.Update();
        Collider2D bx = IfTouchingGetCol<npc_rat>(collision);
        
        if (bx != null)
        {
            if (lastBx != bx)
            {
                isOn = isOn ? false : true;
                lastBx = bx;
            }
        }
    }
}
