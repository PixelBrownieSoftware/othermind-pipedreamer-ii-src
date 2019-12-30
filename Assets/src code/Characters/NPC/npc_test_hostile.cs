using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class npc_test_hostile : PDII_character
{
    
    new private void Start()
    {
        target = GameObject.Find("Player").GetComponent<PDII_character>();
        terminalspd = 60f;
        AI = true;
        control = true;
        CONTACT_TYPE = ON_CONTACT.IMMOBILIZE;

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
        switch (CHARACTER_STATE)
        {
            case CHARACTER_STATES.STATE_IDLE:
                
                if (CheckTargetDistance(target, 150))
                {
                    CHARACTER_STATE = CHARACTER_STATES.STATE_MOVING;
                }
                break;

            case CHARACTER_STATES.STATE_MOVING:

                GetComponent<SpriteRenderer>().color = Color.white;
                direction = LookAtTarget(target);

                if (!target.control)
                {
                    direction = -LookAtTarget(target);
                }
                
                if (target.control && CheckTargetDistance(target, 60))
                {
                    StartCoroutine(Attack());
                }

                break;
                
            case CHARACTER_STATES.STATE_ATTACK:

                GetComponent<SpriteRenderer>().color = Color.green;
                Vector2 vec = new Vector2(direction.x, direction.y).normalized;
                
                break;

        }
    }
    public IEnumerator Attack()
    {
        CHARACTER_STATE = CHARACTER_STATES.STATE_ATTACK;
        angle = ReturnAngle(new Vector3(direction.x, direction.y, 0));

        //ShootBullet(1);

        yield return new WaitForSeconds(0.7f);
        DisableAttack();
        CHARACTER_STATE = CHARACTER_STATES.STATE_MOVING;
    }

    new private void Update()
    {
        base.Update();
    }
    


}
