using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;
using System.Runtime.Serialization.Formatters.Binary;
using MagnumFoudation;
using System;

[System.Serializable]
public struct ev_integer
{
    public int integer;
    public string integer_name;
}


public class s_leveledit : s_levelloader
{
    Dictionary<string, Queue<s_object>> objectPoolList = new Dictionary<string, Queue<s_object>>();
    string Lastzone;
    public Canvas canvas;
    
    int o = 0;  //For selecting enums

    public int id = 0;
    
    dat_save sa;

    int index = 0;
    
    s_triggerhandler triggerHand;

    private void Awake()
    {
        LevEd = this;
        canvas.gameObject.SetActive(true);
        InitializeLoader();
        Screen.SetResolution(1280, 720, false);
        InitializeGameWorld();
    }

    public void ChangeCharacterPossesData(string charname, bool possesed)
    {
        s_map.s_chara ch = mapDat.objectdata.Find(c => c.charname == charname);
        if (ch != null)
            ch.possesed = possesed;
    }
    public void ChangeCharacterPossesData(PDII_character chara, bool possesed)
    {
        if (chara.WorldsToAcess.Count == 0)
            return;
        if (chara == null)
            return;
        s_map m = maps.Find(x => x.name == chara.WorldsToAcess[0]);
        if (m == null)
            return;
        s_map.s_chara ch = m.objectdata.Find(c => c.charname == chara.name);
        if (ch != null)
            ch.possesed = possesed;
    }

    public void SetEntity(string nameOfObj, s_map.s_customType customval, string customName)
    {
        s_map.s_chara ch = mapDat.objectdata.Find(x => x.charname == nameOfObj);
        s_map.s_customType cust = ch.CustomTypes.Find(x=> x.name == customName);
        cust = new s_map.s_customType(customName, customval.type, customval.type2, customval.type3);
    }
    
    /*
    public s_map GetMap()
    {
        s_map mapfil = new s_map();
        if (mapscript != null)
            mapfil.mapscript = mapscript.name;
        GameObject mapn = SceneLevelObject;
        o_character[] objectsInMap = null;
        s_object[] triggersInMap = null;
        s_object[] tilesInMap = null;
        o_itemObj[] itemsInMap = null;

        itemsInMap = mapn.transform.Find("Items").GetComponentsInChildren<o_itemObj>();
        tilesInMap = mapn.transform.Find("Tiles").GetComponentsInChildren<s_object>();
        objectsInMap = mapn.transform.Find("Entities").GetComponentsInChildren<o_character>();
        triggersInMap = mapn.transform.Find("Triggers").GetComponentsInChildren<s_object>();
        List<s_map.s_trig> triggerlist = new List<s_map.s_trig>();
        List<s_map.s_chara> charalist = new List<s_map.s_chara>();
        List<s_map.s_tileobj> tilelist = new List<s_map.s_tileobj>();
        List<s_save_item> itemlist = new List<s_save_item>();
        mapfil.mapsize = new s_save_vector(mapsizeToKeep.x, mapsizeToKeep.y);

        mapfil.graphicTiles.Clear();
        GetTileDat(ref mapfil);
        //print(mapfil.graphicTiles.Count);

        foreach (s_object obj in triggersInMap)
        {
        }
        mapfil.triggerdata = triggerlist;
        mapfil.Map_Script = mapDat.Map_Script;
        mapfil.map_script_labels = mapDat.map_script_labels;

        foreach (s_object obj in objectsInMap)
        {
        }
        mapfil.objectdata = charalist;

        foreach (s_object obj in tilesInMap)
        {
            if (obj.name == "SpawnPoint")
                mapfil.spawnPoint = new s_save_vector(obj.transform.position.x, obj.transform.position.y);    //Make the spawnpoint value of the vector
            else
            {
                if (obj.name == "BoundTile")  //This will be spawned within the boundary rather than the tiles    
                {
                    continue;
                }
                if (obj.GetComponent<o_generic>())
                {
                    s_map.s_tileobj tilet = new s_map.s_tileobj(obj.transform.position, obj.ID, -1,
                    obj.GetComponent<o_generic>().character);
                    if (obj.ID == "teleport_object") {
                        o_generic co = obj.GetComponent<o_generic>();
                        tilet.enumthing = -1;
                        tilet.teleportpos = new s_save_vector(co.transform.position.x, co.transform.position.y);
                        tilet.name = obj.name;
                    }
                    if (obj.ID == "bound")
                    {
                        o_generic co = obj.GetComponent<o_generic>();
                        tilet.enumthing = -1;
                        tilet.teleportpos = new s_save_vector(co.transform.position.x, co.transform.position.y);
                        tilet.name = obj.name;
                        //tilet.scale = new s_save_vector(obj.transform.localScale.x, obj.transform.localScale.y);
                        tilelist.Add(tilet);
                        continue;
                    }
                    if (obj.GetComponent<BoxCollider2D>())
                        tilet.size = new s_save_vector(obj.GetComponent<BoxCollider2D>().size.x, obj.GetComponent<BoxCollider2D>().size.y);
                    tilelist.Add(tilet);
                    continue;
                }
                print(obj.name);
                s_map.s_tileobj tile = new s_map.s_tileobj(
                    obj.transform.position,
                    obj.ID,
                    (int)obj.GetComponent<o_generic>().collision_type,
                    obj.GetComponent<o_generic>().character);
                tile.size = new s_save_vector(obj.GetComponent<BoxCollider2D>().size.x, obj.GetComponent<BoxCollider2D>().size.y);
                tile.issolid = obj.GetComponent<o_generic>().issolid;
                tile.cannotPassChar = obj.GetComponent<o_generic>().characterCannot;
                tilelist.Add(tile);
            }
        }
        mapfil.tilesdata.AddRange(tilelist);

        foreach (o_itemObj obj in itemsInMap)
        {
        }
        mapfil.itemdat = itemlist;

        print(mapfil.tilesdata);
        
        return mapfil;
    }
    */
    

    public void SetItemData(string nameOfItem, bool dataSet)
    {
        s_save_item it = mapDat.itemdat.Find(x => x.name == nameOfItem);
        it.iscollected = dataSet;
    }

