using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.IO;

/*
[CustomEditor(typeof(s_leveledit))]
public class ed_level : EditorWindow {

    int i = 0;
    bool erase = false;
    bool floodfill = false;

    bool isfilling = false;
    bool isactive = false;

    string dir;
    int layer = 0;
    s_leveledit ed;
    GameObject giz; GameObject go;
    Vector2 calculatedmouse;
    GameObject levelobj;
    GameObject[] brushes_blocks;
    GameObject spawnpoint;
    
    COLLISION_T CollisionType;

    BoxCollider2D gizbox;
    GameObject im;
    Tool lasttool = Tool.None;
    s_map.s_block.LAYER la;
    bool mousedown = false;

    //Pixel's Ultra Propetary Plane Editing Tool
    [MenuItem("Brownie/PUPPET")]
    static void init()
    {
        GetWindow<ed_level>("PUPPET");
    }

    void LoadStuff()
    {
        isactive = true;
        ed = GameObject.Find("General").GetComponent<s_leveledit>();
        brushes_blocks = new GameObject[ed.objPoolDatabase.Count];
        for (int i = 0; i< ed.objPoolDatabase.Count; i++)
        {
            s_pooler_data pd = ed.objPoolDatabase[i];
            brushes_blocks[i] = pd.gameobject;
        }
        //brushes_blocks = Resources.LoadAll<GameObject>("Blocks");
    }

    void OldFloodFill(s_object selected_point)
    {
        List<s_object> openList = new List<s_object>();
        List<s_object> closeList = new List<s_object>();

        s_object[] obj = new s_object[4];
        int la = 0;
        if (selected_point.GetComponent<o_tile>())
            la = 1;

        Vector2[] pos = {
            new Vector2(0,20),
            new Vector2(0,-20),
            new Vector2(20,0),
            new Vector2(-20,0)
        };

        for (int i =0; i < obj.Length; i++) {

            //1 0
            obj[i] = GetObjectFromMouse(giz.transform.position + (Vector3)pos[i], la).GetComponent<s_object>();
            if (obj == null)
                continue;
            if (!closeList.Contains(obj[i]))
                openList.Add(obj[i]);

        }

        //selected_point.GetComponent<o_tile>()
        if (im.GetComponent<o_tile>())
        {
            foreach (s_object o in obj) {
                if (o == null) {
                    if (selected_point != null)
                        continue;
                    else {

                        //GameObject lo = Instantiate(im, new Vector3(0,20), Quaternion.identity);
                        //lo.name = im.name;
                    }
                }
                o_tile t = o.GetComponent<o_tile>();
                if (!t)
                    continue;

                t.SpirteRend = im.GetComponent<o_tile>().SpirteRend;
            }
        }
        if (selected_point.GetComponent<o_tile>())
        {

        }

    }

    void FloodFill(GameObject selected_point, s_object selected_blk)
    {
        Vector2 targ = new Vector2(0,0);
        List<Vector2> openList = new List<Vector2>();
        List<Vector2> closeList = new List<Vector2>();
        
        Vector2[] pos = {
            new Vector2(0,20),
            new Vector2(0,-20),
            new Vector2(20,0),
            new Vector2(-20,0),
            new Vector2(0,0)
        };

        Vector2 origin = giz.transform.position;
        openList.Add(origin);
        int test = 0;

        while (openList.Count != 0)
        {
            for (int i = 0; i < openList.Count; i++)
            {
                if (closeList.Contains(openList[i]))
                    continue;
                //Picks anything on the top of the list and goes through that
                targ = openList[i];
                openList.Remove(targ);
                closeList.Add(targ);
                break;
            }

            if (targ != null)
                origin = targ;
            if (!erase)
                PlaceBlock(origin, go);
            else
                DestroyImmediate(GetObjectFromMouse(origin, layer));
           

            for (int i = 0; i < pos.Length; i++)
            {
                //Set Boundaries
                if ((origin + pos[i]).x > (ed.mapDat.mapsize.x * 20))
                    continue;
                if ((origin + pos[i]).y > (ed.mapDat.mapsize.y * 20))
                    continue;
                if ((origin + pos[i]).x < 0)
                    continue;
                if ((origin + pos[i]).y < 0)
                    continue;

                //Check around the currently selected node
                GameObject go = GetObjectFromMouse(origin + pos[i], layer);
                if (erase)
                {
                    if (go != null && !closeList.Contains(origin + pos[i]))
                    {
                        if (!openList.Contains(origin + pos[i]))
                            openList.Add(origin + pos[i]);
                        //Does the calculation again
                        //go = GetObjectFromMouse(origin + pos[i], layer);
                    }
                }
                else
                {
                    if (go == null && !closeList.Contains(origin + pos[i]))
                    {
                        if (!openList.Contains(origin + pos[i]))
                            openList.Add(origin + pos[i]);
                        //Does the calculation again
                        //go = GetObjectFromMouse(origin + pos[i], layer);
                    }
                }
            }
            //yield return new WaitForSeconds(Time.deltaTime);
            test++;
            if (test > 200)
                break;
        }
    }

    GameObject GetObjectFromMouse(Vector2 pos, int l)
    {
        ed = GameObject.Find("General").GetComponent<s_leveledit>();
        GameObject mapn = ed.SceneLevelObject;
        s_object[] tilesInMap = null;
        o_generic[] colInMap = null;
        o_character[] objectsInMap = null;
        o_itemObj[] itemsInMap = null;
        s_object[] triggerInMap = null;

        itemsInMap = mapn.transform.Find("Items").GetComponentsInChildren<o_itemObj>();
        objectsInMap = mapn.transform.Find("Entities").GetComponentsInChildren<o_character>();
        triggerInMap = mapn.transform.Find("Triggers").GetComponentsInChildren<s_object>();
        tilesInMap = mapn.transform.Find("Tiles").GetComponentsInChildren<o_tile>();
        colInMap = mapn.transform.Find("Tiles").GetComponentsInChildren<o_generic>();

        List<s_map.s_tileobj> tilelist = new List<s_map.s_tileobj>();
        Vector3 p = pos;
        //Vector2Int p = new Vector2Int((int)pos.x / 25, (int)pos.y / 25);
        switch (l)
        {
            case 0:
                foreach (o_generic o in colInMap)
                {
                    Vector3 ov = o.transform.position;
                    if (new Vector3(ov.x, ov.y, 0) == new Vector3(p.x, p.y, 0))
                        if (o.GetComponent<o_generic>())
                            return o.gameObject;
                }
                break;
            case 1:

                foreach (s_object o in tilesInMap)
                {
                    if (o.transform.position == new Vector3(p.x, p.y, 0))
                        if (o.GetComponent<o_tile>())
                            if(o.GetComponent<o_tile>().layer == (int)la)
                            return o.gameObject;
                }
                break;

            case 2:

                foreach (o_character o in objectsInMap)
                {
                    if (o.transform.position == new Vector3(p.x, p.y, 0))
                        return o.gameObject;
                }
                break;

            case 3:

                foreach (o_itemObj o in itemsInMap)
                {
                    if (o.transform.position == new Vector3(p.x, p.y, 0))
                        return o.gameObject;
                }
                break;
            case 4:

                foreach (s_object o in triggerInMap)
                {
                    if (o.transform.position == p)
                        return o.gameObject;
                }
                break;
        }
        return null;
    }

    void PlaceBlock(Vector3 position, GameObject go)
    {
        if (im == null)
            return;

        i++;
        if (im.name == "SpawnPoint")
        {
            spawnpoint.transform.position = position;
            spawnpoint.transform.SetParent(levelobj.transform.Find("Tiles"));
            return;
        }

        if (im.GetComponent<o_tile>())
        {
            if (levelobj.transform.Find("Tiles") != null)
            {

                if (go != null)
                {
                    GameObject lotex = Instantiate(im, position, Quaternion.identity);
                    SpriteRenderer re = lotex.GetComponent<SpriteRenderer>();
                    lotex.GetComponent<SpriteRenderer>().sprite = im.GetComponent<SpriteRenderer>().sprite;
                    lotex.name = im.name;
                    lotex.transform.SetParent(levelobj.transform.Find("Tiles"));
                    return;
                }
                for (int y = 0; y < 5; y++)
                {
                    for (int x = 0; x < 5; x++)
                    {
                        GameObject tempgo = GetObjectFromMouse(calculatedmouse + new Vector2(20 * x, 20 * y), layer);
                        if (tempgo != null)
                        {
                            SpriteRenderer re = tempgo.GetComponent<SpriteRenderer>();
                            if (tileBatch[x, -y + 4] == null)
                                continue;
                            re.sprite = tileBatch[x, -y + 4];
                            tempgo.GetComponent<o_tile>().layer = (int)la;
                            tempgo.GetComponent<o_tile>().Intialize();
                        }
                        else
                        {
                            if (tileBatch[x, -y + 4] == null)
                                continue;
                            GameObject lotex = Instantiate(im, calculatedmouse + new Vector2(20 * x, 20 * y), Quaternion.identity);
                            SpriteRenderer re = lotex.GetComponent<SpriteRenderer>();
                            lotex.GetComponent<SpriteRenderer>().sprite = tileBatch[x, -y + 4];
                            lotex.name = im.name;
                            lotex.transform.SetParent(levelobj.transform.Find("Tiles"));
                        }
                    }
                }
            }
        }

        if (im.GetComponent<o_generic>())
            if (levelobj.transform.Find("Tiles") != null)
            {
                if (go != null)
                {
                    if (go.name == "Tile" || go.name == "Tile(Clone)")
                    {
                        o_generic sel = go.GetComponent<o_generic>();
                        sel.collision_type = CollisionType;

                        SpriteRenderer spr = go.GetComponent<SpriteRenderer>();

                        switch (sel.collision_type)
                        {
                            case COLLISION_T.CLIMBING:
                                spr.color = sel.climbingColour;
                                break;
                            case COLLISION_T.DAMAGE:
                                break;
                            case COLLISION_T.WALL:
                                spr.color = new Color32(255, 0, 255, 150);
                                break;
                            case COLLISION_T.MOVING_PLATFORM:
                                spr.color = Color.blue;
                                break;
                            case COLLISION_T.FALLING:
                                spr.color = sel.fallingColour;
                                break;
                            case COLLISION_T.DITCH:
                                spr.color = sel.ditchColour;
                                break;
                            case COLLISION_T.FALLING_ON_LAND:
                                spr.color = sel.fallingOnGroundColour;
                                break;
                        }
                    }
                    return;
                }
            }
            
        if (im.GetComponent<o_maptransition>())
            if (levelobj.transform.Find("Tiles") != null)
            {
                if (go != null)
                {
                    return;
                }
            }

        if (im.GetComponent<o_trigger>() || im.GetComponent<s_utility>())
            if (levelobj.transform.Find("Triggers") != null)
            {
                if (go != null)
                {
                    return;
                }
            }

        if (im.GetComponent<o_itemObj>())
            if (levelobj.transform.Find("Items") != null)
            {
                if (go != null)
                {
                    return;
                }
            }

        if (go != null)
        {
            if (im.GetComponent<o_character>())
                return;
            if (im.GetComponent<o_generic>())
                return;
            if (im.GetComponent<o_itemObj>())
                return;
            if (im.GetComponent<o_trigger>() || im.GetComponent<s_utility>())
                return;
        }

        GameObject lo = Instantiate(im, position, Quaternion.identity);
        lo.name = im.name;
        //mapsiz[(int)calculatedmouse.x, (int)calculatedmouse.y] = lo;

        s_object namer = lo.GetComponent<s_object>();
        if (namer != null)
            namer.ID = im.gameObject.name;

        if (im.GetComponent<o_character>())
            if (levelobj.transform.Find("Entities") != null)
            {
                lo.gameObject.name = lo.gameObject.name + "_" + i;
                lo.transform.SetParent(levelobj.transform.Find("Entities"));
                return;
            }

        if (im.GetComponent<o_trigger>() || im.GetComponent<s_utility>())
            if (levelobj.transform.Find("Triggers") != null)
            {
                lo.gameObject.name = lo.gameObject.name + "_" + i;
                lo.transform.SetParent(levelobj.transform.Find("Triggers"));
                return;
            }

        if (lo.GetComponent<o_itemObj>())
            if (levelobj.transform.Find("Items") != null)
            {
                lo.transform.SetParent(levelobj.transform.Find("Items"));
                lo.gameObject.name = lo.gameObject.name;
                lo.transform.SetParent(levelobj.transform.Find("Items"));

                return;
            }

        if (im.GetComponent<o_generic>())
            if (levelobj.transform.Find("Tiles") != null)
            {
                o_generic sel = lo.GetComponent<o_generic>();
                sel.collision_type = CollisionType;

                SpriteRenderer spr = lo.GetComponent<SpriteRenderer>();

                switch (sel.collision_type)
                {
                    case COLLISION_T.CLIMBING:
                        spr.color = sel.climbingColour;
                        break;
                    case COLLISION_T.DAMAGE:
                        break;
                    case COLLISION_T.WALL:
                        spr.color = new Color32(255, 0, 255, 150);
                        break;
                    case COLLISION_T.MOVING_PLATFORM:
                        spr.color = Color.blue;
                        break;
                    case COLLISION_T.FALLING:
                        spr.color = sel.fallingColour;
                        break;
                    case COLLISION_T.DITCH:
                        spr.color = sel.ditchColour;
                        break;
                    case COLLISION_T.FALLING_ON_LAND:
                        spr.color = sel.fallingOnGroundColour;
                        break;
                }
            }
        lo.transform.SetParent(levelobj.transform.Find("Tiles"));
    }

    void MouseEvent(Event e)
    {
        if (isactive)
        {
            GameObject go = GetObjectFromMouse(calculatedmouse, layer);
            if (mousedown)
            {
                if (calculatedmouse.x < 0 || calculatedmouse.y < 0)
                    return;
                if (calculatedmouse.x > 2000 || calculatedmouse.y > 2000)
                    return;
                if (erase)
                {
                    if (e.button == 0)
                    {
                        go = GetObjectFromMouse(calculatedmouse, layer);
                        if (go != null)
                        {
                            if (go != giz)
                                DestroyImmediate(go);
                        }
                    }
                }
                else
                {
                    if (e.button == 0)
                    {
                        //Debug.Log(go);
                        if (floodfill)
                        {
                            if (!isfilling)
                            {
                                //ed.StartCoroutine(ed.FloodFill(giz.transform.position, erase, im, layer));
                                FloodFill(go, im.GetComponent<s_object>());
                                //isfilling = true;
                            }
                        }
                        else
                        {
                            PlaceBlock(giz.transform.position, go);
                        }
                    }
                }

            }
        }
    }

    void SceneGUI(SceneView sv)
    {
        if (giz == null)
            giz = GameObject.Find("Gizmo");

        if (gizbox == null)
            gizbox = giz.GetComponent<BoxCollider2D>();

        //if (im == null)
        //im = GameObject.Find("Prefabex");

        Event e = Event.current;
        Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
        Vector2 mousepos = ray.origin;

        calculatedmouse = new Vector2(Mathf.Round(mousepos.x/ 20) * 20,
            Mathf.Round(mousepos.y / 20) * 20);

        if (ed != null)
        {
            if (ed.mapDat != null)
            {
                if (calculatedmouse.x > (ed.mapDat.mapsize.x - 1) * 20)
                {
                    calculatedmouse.x = (ed.mapDat.mapsize.x - 1) * 20;
                }
                if (calculatedmouse.y > (ed.mapDat.mapsize.y - 1) * 20)
                {
                    calculatedmouse.y = (ed.mapDat.mapsize.y - 1) * 20;
                }
                if (calculatedmouse.x < 0)
                {
                    calculatedmouse.x = 0;
                }
                if (calculatedmouse.y < ed.mapDat.mapsize.y)
                {
                    calculatedmouse.y = 0;
                }

            }
        }
        giz.transform.position = calculatedmouse;


        int contrID = GUIUtility.GetControlID(FocusType.Passive);
        if (e.type == EventType.Layout)
        {
            HandleUtility.AddDefaultControl(contrID);
        }

        switch (e.type)
        {
            case EventType.MouseDown:
                mousedown = true;
                break;
            case EventType.MouseUp:
                mousedown = false;
                break;

            case EventType.KeyDown:

                if (e.keyCode == KeyCode.S)
                {
                    Debug.Log(giz.transform.position);
                }
                break;

        }
        MouseEvent(e);
    }
    
    private void OnEnable()
    {
        SceneView.onSceneGUIDelegate += SceneGUI;
        lasttool = Tools.current;
    }

    void SceneGUI()
    {
        Event e = Event.current;
    }
    
    public void NewMap()
    {
        GameObject mapn = ed.SceneLevelObject;
        o_character[] objectsInMap = null;
        s_utility[] triggersInMap = null;
        s_object[] tilesInMap = null;
        o_itemObj[] itemsInMap = null;

        itemsInMap = mapn.transform.Find("Items").GetComponentsInChildren<o_itemObj>();
        tilesInMap = mapn.transform.Find("Tiles").GetComponentsInChildren<s_object>();
        objectsInMap = mapn.transform.Find("Entities").GetComponentsInChildren<o_character>();
        triggersInMap = mapn.transform.Find("Triggers").GetComponentsInChildren<s_utility>();

        if (triggersInMap != null)
        {
            foreach (s_utility obj in triggersInMap)
            {
                DestroyImmediate(obj.gameObject, true);
            }
        }

        if (objectsInMap != null)
        {
            foreach (s_object obj in objectsInMap)
            {
                DestroyImmediate(obj.gameObject, true);
            }
        }

        if (itemsInMap != null)
        {
            foreach (o_itemObj obj in itemsInMap)
            {
                DestroyImmediate(obj.gameObject, true);
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
    }

    GameObject FindBrush(string nameObj)
    {
        GameObject result = null;
        foreach (GameObject b in brushes_blocks)
        {
            if (nameObj == b.name)
                result = b;
        }
        return result;
    }
    

    public void LoadMap(s_map mapdat)
    {

        
        //current_area = nam;

        GameObject mapIG = new GameObject();
        GameObject triggerIG = new GameObject();
        GameObject entityIG = new GameObject();
        GameObject tileIG = new GameObject();

        mapIG.name = nam;
        triggerIG.name = "Triggers";
        entityIG.name = "Entities";
        tileIG.name = "Tiles";
        GameObject.Find("Player").GetComponent<o_character>().positioninworld = new Vector3(mapdat.spawnPoint.x, mapdat.spawnPoint.y);

        tileIG.transform.SetParent(mapIG.transform);
        triggerIG.transform.SetParent(mapIG.transform);
        entityIG.transform.SetParent(mapIG.transform);

        for (int i = 0; i < mapdat.triggerdata.Count; i++)
        {
            o_trigger trig = Instantiate(, new Vector2(mapdat.triggerdata[i].pos_x, mapdat.triggerdata[i].pos_y), Quaternion.identity).GetComponent<o_trigger>();
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
            s_map.s_chara objdata = mapdat.objectdata[i];
            o_character trig = Instantiate(FindBrush("Stairs"), new Vector2(mapdat.objectdata[i].pos_x, mapdat.objectdata[i].pos_y), Quaternion.identity).GetComponent<o_character>();
            trig.positioninworld = new Vector3(mapdat.objectdata[i].pos_x, mapdat.objectdata[i].pos_x, 1);
            trig.transform.SetParent(entityIG.transform);
        }
        for (int i = 0; i < mapdat.tilesdata.Count; i++)
        {
            s_map.s_tileobj tile = mapdat.tilesdata[i];
            o_collidableobject trig = Instantiate(FindBrush("Stairs"), new Vector2(tile.pos_x, tile.pos_y), Quaternion.identity).GetComponent<o_collidableobject>();
            trig.collision_type = (o_collidableobject.COLLISION_T)tile.enumthing;
            trig.positioninworld = new Vector3(tile.pos_x, tile.pos_x, 1);
            trig.transform.SetParent(tileIG.transform);
        }
        
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
    }

    private void OnGUI()
    {
        if (brushes_blocks == null)
            LoadStuff();
        if (ed == null)
            if (GameObject.Find("General"))
            ed = GameObject.Find("General").GetComponent<s_leveledit>();
        
        if (ed != null)
        {
            if (levelobj == null)
                levelobj = ed.SceneLevelObject;
            if (!isfilling)
            {
                if (!erase)
                {
                    if (GUI.Button(new Rect(35 * 11, 10, 160, 40), "Brush"))
                        erase = true;
                }
                else
                {
                    if (GUI.Button(new Rect(35 * 11, 10, 160, 40), "Erase"))
                        erase = false;
                }

                if (!floodfill)
                {
                    if (GUI.Button(new Rect(35 * 16, 10, 160, 40), "FLOOD FILL: OFF"))
                        floodfill = true;
                }
                else
                {
                    if (GUI.Button(new Rect(35 * 16, 10, 160, 40), "FLOOD FILL: ON"))
                        floodfill = false;
                }

                if (GUI.Button(new Rect(35 * 28, 50, 80, 40), "Get tile data"))
                {
                    ed.CreatePolygon();
                    //ed.GetTileDat();
                }
                if (GUI.Button(new Rect(35 * 18.5f, 50, 80, 40), "New"))
                {
                    ed.NewMap();
                }
                if (GUI.Button(new Rect(35 * 13.5f, 50, 80, 40), "Save As"))
                {
                    dir = EditorUtility.SaveFilePanel("Save Json level file", "Assets/Levels/", "Unnamed", "txt");
                    if (dir != string.Empty)
                        ed.SaveMap(dir);
                }
                if (GUI.Button(new Rect(35 * 11, 50, 80, 40), "Save"))
                {
                    if(dir != null)
                        ed.SaveMap(dir);
                }
                if (GUI.Button(new Rect(35 * 16, 50, 80, 40), "Load"))
                {
                    dir = EditorUtility.OpenFilePanel("Open Json level file", "Assets/Levels/", "");
                    if (dir != string.Empty)
                        ed.LoadMap(ed.JsonToObj(dir));
                }

                if (GUI.Button(new Rect(35 * 11f, 120, 80, 40), "Tiles : 0")) { layer = 0; }
                if (GUI.Button(new Rect(35 * 13.5f, 120, 80, 40), "Decor : 1"))
                {
                    layer = 1;
                }
                if (layer == 1)
                {
                    if (GUI.Button(new Rect(35 * 11f, 200, 80, 40), "Top"))
                    {
                        la = s_map.s_block.LAYER.TOP_LAYER;
                    }
                    if (GUI.Button(new Rect(35 * 13.5f, 200, 80, 40), "Middle"))
                    {
                        la = s_map.s_block.LAYER.MIDDLE_LAYER;
                    }
                    if (GUI.Button(new Rect(35 * 16.5f, 200, 80, 40), "Bottom"))
                    {
                        la = s_map.s_block.LAYER.BOTTOM_LAYER;
                    }
                }

                if (GUI.Button(new Rect(35 * 16f, 120, 80, 40), "Entities: 2")) { layer = 2; }
                if (GUI.Button(new Rect(35 * 18.5f, 120, 80, 40), "Items: 3")) { layer = 3; }
                if (GUI.Button(new Rect(35 * 21f, 120, 80, 40), "Triggers: 4")) { layer = 4; }
                EditorGUI.LabelField(new Rect(35 * 13f, 160, 160, 40), "Current Layer : " + layer);

                if (go)
                {
                    o_generic sel = go.GetComponent<o_generic>();
                    if (sel)
                    {
                        EditorGUI.LabelField(new Rect(35 * 15f, 180, 160, 40), "CurrentBlock char: " + go.GetComponent<o_generic>().character);
                        sel.collision_type = (COLLISION_T)EditorGUI.EnumPopup(
                            new Rect(35 * 17, 165, 80, 25),
                            sel.collision_type);

                        EditorGUI.LabelField(new Rect(35 * 17, 195, 210, 25), "Position: " + go.transform.position.x + ", " + go.transform.position.y);
                    }
                    else
                    {
                        EditorGUI.LabelField(new Rect(35 * 15f, 180, 160, 40), "CurrentBlock : " + go.name);
                    }
                }

                CollisionType = (COLLISION_T)EditorGUI.EnumPopup(
                    new Rect(35 * 17, 165, 80, 25),
                    CollisionType);

                ed.zone = GUI.TextArea(new Rect(35 * 14, 95, 80, 25), ed.zone);
                ed.mapscript = (o_trigger)EditorGUI.ObjectField(new Rect(35 * 17, 95, 80, 25), ed.mapscript, typeof(o_trigger), true);

                if (ed.mapDat != null)
                {
                    ed.mapsizeToKeep.x = EditorGUI.FloatField(new Rect(35 * 20, 95, 40, 25), ed.mapsizeToKeep.x);
                    ed.mapsizeToKeep.y = EditorGUI.FloatField(new Rect(35 * 22, 95, 40, 25), ed.mapsizeToKeep.y);
                    
                }
            }
            else
            {
                if (GUI.Button(new Rect(35 * 11, 10, 160, 40), "Cancel fill"))
                {
                    ed.StopAllCoroutines();
                    isfilling = false;
                }
            }
            
        }
        if (spawnpoint == null)
            spawnpoint = GameObject.Find("SpawnPoint");


        ed = GameObject.Find("Main Camera").GetComponent<s_leveledit>();
        GameObject mapn = ed.SceneLevelObject;
        s_object[] tilesInMap = null;
        
        tilesInMap = mapn.transform.Find("Tiles").GetComponentsInChildren<s_object>();
        Texture2D tex = new Texture2D(15, 15);
        Color32[] col = tex.GetPixels32(0);
        int ay = 0;
        for (int a = 0; a < col.Length; a++)
        {
            if (tilesInMap[a])
            {
                col[a] = new Color32(200, 0, 0, 255);
            }
            if (a % 200 == 0) {
                ay++;
            }
        }
        tex.SetPixels32(col, 0);
        tex.Apply(false);
        EditorGUI.ObjectField(new Rect(35 * 15, 140, 90, 90), tex, typeof(Texture2D), false);

        int i = 0;
        List<GameObject> obs = layerMatch();
        for (int y = 0; y < 10; y++)
        {
            for (int x = 0; x < 10; x++)
            {
                if (i < obs.Count)
                {
                    if (obs[i].GetComponent<SpriteRenderer>() != null)
                    {
                        if (obs[i].GetComponent<SpriteRenderer>().sprite != null)
                            if (GUI.Button(new Rect(10 + x * 35, 10 + y * 45, 30, 40), obs[i].GetComponent<SpriteRenderer>().sprite.texture))
                            {
                                im = obs[i];
                            }
                    }
                    else {
                        if (GUI.Button(new Rect(10 + x * 35, 10 + y * 45, 30, 40), obs[i].transform.Find("sprite_obj").gameObject.GetComponent<SpriteRenderer>().sprite.texture))
                        {
                            im = obs[i];
                        }
                    }
                }
                else
                {
                    if (GUI.Button(new Rect(10 + x * 35, 10 + y * 45, 30, 40), "???"))
                    {
                        im = null;
                    }
                }
                i++;
            }
        }
        
        if (im != null)
        {
            EditorGUI.LabelField(new Rect(35 * 13, 170, 90, 90), im.name);
        }

        /*
if (Selection.activeGameObject != null)
{

    ed = Selection.activeGameObject.GetComponent<s_leveledit>();
    if (ed != null)
    {
        if (GUI.Button(new Rect(10, 40, 150, 10), "Load"))
        {
            string dir = EditorUtility.OpenFilePanel("Open Json level file", "Assets/Levels/", "");
            ed.LoadMap(ed.JsonToObj(dir));
        }
        if (GUI.Button(new Rect(10, 70, 150, 10), "New"))
        {
            ed.NewMap();
        }
        if (GUI.Button(new Rect(10, 100, 150, 10), "Save"))
        {
            string dir = EditorUtility.SaveFilePanel("Save Json level file", "Assets/Levels/", "Unnamed", "txt");
            ed.SaveMap(dir);
        }
    }
}
    }

    private void OnDestroy()
    {
        isactive = false;
    }

    List<GameObject> layerMatch()
    {
        List<GameObject> listofObj = new List<GameObject>();
        
        switch (layer)
        {
            case 0:
                foreach (GameObject g in brushes_blocks)
                {
                    if (g == null)
                        continue;
                    if (g.GetComponent<o_generic>())
                        listofObj.Add(g);
                }
                break;

            case 1:
                foreach (GameObject g in brushes_blocks)
                {
                    if (g == null)
                        continue;
                    if (g.GetComponent<o_tile>())
                        listofObj.Add(g);
                }
                break;

            case 2:
                foreach (GameObject g in brushes_blocks)
                {
                    if (g == null)
                        continue;
                    if (g.GetComponent<o_character>())
                        listofObj.Add(g);
                }
                    break;

            case 3:
                foreach (GameObject g in brushes_blocks)
                {
                    if (g == null)
                        continue;
                    if (g.GetComponent<o_itemObj>())
                        listofObj.Add(g);
                }
                break;

            case 4:
                foreach (GameObject g in brushes_blocks)
                {
                    if (g == null)
                        continue;
                    if (g.GetComponent<o_trigger>() ||
                        g.GetComponent<s_utility>())
                        listofObj.Add(g);
                }
                break;
        }
        return listofObj;
    }

}

    */