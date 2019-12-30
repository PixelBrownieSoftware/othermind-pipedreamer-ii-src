using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class npc_crystalball : PDII_character
{

    public bool GetOff = false;

    new void Start()
    {
        AI = true;
        Initialize();
        base.Start();
    }

    public override void PlayerControl()
    {
        base.PlayerControl();

        if (Input.GetMouseButtonDown(0))
        {
            mouse = MouseAng();
            angle = ReturnAngle(new Vector3(mouse.x, mouse.y, 0));
            ShootBullet(0.5f);
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
