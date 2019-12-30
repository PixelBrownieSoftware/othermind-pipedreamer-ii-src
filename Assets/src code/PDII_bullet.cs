using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MagnumFoudation;

public class PDII_bullet : MagnumFoudation.o_bullet {
    
    float timer = 0;
    PDII_character ch;
    public AudioClip possesSound;
    public enum BULLET_T
    {
        POSSES,
        KILL,
        STUN,
        DEPOSSES
    }
    public BULLET_T BULLET_TYPE = BULLET_T.STUN;

    public void SetTimer(float dur)
    {
        timer = dur;
    }

	new void Start ()
    {
        animHand = GetComponent<s_animhandler>();
        base.Start();
	}
	
	new void Update ()
    {
        if (isbullet)
        {
            SetAnimation("orb", true);
            transform.Translate(Vector2.up * 7 * 60 * Time.deltaTime);
            
            Collider2D c = IfTouchingGetCol<PDII_character>(collision);
            if (c != null)
            {
                if (c != parent)
                {
                    ch = c.GetComponent<PDII_character>();
                    if (ch == null)
                        return;
                    if (ch != parent)
                    {
                        switch (BULLET_TYPE)
                        {
                            case BULLET_T.POSSES:
                                if (ch.isControllable)
                                {
                                    o_plcharacter pl = null;
                                    //parent.GetComponent<SpriteRenderer>().color = Color.clear;
                                    if (parent.GetComponent<o_plcharacter>() != null)
                                        pl = parent.GetComponent<o_plcharacter>();
                                    else
                                        pl = s_leveledit.LevEd.player.GetComponent<o_plcharacter>();
                                    pl.host = ch;
                                    pl.host.AI = true;
                                    pl.SpriteObj.GetComponent<SpriteRenderer>().color = Color.clear;
                                    pl.control = false;
                                    pl.rbody2d.velocity = Vector2.zero;
                                    pl.gameObject.SetActive(false);
                                    s_soundmanager.sound.PlaySound(ref possesSound,false);
                                    OnImpact(ch);
                                }

                                break;

                            case BULLET_T.DEPOSSES:
                                break;

                            case BULLET_T.KILL:
                                ch.DespawnObject();
                                OnImpact(ch);
                                break;
                        }
                    }

                    
                    if (parent.GetTargets().Find(x => x == c.GetComponent<o_character>()) != null)
                    {
                    }
                }
            }
            /*
            Collider2D c2 = IfTouchingGetCol(collision, "o_collidableobject");
            if (c2 != null)
            {
                if (c2 != parent)
                {
                    o_collidableobject ch = c2.GetComponent<o_collidableobject>();
                    if (ch == null)
                        return;
                    if (ch.collision_type != COLLISION_T.DITCH)
                    {
                        //OnImpact();
                    }
                }
            }
            */

            if (timer < 0) DespawnObject(); else timer -= Time.deltaTime;
        }
    }

    public void OnImpact(PDII_character character)
    {
        switch (BULLET_TYPE)
        {
            case BULLET_T.POSSES:
                character.OnPosess();
                break;

            case BULLET_T.DEPOSSES:
                break;

            case BULLET_T.KILL:
                ch.DespawnObject();
                //OnImpact();
                break;
        }
        base.OnImpact(character);
    }
    
}
