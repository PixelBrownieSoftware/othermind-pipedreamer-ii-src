
#region LEVEL_EDIT_CODE
/*
 
ADDED ON 09/09/2018
 
    public override void OnInspectorGUI()
    {
        s_leveledit lev = (s_leveledit)target;

        if (lev != null)
        {
            if (lev.maps != null)
            {
                if (lev.maps.Count >= 2)
                {
                    levelsel = (int)GUILayout.HorizontalSlider(lev.current_area, 0, lev.maps.Count - 1);

                    if (GUILayout.Button("Select level"))
                        lev.current_area = levelsel;

                    //if (lev.maps[lev.current_area] != null)
                        //EditorGUILayout.LabelField(lev.maps[lev.current_area].name);
                }
            }
        }
        foreach (s_map ma in lev.maps)
        {
            EditorGUILayout.LabelField(ma.name);

        }

        
        lev.nam = GUILayout.TextArea(lev.nam);


        if (GUILayout.Button("Load Level"))
        {
            lev.current_map = lev.maps[lev.current_area];
            //lev.JsonToObj();
        }
        if (GUILayout.Button("New Level"))
        {
            lev.NewMap();
        }
        if (GUILayout.Button("Save Level"))
        {
            lev.SaveMap("4");
        }

        base.OnInspectorGUI();
        Repaint();
    }
    
    private void OnSceneGUI()
    {
        s_leveledit lev = (s_leveledit)target;
        if (Event.current.isKey)
        {
            switch (Event.current.keyCode)
            {
                case KeyCode.A:

                    lev.SpawnObj(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                    break;
            }
        }
        
    }
    
    public void AssignParent(string na, GameObject ga)
    {
        ga.transform.SetParent(GameObject.Find(na).transform);
    }

    Vector3 Snap(Vector3 mousepos)
    {
        return new Vector3(Mathf.Floor(mousepos.x / 20) * 20, Mathf.Floor(mousepos.y / 20) * 20);
    }
 
 */
#endregion

#region REMOVED_AABB_COLLISION
/*
ADDED ON 10/08/2018

This was in the update function

 Bounds thing = new Bounds();
        if (Physics2D.OverlapBox(transform.position, collision.size, 0, 256) != null)
        {
            thing = Physics2D.OverlapBox(transform.position, collision.size, 0, 256).bounds;
            thing.Expand(skinwdth * -2);
        }

        if (thing.min != (Vector3)new Vector2(0, 0) && thing.max != (Vector3)new Vector2(0, 0))
        {
            AABB_COLLISION(thing);
            
            bool collisioncheckXy = this.collision.bounds.min.x * 1.1f > thing.max.x && this.collision.bounds.max.x / 1.1f < thing.min.x;
            if (!collisioncheckXy) 
            {
                if (velocity.x != 0)
                    AABB_CollisionX(thing);
                print(collisioncheckXy + "Y");
            }
            bool collisioncheckYx = this.collision.bounds.min.y * 1.1f > thing.max.y && this.collision.bounds.max.y / 1.1f < thing.min.y;
            if (!collisioncheckYx)
            {
                if (velocity.y != 0) 
                    AABB_CollisionY(thing);
            }




void AABB_CollisionX(Bounds collision)
{

    float directionVecX = Mathf.Sign(velocity.x);
    bool collisioncheckYx = this.collision.bounds.min.y * 1.1f > collision.max.y && this.collision.bounds.max.y / 1.1f < collision.min.y;
    print(collisioncheckYx + "X");
    bool collisioncheckX = directionVecX != -1 ? this.collision.bounds.min.x < collision.max.x : this.collision.bounds.max.x > collision.min.x;

    if (collisioncheckX)
    {
        if (directionVecX == -1)
            velocity.x = Mathf.Sign(this.collision.bounds.min.x - collision.max.x) * directionVecX;

        if (directionVecX == 1)
            velocity.x = Mathf.Sign(collision.min.x - this.collision.bounds.max.x) * directionVecX;

    }

}

void AABB_CollisionY(Bounds collision)
{
    float directionVecY = Mathf.Sign(velocity.y);

    bool collisioncheckY = directionVecY != -1 ? this.collision.bounds.min.y < collision.max.y : this.collision.bounds.max.y > collision.min.y;

    if (collisioncheckY)
    {
        if (directionVecY == -1)
            velocity.y = Mathf.Sign(collision.max.y - this.collision.bounds.min.y) * -directionVecY;

        if (directionVecY == 1)
            velocity.y = Mathf.Sign(this.collision.bounds.max.y - collision.min.y) * -directionVecY;
    }
}
*/

