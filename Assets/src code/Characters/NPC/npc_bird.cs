using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class npc_bird : PDII_character
{
    public int worms = 0;

    new void Start()
    {
        terminalspd = 25f;
        AI = true;
        WorldsToAcess.Add("seasonal_forest");
        control = true;
        jump_height = 25.5f;
        isarial = true;
        CHARACTER_STATE = CHARACTER_STATES.STATE_IDLE;
        Initialize();
        base.Start();
    }

    public override void OnPosess()
    {
        s_gui.DisplayNotificationText("Keep pressing space to fly", 2f);
        base.OnPosess();
    }

    new void FixedUpdate()
    {
        base.FixedUpdate();
        if (grounded)
        {
            terminalspd = terminalSpeedOrigin / 2.5f;
        }
        else
        {
            terminalspd = terminalSpeedOrigin;
        }
    }

    public override void ArtificialIntelleginceControl()
    {
       // WalkControl();
        //JumpWithoutGround(2.7f, 30);
    }

    public override void PlayerControl()
    {
        if (ArrowKeyControl())
            CHARACTER_STATE = CHARACTER_STATES.STATE_MOVING;
        else
            CHARACTER_STATE = CHARACTER_STATES.STATE_IDLE;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            JumpWithoutGround(2.7f, 30);
        }
        base.PlayerControl();
    }

    new void Update()
    {
        if (direction.x >= 0)
            rendererObj.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        else
            rendererObj.transform.rotation = Quaternion.Euler(new Vector3(0, -180, 0));
        if (grounded)
        {
            if (CHARACTER_STATE == CHARACTER_STATES.STATE_MOVING)
            {
                if (direction.y <= 0)
                    SetAnimation("walk_d", true);
                else
                    SetAnimation("walk_u", true);
            }
            if (CHARACTER_STATE == CHARACTER_STATES.STATE_IDLE)
            {
                if (direction.y <= 0)
                    SetAnimation("idle_d", true);
                else
                    SetAnimation("idle_u", true);
            }
        }
        else
        {
            if (direction.y <= 0)
                SetAnimation("fly_d", true);
            else
                SetAnimation("fly_u", true);
        }
        base.Update();
    }
}
