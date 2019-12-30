using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using MagnumFoudation;

[System.Serializable]
public struct o_attack
{
    public float damage;
    public float attackDuration;
    public float poise;
}

public class PDII_character : o_character
{
    #region variables
    struct ray_origins
    {
        public Vector2 top_left, top_right;
        public Vector2 bottom_left, bottom_right;
    }
    //public s_animhandler animhand;

    public enum ON_CONTACT {
        NONE,
        TELEPORT,
        IMMOBILIZE
    }
    public ON_CONTACT CONTACT_TYPE;
    
    public Vector2 shootDirection;

    public string map_origin;
    
    protected float jump_height = 2f;
    public s_animhandler anim;

    //FOR NPCS ONLY
    public Vector2 position_to_teleport_to;
    public string map_to_telport_to;
    public bool in_wall;

    bool canjump = true;
    public bool only_land = false;
    protected bool icetolerant = false;
    protected bool scaleLedges = false;
    protected bool isarial = false;
    public bool respawnOnDeposess = true;

    public s_animhandler charAnimation;
    public bool isControllable = true;

    //protected float yvel = 0, xvel = 0;

    public LayerMask lay;
    const float wldgravity = 3.98f;
    public string teleportLoc;

    public s_node CurrentNode;
    
    float freezetimer = 0;

    public bool consious = true;

    public List<string> WorldsToAcess = new List<string>();
    #endregion

    public new void Initialize()
    {
        maxHealth = 1;
        base.Initialize();
        animHand = SpriteObj.GetComponent<s_animhandler>();
    }
    
    public void ReAddFactions()
    {
        List<o_character> alc = s_leveledit.LevEd.allcharacters;
        targets.Clear();
        foreach (o_character c in alc)
        {
            if (c == null)
                continue;
            if (c.faction == faction &&
                c.faction != "" || c.faction == "noattk")
                continue;
            targets.Add(c);
        }
    }
    
    public void ShootBullet(float duration)
    {
        PDII_bullet bullt = s_levelloader.LevEd.SpawnObject<PDII_bullet>("Bullet", transform.position, Quaternion.Euler(0, 0, angle));
        bullt.SetTimer(duration);
        bullt.parent = this;
    }
    
    internal struct colbool
    {
        public colbool(bool hCollision, bool collisionMade, int signedval)
        {
            this.signedval = signedval;
            pos = new Vector2(0,0);
            this.hCollision = hCollision;
            this.collisionMade = collisionMade;
        }
        public Vector2 pos;
        public int signedval;
        public bool hCollision;
        public bool collisionMade;
    }