#endregion

#region OLD_COLLISION_SYSTEM
/*
ADDED ON 10/08/2018
This was meant to be the collision system



                if (objectcol != null)
                {
                    foreach (RaycastHit2D sur in objectcol)
                    {
                        //print(sur.collider.gameObject.name);

                        if (sur.collider.transform.parent.GetComponent<o_collidableobject>())
                        {
                            o_collidableobject collidable = sur.collider.transform.parent.GetComponent<o_collidableobject>();


                            if (grounded && zpos < collidable.zpos )
                            {
                                gravity = 1.2f;
                            }
                        }
                    }

                }


    public class ray_z
    {
        private o_character character;
        public float zpos;
        Vector2 position;
        bool is_up;
        LayerMask layer;
        public ray_z(o_character character, bool ceiling)
        {
            this.character = character;
            layer = character.lay;
            is_up = ceiling;
            if (ceiling)
            {
                zpos = character.zpos + character.height;
            }
            else
            {
                zpos = character.zpos;
            }
        }

        public rayhit istouching { get
            {
                if (is_up)
                    zpos = character.zpos + character.height;
                else
                    zpos = character.zpos;

                float raypos = character.collision.size.x / 5;

                float raypos_x = character.collision.size.x / 5;
                float raypos_y = character.collision.size.y / 5;

                for (int y = 1; y != 5; y++)
                {
                    for (int x = 1; x != 5; x++)
                    {

                        Vector2 position = new Vector2(character.collision.bounds.min.x + (raypos_x * x),
                                    character.collision.bounds.min.y + (raypos_y * y) - zpos);
                        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(position, new Vector2(1, 1), 0, layer);
                        if (collider2Ds.Length > 0)
                        {
                            foreach (Collider2D c in collider2Ds)
                            {
                                o_collidableobject collidable = c.transform.parent.GetComponent<o_collidableobject>();
                                if (collidable.zpos + collidable.height > zpos && collidable.zpos < zpos)
                                {
                                    return new rayhit(true, zpos - collidable.zpos);
                                }
                            }
                        }
                    }
                }
                return new rayhit(false, 0);
            }
        }
    }


////////////STUFF FOR THE GRAVITY OF THE Z AXIS////////////////
        if ( zpos < 0.1f)
        {
            if (!grounded )
            {
                gravity = 0;
                grounded = true;
            }
        }
        else
        {
            grounded = false;
            gravity -= Time.deltaTime * wldgravity; floorz = 0;
        }

///////////////DRAWING RAYS////////////////////////////////

void DrawRaysLitHor(ref Bounds bound)
{
    float rayspacing = bound.size.y / raycount;
    int i = 0;
    for (i = 0; i < raycount; i++)
    {
        float signedvec = Mathf.Sign(velocity.x);
        float ray_zpos = this.zpos + 1 + (rayspacing * i);
        float rayleng = Mathf.Abs(signedvec);

        Vector2 raypos = (signedvec != -1 ? new Vector2(bound.max.x, bound.min.y) : new Vector2(bound.min.x, bound.min.y));
        raypos += Vector2.up * (rayspacing * i);
        RaycastHit2D hit = Physics2D.Raycast(raypos, Vector2.right * signedvec, rayleng, 256);

        Debug.DrawRay(raypos, Vector2.right * signedvec, Color.blue);

        if (hit)
        {
            o_collidableobject collidedthing = hit.collider.gameObject.transform.parent.GetComponent<o_collidableobject>();
            if (Mathf.Floor(collidedthing.zpos + collidedthing.height) > ray_zpos   //Player is higher
               && ray_zpos > collidedthing.zpos //Player is lower
               )
            {
                velocity.x = (hit.distance - skinwdth);// *signedvec;
                rayleng = hit.distance;
            }
            else { continue; }


        }
    }
}
void DrawRaysLitVer(ref Bounds bound)
{
    float rayspacing = bound.size.x / raycount;
    int i = 0;
    for (i = 0; i < raycount; i++)
    {
        float signedvec = Mathf.Sign(velocity.y);
        float ray_zpos = this.zpos + 1 + (rayspacing * i);
        float rayleng = Mathf.Abs(signedvec);

        Vector2 raypos = (signedvec != -1 ? new Vector2(bound.min.x, bound.max.y) : new Vector2(bound.min.x, bound.min.y));
        raypos += Vector2.right * (rayspacing * i);
        RaycastHit2D hit = Physics2D.Raycast(raypos, Vector2.up * signedvec, rayleng, 256);


        Debug.DrawRay(raypos, Vector2.up * signedvec, Color.blue);
        //Debug.Log(Vector2.up * signedvec);

        if (hit)
        {
            o_collidableobject collidedthing = hit.collider.gameObject.transform.parent.GetComponent<o_collidableobject>();
            if (Mathf.Floor(collidedthing.zpos + collidedthing.height) > ray_zpos   //Player is higher
                && ray_zpos > collidedthing.zpos //Player is lower
                )
            {
                velocity.y = (hit.distance - skinwdth);
                rayleng = hit.distance;
            }
            else { continue; }
        }
    }
}
*/
#endregion

