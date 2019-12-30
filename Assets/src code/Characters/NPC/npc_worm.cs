using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
public class npc_worm : PDII_character
{
    float wormtimer = 0;

    new void Start()
    {
        AI = true;
        target = GameObject.Find("Player").GetComponent<PDII_character>();
        terminalspd = 120f;
        base.Start();
        Initialize();
    }

    public override void ArtificialIntelleginceControl()
    {
        if (target != null)
        {

            if (!CheckTargetDistance(target, 150))
            {
                if (target.GetComponent<PDII_character>().consious)
                {
                    //MoveMotor(-LookAtTarget(target));
                }
            }
        }
        else
            target = GetClosestTarget(200);
    }

    public override void PlayerControl()
    {
        base.PlayerControl();
        if (ArrowKeyControl())
            CHARACTER_STATE = CHARACTER_STATES.STATE_MOVING;
        else
            CHARACTER_STATE = CHARACTER_STATES.STATE_IDLE;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump(1.5f);
        }
    }

    new private void FixedUpdate()
    {
        base.FixedUpdate();
    }

    new void Update()
    {
        base.Update();
    }
}
*/
