using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class npc_yeti : PDII_character
{
    public int caughtFish = 0;

    enum YETI_MODE
    {
        PASSIVE,
        AGGRESSIVE
    }
    YETI_MODE AI_MODE;

    new void Start()
    {
        terminalSpeedOrigin = 60f;
        AI = true;
        faction = "iceMonster";
        control = true;
        icetolerant = true;
        CONTACT_TYPE = ON_CONTACT.TELEPORT;
        
        Initialize();
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

                if (!ArrowKeyControl())
                {
                    CHARACTER_STATE = CHARACTER_STATES.STATE_IDLE;
                }

                if (CurrentNode != null)
                    if (CurrentNode.COLTYPE == (int)COLLISION_T.WATER_TILE)
                        if (Input.GetKeyDown(KeyCode.Space))
                        {
                            Dash(0.5f, terminalSpeedOrigin * 3);
                        }
                break;
                
        }
        base.PlayerControl();
    }
    public override void ArtificialIntelleginceControl()
    {

        switch (AI_MODE)
        {
            case YETI_MODE.PASSIVE:

                WalkControl();
                target = GetClosestTarget(200);
                if (target != null)
                {
                    AI_MODE = YETI_MODE.AGGRESSIVE;
                }
                break;

            case YETI_MODE.AGGRESSIVE:
                if (target.GetComponent<PDII_character>().consious || target != null)
                {
                    if (target.GetComponent<PDII_character>().consious)
                    {
                        direction = LookAtTarget(target);
                        CHARACTER_STATE = CHARACTER_STATES.STATE_MOVING;
                    }
                }
                if (!CheckTargetDistance(target, 200))
                {
                        AI_MODE = YETI_MODE.PASSIVE;
                }
                break;
        }

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
        Collider2D col = IfTouchingGetCol(collision, "npc_icefish");
        if (col != null)
        {
            npc_icefish ob = col.GetComponent<npc_icefish>();
            caughtFish++;
            ob.DespawnObject();
        }
        base.FixedUpdate();
    }

    public override void OnDeposess()
    {
        base.OnDeposess();
    }

    public override void OnPosess()
    {
        base.OnPosess();
    }

    new void Update()
    {

        if (direction.x >= 0)
            rendererObj.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        else
            rendererObj.transform.rotation = Quaternion.Euler(new Vector3(0, -180, 0));

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
        base.Update();
    }
}