#region CHECK_VARIABLES
/*
public bool CheckIntegersEqual(string nameofint, int integer)
{
    foreach (ev_integer integr in EventIntegers)
    {
        if (integr.integer_name == nameofint)
        {
            return integer == integr.integer;
        }
    }
    return false;
}

public bool CheckIntegersGreaterThan(string nameofint, int integer)
{
    foreach (ev_integer integr in EventIntegers)
    {
        if (integr.integer_name == nameofint)
        {
            return integer < integr.integer;
        }
    }
    return false;
}

public bool CheckIntegersLessThan(string nameofint, int integer)
{
    foreach (ev_integer integr in EventIntegers)
    {
        if (integr.integer_name == nameofint)
        {
            return integer > integr.integer;
        }
    }
    return false;
}*/
#endregion

#region TRIGGER_WALK
/*
 IEnumerator Walk()
 {
     Vector2 pos = this.transform.position;
     Vector2 lastpos = transform.position;

     List<Vector2> points = new List<Vector2>();

     foreach (trigger_obj trig in movesteps)
     {
         pos = lastpos + (trig.direcion.normalized * trig.steps * speed);
         lastpos = pos;
         points.Add(lastpos);
     }


     Vector2 lgoalpoint = transform.position;
     while (i < points.Count)
     {
         if (!movesteps[i].is_Teleport)
         {
             selobj.velocity = Vector2.zero;
             Vector2 goalpoint = points[i]; 

             float timer = Vector2.Distance(lgoalpoint, goalpoint) / (speed);
             lgoalpoint = points[i];
             print(timer);
             while (timer > 0)
             {
                 selobj.velocity = (goalpoint - (Vector2)selobj.transform.position).normalized * speed;

                 timer -= Time.deltaTime;
                 yield return new WaitForSeconds(Time.deltaTime);
             }
         }
         else {
             Vector2 vecpos = movesteps[i].direcion * 44;
             vecpos *= movesteps[i].steps;
         }
         i++;
     }
 }
             */

#endregion

#region TRIGGER_LOAD_MAP
/*
ADDED ON 17/11/2018
    case ev_details.EVENT_TYPES.LOAD_MAP:

        EditorGUILayout.LabelField("Name of map");
        ev.string0 = EditorGUILayout.TextArea(ev.string0);

        if (GUILayout.Button("Open Level file"))
        {
            string levelload = EditorUtility.OpenFilePanel("Open Json level file", "Assets/Levels/", "");
            if (levelload != null)
                LoadTempLevel(levelload);
        }
        EditorGUILayout.LabelField("Mouse Position X: " + mouseArea.transform.position.y + " Mouse Position Y: " + mouseArea.transform.position.y + " Press S to confirm");

        EditorGUILayout.LabelField("Spawn position from this point");
        ev.float0 = EditorGUILayout.FloatField("XPOS.", ev.float0);
        ev.float1 = EditorGUILayout.FloatField("YPOS.", ev.float1);
        break;
        */

