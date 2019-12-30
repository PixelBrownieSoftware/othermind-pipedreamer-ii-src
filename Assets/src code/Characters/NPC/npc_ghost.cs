using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class npc_ghost : PDII_character
{
    
	new void Start () {
        target = GameObject.Find("Player").GetComponent<PDII_character>();
        terminalspd = 30f;
        AI = true;
        CONTACT_TYPE = ON_CONTACT.TELEPORT;
        position_to_teleport_to = new Vector2(90, 200);    //For now I'll just hard-code it to be a certain value
        map_to_telport_to = "testlevel1";
        IS_KINEMATIC = false;

        SetAttackObject();
        base.Start();
	}


    public override void PlayerControl()
    {
        switch (CHARACTER_STATE)
        {
            case CHARACTER_STATES.STATE_IDLE:
                
                if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
                {
                    CHARACTER_STATE = CHARACTER_STATES.STATE_MOVING;
                }
                break;

            case CHARACTER_STATES.STATE_MOVING:

                GetComponent<SpriteRenderer>().color = Color.white;

                if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
                {
                    direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
                }

                break;



            case CHARACTER_STATES.STATE_ATTACK:

                GetComponent<SpriteRenderer>().color = Color.green;
                Vector2 vec = new Vector2(direction.x, direction.y).normalized;
                
                break;

        }
    }

    public override void ArtificialIntelleginceControl()
    {
        switch (CHARACTER_STATE)
        {
            case CHARACTER_STATES.STATE_IDLE:
                
                if (CheckTargetDistance(target, 250))
                {
                    CHARACTER_STATE = CHARACTER_STATES.STATE_MOVING;
                }
                break;

            case CHARACTER_STATES.STATE_MOVING:

                GetComponent<SpriteRenderer>().color = Color.white;
                direction = LookAtTarget(target);
                
                break;

            case CHARACTER_STATES.STATE_ATTACK:

                GetComponent<SpriteRenderer>().color = Color.green;
                Vector2 vec = new Vector2(direction.x, direction.y).normalized;
                break;

        }
    }

    new void Update () {
        base.Update(); 
    }
}
