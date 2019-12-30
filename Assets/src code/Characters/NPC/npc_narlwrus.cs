using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class npc_narlwrus : PDII_character
{
    bool textshow = false;
    o_plcharacter pl;
    o_popup pop2;
    public Sprite popSprite;

    new void Start()
    {
        AI = true;
        only_land = true;
        base.Start();
        respawnOnDeposess = false;
        pl = GameObject.Find("Player").GetComponent<o_plcharacter>();
        Initialize();
    }

    public override void PlayerControl()
    {
        if (CurrentNode != null)
        {
            switch ((COLLISION_T)CurrentNode.COLTYPE)
            {
                default:
                    if(pop2 != null)
                        pop2.DespawnObject();
                    textshow = false;
                    break;

                case COLLISION_T.LANDING_DOWN:
                case COLLISION_T.LANDING_LEFT:
                case COLLISION_T.LANDING_RIGHT:
                case COLLISION_T.LANDING_UP:

                    if (!textshow)
                    {
                        pop2 = s_leveledit.LevEd.SpawnObject<o_popup>("thought", transform);
                        pop2.parent = this;
                        pop2.timed = false;
                        pop2.offsetPos = 50;
                        textshow = true;
                        pop2.typeOfThing = o_popup.NOTTYPE.JUMPOFF;
                    }
                    break;
            }
        }
        switch (CHARACTER_STATE)
        {
            case CHARACTER_STATES.STATE_IDLE:
                if (ArrowKeyControl())
                {
                    CHARACTER_STATE = CHARACTER_STATES.STATE_MOVING;
                }
                break;

            case CHARACTER_STATES.STATE_MOVING:
                if (!ArrowKeyControl())
                {
                    CHARACTER_STATE = CHARACTER_STATES.STATE_IDLE;
                }
                break;
        }
        base.PlayerControl();
    }

    public override void OnDeposess()
    {
        if (CurrentNode != null)
        {
            switch ((COLLISION_T)CurrentNode.COLTYPE)
            {
                case COLLISION_T.LANDING_DOWN:
                    OnDeposessWOPos(new Vector3(CurrentNode.realPosition.x + 10, CurrentNode.realPosition.y - 40));
                    break;
                case COLLISION_T.LANDING_LEFT:
                    OnDeposessWOPos(new Vector3(CurrentNode.realPosition.x - 40, CurrentNode.realPosition.y + 10));
                    break;
                case COLLISION_T.LANDING_RIGHT:
                    OnDeposessWOPos(new Vector3(CurrentNode.realPosition.x + 40, CurrentNode.realPosition.y + 10));
                    break;
                case COLLISION_T.LANDING_UP:
                    OnDeposessWOPos(new Vector3(CurrentNode.realPosition.x + 10, CurrentNode.realPosition.y + 40));
                    break;
            }
        }
    }

    public override void ArtificialIntelleginceControl()
    {
        base.ArtificialIntelleginceControl();

        if (pop2 != null)
            pop2.DespawnObject();
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
