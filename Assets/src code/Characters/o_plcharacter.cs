using MagnumFoudation;
using UnityEngine;

public class o_plcharacter : PDII_character
{
    public LayerMask trig;
    public PDII_character host;
    int selected_weapon_num;
    UnityEngine.UI.Text gui;

    public UnityEngine.UI.Text txt;
    public GameObject systgui;

    public GameObject DEBUGMARKER;
    public GameObject DEBUGMARKER2;

    public bool canDeposses = true;
    public AudioClip shootSound;

    new void Start ()
    {
        MagnumFoudation.s_camera.SetPlayer(this);
        ID = "player";
        gameObject.layer = 9;
        Initialize();
    }

    public override void PlayerControl()
    {
        switch (CHARACTER_STATE)
        {
            case CHARACTER_STATES.STATE_IDLE:

               // SetAnimation("test", true);

                if (ArrowKeyControl())
                {
                    CHARACTER_STATE = CHARACTER_STATES.STATE_MOVING;
                }
                if (Input.GetMouseButtonDown(0))
                {
                    mouse = MouseAng();
                    angle = ReturnAngle(new Vector3(mouse.x, mouse.y, 0));
                    ShootBullet(0.35f);
                    s_soundmanager.sound.PlaySound(ref shootSound, false);
                }

                /*
                if (Input.GetMouseButtonDown(1))
                {
                    StartCoroutine(Attack(weapons[selected_weapon_num]));
                }
                */
                if (Input.GetKeyDown(s_globals.arrowKeyConfig["jump"]))
                {
                    Jump(1.95f);
                }

                break;

            case CHARACTER_STATES.STATE_MOVING:
                if (control)
                {
                    //charAnimation.SetAnimation("test2", true);

                    if (!ArrowKeyControl())
                    {
                        CHARACTER_STATE = CHARACTER_STATES.STATE_IDLE;
                    }

                    if (grounded)
                    {
                        if (Input.GetMouseButtonDown(0))
                        {
                            mouse = MouseAng();
                            angle = ReturnAngle(new Vector3(mouse.x, mouse.y, 0));
                            ShootBullet(0.35f);
                            s_soundmanager.sound.PlaySound(ref shootSound, false);
                        }
                        
                        if (Input.GetKeyDown(s_globals.arrowKeyConfig["jump"]))
                        {
                            Jump(1.95f);
                        }
                    }
                }

                break;


            case CHARACTER_STATES.STATE_CLIMBING:
                if (!IfTouchingGetCol(collision, 3f, trig))
                {
                    CHARACTER_STATE = CHARACTER_STATES.STATE_MOVING;
                }
                break;

            case CHARACTER_STATES.STATE_ATTACK:

                Vector2 vec = new Vector2(mouse.x, mouse.y).normalized;
                
                break;

        }
    }

    new void Update ()
    {
        Collider2D trig = IfTouchingGetCol<o_trigger>(collision);
        if (trig != null)
        {
            if (trig.GetComponent<o_trigger>().TRIGGER_T == o_trigger.TRIGGER_TYPE.CONTACT_INPUT)
            {
                systgui.gameObject.SetActive(true);
                txt.text = "Press " + s_globals.arrowKeyConfig["select"].ToString();
            }
        }
        else
        {
            systgui.gameObject.SetActive(false);
            txt.text = "";
        }

        int verticalDir = Mathf.RoundToInt(direction.y);
        int horizontalDir = Mathf.RoundToInt(direction.x);

        if (CHARACTER_STATE == CHARACTER_STATES.STATE_MOVING)
        {
            if (verticalDir == -1 && horizontalDir == 0)
                SetAnimation("walk_down", true);
            else if (verticalDir == 1 && horizontalDir == 0)
                SetAnimation("walk_up", true);
            else if (horizontalDir == -1 && verticalDir == 1 ||
                horizontalDir == -1 && verticalDir == -1 || horizontalDir == -1)
            {
                rendererObj.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                SetAnimation("walk_side", true);
            }
            else if (horizontalDir == 1 && verticalDir == 1 ||
                horizontalDir == 1 && verticalDir == -1 || horizontalDir == 1)
            {
                rendererObj.transform.rotation = Quaternion.Euler(new Vector3(0, -180, 0));
                SetAnimation("walk_side", true);
            }
        }
        if (CHARACTER_STATE == CHARACTER_STATES.STATE_IDLE)
        {
            if (verticalDir == -1 && horizontalDir == 0)
                SetAnimation("idle_down", true);
            else if (verticalDir == 1 && horizontalDir == 0)
                SetAnimation("idle_up", true);
            else if (horizontalDir == -1 && verticalDir == 1 ||
                horizontalDir == -1 && verticalDir == -1 || horizontalDir == -1)
            {
                rendererObj.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                SetAnimation("idle_side", true);
            }
            else if (horizontalDir == 1 && verticalDir == 1 ||
                horizontalDir == 1 && verticalDir == -1 || horizontalDir == 1)
            {
                rendererObj.transform.rotation = Quaternion.Euler(new Vector3(0, -180, 0));
                SetAnimation("idle_side", true);
            }

        }

        base.Update();
        if (CurrentNode != null)
        {
            DEBUGMARKER.transform.position = CurrentNode.realPosition + new Vector2 (-20,0);
            DEBUGMARKER2.transform.position = transform.position + (Vector3)offsetCOL;
        }

       // gui.text = "Current weapon: " + weapons[selected_weapon_num].name;
        //GetComponent<SpriteRenderer>().sortingOrder = (int)(positioninworld.y / 20);

        mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        angle = ReturnAngle(new Vector3(mouse.x, mouse.y, 0));

        if (Input.GetKeyDown(KeyCode.E))
        {
            print("Tile type: " + CheckNode(transform.position + (Vector3)offsetCOL).COLTYPE);
        }
    }

    new private void FixedUpdate()
    {
        base.FixedUpdate();
    }
    
}