    #region EDITOR
    public override GameObject SetTrigger(s_map.s_trig trigger)
    {
        GameObject trigObj = null;
        Vector2 pos = new Vector2(trigger.pos_x, trigger.pos_y);

        if (InEditor)
            switch (trigger.util)
            {
                default:
                    trigObj = Instantiate(FindOBJ("Trigger"), pos, Quaternion.identity);
                    break;

                case "u_boundary":
                    trigObj = Instantiate(FindOBJ("GemBound"), pos, Quaternion.identity);
                    break;

                case "u_leemminigame":
                    trigObj = Instantiate(FindOBJ("LeemHost"), pos, Quaternion.identity);
                    break;

                case "u_feedinggame":
                    trigObj = Instantiate(FindOBJ("baby_birds"), pos, Quaternion.identity).gameObject;
                    break;

                case "u_delivery":
                    trigObj = Instantiate(FindOBJ("delivery_host"), pos, Quaternion.identity).gameObject;
                    break;

                case "u_yetifishing":
                    trigObj = Instantiate(FindOBJ("yeti_fishing"), pos, Quaternion.identity).gameObject;
                    break;

                case "u_collect":
                    trigObj = Instantiate(FindOBJ("seacollection"), pos, Quaternion.identity).gameObject;
                    break;
            }
        else
            switch (trigger.util)
            {
                default:
                    trigObj = SpawnObject<s_object>(FindOBJ("Trigger"), pos, Quaternion.identity).gameObject;
                    break;

                case "u_boundary":
                    trigObj = SpawnObject<s_object>(FindOBJ("GemBound"), pos, Quaternion.identity).gameObject;
                    break;

                case "u_leemminigame":
                    trigObj = SpawnObject<s_object>(FindOBJ("LeemHost"), pos, Quaternion.identity).gameObject;
                    break;

                case "u_feedinggame":
                    trigObj = SpawnObject<s_object>(FindOBJ("baby_birds"), pos, Quaternion.identity).gameObject;
                    break;

                case "u_delivery":
                    trigObj = SpawnObject<s_object>(FindOBJ("delivery_host"), pos, Quaternion.identity).gameObject;
                    break;

                case "u_yetifishing":
                    trigObj = SpawnObject<s_object>(FindOBJ("yeti_fishing"), pos, Quaternion.identity).gameObject;
                    break;

                case "u_collect":
                    trigObj = SpawnObject<s_object>(FindOBJ("seacollection"), pos, Quaternion.identity).gameObject;
                    break;
            }
        trigObj.transform.SetParent(triggersObj.transform);
        o_trigger trig = trigObj.GetComponent<o_trigger>();

        if (!InEditor)
            if (trigObj.GetComponent<s_object>() != null)
                if (trigObj.GetComponent<s_object>().rendererObj != null)
                    trigObj.GetComponent<s_object>().rendererObj.color = Color.clear;

        s_utility util = null;
        if (trig == null)
            util = trigObj.GetComponent<s_utility>();

        //trig.TRIGGER_T = mapdat.triggerdata[i].trigtye;
        if (trigger.util != null)
        {
            string n = trigger.util;
            switch (n)
            {
                case "u_boundary":
                    print("K");
                    trigObj.transform.GetChild(0).transform.localScale = new Vector3(trigger.trigSize.x, trigger.trigSize.y);
                   // trigObj.GetComponent<BoxCollider2D>().size = 20 * new Vector3(trigger.trigSize.x, trigger.trigSize.y);
                    trigObj.name = trigger.name;
                    u_boundary bo = trigObj.GetComponent<u_boundary>();
                    s_map.s_customType o = trigger.CustomTypes.Find(x => x.name == "GemReq");
                    s_map.s_customType flCh = trigger.CustomTypes.Find(x => x.name == "flagCheck");
                    s_map.s_customType flCond = trigger.CustomTypes.Find(x => x.name == "flagCond");
                    s_map.s_customType flName = trigger.CustomTypes.Find(x => x.name == "flagName");
                    s_map.s_customType flDesc = trigger.CustomTypes.Find(x => x.name == "flagDesc");
                    //print(o.type);
                    //print(o.name);

                    bo.description = flDesc.type3;
                    bo.gemRequire = o.type;
                    bo.checkFlag = flCh.type == 1 ? true : false;
                    if (bo.checkFlag)
                    {
                        bo.flagname = flName.type3;
                        bo.flagCheck = flCond.type;
                    }
                    break;

                case "u_leemminigame":
                    //print("K");
                    trigObj.name = trigger.name;
                    u_leemminigame lm1 = trigObj.GetComponent<u_leemminigame>();
                    s_map.s_customType lmb1 = trigger.CustomTypes.Find(x => x.name == "bound");
                    GameObject go1 = GameObject.Find(lmb1.type3);
                    if (go1 != null)
                        lm1.boundary = GameObject.Find(lmb1.type3).GetComponent<o_generic>();
                    break;

                case "u_collect":

                    u_collect collec = trigObj.GetComponent<u_collect>();
                    s_map.s_customType[] items = trigger.CustomTypes.FindAll(x => x.name == "reqItem").ToArray();
                    break;
                    
                    /*
                case "u_delivery":
                    print("K");
                    trigObj.name = trigger.name;
                    u_delivery lm = trigObj.GetComponent<u_delivery>();
                    s_map.s_customType[] customers = trigger.CustomTypes.FindAll(x => x.name == "char").ToArray();
                    s_map.s_customType[] customerItem = trigger.CustomTypes.FindAll(x => x.name == "charItem").ToArray();
                    List<npc_customer> customerObj = new List<npc_customer>();
                    for (int i =0; i < customers.Length; i++)
                    {
                        s_map.s_customType ch = customers[i];
                        npc_customer cu = GameObject.Find(ch.type3).GetComponent<npc_customer>();
                        cu.desiredItemName = customerItem[i].type3;
                        customerObj.Add(cu);
                    }
                    lm.characters = customerObj.ToArray();
                    break; 
                    
                    case "u_save":
                        trig.gameObject.AddComponent<u_save>();
                        break;

                    case "u_teleport":
                        u_teleport te = trig.gameObject.AddComponent<u_teleport>();
                        List<s_map.s_customType> aw = trigger.CustomTypes.FindAll(x => x.name == "area_warp");
                        s_map.s_customType un = trigger.CustomTypes.Find(x => x.name == "unlocked");
                        foreach (s_map.s_customType ct in aw)
                        {
                            u_teleport.teleporter_data tp = new u_teleport.teleporter_data((string)ct.type2, (string)ct.type3);

                            TextAsset ta = jsonMaps.Find(x => x.name == (string)ct.type2);
                            s_map m = JsonUtility.FromJson<s_map>(ta.text);
                            List<s_map.s_trig> tr = m.triggerdata.FindAll(x => x.util == "u_teleport");
                            s_map.s_trig findUnlocked = tr.Find(x => x.name == (string)ct.type3);

                            bool bl = (int)findUnlocked.CustomTypes.Find(b => b.name == "unlocked").type;
                            tp.unlocked = bl;
                            te.teleportarea.Add(tp);
                        }
                        break;

                    case "u_feedinggame":
                        u_feedinggame feed = trigObj.GetComponent<u_feedinggame>();
                        s_map.s_customType[] worms = trigger.CustomTypes.FindAll(x => x.name == "worm").ToArray();
                        foreach (s_map.s_customType ct in worms)
                        {
                            o_itemObj ob = GameObject.Find(ct.type3).GetComponent<o_itemObj>();
                            if (ob != null)
                                feed.wormPos.Add(ob);
                        }
                        break;

                
                case "u_yetifishing":

                    u_yetifishing fish = trigObj.GetComponent<u_yetifishing>();
                    s_map.s_customType[] yetc = trigger.CustomTypes.FindAll(x => x.name == "op").ToArray();
                    fish.yetiCharacters = new npc_yeti[yetc.Length];
                    for (int a = 0; a < fish.yetiCharacters.Length; a++)
                    {
                        GameObject gyo = GameObject.Find(yetc[a].type3);
                        if (gyo != null)
                            fish.yetiCharacters[a] = GameObject.Find(yetc[a].type3).GetComponent<npc_yeti>();
                    }
                    break;
                    */
            }
        }
        if (trig != null)
        {
            trig.ev_num = 0;    //TODO: IF THE TRIGGER DOES NOT STATICALLY STORE ITS EVENT NUMBER, SET IT TO 0

            if (trigger.name != "")
                trig.name = trigger.name;
            trig.isactive = false;
            trig.TRIGGER_T = trigger.trigtye;
            trig.LabelToJumpTo = trigger.labelToJumpTo;
            //trig.TRIGGER_T = mapdat.triggerdata[i].trigtye;

            s_save_vector ve = trigger.trigSize;

                trig.GetComponent<BoxCollider2D>().size = new Vector2(ve.x, ve.y);

            trig.transform.SetParent(trig.transform);
        }
        else if (util != null)
        {
            if (trigger.name != "")
                util.name = trigger.name;
            //trig.TRIGGER_T = mapdat.triggerdata[i].trigtye;

            s_save_vector ve = trigger.trigSize;

            if (trigger.util != "u_boundary")
                util.GetComponent<BoxCollider2D>().size = new Vector2(ve.x, ve.y);

            util.transform.SetParent(util.transform);
        }

        //trig.Events.ev_Details = mapdat.triggerdata[i].listofevents;

        return trigObj;
    }
    public override void SetEntities(List<s_map.s_chara> characters)
    {
        for (int i = 0; i < characters.Count; i++)
        {
            s_map.s_chara characterdat = characters[i];
            Vector2 characterPos = new Vector2(characterdat.pos_x, characterdat.pos_y);

            PDII_character trig = null;
            if (InEditor)
            {
                //print(characters[i].charType);
                if (characterdat.charType == "")
                    continue;
                if (FindOBJ(characterdat.charType) == null)
                {
                    print("Couldn't find object '" + characterdat.charType + "' in the pool, please add it to the pooler.");
                    continue;
                }
                trig = Instantiate(FindOBJ(characterdat.charType), characterPos, Quaternion.identity).GetComponent<PDII_character>();

                if (characterdat.charType == "polar_bear")
                {
                    npc_pbear p = trig.GetComponent<npc_pbear>();
                    p.teleportLoc = characterdat.CustomTypes.Find(x => x.name == "mapTPLoc").type3;
                    p.map_to_telport_to = characterdat.CustomTypes.Find(x => x.name == "mapTP").type3;
                }
                if (characterdat.charType == "Sharlwrus")
                {
                    npc_sharlwrus p = trig.GetComponent<npc_sharlwrus>();
                    p.teleportLoc = characterdat.CustomTypes.Find(x => x.name == "mapTPLoc").type3;
                    p.map_to_telport_to = characterdat.CustomTypes.Find(x => x.name == "mapTP").type3;
                }
                if (characterdat.charType == "delivery_customer")
                    trig.GetComponent<npc_customer>().itemName = characterdat.CustomTypes.Find(x => x.name == "itemName").type3;
            }
            else
            {
                //print(characters[i].charType);
                if (characters[i].charType == "")
                    continue;
                //TODO find character equal to the id and spawn that
                if (characters[i].possesed)
                    continue;
                trig = SpawnObject<PDII_character>(characters[i].charType, characterPos, Quaternion.identity);

                if (characterdat.charType == "polar_bear")
                {
                    npc_pbear p = trig.GetComponent<npc_pbear>();
                    p.teleportLoc = characterdat.CustomTypes.Find(x => x.name == "mapTPLoc").type3;
                    p.map_to_telport_to = characterdat.CustomTypes.Find(x => x.name == "mapTP").type3;
                }
                if (characterdat.charType == "Sharlwrus")
                {
                    npc_sharlwrus p = trig.GetComponent<npc_sharlwrus>();
                    p.teleportLoc = characterdat.CustomTypes.Find(x => x.name == "mapTPLoc").type3;
                    p.map_to_telport_to = characterdat.CustomTypes.Find(x => x.name == "mapTP").type3;
                }
                if (characterdat.charType == "delivery_customer")
                    trig.GetComponent<npc_customer>().itemName = characterdat.CustomTypes.Find(x => x.name == "itemName").type3;
            }
            trig.map_origin = mapDat.name;
            trig.control = true;
            trig.SetSpawnPoint(characterPos);
            trig.name = characterdat.charname;
            trig.transform.position = new Vector3(characterdat.pos_x, characterdat.pos_y, 1);
            trig.transform.SetParent(entitiesObj.transform);

            allcharacters.Add(trig);
        }
    }
    public override void SetTileMap(s_map mapdat)
    {
        if (!InEditor)
        {
            print(mapdat.name);
            AudioClip clip = GetComponent<s_bgm>().GetMusic(mapdat.name);
            if (clip != null)
                GetComponent<s_bgm>().PlayMusic(clip);
        }

        List<s_map.s_block> tile = mapdat.graphicTiles;
        List<s_map.s_block> tileMid = mapdat.graphicTilesMiddle;
        List<s_map.s_block> tileTop = mapdat.graphicTilesTop;
        List<s_map.s_tileobj> coll = mapdat.tilesdata;

        foreach (s_map.s_block b in tile)
        {
            tm.SetTile(new Vector3Int((int)b.position.x / 20, (int)b.position.y / 20, 0), tilesNew.Find(ti => ti.name == b.sprite));
        }
        foreach (s_map.s_block b in tileMid)
        {
            tm2.SetTile(new Vector3Int((int)b.position.x / 20, (int)b.position.y / 20, 0), tilesNew.Find(ti => ti.name == b.sprite));
        }
        foreach (s_map.s_block b in tileTop)
        {
            tm3.SetTile(new Vector3Int((int)b.position.x / 20, (int)b.position.y / 20, 0), tilesNew.Find(ti => ti.name == b.sprite));
        }
        for (int i = 0; i < coll.Count; i++)
        {
            s_map.s_tileobj b = coll[i];
            string tilename = "";
            COLLISION_T tileType = (COLLISION_T)b.enumthing;
            Tile t = null;
            if (InEditor)
            {
                switch (tileType)
                {
                    case COLLISION_T.CLIMBING:
                        tilename = "climbing";
                        break;
                    case COLLISION_T.DITCH:
                        tilename = "ditch";
                        break;
                    case COLLISION_T.FALLING_ON_LAND:
                        tilename = "falling_on_land";
                        break;
                    case COLLISION_T.FALLING:
                        tilename = "falling";
                        break;
                    case COLLISION_T.WALL:
                        tilename = "collision";
                        break;
                    case COLLISION_T.WATER_TILE:
                        tilename = "water_collision";
                        break;
                    case COLLISION_T.NO_DEPOSEESS:
                        tilename = "no_deposses";
                        //t = collisionTiles.GetTile<Tile>(new Vector3Int(-4, 0,0));
                        break;
                    case COLLISION_T.LANDING_DOWN:
                        // print("This");
                        tilename = "landing_down";
                        break;
                    case COLLISION_T.LANDING_LEFT:
                        tilename = "landing_left";
                        break;
                    case COLLISION_T.LANDING_RIGHT:
                        tilename = "landing_right";
                        break;
                    case COLLISION_T.LANDING_UP:
                        tilename = "landing_up";
                        break;
                }
                if (tilename != "")
                    colmp.SetTile(new Vector3Int(b.pos_x / 20, b.pos_y / 20, 0), collisionList.Find(ti => ti.name == tilename));
                else
                    colmp.SetTile(new Vector3Int(b.pos_x / 20, b.pos_y / 20, 0), t);
            }
            else
            {
                switch (tileType)
                {
                    case COLLISION_T.WALL:
                        if (b.TYPENAME == "teleport_object")
                            continue;
                        colmp.SetTile(new Vector3Int(b.pos_x / 20, b.pos_y / 20, 0), collisionTile);
                        break;
                }
            }

        }
        //print("Done");
        for (int i = 0; i < mapdat.tilesdata.Count; i++)
        {
            s_map.s_tileobj ma = mapdat.tilesdata[i];
            s_object trig = null;
            GameObject targname = null;
            //print((s_map.s_mapteleport)mapdat.tilesdata[i]);
            if (InEditor)
            {
                switch (mapdat.tilesdata[i].TYPENAME)
                {
                    default:
                        continue;

                    case "mapteleport":

                        /*
                        trig = Instantiate(FindOBJ("Teleporter"), new Vector2(mapdat.tilesdata[i].pos_x, mapdat.tilesdata[i].pos_y), Quaternion.identity).GetComponent<o_maptransition>();
                        targname = prefabs[4];
                        */
                        break;
                    case "boat":
                        trig = Instantiate(FindOBJ(mapdat.tilesdata[i].TYPENAME), new Vector2(mapdat.tilesdata[i].pos_x, mapdat.tilesdata[i].pos_y), Quaternion.identity).GetComponent<o_generic>();
                        break;

                    case "teleport_object":
                        trig = Instantiate(FindOBJ("teleport_object"), new Vector2(mapdat.tilesdata[i].pos_x, mapdat.tilesdata[i].pos_y), Quaternion.identity).GetComponent<o_generic>();
                        break;

                    case "bound":
                        trig = Instantiate(FindOBJ("bound"), new Vector2(mapdat.tilesdata[i].pos_x, mapdat.tilesdata[i].pos_y), Quaternion.identity).GetComponent<o_generic>();
                        break;

                    case "money":

                        trig = Instantiate(FindOBJ("money"), new Vector2(mapdat.tilesdata[i].pos_x, mapdat.tilesdata[i].pos_y), Quaternion.identity).GetComponent<o_itemObj>();
                        
                        break;

                    case "health_increase":

                        trig = Instantiate(FindOBJ("health_increase"), new Vector2(mapdat.tilesdata[i].pos_x, mapdat.tilesdata[i].pos_y), Quaternion.identity).GetComponent<o_itemObj>();
                        targname = FindOBJ("health_increase");
                        break;
                    case "NPCBound":

                        trig = Instantiate(FindOBJ("NPCBound"), new Vector2(mapdat.tilesdata[i].pos_x, mapdat.tilesdata[i].pos_y), Quaternion.identity).GetComponent<o_generic>();
                        targname = FindOBJ("NPCBound");
                        break;
                }
            }
            else
            {
                switch (mapdat.tilesdata[i].TYPENAME)
                {
                    default:

                        if ((COLLISION_T)mapdat.tilesdata[i].enumthing == COLLISION_T.WALL)
                        {
                            colmp.SetTile(new Vector3Int(mapdat.tilesdata[i].pos_x / 20, mapdat.tilesdata[i].pos_y / 20, 0), tilesNew[0]);
                            //print("SetTile");
                            continue;
                        }
                        //trig = SpawnObject("Tile", new Vector2(mapdat.tilesdata[i].pos_x, mapdat.tilesdata[i].pos_y), Quaternion.identity).GetComponent<o_collidableobject>();
                        else
                            continue;

                    case "boat":
                        trig = SpawnObject<s_object>(mapdat.tilesdata[i].TYPENAME, new Vector2(mapdat.tilesdata[i].pos_x, mapdat.tilesdata[i].pos_y), Quaternion.identity);
                        break;

                    case "mapteleport":
                        //trig = SpawnObject("Teleporter", new Vector2(mapdat.tilesdata[i].pos_x, mapdat.tilesdata[i].pos_y), Quaternion.identity).GetComponent<o_maptransition>();
                        break;

                    case "teleport_object":
                        // trig = SpawnObject<s_object>("teleport_object", new Vector2(mapdat.tilesdata[i].pos_x, mapdat.tilesdata[i].pos_y), Quaternion.identity).GetComponent<o_generic>();
                        continue;

                    case "bound":
                        trig = SpawnObject<s_object>("bound", new Vector2(mapdat.tilesdata[i].pos_x, mapdat.tilesdata[i].pos_y), Quaternion.identity).GetComponent<o_generic>();
                        break;

                    case "money":
                        trig = SpawnObject<s_object>("money", new Vector2(mapdat.tilesdata[i].pos_x, mapdat.tilesdata[i].pos_y), Quaternion.identity).GetComponent<o_itemObj>();
                        break;

                    case "health_increase":
                        trig = SpawnObject<s_object>("health_increase", new Vector2(mapdat.tilesdata[i].pos_x, mapdat.tilesdata[i].pos_y), Quaternion.identity).GetComponent<o_itemObj>();
                        break;

                    case "NPCBound":
                        trig = SpawnObject<s_object>("NPCBound", new Vector2(mapdat.tilesdata[i].pos_x, mapdat.tilesdata[i].pos_y), Quaternion.identity).GetComponent<o_generic>();
                        targname = FindOBJ("NPCBound");
                        trig.GetComponent<SpriteRenderer>().color = Color.clear;
                        break;

                }
            }

            if (trig == null)
                continue;
            if (trig.GetComponent<o_generic>())
            {
                o_generic col = trig.GetComponent<o_generic>();

                col.character = mapdat.tilesdata[i].exceptionChar;
                if (mapdat.tilesdata[i].TYPENAME == "teleport_object")
                {
                    col.name = mapdat.tilesdata[i].name;
                }
                if (mapdat.tilesdata[i].TYPENAME == "bound")
                {
                    col.name = ma.name;
                    //col.transform.localScale = new Vector3(ma.scale.x, ma.scale.y);
                }
                //col.collision_type = (COLLISION_T)mapdat.tilesdata[i].enumthing;
                if (!mapdat.tilesdata[i].issolid)
                {
                    col.characterCannot = mapdat.tilesdata[i].cannotPassChar;
                    col.issolid = false;
                }
                else
                {
                    col.issolid = true;
                    col.characterCannot = null;
                }

                SpriteRenderer spr = trig.GetComponent<SpriteRenderer>();
                BoxCollider2D bx = trig.GetComponent<BoxCollider2D>();
                if (bx)
                    bx.size = new Vector2(ma.size.x, ma.size.y);
                if (mapdat.tilesdata[i].TYPENAME == "bound")
                {
                    if (bx)
                        bx.size = new Vector2(20, 20);
                }
            }
            /*
            if (trig.GetComponent<o_maptransition>())
            {
                o_maptransition col = trig.GetComponent<o_maptransition>();
                if (ma.flagchecks != null)
                {
                    col.flagcheck = ma.flagname;
                    col.flags = new o_maptransition.s_flagcheck[ma.flagchecks.Length];
                    for (int a = 0; a < ma.flagchecks.Length; a++)
                    {
                        col.flags[a] = new o_maptransition.s_flagcheck(ma.flagchecks[a], ma.mapnames[a]);
                    }
                }
                //print(col);

                col.position = new Vector2(ma.teleportpos.x, ma.teleportpos.y);
                col.sceneToTransferTo = ma.mapname;
                col.areaInScene = ma.areaname;
            }
            */
            trig.transform.position = new Vector3(ma.pos_x, ma.pos_y, 0);
            trig.transform.SetParent(tilesObj.transform);

            if (targname != null)
                trig.name = targname.name;
        }
    }
    public override void SetItem(s_save_item item)
    {
        o_itemObj trig = null;
        //print((s_map.s_mapteleport)mapdat.tilesdata[i]);
        if (InEditor)
        {
            if (FindOBJ<o_itemObj>(item.stID) == null)
                return;
            trig = Instantiate<o_itemObj>(FindOBJ<o_itemObj>(item.stID), new Vector2(item.pos.x, item.pos.y), Quaternion.identity);
        }
        else
        {
            if (item.iscollected)
                return;
            if (FindOBJ<o_itemObj>(item.stID) == null)
                return;
            trig = SpawnObject<o_itemObj>(item.stID, new Vector2(item.pos.x, item.pos.y), Quaternion.identity);
        }
        trig.indexID = item.ID;
        //trig.ItemContain = item;
        trig.name = item.name;
        trig.it = (o_itemObj.ITEM_TYPE)item.type;

        trig.transform.position = new Vector3(item.pos.x, item.pos.y, 1);
        trig.transform.SetParent(itemsObj.transform);
    }

