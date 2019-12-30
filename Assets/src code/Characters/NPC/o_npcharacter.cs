using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class o_npcharacter : PDII_character
{
	new void Start ()
    {
        terminalspd = 145f;
        AI = true;
        /*
        WorldsToAcess.Add("blue_meadow");
        WorldsToAcess.Add("testlevel2");
        WorldsToAcess.Add("blue_forest_boss"); 
        */
        control = true;
        scaleLedges = true;
        Initialize();
        CHARACTER_STATE = CHARACTER_STATES.STATE_MOVING;
        base.Start();
    }
    public override void OnPosess()
    {
        s_gui.DisplayNotificationText("Can climb on slopes.", 2f);
        base.OnPosess();
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
            Jump(1.8f);
        }
    }

    public override void ArtificialIntelleginceControl()
    {
        WalkControl();
    }

    new void FixedUpdate()
    {
        if (direction.x >= 0)
            rendererObj.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        else
            rendererObj.transform.rotation = Quaternion.Euler(new Vector3(0, -180, 0));
        if (Z_offset <= 0)
        {
            if (CHARACTER_STATE == CHARACTER_STATES.STATE_MOVING)
            {
                if (direction.y >= 0.1f)
                    SetAnimation("walk_u", true);
                else
                    SetAnimation("walk_d", true);
            }
            if (CHARACTER_STATE == CHARACTER_STATES.STATE_IDLE)
            {
                if (direction.y >= 0.1f)
                    SetAnimation("idle_u", true);
                else
                    SetAnimation("idle_d", true);
            }
        }
        else
        {
            if (direction.y >= 0.1f)
                SetAnimation("jump_u", false);
            else
                SetAnimation("jump_d", false);
        }
        base.FixedUpdate();
    }

    new void Update()
    {
        base.Update();
    }
}