/*
    case ev_details.EVENT_TYPES.LOAD_MAP:

        s_globals.SaveData();
        doingEvents = false;
        s_leveledit led = GameObject.Find("General").GetComponent<s_leveledit>();
        isactive = false;
        led.TriggerSpawn(selobj.GetComponent<o_plcharacter>(), current_ev.string0, new Vector2(current_ev.float0, current_ev.float1), this);

        break;*/
#endregion

#region MAPLOADING

/*
ADDED ON 23/03/2019
    GameObject levedatabase = Resources.Load("LevelDatabase") as GameObject;

    if(dat == null)
        dat = levedatabase.GetComponent<s_leveldatabase>();
    print(dat.maps.Count);


    s_map mapdat = new s_map();
    for (int i = 0; i < dat.maps.Count; i++)
    {
        if(dat.maps[i].name == levelname)
            mapdat = dat.maps[i];
    }
    current_area = levelname;

    GameObject mapIG = new GameObject();
    GameObject triggerIG = new GameObject();
    GameObject entityIG = new GameObject();
    GameObject tileIG = new GameObject();

    mapIG.name = current_area;
    triggerIG.name = "Triggers";
    entityIG.name = "Entities";
    tileIG.name = "Tiles";

    tileIG.transform.SetParent(mapIG.transform);
    triggerIG.transform.SetParent(mapIG.transform);
    entityIG.transform.SetParent(mapIG.transform);

    for (int i = 0; i < mapdat.triggerdata.Count; i++)
    {
        o_trigger trig = Instantiate(triggerprefab, new Vector2(mapdat.triggerdata[i].pos_x, mapdat.triggerdata[i].pos_y), Quaternion.identity).GetComponent<o_trigger>();
        trig.Events.ev_Details = mapdat.triggerdata[i].listofevents;
        if (mapdat.triggerdata[i].util != null)
        {
            string n = mapdat.triggerdata[i].util.GetType().ToString();
            switch (n)
            {
                case "util_shop":
                    //TODO: LOAD FROM RESOURCES
                    break;
            }
        }

        trig.transform.SetParent(triggerIG.transform);
    }
    for (int i = 0; i < mapdat.objectdata.Count; i++)
    {
        o_character trig = Instantiate(charactersSpawnlist[0], new Vector2(mapdat.objectdata[i].pos_x, mapdat.objectdata[i].pos_y), Quaternion.identity).GetComponent<o_character>();
        trig.positioninworld = new Vector3(mapdat.objectdata[i].pos_x, mapdat.objectdata[i].pos_x,1);
        trig.transform.SetParent(entityIG.transform);
    }
    for (int i = 0; i < mapdat.tilesdata.Count; i++)
    {
        o_collidableobject trig = Instantiate(prefab, new Vector2(mapdat.tilesdata[i].pos_x, mapdat.tilesdata[i].pos_y), Quaternion.identity).GetComponent<o_collidableobject>();
        trig.height = mapdat.tilesdata[i].height;
        trig.positioninworld = new Vector3(mapdat.tilesdata[i].pos_x, mapdat.tilesdata[i].pos_x, 1);
        trig.transform.SetParent(tileIG.transform);
    }


    */
#endregion

#region EDITOR_MODE
/*
 * ADDED ON 23/03/2019
    //current_area = (int)GUI.HorizontalSlider(new Rect(10,20,150,10),current_area, 0, maps.Count - 1);

    GUI.Label(new Rect(10, 180, 300, 90), "Switch modee: " + EDIT_MODE);
    prefabselect = (int)GUI.HorizontalSlider(new Rect(10, 120, 150, 20), prefabselect, 0, prefabs.Count - 1);
    //GUI.Label(new Rect(10, 190, 300, 90), "Layer " + current_map.layers[0].objects.Length);

    if (GUI.Button(new Rect(10, 200, 150, 10), "Switch mode +1"))
    {
        EDIT_MODE += 1;
    }
    if (GUI.Button(new Rect(10, 230, 150, 10), "Switch mode -1"))
    {
        EDIT_MODE -= 1;
    }
    GUI.Label(new Rect(10, 180, 300, 90), "Switch modee: " + EDIT_MODE);
    Debug();
    */