    public IEnumerator Unconsious()
    {
        consious = false;
        control = false;
        float secs = 0;
        direction = Vector2.zero;
        while (secs < 3.5f)
        {
            direction = Vector2.zero;
            if (!AI && GetType() != typeof(o_plcharacter))
            {
                print("k");
                consious = true;
                control = true;
                yield break;
            }
            secs += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        consious = true;
        float blackOut = 0;
        while (rendererObj.color != Color.clear)
        {
            blackOut += Time.deltaTime;
            rendererObj.color = Color.Lerp(rendererObj.color, Color.clear, blackOut);
            yield return new WaitForSeconds(Time.deltaTime);
        }

        if (map_origin != s_leveledit.LevEd.mapDat.name)
            DespawnObject();
        else
            ResetLocation();
    }

    public IEnumerator FadeOutRespawn()
    {
        float blackOut = 0;
        while (rendererObj.color != Color.clear)
        {
            blackOut += Time.deltaTime;
            rendererObj.color = Color.Lerp(rendererObj.color, Color.clear, blackOut);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        ResetLocation();
    }

    public void ResetLocation()
    {
            if (GetType() != typeof(o_plcharacter))
        {
            //TODO, add in an entity flag into the entity data struct
            control = true;
            /*
            if (s_levelloader.LevEd.mapDat.name != map_origin)
                DespawnObject();
            else
                */
            transform.position = spawnpoint;
        }
        control = true;
    }

    public PDII_character GetClosestTarget(float range)
    {
        PDII_character targ = null;
        float smallest = float.MaxValue;
        foreach (PDII_character c in targets)
        {
            if (c == this)
                continue;
            if (!c.gameObject.activeSelf)
                continue;
            if (Physics2D.Linecast(transform.position, c.transform.position, lay))
                continue;
            float dist = TargetDistance(c);
            if (dist > range)
                continue;
            if (dist < smallest)
            {
                targ = c;
                smallest = dist;
            }
        }
        return targ;
    }
    public PDII_character GetClosestTarget()
    {
        PDII_character targ = null;
        float smallest = float.MaxValue;
        foreach (PDII_character c in targets)
        {
            if (c == this)
                continue;
            if (Physics2D.Linecast(transform.position, c.transform.position, 8))
                continue;
            float dist = TargetDistance(c);
            if (dist < smallest) {
                targ = c;
                smallest = dist;
            }
        }
        return targ;
    }
    
    internal Collider2D IfTouchingGetCol(BoxCollider2D collisn, string characterdata)
    {
        if (collisn == null)
            return null;

        Collider2D[] chara = Physics2D.OverlapBoxAll(transform.position, collisn.size, 0);

        for (int i = 0; i < chara.Length; i++)
        {

            Collider2D co = chara[i];
            if (co == null)
                continue;

            if (co.gameObject == gameObject)
                continue;
            if (co.name == name)
                continue;

            //print(name + " Target: " + co.name);
            object o = co.GetComponent(characterdata);

            if (o == null)
                continue;
            //print(o.GetType() + " Target type: " + characterdata);

            if (o.GetType().ToString() == characterdata || o.GetType().IsSubclassOf(System.Type.GetType(characterdata)))
                return co;
            else
                continue;
            //if (obj.GetType() == characterdata)
            //  return co;

            return null;
        }
        return null;
    }
    internal Collider2D IfTouchingGetCol(BoxCollider2D collisn, float size_multip)
    {
        if (collisn == null)
            return null;

        Collider2D[] chara = Physics2D.OverlapBoxAll(transform.position, collisn.size * size_multip, 0);

        for (int i = 0; i < chara.Length; i++)
        {
            Collider2D co = chara[i];

            if (co == null)
                continue;
            print(co.name);
            if (co.gameObject == gameObject)
                continue;
            if (co.name == name)
                continue;

            return co;
        }

        return null;
    }
    internal Collider2D IfTouchingGetCol(BoxCollider2D collisn, string characterdata, float size_multip)
    {
        if (collisn == null)
            return null;

        Collider2D[] chara = Physics2D.OverlapBoxAll(transform.position, collisn.size * size_multip, 0);

        for (int i = 0; i < chara.Length; i++)
        {

            Collider2D co = chara[i];
            if (co == null)
                continue;

            if (co.gameObject == gameObject)
                continue;
            if (co.name == name)
                continue;

            //print(name + " Target: " + co.name);
            object o = co.GetComponent(characterdata);

            if (o == null)
                continue;
            //print(o.GetType() + " Target type: " + characterdata);

            if (o.GetType().ToString() == characterdata || o.GetType().IsSubclassOf(System.Type.GetType(characterdata)))
                return co;
            else
                continue;

        }

        return null;
    }

    private void OnDrawGizmos()
    {
        if (collision != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawCube(transform.position + offsetCOL, new Vector3(2,2));
        }
    }

    protected new void FixedUpdate()
    {
        /*
        PDII_bullet b = GetBullet<PDII_bullet>(collision);
        if (b != null)
        {
            if(isControllable)
                b.OnImpact(this);
        }
        */
        COLLISIONDET();
        base.FixedUpdate();
    }
    protected new void Update ()
    {
        //PHYSICS RELATED STUFF
        base.Update();

        if(SpriteObj != null)
            SpriteObj.transform.position = new Vector2(transform.position.x, transform.position.y + Z_offset);

        if (control) {
            if (AI)
            {
                ArtificialIntelleginceControl();

                Collider2D col = IfTouchingGetCol<PDII_character>(collision);
                if (col != null) {

                    PDII_character ob = col.GetComponent<PDII_character>();

                    switch (CONTACT_TYPE)
                    {
                        case ON_CONTACT.IMMOBILIZE:
                            if (ob != null)
                                if (ob.control)
                                    ob.StartCoroutine(ob.Unconsious());
                            break;

                        case ON_CONTACT.TELEPORT:

                            if (ob != null)
                                if (!ob.AI)
                                {
                                    s_map.s_tileobj to = s_levelloader.LevEd.mapDat.tilesdata.Find(x => x.name == teleportLoc);
                                    if (to != null)
                                    {
                                        ob.gameObject.transform.position = new Vector3(to.pos_x, to.pos_y);
                                    }
                                }
                            break;
                    }
                }
            }
            else
                PlayerControl();
        }
        
    }

    public override void PlayerControl()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            OnDeposess();
        }
    }

