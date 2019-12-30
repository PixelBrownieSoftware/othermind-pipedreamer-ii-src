using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class npc_pbear : PDII_character
{
    public bool isSleeping = false;

    const float sleepStart = 35f;
    const float targetTimerStart = 15f;
    public float sleepTimer = 0;
    public float noSeeTargetTimer = targetTimerStart;
    const float possesTimerStart = 45f;
    float possesTimer = 0f;

    enum POLAR_B_AI
    {
        IDLE,
        RETURN_SLEEP
    }
    POLAR_B_AI PB_AI = POLAR_B_AI.IDLE;

    new void Start()
    {
        terminalspd = 60f;
        AI = true;
        control = true;
        icetolerant = true;
        CONTACT_TYPE = ON_CONTACT.TELEPORT;
        faction = "monster";
        Initialize();
        isControllable = false;

        SetAttackObject();
        base.Start();
        Initialize();
    }

    public override void PlayerControl()
    {
        base.PlayerControl();

        possesTimer -= Time.deltaTime;
        if (possesTimer <= 10)
            terminalspd = terminalSpeedOrigin / 2;
        if (possesTimer <= 0)
            OnDeposess();

        switch (CHARACTER_STATE)
        {
            case CHARACTER_STATES.STATE_IDLE:
                
                if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
                {
                    CHARACTER_STATE = CHARACTER_STATES.STATE_MOVING;
                }
                break;

            case CHARACTER_STATES.STATE_MOVING:

                ArrowKeyControl();
                break;

            case CHARACTER_STATES.STATE_ATTACK:

                GetComponent<SpriteRenderer>().color = Color.green;
                Vector2 vec = new Vector2(direction.x, direction.y).normalized;
                
                break;

        }
    }

    public override void ArtificialIntelleginceControl()
    {
        target = GetClosestTarget(200);

        if (CHARACTER_STATE == CHARACTER_STATES.STATE_IDLE)
            SetAnimation("Idle", true);

        if (target != null)
        {
            if (target.AI)
                target = null;
            if (CheckTargetDistance(target, 200))
            {
                if (target.GetComponent<PDII_character>().consious)
                {
                    direction = LookAtTarget(target);
                    CHARACTER_STATE = CHARACTER_STATES.STATE_MOVING;
                }
            }
            else
            {
                WalkControl();
            }
        }

        /*
        switch (PB_AI)
        {
            case POLAR_B_AI.IDLE:

                break;

            case POLAR_B_AI.RETURN_SLEEP:

                if (CheckTargetDistance(target, 90) || sleepTimer <= 0)
                {
                    if (target.GetComponent<PDII_character>().consious)
                    {
                        noSeeTargetTimer = targetTimerStart;
                        PB_AI = POLAR_B_AI.IDLE;
                    }
                }
                else
                {
                    if (CheckTargetDistance(spawnpoint, 10))
                    {
                        isSleeping = true;
                    }
                    direction = LookAtTarget(spawnpoint);
                    MoveMotor(direction);
                }
                break;
        }
        */
    }

    public override void OnPosess()
    {
        if (isSleeping)
        {
            possesTimer = possesTimerStart;
            base.OnPosess();
        }
    }

    public override void OnDeposess()
    {
        base.OnDeposess();
    }

    new void FixedUpdate()
    {
        base.FixedUpdate();
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
