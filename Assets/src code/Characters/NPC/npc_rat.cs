using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class npc_rat : PDII_character
{
    new void Start()
    {
        terminalspd = 250f;
        AI = true;
        WorldsToAcess.Add("underground_cmplx");
        control = true;
        Initialize();
        CHARACTER_STATE = CHARACTER_STATES.STATE_MOVING;
        base.Start();
    }

    public override void ArtificialIntelleginceControl()
    {
        base.ArtificialIntelleginceControl();
    }

    public override void PlayerControl()
    {
        switch (CHARACTER_STATE)
        {
            case CHARACTER_STATES.STATE_IDLE:
                SetAnimation("idle", true);
                if (ArrowKeyControl())
                {
                    CHARACTER_STATE = CHARACTER_STATES.STATE_MOVING;
                }
                break;

            case CHARACTER_STATES.STATE_MOVING:
                SetAnimation("walk", true);
                ArrowKeyControl();
                //Debug.DrawRay(positioninworld, (target.positioninworld - positioninworld).normalized * 15, Color.red);
                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    Dash(2.5f);
                }
                //RaycastHit2D[] objectcol = Physics2D.RaycastAll(positioninworld, (target.positioninworld - positioninworld).normalized * 15, 0, lay);
                break;
        }
    }

    public override void OnPosess()
    {
        s_gui.DisplayNotificationText("Left shift to dash across edges", 2f);
    }

    public override void AfterDash()
    {
        base.AfterDash();
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
