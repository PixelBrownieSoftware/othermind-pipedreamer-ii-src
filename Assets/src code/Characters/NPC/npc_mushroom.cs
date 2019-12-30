using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class npc_mushroom : PDII_character
{

    new void Start()
    {
        terminalspd = 60f;
        AI = true;
        WorldsToAcess.Add("seasonal_forest");
        control = true;
        jump_height = 25.5f;
        isarial = true;
        CHARACTER_STATE = CHARACTER_STATES.STATE_MOVING;
        Initialize();
        base.Start();
    }

    public override void OnPosess()
    {
        s_gui.DisplayNotificationText("Space to jump", 2f);
        base.OnPosess();
    }

    new void FixedUpdate()
    {
        base.FixedUpdate();

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
    }
    
    public override void ArtificialIntelleginceControl()
    {
        WalkControl();
    }

    public override void PlayerControl()
    {
        base.PlayerControl();
        if (ArrowKeyControl())
        {
            CHARACTER_STATE = CHARACTER_STATES.STATE_MOVING;
        }
        else
        {
            CHARACTER_STATE = CHARACTER_STATES.STATE_IDLE;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump(4.5f);
        }
    }
}