#endregion

#region EDITOR_BRUSH_MODES
/*
 * ADDED ON 23/03/2019
    Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    mousePosition = Snap(mousePosition);
    switch (EDIT_MODE)
    {
        case EDITMODE.BRUSH:

            Vector2Int mappos = snapvec(mousePosition);
            if (Input.GetMouseButton(0))
            {
                if (current_map.layers == null)
                    return;
                if (current_map.layers[0].objects[mappos.x, mappos.y] != null)
                    return;
                GameObject obj = Instantiate(prefabs[prefabselect], mousePosition, Quaternion.identity);

                o_collidableobject collidable = obj.GetComponent<o_collidableobject>();
                if (collidable != null)
                {
                    float zposplacement = zposition * boxsize;
                    collidable.positioninworld = new Vector3(mousePosition.x, mousePosition.y + zposplacement, zposition);
                }
                i++;
                string nom = "Thing " + i;
                obj.name = nom;
                AssignParent("Tiles", obj.gameObject);
                print(current_map.layers);
                current_map.layers[0].objects[mappos.x, mappos.y] = collidable;
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                o_npcharacter chara = Instantiate(charactersSpawnlist[0].gameObject, mousePosition, Quaternion.identity).GetComponent<o_npcharacter>();
                float zposplacement = zposition * boxsize;
                chara.positioninworld = new Vector3(mousePosition.x, mousePosition.y + zposplacement, zposition);
                i++;
                string nom = "character " + i;
                chara.name = nom;
                //chara.SetShadowPos();
                AssignParent("Entities", chara.gameObject);
            }

    break;

        case EDITMODE.PROPERTIES:
            if (Input.GetMouseButtonDown(1))
            {
                Collider2D col = Physics2D.OverlapBox(mousePosition, new Vector2(20, 20), 0);
                if (col != null)
                {
                    prpo = col.gameObject.GetComponent<o_collidableobject>();
                    if (prpo != null)
                    {
                        print("dgfd");
                        debug_mode = true;
                    }
                    else
                        debug_mode = false;
                }
                else
                    debug_mode = false;
            }
            if (debug_mode)
            {
                if(prpo)
                    EditProperties(prpo);
            }

            break;
    }*/
#endregion

#region GET_TILE_DECOR_CODE
/*
Added on 11/05/2019
Was meant to save tiledata from the tileobjects in the map

if (obj.GetComponent<o_tile>())  //This will be spawned in a different section
{
    if (obj.GetComponent<SpriteRenderer>().sprite == null)
        continue;
    mapfil.graphicTiles.Add(
        new s_map.s_block(obj.GetComponent<SpriteRenderer>().sprite.name, 
        obj.transform.position, 
        (s_map.s_block.LAYER)obj.GetComponent<o_tile>().layer));
    continue;
}*/
#endregion

/*
OLD LEVEL DATABASE CLASS
ADDED ON 29/05/2019

public class s_leveldatabase : MonoBehaviour {

    public s_map current_map;
    public List<s_map> maps = new List<s_map>();
    s_leveledit led;

    private void Awake()
    {

        string path = @"C:\Users\hamza\Own Games\Unity\InDevelopment\Codename - Magnum Foundation\Assets\Levels.txt";
        string file = JsonUtility.ToJson(maps[0]);
        //FileStream fs = new FileStream("Levels.txt", FileMode.Create);
        StreamWriter st = new StreamWriter(path, true);
        st.WriteLine(file);
        st.Close();
                //AssetDatabase.ImportAsset("Levels.txt");

        StreamReader reader = new StreamReader(path);
        Debug.Log(reader.ReadToEnd());
        reader.Close();

        
        led = GetComponent<s_leveledit>();
    }

    void SaveMap()
    {
    }
}
*/