    void COLLISIONDET()
    {
        CurrentNode = CheckNode(transform.position + offsetCOL);

        if (IS_KINEMATIC)
        {
            if(collision != null)
                collision.isTrigger = false;
            if (only_land)
            {
                in_wall = true;
            }
            // s_gui.AddText(nodeg.PosToVec(new Vector2( positioninworld.x + collision.size.x / 2,  positioninworld.y + collision.size.y / 2)).ToString());

            if (CurrentNode != null)
            {
                //print(le1.COLTYPE);
                switch (CHARACTER_STATE)
                {
                    case CHARACTER_STATES.STATE_IDLE:
                    case CHARACTER_STATES.STATE_MOVING:
                        if ((COLLISION_T)CurrentNode.COLTYPE != COLLISION_T.DITCH &&
                            (COLLISION_T)CurrentNode.COLTYPE != COLLISION_T.WATER_TILE &&
                            (COLLISION_T)CurrentNode.COLTYPE != COLLISION_T.NO_LASTPOSITION &&
                            (COLLISION_T)CurrentNode.COLTYPE != COLLISION_T.LANDING_DOWN &&
                            (COLLISION_T)CurrentNode.COLTYPE != COLLISION_T.FALLING )
                            if (grounded && !only_land)
                                lastposbeforefall = transform.position;
                        if (only_land)
                            switch ((COLLISION_T)CurrentNode.COLTYPE)
                            {
                                case COLLISION_T.DITCH:
                                case COLLISION_T.LANDING_DOWN:
                                case COLLISION_T.LANDING_RIGHT:
                                case COLLISION_T.LANDING_UP:
                                case COLLISION_T.LANDING_LEFT:
                                    lastposbeforefall = transform.position;
                                    break;
                            }
                        break;
                }
                if ((COLLISION_T)CurrentNode.COLTYPE != COLLISION_T.FALLING_ON_LAND &&
                    (COLLISION_T)CurrentNode.COLTYPE != COLLISION_T.WATER_TILE)
                    terminalspd = terminalSpeedOrigin;

                if ((COLLISION_T)CurrentNode.COLTYPE != COLLISION_T.WATER_TILE)
                    freezetimer = 1.45f;

                if ((COLLISION_T)CurrentNode.COLTYPE != COLLISION_T.NONE &&
                    (COLLISION_T)CurrentNode.COLTYPE != COLLISION_T.NO_DEPOSEESS)
                {
                    if (shadow != null)
                        shadow.GetComponent<SpriteRenderer>().color = Color.clear;
                    in_wall = true;
                }
                else
                {
                    if (shadow != null)
                        shadow.GetComponent<SpriteRenderer>().color = new Color(1,1,1, 0.45f);
                    in_wall = false;
                }

                switch ((COLLISION_T)CurrentNode.COLTYPE)
                {
                    case COLLISION_T.NONE:

                        if (only_land)
                        {
                            transform.position = lastposbeforefall;
                        }
                        else
                        {
                            canjump = true;
                            in_wall = false;
                        }
                        break;

                    case COLLISION_T.FALLING:
                        in_wall = true;
                        if (CHARACTER_STATE != CHARACTER_STATES.STATE_FALLING)
                        {
                            print("check");
                            rbody2d.velocity = Vector2.zero;
                            if (grounded)
                            {
                                fallposy = nodegraph.CheckYFall(CurrentNode, (int)COLLISION_T.FALLING);
                                print(fallposy);
                                transform.position = CurrentNode.realPosition;
                                CHARACTER_STATE = CHARACTER_STATES.STATE_FALLING;
                            }
                        }
                        break;

                    case COLLISION_T.FALLING_ON_LAND:
                        in_wall = true;
                        if (scaleLedges)
                        {
                            if (grounded)
                            {
                                terminalspd = terminalSpeedOrigin / 2.5f;
                                break;
                            }
                            else
                            {
                                terminalspd = terminalSpeedOrigin/ 1.5f;
                                break;
                            }
                        }
                        if (CurrentNode != null)
                            if (grounded && CHARACTER_STATE != CHARACTER_STATES.STATE_FALLING)
                            {
                                if(rbody2d != null)
                                    rbody2d.velocity = Vector2.zero;
                                fallposy = nodegraph.CheckYFall(CurrentNode, (int)COLLISION_T.FALLING_ON_LAND)// + collision.offset.y + 80
                                    ;
                                print(fallposy);
                                transform.position = new Vector3(transform.position.x, CurrentNode.realPosition.y);
                                CHARACTER_STATE = CHARACTER_STATES.STATE_FALLING;
                            }
                        break;


                    case COLLISION_T.LANDING_DOWN:
                    case COLLISION_T.LANDING_UP:
                    case COLLISION_T.LANDING_LEFT:
                    case COLLISION_T.LANDING_RIGHT:

                        if (only_land)
                        {
                            in_wall = false;
                        }
                        if (grounded && !only_land &&
                            CHARACTER_STATE != CHARACTER_STATES.STATE_DASHING)
                        {
                            transform.position = lastposbeforefall;
                            /*
                            if (GetType() == typeof(o_plcharacter))
                                transform.position = lastposbeforefall;
                            else
                                ResetLocation();
                            */
                        }
                        break;

                    case COLLISION_T.DITCH:

                        in_wall = true;
                        if (grounded && !only_land &&
                            CHARACTER_STATE != CHARACTER_STATES.STATE_DASHING)
                        {
                            transform.position = lastposbeforefall;
                        }
                        break;

                    case COLLISION_T.WATER_TILE:
                        in_wall = true;
                        if (!icetolerant)
                        {
                            if (grounded)
                            {
                                terminalspd = terminalSpeedOrigin / 4;
                                canjump = false;
                                if (freezetimer > 0)
                                    freezetimer -= Time.deltaTime;
                                else
                                {
                                    transform.position = lastposbeforefall;
                                }
                            }
                        }
                        else
                        {
                            terminalspd = terminalSpeedOrigin / 3;
                        }
                        break;
                }
            }
        }
        else {
            collision.isTrigger = true;
        }
    }

