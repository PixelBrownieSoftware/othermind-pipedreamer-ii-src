using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class npc_maskperson : PDII_character
{
    public o_itemObj currentItem;
    public GameObject speech;
    float speechdelay = 0;
    float speechDuration = 0;
    public float pickup_delay = 0;
    public bool isTalking = false;

    public SpriteRenderer speechSpr;
    public Sprite[] speechDia;

    new void Start()
    {
        speechSpr = speech.GetComponent<SpriteRenderer>();
        AI = true;
        terminalspd = 75f;
        targets.Add(GameObject.Find("Player").GetComponent<PDII_character>());
        target = targets[0].CharacterType<PDII_character>();
        ID = "mask_man";
        isControllable = true;
        Initialize();
    }
    public override void OnDeposess()
    {
        base.OnDeposess();
    }

    public override void OnPosess()
    {
        s_gui.DisplayNotificationText("Left click to ask for items.", 2f);
        base.OnPosess();
    }

    public void ItemGetOrDrop()
    {
        if (currentItem != null)
        {
            s_leveledit.LevEd.SpawnObject<o_itemObj>(currentItem.ID, transform.position, Quaternion.identity);
            pickup_delay = 1.5f;
            currentItem = null;
            return;
        }
        if (pickup_delay > 0)
            return;
        /*
        Collider2D colItem = IfTouchingGetCol(collision, "o_itemObj");
        if (colItem != null)
        {
            o_itemObj obj = colItem.GetComponent<o_itemObj>();
            if (obj != null)
            {
                currentItem = obj;
                currentItem.gameObject.SetActive(false);
                pickup_delay = 1.35f;
                return;
            }
        }
        */
    }

    public override void PlayerControl()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (speech != null)
            {
                if (speechdelay <= 0)
                {
                    isTalking = true;
                    speechDuration = 1;
                    speech.SetActive(true);
                    speechSpr.sprite = speechDia[Random.Range(0, speechDia.Length - 1)];
                    speechdelay = 0.5f;
                }
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            ItemGetOrDrop();
        }

        switch (CHARACTER_STATE)
        {
            case CHARACTER_STATES.STATE_IDLE:
                
                if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
                {
                    CHARACTER_STATE = CHARACTER_STATES.STATE_MOVING;
                }
                break;

            case CHARACTER_STATES.STATE_MOVING:

                if (!ArrowKeyControl()) {
                    CHARACTER_STATE = CHARACTER_STATES.STATE_IDLE;
                }
                break;
        }
        base.PlayerControl();
    }

    public override void ArtificialIntelleginceControl()
    {
        switch (CHARACTER_STATE)
        {
            case CHARACTER_STATES.STATE_IDLE:

                /*
                velocity = Vector2.zero;
                if (target != null) {
                    if (CheckTargetDistance(target, 45))
                    {
                        CHARACTER_STATE = CHARACTER_STATES.STATE_MOVING;
                    }
                }*/
                break;

            case CHARACTER_STATES.STATE_MOVING:
                
                direction = -LookAtTarget(target);
                break;
        }
    }
    new void FixedUpdate()
    {
        terminalspd = 75f;
        pickup_delay = pickup_delay > 0 ? pickup_delay - Time.deltaTime : 0;
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
        if (speechdelay > 0)
        {
            speechdelay -= Time.deltaTime;
        }
        if (speechDuration > 0)
        {
            speechDuration -= Time.deltaTime;
        }
        else {
            isTalking = false;
            speech.SetActive(false);
        }
    }
}