    public override void SetTriggerDependency(ref List<Tuple<GameObject, List<s_map.s_customType>>> triggers)
    {
        if (triggers == null)
            return;
        foreach (Tuple<GameObject, List<s_map.s_customType>> go in triggers)
        {
            if (go.Item1.GetComponent<u_collect>())
            {
                u_collect collec = go.Item1.GetComponent<u_collect>();
                if (collec.itemPositions.Count == 0)
                {
                    collec.itemPositions = new List<Vector2>();
                    foreach (s_map.s_customType c in go.Item2)
                    {
                        if (c.name == "reqItem")
                        {
                            o_itemObj itemO = GameObject.Find(c.type3).GetComponent<o_itemObj>();
                            collec.itemsRequire.Add(itemO);
                            collec.itemPositions.Add(itemO.transform.position);
                        }
                    }
                }
            }
        }
    }
    public override void GetTileDat(ref s_map mapfil)
    {
        Tile[] tiles = new Tile[(int)mapsizeToKeep.x * (int)mapsizeToKeep.y];
        Tile[] colTiles = new Tile[(int)mapsizeToKeep.x * (int)mapsizeToKeep.y];
        Vector2Int vec = new Vector2Int(0, 0);

        for (int x = 0; x < mapsizeToKeep.x; x++)
        {
            for (int y = 0; y < mapsizeToKeep.y; y++)
            {
                Tile coltil = colmp.GetTile<Tile>(new Vector3Int(x, y, 0));
                COLLISION_T colltype = COLLISION_T.NONE;
                if (coltil != null)
                {
                    colTiles[(int)(x + (mapsizeToKeep.x * y))] = coltil;
                    if (coltil != null)
                    {
                        string tileName = coltil.name;
                        switch (tileName)
                        {
                            case "falling_on_land":
                                colltype = COLLISION_T.FALLING_ON_LAND;
                                break;

                            case "falling":
                                colltype = COLLISION_T.FALLING;
                                break;

                            case "ditch":
                                colltype = COLLISION_T.DITCH;
                                break;

                            case "collision":
                                colltype = COLLISION_T.WALL;
                                break;

                            case "water_collision":
                                colltype = COLLISION_T.WATER_TILE;
                                break;

                            case "landing_up":
                                colltype = COLLISION_T.LANDING_UP;
                                break;

                            case "landing_left":
                                colltype = COLLISION_T.LANDING_LEFT;
                                break;

                            case "landing_right":
                                colltype = COLLISION_T.LANDING_RIGHT;
                                break;

                            case "landing_down":
                                colltype = COLLISION_T.LANDING_DOWN;
                                break;

                            case "no_deposses":
                                colltype = COLLISION_T.NO_DEPOSEESS;
                                break;

                        }
                        s_map.s_tileobj tilo = new s_map.s_tileobj(new Vector2(x * 20, y * 20), null,
                            (int)colltype);

                        tilo.tilemapPos = new s_save_vector();
                        mapfil.tilesdata.Add(tilo);
                    }

                }

                Tile til = tm.GetTile<Tile>(new Vector3Int(x, y, 0));
                if (til != null)
                {
                    mapfil.graphicTiles.Add(
                               new s_map.s_block(til.name,
                               new Vector2(x * 20, y * 20)));
                    //print(til.name);
                    /*
                    if (til.sprite == null)
                    {
                    }

                    mapfil.graphicTiles.Add(
                               new s_map.s_block(til.sprite.name,
                               new Vector2(x * 20, y * 20)));
                    */
                }

                Tile tilmid = tm2.GetTile<Tile>(new Vector3Int(x, y, 0));
                if (tilmid != null)
                {
                    mapfil.graphicTilesMiddle.Add(
                               new s_map.s_block(tilmid.name,
                               new Vector2(x * 20, y * 20)));
                    /*
                    if (tilmid.sprite == null)
                    {
                    }
                    mapfil.graphicTilesMiddle.Add(
                               new s_map.s_block(tilmid.sprite.name,
                               new Vector2(x * 20, y * 20)));
                    */
                }

                Tile tiltop = tm3.GetTile<Tile>(new Vector3Int(x, y, 0));
                if (tiltop != null)
                {
                    mapfil.graphicTilesTop.Add(
                               new s_map.s_block(tiltop.name,
                               new Vector2(x * 20, y * 20)));
                    /*
                    if (tiltop.sprite == null)
                    {
                    }
                    mapfil.graphicTilesTop.Add(
                               new s_map.s_block(tiltop.sprite.name,
                               new Vector2(x * 20, y * 20)));
                    */
                }
            }
        }
    }
    public override List<s_map.s_chara> GetEntities(o_character[] characters)
    {
        List<s_map.s_chara> charalist = new List<s_map.s_chara>();
        foreach (o_character c in characters)
        {
            /*
            bool defeat = false;
            if (c.CHARACTER_STATE == o_character.CHARACTER_STATES.STATE_DEFEAT)
                defeat = true;
            */

            s_map.s_chara ch = new s_map.s_chara(
            c.transform.position,
            c.name,
            c.ID,
            c.control,
            false,
            false,
            c.faction);

            if (c.ID == "polar_bear")
            {
                ch.AddCustomTag("mapTP", "ice_cave");
                ch.AddCustomTag("mapTPLoc", c.GetComponent<PDII_character>().teleportLoc);
            }
            if (c.ID == "Sharlwrus")
            {
                ch.AddCustomTag("mapTP", "underground_sea");
                ch.AddCustomTag("mapTPLoc", c.GetComponent<PDII_character>().teleportLoc);
            }
            if (c.ID == "delivery_customer")
            {
                ch.AddCustomTag("itemName", c.GetComponent<npc_customer>().itemName);
            }

            charalist.Add(ch);
        }
        return charalist;
    }
    public override List<s_map.s_trig> GetTriggers(s_object[] triggers)
    {
        List<s_map.s_trig> trigs = new List<s_map.s_trig>();
        foreach (s_object obj in triggers)
        {
            /*
            if (obj.GetComponent<u_delivery>())
            {
                u_delivery b = obj.GetComponent<u_delivery>();
                s_map.s_trig t = new s_map.s_trig(b.name, obj.transform.position);
                t.util = "u_delivery";
                foreach (o_character c in b.characters)
                {
                    t.AddCustomTag("char", c.name);
                    t.AddCustomTag("charItem", c.GetComponent<npc_customer>().desiredItemName);
                }
                trigs.Add(t);
                continue;
            }
            */
            if (obj.GetComponent<u_boundary>())
            {
                u_boundary b = obj.GetComponent<u_boundary>();
                print(obj.GetComponent<s_utility>().ToString());
                s_map.s_trig t = new s_map.s_trig(obj.gameObject.transform.localScale);

                t.pos_x = (int)obj.transform.position.x;
                t.pos_y = (int)obj.transform.position.y;
                t.name = b.name;
                t.util = "u_boundary";
                
                t.AddCustomTag("GemReq", b.gemRequire);
                int cond = b.checkFlag == true ? 1 : 0; 
                t.AddCustomTag("flagCond", cond);
                t.AddCustomTag("flagName", b.flagname);
                t.AddCustomTag("flagCheck", b.flagCheck);
                t.AddCustomTag("flagDesc", b.description);

                t.utilbool = b.IsOverworldRequire;
                trigs.Add(t);
                continue;
            }
            if (obj.GetComponent<u_leemminigame>())
            {
                u_leemminigame b = obj.GetComponent<u_leemminigame>();
                s_map.s_trig t = new s_map.s_trig(obj.gameObject.transform.localScale);

                t.pos_x = (int)obj.transform.position.x;
                t.pos_y = (int)obj.transform.position.y;
                t.name = b.name;
                t.util = "u_leemminigame";

                t.CustomTypes = new List<s_map.s_customType>();
               // t.CustomTypes.Add(new s_map.s_customType("bound", b.boundary.name));
                trigs.Add(t);
                continue;
            }
            /*
            if (obj.GetComponent<u_feedinggame>())
            {
                u_feedinggame b = obj.GetComponent<u_feedinggame>();
                s_map.s_trig t = new s_map.s_trig(obj.gameObject.transform.localScale);

                t.pos_x = (int)obj.transform.position.x;
                t.pos_y = (int)obj.transform.position.y;
                t.name = b.name;
                t.util = "u_feedinggame";

                t.CustomTypes = new List<s_map.s_customType>();
                foreach (o_itemObj v in b.wormPos)
                {
                    t.AddCustomTag("worm", v.name);
                }
                trigs.Add(t);
                continue;
            }
            if (obj.GetComponent<u_yetifishing>())
            {
                u_yetifishing b = obj.GetComponent<u_yetifishing>();
                s_map.s_trig t = new s_map.s_trig(obj.gameObject.transform.localScale);

                t.pos_x = (int)obj.transform.position.x;
                t.pos_y = (int)obj.transform.position.y;
                t.name = b.name;
                t.util = "u_yetifishing";

                t.CustomTypes = new List<s_map.s_customType>();
                foreach (npc_yeti yet in b.yetiCharacters)
                {
                    t.AddCustomTag("op", yet.name);
                }
                trigs.Add(t);
                continue;
            }
            */
            if (obj.GetComponent<u_collect>())
            {
                u_collect b = obj.GetComponent<u_collect>();
                s_map.s_trig t = new s_map.s_trig(obj.gameObject.transform.localScale);

                t.requiresDependency = true;
                t.pos_x = (int)obj.transform.position.x;
                t.pos_y = (int)obj.transform.position.y;
                t.name = b.name;
                t.util = "u_collect";

                t.CustomTypes = new List<s_map.s_customType>();
                foreach (o_itemObj it in b.itemsRequire)
                {
                    t.AddCustomTag("reqItem", it.name);
                }
                trigs.Add(t);
                continue;
            }
            o_trigger tri = obj.GetComponent<o_trigger>();
            //print(obj.name);
            s_map.s_trig tr = new s_map.s_trig(obj.name, obj.transform.position, tri.TRIGGER_T, obj.GetComponent<BoxCollider2D>().size, false);
            tr.labelToJumpTo = tri.LabelToJumpTo;
            trigs.Add(tr);
        }
        return trigs;
    }
    public override List<s_save_item> GetItems(MagnumFoudation.o_itemObj[] itemsInMap)
    {
        List<s_save_item> itemlist = new List<s_save_item>();
        int ind = 0;
        for (int i = 0; i < itemsInMap.Length; i++)
        {
            MagnumFoudation.o_itemObj obj = itemsInMap[i];
            if (obj.indexID == -1)
                itemlist.Add(new s_save_item(obj.transform.position, ind, (ITEM_TYPE)obj.it, obj.name, obj.ID));
            else
                itemlist.Add(new s_save_item(obj.transform.position, obj.indexID, (ITEM_TYPE)obj.it, obj.name, obj.ID));
            ind++;
        }
        return itemlist;
    }
    public override List<s_map.s_tileobj> GetTiles(s_object[] tiles)
    {
        List<s_map.s_tileobj> tilelist = new List<s_map.s_tileobj>();
        foreach (s_object obj in tiles)
        {
            if (obj.name == "BoundTile")  //This will be spawned within the boundary rather than the tiles    
            {
                continue;
            }
            if (obj.GetComponent<o_generic>())
            {
                s_map.s_tileobj tilet = new s_map.s_tileobj(obj.transform.position, obj.ID, -1,
                obj.GetComponent<o_generic>().character);
                if (obj.ID == "teleport_object")
                {
                    o_generic co = obj.GetComponent<o_generic>();
                    tilet.enumthing = -1;
                    tilet.teleportpos = new s_save_vector(co.transform.position.x, co.transform.position.y);
                    tilet.name = obj.name;
                }
                if (obj.ID == "bound")
                {
                    o_generic co = obj.GetComponent<o_generic>();
                    tilet.enumthing = -1;
                    tilet.teleportpos = new s_save_vector(co.transform.position.x, co.transform.position.y);
                    tilet.name = obj.name;
                    //tilet.scale = new s_save_vector(obj.transform.localScale.x, obj.transform.localScale.y);
                    tilelist.Add(tilet);
                    continue;
                }
                if (obj.GetComponent<BoxCollider2D>())
                    tilet.size = new s_save_vector(obj.GetComponent<BoxCollider2D>().size.x, obj.GetComponent<BoxCollider2D>().size.y);
                tilelist.Add(tilet);
                continue;
            }
            //print(obj.name);
            s_map.s_tileobj tile = new s_map.s_tileobj(
                obj.transform.position,
                obj.ID,
                (int)obj.GetComponent<o_generic>().collision_type,
                obj.GetComponent<o_generic>().character);
            tile.size = new s_save_vector(obj.GetComponent<BoxCollider2D>().size.x, obj.GetComponent<BoxCollider2D>().size.y);
            tile.issolid = obj.GetComponent<o_generic>().issolid;
            tile.cannotPassChar = obj.GetComponent<o_generic>().characterCannot;
            tilelist.Add(tile);
        }
        return tilelist;
    }
    #endregion

