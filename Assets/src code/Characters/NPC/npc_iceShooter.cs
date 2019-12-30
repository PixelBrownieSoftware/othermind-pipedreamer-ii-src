using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class npc_iceShooter : PDII_character
{
    new void Start()
    {
        terminalspd = 165f;
        AI = true;
        control = true;
        icetolerant = true;
        isControllable = false;

        SetAttackObject();
        Initialize();
        base.Start();
    }
    
    public override void ArtificialIntelleginceControl()
    {
        if (target != null)
        {
            if (target.GetComponent<PDII_character>().consious)
            {
                if (!CheckTargetDistance(target, 410))
                {
                    target = null;
                }
                //ShootBullet(1);
               // MoveMotor(LookAtTarget(target));
            }
        }
        else
            target = GetClosestTarget(395);
    }
    /*
    switch (CHARACTER_STATE)
    {
        case CHARACTER_STATES.STATE_IDLE:

            velocity = Vector2.zero;
            break;

        case CHARACTER_STATES.STATE_MOVING:

            GetComponent<SpriteRenderer>().color = Color.white;
            direction = LookAtTarget(target);

            if (!target.control)
            {
                direction = -LookAtTarget(target);
            }
            velocity += new Vector2(direction.x * 6, direction.y * 6);

            if (target.control && CheckTargetDistance(target, 60))
            {
            }

            break;

        case CHARACTER_STATES.STATE_ATTACK:

            GetComponent<SpriteRenderer>().color = Color.green;
            Vector2 vec = new Vector2(direction.x, direction.y).normalized;

            velocity += new Vector2(vec.x, vec.y) * (50);
            break;

    }
    */

    public IEnumerator Attack()
    {
        CHARACTER_STATE = CHARACTER_STATES.STATE_ATTACK;
        angle = ReturnAngle(new Vector3(direction.x, direction.y, 0));

        //ShootBullet(1);

        yield return new WaitForSeconds(0.7f);
        DisableAttack();
        CHARACTER_STATE = CHARACTER_STATES.STATE_MOVING;
    }

    new void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void OnDeposess()
    {
        base.OnDeposess();
    }

    new void Update()
    {
        base.Update();
    }
}
