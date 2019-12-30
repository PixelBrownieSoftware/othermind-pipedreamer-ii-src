using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MagnumFoudation;

public enum COLLISION_T
{
    NONE = -1,
    WALL,
    FALLING,
    FALLING_ON_LAND,
    CLIMBING,
    DITCH,
    DAMAGE,
    STAIRS,
    MOVING_PLATFORM,
    LANDING_DOWN,
    WATER_TILE,
    NO_DEPOSEESS,
    LANDING_LEFT,
    LANDING_RIGHT,
    LANDING_UP,
    NO_LASTPOSITION
}
/*
public class o_generic : s_object {
    
    public Vector2 uppercollisionsize { get; set; }
    public Vector2 uppercollision { get; set; }
    public GameObject graphic;
    public bool issolid = true;
    public LayerMask layuer; Collider2D cha;
    public string character;
    public string characterCannot = null;

    public COLLISION_T collision_type;
    
    public Color32 fallingColour = new Color32(0, 150, 10, 145);
    public Color32 climbingColour = new Color32(153, 10, 10, 145);
    public Color32 defaultColour = new Color32(255, 0, 255, 145);
    public Color32 fallingOnGroundColour = new Color32(88, 230, 250, 145);
    public Color32 ditchColour = new Color32(30, 20, 40, 145);

    public COLLISION_T GetCollisionType()
    {
        return collision_type;
    }

    new private void Start()
    {
        base.Start();
    }

    new void Update ()
    {
        base.Update();
        positioninworld = transform.position;

        switch (collision_type)
        {
            case COLLISION_T.STAIRS:
                cha = GetCharacter(collision, layuer, "PlayerHitbox");
                if (cha != null)
                {
                    o_character pl = cha.transform.parent.GetComponent<o_character>();
                    if (Input.GetAxisRaw("Horizontal") > 0)
                    {
                        pl.positioninworld.y+= 0.2f;
                    }
                    if (Input.GetAxisRaw("Horizontal") < 0)
                    {
                        pl.positioninworld.y -= 0.2f;
                    }
                }
                break;

            case COLLISION_T.FALLING:
            
                cha = GetCharacter(collision, layuer);
                if (cha != null)
                {

                    o_character pl = cha.GetComponent<o_character>();
                    pl.CHARACTER_STATE = o_character.CHARACTER_STATES.STATE_IDLE;

                    if (pl.grounded == true)
                        pl.CHARACTER_STATE = o_character.CHARACTER_STATES.STATE_FALLING;
                }
                break;
            case COLLISION_T.FALLING_ON_LAND:
                cha = GetCharacter(collision, layuer, "Player");
                if (cha != null)
                {
                    o_character pl = cha.GetComponent<o_character>();
                    if (pl.grounded)
                    {
                        pl.CHARACTER_STATE = o_character.CHARACTER_STATES.STATE_FALLING;
                    }
                }
                break;

            case COLLISION_T.CLIMBING:
                cha = GetCharacter(collision, layuer, "Player");
                if (cha != null)
                {
                    o_character pl = cha.GetComponent<o_character>();
                    if (pl.grounded)
                    {
                        pl.CHARACTER_STATE = o_character.CHARACTER_STATES.STATE_CLIMBING;
                    }
                }
                break;

            case COLLISION_T.DITCH:

                cha = GetCharacter(collision, layuer, "Player");
                if (cha != null)
                {
                    o_character pl = cha.GetComponent<o_character>();
                    if (pl.grounded)
                    {
                        pl.GetComponent<o_plcharacter>().TeleportAfterFall();
                    }
                }
                break;

            case COLLISION_T.LANDING_DOWN:
            case COLLISION_T.LANDING_UP:
            case COLLISION_T.LANDING_LEFT:
            case COLLISION_T.LANDING_RIGHT:

                issolid = false;
                break;
        }

        //transform.GetChild(2).GetComponent<SpriteRenderer>().sortingOrder = (int)(positioninworld.y/height) - 1;
        //Collider2D collision = transform.GetChild(1).GetComponent<BoxCollider2D>();
        //Bounds bound = collision.bounds;

        //bound.center = new Vector3(positioninworld.x, positioninworld.y, 0) ;

    }

    private void OnDrawGizmos()
    {
        if (collision == null)
            collision = GetComponent<BoxCollider2D>();
        else
        {
            if (character != "")
                Gizmos.DrawWireCube(transform.position, collision.size);
        }
    }

}
*/