/*
Added 29/05/2019
Old class for saving levels
public class s_save : MonoBehaviour {

    public GameObject loadedLevel;
    public GameObject res;

    GameObject targetLevel;

    struct save_dat
    {
        public List<trigger_obj> triggers;
        public List<o_collidableobject> collidableobjects;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            SaveData();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            LoadData();
        }
    }

    public void SaveData()
    {
        if (PrefabUtility.FindPrefabRoot(res) == null)
        {
            PrefabUtility.CreatePrefab("Assets/Levels/" + "Obj.prefab", loadedLevel);
        }
        else
        {
            PrefabUtility.ReplacePrefab(loadedLevel, res);
        }
    }

    public void LoadData()
    {
        if (PrefabUtility.FindPrefabRoot(res) == null)
        {
            PrefabUtility.CreatePrefab("Assets/Levels/" + "Obj.prefab", loadedLevel);
        }
        else
        {
            loadedLevel = PrefabUtility.FindPrefabRoot(res);
        }
    }
}
*/

#region ZONE
/*
Added on 29/05/2019
Zones were meant to have a number of maps associated with them, every creature would be in a zone and would be deposessed if they were outside of their zone

public class s_zone {

    public List<s_map> maps = new List<s_map>();
}

[System.Serializable]
public class s_zonedata
{
    public List<character_data> characterdat = new List<character_data>();
    public List<trigger_data> triggerdat = new List<trigger_data>();

    public class trigger_data
    {
        public int evnum;
        public string map;
        public bool isstatic;
    }
    public class character_data
    {
        public bool isdefaeted;
    }

}
*/
#endregion

#region TELEPORTER
/*
Added on 29/05/2019
Was the old teleportation object before triggers took up the purpose

public class o_maptransition : s_object {
    
    [System.Serializable]
    public struct s_flagcheck
    {
        public int flagCondition;
        public string mapname;
        public s_flagcheck(int flagCondition, string mapname)
        {
            this.flagCondition = flagCondition;
            this.mapname = mapname;
        }
    }

    public string sceneToTransferTo;
    public string areaInScene;
    public string[] allowedCharacters;
    public Vector2 position;
    BoxCollider2D col;
    public string flagcheck;
    int flagevent;
    public s_flagcheck[] flags;

    private new void Start()
    {
        flagevent = 0;
        collision = GetComponent<BoxCollider2D>();
    }

    private new void Update()
    {
        base.Update();
        Collider2D c = IfTouchingGetCol(collision, "o_character");
        if (c != null)
        {
            if (c.GetComponent<o_character>() != null)
            {
                Transition(sceneToTransferTo);
                if (allowedCharacters != null || allowedCharacters.Length == 0)
                {
                    foreach (string na in allowedCharacters)
                    {
                        if (na == c.name)
                            Transition(sceneToTransferTo,areaInScene);
                    }
                }
                else
                    Transition(sceneToTransferTo, areaInScene);
            }
        }
    }

    public void Transition(string scene)
    {
        //s_globals.SaveData();
        s_leveledit led = GameObject.Find("General").GetComponent<s_leveledit>();

        o_plcharacter pl = GameObject.Find("Player").GetComponent<o_plcharacter>();
        if (pl.host != null)
            if (pl.host.WorldsToAcess.Find(x => x == scene) == null)
            {
                o_character hos = pl.host;
                pl.Depossess();
            }

        led.TriggerSpawn(scene, position);
        //Call some global stuff 
    }
    public void Transition(string scene, string area)
    {
        //s_globals.SaveData();
        s_leveledit led = GameObject.Find("General").GetComponent<s_leveledit>();

        o_plcharacter pl = GameObject.Find("Player").GetComponent<o_plcharacter>();
        if (pl.host != null)
            if (pl.host.WorldsToAcess.Find(x => x == scene) == null)
            {
                o_character hos = pl.host;
                pl.Depossess();
            }

        TextAsset ta = led.jsonMaps.Find(x => x.name == area);
        s_map m = JsonUtility.FromJson<s_map>(ta.text);

        s_map.s_tileobj o = m.tilesdata.Find(x => x.TYPENAME == "teleport_object" && x.name == area);
        led.TriggerSpawn(scene, new Vector2(o.teleportpos.x, o.teleportpos.y));
        //Call some global stuff 
    }

}
*/
#endregion

