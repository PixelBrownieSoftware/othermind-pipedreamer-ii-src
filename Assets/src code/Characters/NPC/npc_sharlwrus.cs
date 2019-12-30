using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MagnumFoudation;

public class npc_sharlwrus : PDII_character
{
    float returnTimer = 1.6f;
    bool dispapering = false;

    new void Start()
    {
        AI = true;
        only_land = true;
        terminalSpeedOrigin = 65f;
        CONTACT_TYPE = ON_CONTACT.TELEPORT;
        isControllable = false;
        Initialize();
        base.Start();
    }

    public override void ArtificialIntelleginceControl()
    {
        base.ArtificialIntelleginceControl();
        if (target == null)
        {
            if (transform.position != spawnpoint)
            {
                if (returnTimer <= 0) {
                    if (!dispapering)
                    {
                        StartCoroutine(FadeOutRespawn());
                        dispapering = true;
                    }
                }
                else
                    returnTimer -= Time.deltaTime;
            }
            else
                dispapering = false;
            SetAnimation("idle",true);
            target = GetClosestTarget(250);
        }
        else
        {
            if (CheckTargetDistance(target, 400) && !Physics2D.Linecast(transform.position, target.transform.position, lay))
            {
                SetAnimation("attack", true);
                direction = LookAtTarget(target);
                s_node nextNode = nodegraph.PosToNode((Vector2)transform.position + (direction * 10));
                CHARACTER_STATE = CHARACTER_STATES.STATE_MOVING;
            }
            else
            {
                target = null;
            }
        }
    }

    new private void FixedUpdate()
    {
        base.FixedUpdate();
        if (CurrentNode != null)
        {
            switch ((COLLISION_T)CurrentNode.COLTYPE)
            {
                case COLLISION_T.NONE:
                    
                    break;
                    
            }
        }
    }

    new void Update()
    {
        base.Update();
    }
}
