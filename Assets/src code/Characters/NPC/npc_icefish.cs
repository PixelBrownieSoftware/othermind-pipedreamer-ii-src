using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class npc_icefish : PDII_character
{
    new void Start()
    {
        terminalspd = 90f;
        AI = true;
        control = true;
        icetolerant = true;
        isControllable = false;

        Initialize();
        base.Start();
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
        base.ArtificialIntelleginceControl();
    }
    
    new void FixedUpdate()
    {
        base.FixedUpdate();
    }

    new void Update()
    {
        base.Update();
    }
}
