using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MagnumFoudation;
/*
public class u_feedinggame : s_utility
{
    public int minimumfeed = 0;

    public float feeding_timer = 15f;
    const float feedingTmrStrt = 15f;

    public List<o_itemObj> wormPos = new List<o_itemObj>();

    float wormSpawnTimer = 1.35f;
    public npc_bird bird;

    public override void EventStart()
    {
        eventState = 2;
    }

    new void Update()
    {
        if (eventState == 2)
        {
            feeding_timer -= Time.deltaTime;
            wormSpawnTimer -= Time.deltaTime;

            if (feeding_timer <= 0)
                eventState = 1;

            Collider2D co = IfTouchingGetCol<npc_bird>(collision);
            if (co != null)
            {
                npc_bird b = co.GetComponent<npc_bird>();
                if (b != null)
                {
                    if (b.worms >= minimumfeed)
                    {
                        feeding_timer = feedingTmrStrt;
                    }
                }
            }
        }
    }
}
*/