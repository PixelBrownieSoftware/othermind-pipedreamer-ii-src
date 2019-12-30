using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class npc_venusmonster : PDII_character
{
    o_plcharacter pl = null;
    new void Start()
    {
        target = GameObject.Find("Player").GetComponent<o_plcharacter>();
        pl = target.GetComponent<o_plcharacter>();

        terminalspd = 60f;
        AI = true;
        control = true;
        CONTACT_TYPE = ON_CONTACT.IMMOBILIZE;
        Initialize();
        SetAttackObject();
        base.Start();
    }
    
    new void Update()
    {
        base.Update();
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

                if (pl.host != null)
                {
                    if (CheckTargetDistance(pl.host, 150))
                    {
                        CHARACTER_STATE = CHARACTER_STATES.STATE_MOVING;
                    }

                }
                else
                {
                    if (CheckTargetDistance(target, 150))
                    {
                        CHARACTER_STATE = CHARACTER_STATES.STATE_MOVING;
                    }
                }
                break;

            case CHARACTER_STATES.STATE_MOVING:

                GetComponent<SpriteRenderer>().color = Color.white;

                if (pl.host)
                {

                    direction = LookAtTarget(pl.host);

                    if (!pl.host.control)
                    {
                        direction = -LookAtTarget(pl.host);
                    }

                    if (pl.host.control && CheckTargetDistance(pl.host, 60))
                    {
                        StartCoroutine(Attack());
                    }
                }
                else {

                    direction = LookAtTarget(target);

                    if (!target.control)
                    {
                        direction = -LookAtTarget(target);
                    }

                    if (target.control && CheckTargetDistance(target, 60))
                    {
                        StartCoroutine(Attack());
                    }
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
        

        yield return new WaitForSeconds(0.7f);
        DisableAttack();
        CHARACTER_STATE = CHARACTER_STATES.STATE_MOVING;
    }
}