#region SPAWN_OBJECT
/*
ADDED ON 01/06/2019

public void SpawnObj(Vector2 pos)
{
    s_object obje = Instantiate(prefab, pos, Quaternion.identity).GetComponent<s_object>();
    obje.positioninworld = new Vector3(pos.x, pos.y);
}
*/
#endregion

#region LOAD_TEMPMAP_FOR_TELEPORTATION_LOCATION
/*
ADDED ON 01/06/2019
Meant to be code for loading a tempoary map so you could easily put in a spawn area

public void LoadTempMap(s_map mapdat)
{
    //current_area = nam;
    GameObject levobj = GameObject.Find("TempLevel");

    GameObject triggerIG = GameObject.Find("TempTriggers");
    GameObject entityIG = GameObject.Find("TempEntities");
    GameObject tileIG = GameObject.Find("TempTiles");

    GameObject mapn = SceneLevelObject;
    o_character[] objectsInMap = null;
    o_trigger[] triggersInMap = null;
    o_generic[] tilesInMap = null;

    tilesInMap = tileIG.GetComponentsInChildren<o_generic>();
    objectsInMap = entityIG.GetComponentsInChildren<o_character>();
    triggersInMap = triggerIG.GetComponentsInChildren<o_trigger>();

    if (triggersInMap != null)
    {
        foreach (o_trigger obj in triggersInMap)
        {
            if (InEditor)
                DestroyImmediate(obj.gameObject, true);
            else
                DespawnObject(obj);

        }
    }

    if (objectsInMap != null)
    {
        foreach (s_object obj in objectsInMap)
        {
            if (InEditor)
                DestroyImmediate(obj.gameObject, true);
            else
                DespawnObject(obj);
        }
    }

    foreach (s_object obj in tilesInMap)
    {
        if (obj.name == "SpawnPoint")
        {
            continue;
        }
        else
        {
            DestroyImmediate(obj.gameObject, true);
            continue;
        }
    }

    for (int i = 0; i <
        mapdat.triggerdata.Count;
        i++)
    {
        GameObject trigObj = Instantiate(FindOBJ("Trigger"), new Vector2(mapdat.triggerdata[i].pos_x, mapdat.triggerdata[i].pos_y), Quaternion.identity);
        o_trigger trig = trigObj.GetComponent<o_trigger>();
        if (trig == null)
            continue;

        trig.Events.ev_Details = mapdat.triggerdata[i].listofevents;
        if (mapdat.triggerdata[i].util != null)
        {
            string n = mapdat.triggerdata[i].util.GetType().ToString();
            switch (n)
            {
                case "util_shop":
                    //TODO: LOAD FROM RESOURCES
                    break;
            }
        }

        trig.transform.SetParent(triggerIG.transform);
    }
    for (int i = 0; i < mapdat.objectdata.Count; i++)
    {
        o_character trig = Instantiate(prefabs[1], new Vector2(mapdat.objectdata[i].pos_x, mapdat.objectdata[i].pos_y), Quaternion.identity).GetComponent<o_character>();
        trig.positioninworld = new Vector3(mapdat.objectdata[i].pos_x, mapdat.objectdata[i].pos_x, 1);
        trig.transform.SetParent(entityIG.transform);
    }
    for (int i = 0; i < mapdat.tilesdata.Count; i++)
    {
        o_generic trig = Instantiate(prefab, new Vector2(mapdat.tilesdata[i].pos_x, mapdat.tilesdata[i].pos_y), Quaternion.identity).GetComponent<o_generic>();
        trig.collision_type = (COLLISION_T)mapdat.tilesdata[i].enumthing;
        trig.positioninworld = new Vector3(mapdat.tilesdata[i].pos_x, mapdat.tilesdata[i].pos_y, 1);
        trig.transform.position = new Vector3(mapdat.tilesdata[i].pos_x, mapdat.tilesdata[i].pos_y, 1);
        trig.transform.SetParent(tileIG.transform);
    }

}
*/
#endregion