    public void LoadMap(string mapname, Vector2 position)
    {
        LoadMap(maps.Find(x => x.name == mapname));
        o_character pl = GameObject.Find("Player").GetComponent<o_character>();

        if (pl)
        {
            pl.transform.position = new Vector3(position.x, position.y);
        }
    }
    public void LoadMap(string mapname)
    {
        LoadMap(maps.Find(x => x.name == mapname));
    }
    /*
s_map.s_tileobj tilet = new s_map.s_tileobj(obj.transform.position, obj.ID, (int)obj.GetComponent<o_collidableobject>().GetCollisionType(),
obj.GetComponent<o_collidableobject>().character);
if(obj.GetComponent<BoxCollider2D>() != null)
    tilet.size = new  s_save_vector(obj.GetComponent<BoxCollider2D>().size.x, obj.GetComponent<BoxCollider2D>().size.y);
tilet.cannotPassChar = obj.GetComponent<o_collidableobject>().characterCannot;
tilet.issolid = obj.GetComponent<o_collidableobject>().issolid;
*/
    /*
    if (obj.name == "SpawnPoint")
        mapfil.spawnPoint = new s_save_vector(obj.transform.position.x, obj.transform.position.y);    //Make the spawnpoint value of the vector
    else
    {

    }
    */
    /*
    if (obj.GetComponent<o_maptransition>())
    {
        print("MAPTRANS");
        o_maptransition trans = obj.GetComponent<o_maptransition>();

        int[] flags = new int[trans.flags.Length];
        string[] mapnames = new string[trans.flags.Length];

        for (int a = 0; a < trans.flags.Length; a++)
        {
            flags[a] = trans.flags[a].flagCondition;
            mapnames[a] = trans.flags[a].mapname;
        }
        s_map.s_tileobj tilet = new s_map.s_tileobj(obj.transform.position, "mapteleport", trans.position, trans.sceneToTransferTo, trans.flagcheck, flags, mapnames);
        tilet.areaname = obj.GetComponent<o_maptransition>().areaInScene;
        tilelist.Add(tilet);
        continue;
    }
    */
    /*
    colmp.color = Color.clear;
    canv.gameObject.SetActive(true);
    if (LevEd == null)
        LevEd = this;
    nodegraph = GetComponent<s_nodegraph>();
    triggerHand = GetComponent<s_trig>();
    triggerHand.SetTrigObject();
    InEditor = false;

    maps.Clear();

    SetList();
    if (s_mainmenu.isload)
    {
        sa = s_mainmenu.save;
        foreach (s_map ma in sa.savedmaps)
        {
            maps.Add(ma);
        }
        foreach (KeyValuePair<string, int> s in sa.gbflg.Flags)
        {
            s_globals.SetGlobalFlag(s.Key, s.Value);
        }
        LoadMap(maps[0]);

    }
    else
    {
        //GameObject.Find("Player").transform.position = GameObject.Find("SpawnPoint").transform.position;
        foreach (TextAsset asset in jsonMaps)
        {
            s_map cu = JsonUtility.FromJson<s_map>(asset.text);
            cu.name = asset.name;
            maps.Add(cu);
        }
        print("bnaf");
        LoadMap(maps[0]);
    }

    Screen.SetResolution(1280, 720, false);
    */
    /*
public void GrabNodeGraphdata()
{
    s_node[] nodegr = nodegraph.CreateNodeArray();
    List<s_node> doneNodes = new List<s_node>();
    s_node currentnode = nodegr[0];
    int ptr = 1;

    int leng = nodegraph.xsi;

    while (doneNodes.Count != nodegr.Length)
    {
        int x_leng = 0, y_leng = 0;
        int x_count = 0, y_count = 0;
        while (nodegr[ptr] != null) {
            ptr++;
            x_count++;
            if (x_count == leng)
            {
                y_leng++;
                x_count = 0;
            } 
        }

    }

}
    */
    /*
        if (PrefabUtility.FindPrefabRoot(maps[0].gameObject) == null)
        {
            PrefabUtility.CreatePrefab("Assets/Levels/" + "Obj.prefab", current_map.gameObject);
        }
        else
        {
            //loadedLevel = PrefabUtility.FindPrefabRoot(res);
        }
        s_map mapfil = new s_map();

        mapfil.name = current_area;
        o_character[] objectsInMap = null;
        o_trigger[] triggersInMap = null;
        o_collidableobject[] tilesInMap = null;

        if (GameObject.Find(current_area) != null)
        {
            objectsInMap = GameObject.Find(current_area).transform.Find("Entities").GetComponentsInChildren<o_character>();
            triggersInMap = GameObject.Find(current_area).transform.Find("Triggers").GetComponentsInChildren<o_trigger>();
            tilesInMap = GameObject.Find(current_area).transform.Find("Tiles").GetComponentsInChildren<o_collidableobject>();
        }
        else
        {
            objectsInMap = GameObject.Find("New Level").transform.Find("Entities").GetComponentsInChildren<o_character>();
            triggersInMap = GameObject.Find("New Level").transform.Find("Triggers").GetComponentsInChildren<o_trigger>();
            tilesInMap = GameObject.Find("New Level").transform.Find("Tiles").GetComponentsInChildren<o_collidableobject>();
        }

        List<s_map.s_trig> triggerlist = new List<s_map.s_trig>();
        List<s_map.s_chara> charalist = new List<s_map.s_chara>();
        List<s_map.s_tileobj> tilelist = new List<s_map.s_tileobj>();

        foreach (o_trigger obj in triggersInMap)
        {
            triggerlist.Add(new s_map.s_trig(obj.transform.position, obj.Events.ev_Details));
        }
        mapfil.triggerdata = triggerlist;

        foreach (s_object obj in objectsInMap)
        {
            charalist.Add(new s_map.s_chara(obj.positioninworld));
        }
        mapfil.objectdata = charalist;

        foreach (s_object obj in tilesInMap)
        {
            tilelist.Add(new s_map.s_tileobj(obj.positioninworld, obj.height));
        }
        mapfil.tilesdata = tilelist;

        GameObject levedatabase = GameObject.Find("LevelDatabase");
        print(levedatabase.name);
        dat = levedatabase.GetComponent<s_leveldatabase>();

        dat.maps.Add(mapfil);

        dat = null;
        if (GameObject.Find(current_area) != null)
            GameObject.Find(current_area).SetActive(false);
        else
        {
            GameObject.Find("New Level").SetActive(false);
        }
*/
    /*
    public void TriggerSpawn(string string0, Vector2 vec)
    {
        s_map thing = maps.Find(x => x.name == string0);
        NewMap();
        LoadMap(thing);
        CheckCharacters();
        o_plcharacter selobj = null;
        //vec = GameObject.Find("SpawnPoint").transform.position;
        selobj = GameObject.Find("Player").GetComponent<o_plcharacter>();
        if (selobj.host != null)
        {
            selobj.transform.position = new Vector3(vec.x, vec.y);
        }
        else
            selobj.transform.position = new Vector3(vec.x, vec.y);
    }
    */
    /*
    public IEnumerator FloodFill(Vector2 giz, bool erase, GameObject im, int layer)
    {
        GameObject go = null;

        Vector2 targ = new Vector2(0, 0);
        List<Vector2> openList = new List<Vector2>();
        List<Vector2> closeList = new List<Vector2>();

        Vector2[] pos = {
            new Vector2(0,20),
            new Vector2(0,-20),
            new Vector2(20,0),
            new Vector2(-20,0),
            new Vector2(0,0)
        };

        Vector2 origin = giz;
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

            PlaceBlock(origin, go, im, COLLISION_T.WALL);

            for (int i = 0; i < pos.Length; i++)
            {
                //Set Boundaries
                if ((origin + pos[i]).x > (mapDat.mapsize.x * 20))
                    continue;
                if ((origin + pos[i]).y > (mapDat.mapsize.y * 20))
                    continue;
                if ((origin + pos[i]).x < 0)
                    continue;
                if ((origin + pos[i]).y < 0)
                    continue;

                //Check around the currently selected node
                go = GetObjectFromMouse(origin + pos[i], layer, 0);

                if (go == null && !closeList.Contains(origin + pos[i]))
                {
                    if (!openList.Contains(origin + pos[i]))
                        openList.Add(origin + pos[i]);
                    //Does the calculation again
                    //go = GetObjectFromMouse(origin + pos[i], layer);
                }
            }
            yield return new WaitForSeconds(Time.deltaTime);
            test++;
        }
    }

    GameObject GetObjectFromMouse(Vector2 pos, int l, int la)
    {
        GameObject mapn = SceneLevelObject;
        s_object[] tilesInMap = null;
        o_generic[] colInMap = null;
        o_character[] objectsInMap = null;
        o_itemObj[] itemsInMap = null;
        o_trigger[] triggerInMap = null;

        itemsInMap = mapn.transform.Find("Items").GetComponentsInChildren<o_itemObj>();
        objectsInMap = mapn.transform.Find("Entities").GetComponentsInChildren<o_character>();
        triggerInMap = mapn.transform.Find("Triggers").GetComponentsInChildren<o_trigger>();
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
                            if (o.GetComponent<o_tile>().layer == (int)la)
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

                foreach (o_trigger o in triggerInMap)
                {
                    if (o.transform.position == p)
                        return o.gameObject;
                }
                break;
        }
        return null;
    }
    */
    /*
    void PlaceBlock(Vector3 position, GameObject go, GameObject im, COLLISION_T CollisionType)
    {
        if (im == null)
            return;
        GameObject levelobj = SceneLevelObject;
        id++;

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
        if (im.GetComponent<o_trigger>())
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
            if (im.GetComponent<o_trigger>())
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
                lo.gameObject.name = lo.gameObject.name + "_" + id;
                lo.transform.SetParent(levelobj.transform.Find("Entities"));
                return;
            }

        if (im.GetComponent<o_trigger>())
            if (levelobj.transform.Find("Triggers") != null)
            {
                lo.gameObject.name = lo.gameObject.name + "_" + id;
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

        lo.transform.SetParent(levelobj.transform.Find("Tiles"));
    }
    */
    /*
    public void CheckCharacters()
    {
        GameObject mapn = SceneLevelObject;
        o_character[] objectsInMap = null;
        objectsInMap = mapn.transform.Find("Entities").GetComponentsInChildren<o_character>();
        foreach (o_character obj in objectsInMap)
        {
            for (int i = 0; i < mapDat.objectdata.Count; i++)
            {
                s_map.s_chara characterdat = mapDat.objectdata[i];
                s_map.s_chara compare = savedcharalist.Find(x => x.charname == characterdat.charname && x.mapname == characterdat.mapname);
                if (compare != null)
                {

                    obj.GetComponent<SpriteRenderer>().color = Color.white;
                    obj.CHARACTER_STATE = o_character.CHARACTER_STATES.STATE_IDLE;
                    if (!compare.possesed)     //Don't spawn if this character has previously been dead
                    {
                        //obj.CHARACTER_STATE = o_character.CHARACTER_STATES.STATE_DEFEAT;
                    }
                    else
                    {
                    }
                }
            }
        }
    }
    */
    /*
    public s_map JsonToObj(string directory)
    {
        string jso = File.ReadAllText(directory);
        //print(jso);
        //current_map = maps[current_area];
        return JsonUtility.FromJson<s_map>(jso);
        //LoadMap(current_map);
    }
    */
    /*
    if (obj.GetComponent<u_save>())
    {
        u_save b = obj.GetComponent<u_save>();
        print(b.GetType().ToString());
        triggerlist.Add(new s_map.s_trig(obj.name, obj.transform.position, obj.Events.ev_Details, b.GetType().ToString(), obj.TRIGGER_T, false));
        continue;
    }
    */
    /*
    public void LoadMap(s_map mapdat)
    {
        mapscript = null;
        mapDat = mapdat;
        //current_area = nam;
        if (mapdat == null)
            return;
        nodegraph = GetComponent<s_nodegraph>();

        nodegraph.SetPos(new Vector2(mapDat.mapsize.x, mapDat.mapsize.y));
        //nodegraph.CreateNodeGraph();
        allcharacters.Clear();

        SceneLevelObject.name = mapdat.name;

        if (mapdat.zone != Lastzone)
        {
            for (int i = 0; i < savedcharalist.Count; i++)
            {
                if (savedcharalist[i].mapname == Lastzone)
                    savedcharalist.Remove(savedcharalist[i]);
            }
        }

        GameObject triggerIG = GameObject.Find("Triggers");
        GameObject entityIG = GameObject.Find("Entities");
        GameObject tileIG = GameObject.Find("Tiles");
        GameObject itemIG = GameObject.Find("Items");
        zone = mapdat.zone;
        Lastzone = mapdat.zone;

        mapsizeToKeep = new Vector2(mapdat.mapsize.x, mapdat.mapsize.y);

        if (GameObject.Find("Player"))
        {
           // GameObject.Find("Player").transform.position = new Vector3(mapdat.spawnPoint.x, mapdat.spawnPoint.y);
           // GameObject.Find("Player").GetComponent<o_character>().positioninworld = new Vector3(mapdat.spawnPoint.x, mapdat.spawnPoint.y);
        }

        #region SPAWN_ENTITIES
        #endregion

        if(triggerHand != null)
            triggerHand.GetMapEvents(mapDat.map_script_labels, mapDat.Map_Script);

        #region SPAWN_GRAPHIC_TILES
        foreach (s_map.s_block b in mapdat.graphicTiles)
        {
            if (InEditor)
            {
                o_tile t = Instantiate(FindOBJ("TileDecor"), new Vector3(b.position.x, b.position.y), Quaternion.identity).GetComponent<o_tile>();
                t.SpirteRend = t.GetComponent<SpriteRenderer>();
                t.SpirteRend.sprite = TileSprites.Find(til => til.name == b.sprite);
                t.layer = (int)b.layer;
                t.Intialize();
                t.name = "TileDecor";
                t.transform.SetParent(tileIG.transform);
            }
            else
            {
                o_tile t = SpawnObject("TileDecor", new Vector3(b.position.x, b.position.y), Quaternion.identity).GetComponent<o_tile>();
                t.SpirteRend = t.GetComponent<SpriteRenderer>();
                t.SpirteRend.sprite = TileSprites.Find(til => til.name == b.sprite);
                t.layer = (int)b.layer;
                t.Intialize();
                t.name = "TileDecor";
                t.transform.SetParent(tileIG.transform);
            }
        }
        #endregion

        #region SPAWN_TILES
        #endregion
        SetTileMap(mapdat);

        #region SPAWN_TRIGGERS
        if (!InEditor)
        {
            if (allcharacters != null)
                foreach (o_character c in allcharacters)
                {
                    if (c != null)
                        c.AddFactions(allcharacters);
                }
        }
        GameObject.Find("Player").GetComponent<o_character>().AddFactions(allcharacters);
        #endregion

        #region SPAWN_ITEMS
        #endregion

        allcharacters.Add(GameObject.Find("Player").GetComponent<o_plcharacter>());
        foreach (o_character c in allcharacters)
        {
            if (c != null)
                c.AddFactions(allcharacters);
        }

        nodegraph.nodeGraph2 = nodegraph.CreateNodeArray(mapDat.tilesdata.ToArray());
        if (mapscript != null)
            mapscript.CallTrigger();
    }
    
    public GameObject FindOBJ(string obname)
    {
        foreach (s_pooler_data ga in objPoolDatabase)
        {
            if (ga.gameobject.name == obname)
            {
                return ga.gameobject;
            }
        }
        return null;
    }
    */
    /*
    public void EditProperties(o_generic obj)
    {
        GUI.Label(new Rect(10, 270, 150, 90), obj.collision_type.ToString());
        o = (int)GUI.HorizontalSlider(new Rect(10, 290, 150, 20), o, 0, 4);
        if (GUI.Button(new Rect(10, 400, 150, 30), "Set Enum"))
        {
            obj.collision_type = (COLLISION_T)o;
        }
    }
    */
    /*
    Vector2Int snapvec(Vector3 mousepos)
    {
        return new Vector2Int((int)Mathf.Floor(mousepos.x / 20) + 45, (int)Mathf.Floor(mousepos.y / 20) + 45);
    }
    */
    /*
    Vector3 Snap(Vector3 mousepos)
    {
        return new Vector3(Mathf.Floor(mousepos.x / 20) * 20, Mathf.Floor(mousepos.y / 20) * 20);
    }
    */
    /*
            for (int x = 0; x < mapDat.mapsize.x - 1; x++)
            {
                for (int y = 0; y < mapDat.mapsize.y - 1; y++)
                {
                    if (nodegraph != null)
                    {
                        if (nodegraph.nodeGraph == null)
                        {
                            nodegraph.CreateNodeArray();
                        }
                        if (nodegraph.nodeGraph[x, y] != null)
                        {

                            if (!nodegraph.nodeGraph[x, y].walkable)
                            {
                                Gizmos.DrawCube(new Vector3(x * 20, y * 20), new Vector3(20, 20));
                                continue;
                            }
                        }
                        //
                    }
                    //Gizmos.DrawWireCube(new Vector3(x * 20, y * 20), new Vector3(20, 20));
                }
            }
            */
    /*
    for (int z = (int)startpos.z; z < graphsize.x; z++)
    {
        for (int y = (int)startpos.y; y < graphsize.y; y++)
        {
            for (int x = (int)startpos.x; x < graphsize.x; x++)
            {
                Gizmos.color = Color.green;
                if (Physics2D.Raycast(new Vector2(x * boxsize, y * boxsize + z * boxsize), Vector2.up, 1, layerMask))
                {
                    Gizmos.color = Color.red;
                }
                if (isWire)
                    Gizmos.DrawWireCube(new Vector2(x * boxsize, y * boxsize + z * boxsize), new Vector2(1, 1) * boxsize);
                else
                {
                    //DUMMY CODE
                    if (x == 3 && y == 9)
                    {
                        Gizmos.color = Color.blue;

                        Gizmos.DrawCube(new Vector2(x * boxsize, y * boxsize + hieght + z * boxsize), new Vector2(1, 1) * boxsize);
                    }
                    else
                    {
                        Gizmos.DrawCube(new Vector2(x * boxsize, y * boxsize + z * boxsize), new Vector2(1, 1) * boxsize);
                    }
                }

            }
        }

    }
    */

    private void OnDrawGizmos()
    {
        if (mapDat != null)
        {
            Gizmos.DrawWireCube(new Vector3((mapsizeToKeep.x * 20) / 2, (mapsizeToKeep.y * 20) / 2), new Vector3(mapsizeToKeep.x * 20, mapsizeToKeep.y * 20));
            
        }
#if UNITY_EDITOR

#endif
    }
}