    /// <summary>
    /// Deposesses without changing the player's position.
    /// </summary>
    public void OnDeposessWOPos(Vector2 pos)
    {
        if (CurrentNode != null)
            if (CurrentNode.COLTYPE == (int)COLLISION_T.NO_DEPOSEESS ||
                CurrentNode.COLTYPE == (int)COLLISION_T.DITCH ||
                CurrentNode.COLTYPE == (int)COLLISION_T.FALLING ||
                CurrentNode.COLTYPE == (int)COLLISION_T.FALLING_ON_LAND ||
                CurrentNode.COLTYPE == (int)COLLISION_T.WATER_TILE ||
                CurrentNode.COLTYPE == (int)COLLISION_T.WALL)
            {
                o_popup pop = s_leveledit.LevEd.SpawnObject<o_popup>("thought", transform);
                pop.parent = this;
                pop.timer = 1.6f;
                return;
            }
        o_plcharacter pl = s_levelloader.LevEd.player.GetComponent<o_plcharacter>();
        pl.transform.position = pos;
        pl.gameObject.SetActive(true);
        transform.SetParent(GameObject.Find("Entities").transform);
        gameObject.layer = 13;
        AI = true;
        //s_leveledit.LevEd.ChangeCharacterPossesData(name, false);
        pl.CHARACTER_STATE = CHARACTER_STATES.STATE_IDLE;
        MagnumFoudation.s_camera.SetPlayer(pl);
        pl.host = null;
        pl.control = true;
        pl.SpriteObj.GetComponent<SpriteRenderer>().color = Color.white;
        if (respawnOnDeposess)
            StartCoroutine(Unconsious());
    }
    public virtual void OnDeposess()
    {
        if (CurrentNode != null)
            if (CurrentNode.COLTYPE == (int)COLLISION_T.NO_DEPOSEESS ||
                CurrentNode.COLTYPE == (int)COLLISION_T.DITCH ||
                CurrentNode.COLTYPE == (int)COLLISION_T.FALLING ||
                CurrentNode.COLTYPE == (int)COLLISION_T.FALLING_ON_LAND ||
                CurrentNode.COLTYPE == (int)COLLISION_T.WATER_TILE||
                CurrentNode.COLTYPE == (int)COLLISION_T.WALL)
            {
                o_popup pop = s_leveledit.LevEd.SpawnObject<o_popup>("thought", transform);
                pop.parent = this;
                pop.timer = 1.6f;
                pop.timed = true;
                pop.typeOfThing = o_popup.NOTTYPE.NOALLOW;
                return;
            }
        o_plcharacter pl = s_levelloader.LevEd.player.GetComponent<o_plcharacter>();
        pl.gameObject.SetActive(true);
        transform.SetParent(GameObject.Find("Entities").transform);
        gameObject.layer = 13;
        AI = true;
        //s_leveledit.LevEd.ChangeCharacterPossesData(name, false);
        pl.transform.position = transform.position + offsetCOL;
        pl.CHARACTER_STATE = CHARACTER_STATES.STATE_IDLE;
        MagnumFoudation.s_camera.SetPlayer(pl);
        pl.host = null;
        pl.control = true;
        pl.SpriteObj.GetComponent<SpriteRenderer>().color = Color.white;
        if(respawnOnDeposess)
            StartCoroutine(Unconsious());
    }

    public virtual void OnPosess()
    {
        AI = false;
        MagnumFoudation.s_camera.SetPlayer(this);
        transform.SetParent(null);
        //s_leveledit.LevEd.GetComponent<s_leveledit>().SetEntity(name, );
        //s_leveledit.LevEd.ChangeCharacterPossesData(name, true);
        if (ID == "mask_man")
            s_gui.AddText("Click to speak");
        gameObject.layer = 9;
    }

    /*
    bool LegitCollision(s_node nod)
    {
        if (nod == null)
            return false;
        if (!nod.walkable)
        {
            s_node plnode = CheckNode(positioninworld);
            if ((COLLISION_T)plnode.COLTYPE != COLLISION_T.WALL)
                return false;
            if (plnode.characterExclusive == ID)
                return false;
        }
        else
            return false;
        return true;
    }
    */
    /*
    internal colbool CheckCollsionNewH()
    {
        colbool cb = new colbool(false, false,0);
        Bounds b = collision.bounds;
        Vector2[] points = {
            new Vector2(b.max.x - 1, b.min.y + 1), //Left 1
            new Vector2(b.max.x - 1, b.max.y - 1), //Left 2
            new Vector2(b.min.x + 1, b.min.y + 1), //Right 1
            new Vector2(b.min.x + 1, b.max.y - 1), //Right 2
        };
        for (int i = 0; i < points.Length; i++) {
            s_node le1 = CheckNode(points[i]);
            //LOOP THROUGH THE CONTACT POINTS AND CHECK BOTH ONES
            //IF THERE IS A BLOCK LIKE "FALLING" MAKE SURE BOTH ARE TOUCHING IT

            if (le1 == null)
                continue;
            if (!le1.walkable)
            {
                s_node plnode = CheckNode(positioninworld);
                if (plnode.COLTYPE != o_collidableobject.COLLISION_T.WALL)
                    continue;
                if (plnode.characterExclusive == ID)
                    continue;
                if (i > 1)
                {

                    velocity.x = 0;
                    positioninworld.x = plnode.realPosition.x - 20;
                }
                else
                {
                    velocity.x = 0;
                    positioninworld.x = plnode.realPosition.x + 20;
                }
                
                cb.pos = plnode.realPosition;
            }
        }
        return cb;
    }
    */
    /*
    internal bool CheckCollsionNewH()
    {
        colbool cb = new colbool(false, false, 0);
        Bounds b = collision.bounds;
        Vector2[] points = {
            new Vector2(b.max.x - 1, b.min.y + 1), //Left 1
            new Vector2(b.max.x - 1, b.max.y - 1), //Left 2
            new Vector2(b.min.x + 1, b.min.y + 1), //Right 1
            new Vector2(b.min.x + 1, b.max.y - 1), //Right 2
        };
        for (int i = 0; i < points.Length; i++)
        {
            s_node le1 = CheckNode(points[i]);
            //LOOP THROUGH THE CONTACT POINTS AND CHECK BOTH ONES
            //IF THERE IS A BLOCK LIKE "FALLING" MAKE SURE BOTH ARE TOUCHING IT

            if (le1 == null)
                continue;
            if (!le1.walkable)
            {
                s_node plnode = CheckNode(positioninworld);
                if (plnode.COLTYPE != COLLISION_T.WALL)
                    continue;
                if (plnode.characterExclusive == ID)
                    continue;
                return true;
            }
        }
        return false;
    }
    internal bool CheckCollsionNewV()
    {
        colbool cb = new colbool(false, false, 0);
        Bounds b = collision.bounds;
        Vector2[] points = {
            new Vector2(b.max.x- 1,  b.min.y - 1), //Down 1
            new Vector2(b.min.x + 1,  b.min.y - 1), //Down 2
            new Vector2(b.max.x- 1, b.max.y + 1), //Up 1
            new Vector2(b.min.x+ 1, b.max.y + 1) //Up 2
        };
        for (int i = 0; i < points.Length; i++)
        {
            s_node le1 = CheckNode(points[i]);
            //LOOP THROUGH THE CONTACT POINTS AND CHECK BOTH ONES
            //IF THERE IS A BLOCK LIKE "FALLING" MAKE SURE BOTH ARE TOUCHING IT

            if (le1 == null)
                continue;
            if (!le1.walkable)
            {
                s_node plnode = CheckNode(positioninworld);
                if (plnode.COLTYPE != COLLISION_T.WALL)
                    continue;
                if (plnode.characterExclusive == ID)
                    continue;
                return true;
            }
        }
        return false;
    }
    */
    /*
    void Move()
    {
        switch (CHARACTER_STATE)
        {
            case CHARACTER_STATES.STATE_IDLE:
                break;

            case CHARACTER_STATES.STATE_MOVING:

                CheckForVelLimits();
                rbody2d.velocity += direction * 2;
                break;

            case CHARACTER_STATES.STATE_FALLING:
                transform.position -= new Vector3(0,5,0);
                if (transform.position.y <= fallposy)
                {
                    CHARACTER_STATE = CHARACTER_STATES.STATE_MOVING;
                }
                break;

            case CHARACTER_STATES.STATE_DASHING:
                if (dashdelay <= 0)
                    AfterDash();

                if (!AI)
                    if (!Input.GetKey(KeyCode.LeftShift))
                        AfterDash();

                transform.position += (Vector3)new Vector2(velocity.x, velocity.y) * 15 * Time.deltaTime;
                break;

            case CHARACTER_STATES.STATE_DEFEAT:
                GetComponent<SpriteRenderer>().color = Color.black;
                break;
        }
        if(dashdelay > 0)
            dashdelay -= Time.deltaTime;

        if (rbody2d != null)
        {
            float xv = rbody2d.velocity.x;
            float yv = rbody2d.velocity.y;
            if (direction.x == 0)
                xv = rbody2d.velocity.x * 0.85f;
            if (direction.y == 0)
                yv = rbody2d.velocity.y * 0.85f;
            rbody2d.velocity = new Vector2(xv, yv);
        }

        if (rbody2d != null)
            transform.Translate((Vector3)rbody2d.velocity * Time.deltaTime * Time.deltaTime);

        if(collision != null)
        {
            if (IS_KINEMATIC)
                collision.isTrigger = false;
            else
                collision.isTrigger = true;
        }
        COLLISIONDET();
}
    */
    /*
    public IEnumerator Jump()
    {
        grounded = false;
        while (Z_offset > 0)
        {
            gravity -= Time.deltaTime * wldgravity;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        Z_offset = 0;
        grounded = true;
        gravity = 0;
    }
    */
    /*
    void AABB_COLLISION(Bounds thing)
    {
        Vector2 rv = Vector2.zero - velocity;
        float contactvel = Vector2.Dot(rv, velocity.normalized);

        if (contactvel > 0)
            return;

        float e = .1f;

        float j = Mathf.Round((1.0f + e) * contactvel);

        Vector2 imp = j * velocity.normalized;
        velocity = imp;
    }
    */
    /*
    void DrawRaysLitHor(ref Bounds bound)
    {
        float rayspacing = bound.size.y / raycount;
        int i = 0;
        for (i = 0; i < raycount; i++)
        {
            float signedvec = Mathf.Sign(velocity.x);
            float rayleng = Mathf.Abs(signedvec );

            Vector2 raypos = (signedvec != -1 ? new Vector2(bound.max.x, bound.min.y) : new Vector2(bound.min.x, bound.min.y ));
            raypos += Vector2.up * (rayspacing * i );
            RaycastHit2D hit = Physics2D.Raycast(raypos, Vector2.right * signedvec, rayleng, 256);
            if (hit.transform == null)
                continue;
            o_collidableobject col = hit.transform.GetComponent<o_collidableobject>();
            if (col != null)
            {
                if (col.character == ID && col.character != null)
                {
                    continue;
                }
            }
            Debug.DrawRay(raypos, Vector2.right * signedvec, Color.blue);

            if (hit)
            {
                if (col.collision_type == COLLISION_T.WALL)
                {
                    if (col.issolid)
                    {
                        float oldvel = velocity.x;
                        velocity.x = 0;// *signedvec;
                                       //velocity.x = (hit.distance - skinwdth);
                        rayleng = hit.distance;
                        positioninworld.x += rayleng * -signedvec;
                    }
                    else {
                        if (col.characterCannot == ID) {

                            float oldvel = velocity.x;
                            velocity.x = 0;// *signedvec;
                                           //velocity.x = (hit.distance - skinwdth);
                            rayleng = hit.distance;
                            positioninworld.x += rayleng * -signedvec;
                        }
                    }
                }
            }
        }
    }
    void DrawRaysLitVer(ref Bounds bound)
    {
        float rayspacing = bound.size.x / raycount;
        int i = 0;
        //bool touchedwall = false;
        for (i = 0; i < raycount; i++)
        {
            float signedvec = Mathf.Sign(velocity.y);
            float rayleng = Mathf.Abs(signedvec );

            Vector2 raypos = (signedvec != -1 ? new Vector2(bound.min.x, bound.max.y) : new Vector2(bound.min.x, bound.min.y));
            raypos += Vector2.right * (rayspacing * i);
            RaycastHit2D hit = Physics2D.Raycast(raypos, Vector2.up * signedvec, rayleng, 256);
            if (hit.transform == null)
                continue;
           // else
               // touchedwall = true;
            o_collidableobject col = hit.transform.GetComponent<o_collidableobject>();
            Debug.DrawRay(raypos, Vector2.up * signedvec, Color.blue);

            if (col != null)
            {
                if (col.character == ID && col.character != null)
                    continue;
            }
            if (hit)
            {
                if (col.collision_type == COLLISION_T.WALL)
                {

                    if (col.issolid)
                    {
                        float oldvel = velocity.y;
                        velocity.y = 0;
                        rayleng = hit.distance;
                        positioninworld.y += rayleng * -signedvec;
                    }
                    else
                    {
                        if (col.characterCannot == ID)
                        {
                            float oldvel = velocity.y;
                            velocity.y = 0;
                            rayleng = hit.distance;
                            positioninworld.y += rayleng * -signedvec;
                        }
                    }
                }
            }
        }
        //if (touchedwall)
        //    in_wall = true;
       // else
         //   in_wall = false;
    }
    */
    /*
    Collider2D co = IfTouchingGetCol(collision, "o_collidableobject");
    if (co)
    {
        in_wall = true;
        if (only_land)
        {
            o_collidableobject c = co.GetComponent<o_collidableobject>();
            if (c)
            {
                print(c.GetCollisionType());
                switch (c.GetCollisionType())
                {
                    case o_collidableobject.COLLISION_T.LANDING:
                        in_wall = false;
                        break;
                }
            }
        }
    }
    else
    {
        in_wall = false;
        if (only_land)
        {
            in_wall = true;
        }
    }
    */
    /*
    if (collv.collisionMade) {
        velocity.y = 0;
        positioninworld.y = collv.pos.y - (1 * collv.signedval);
    }
    if (velocity.x != 0)
        DrawRaysLitHor(ref bound);
    if (velocity.y != 0)
        DrawRaysLitVer(ref bound);

    if (collh.collisionMade)
    {
        velocity.x = 0;
        positioninworld.x = collh.pos.x - (1 * collh.signedval);
    }
     */
}
