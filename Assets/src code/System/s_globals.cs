using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using MagnumFoudation;

[System.Serializable]
public struct dat_globalflags
{
    public dat_globalflags(Dictionary<string,int> Flags)
    {
        this.Flags = Flags;
    }
    public Dictionary<string, int> Flags;
}

[System.Serializable]
public struct dat_save
{
    public dat_save(dat_globalflags gbflg, int health, string currentmap,List<s_map> maps, Vector2 pos)
    {
        hp = health;
        playerPosition = new s_save_vector(pos);
        this.gbflg = gbflg;
        this.currentmap = currentmap;
        savedmaps = maps;
    }
    public int hp;
    public string currentmap;
    public List<s_map> savedmaps;
    public s_save_vector playerPosition;
    public dat_globalflags gbflg;
}

/*
public class s_globals : MonoBehaviour {

    public Dictionary<string, s_map> maps = new Dictionary<string, s_map>();
    public string currentlevelname;


    bool DEBUG_MODE_ON = false;
    enum EDITOR_MODE
    {
        FLAG,
        MAP_TRANS,
        NOCLIP
    }
    EDITOR_MODE EDIT;
    s_map curmap;

    public static o_plcharacter player;

    public static int Money;
    public static Dictionary<string, int> GlobalFlags = new Dictionary<string, int>();
    public static List<o_item> inventory = new List<o_item>();
    public static HashSet<o_weapon> weapons = new HashSet<o_weapon>();
    public static HashSet<o_item> inventory_unique
    { get
        {
            HashSet<o_item> it = new HashSet<o_item>();
            foreach (o_item i in inventory)
            {
                it.Add(i);
            }
            return it;
        }
    }

    public static void AddItem(o_item it)
    {
            inventory.Add(it);
    }
    public static void RemoveItem(o_item it)
    {
        List<o_item> l = inventory.FindAll(x => x.name == it.name && x.TYPE == it.TYPE);
        foreach (o_item i in l)
        {
            inventory.Remove(i);
        }
    }
    public static void RemoveOneItem(o_item it)
    {
        o_item l = inventory.Find(x => x.name == it.name && x.TYPE == it.TYPE);
        inventory.Remove(l);
    }
    public static bool CheckItem(o_item it)
    {
        o_item i = inventory.Find(x => x.name == it.name && x.TYPE == it.TYPE);
        if (i == null)
            return false;
        else
            return true;
    }

    public static void SetGlobalFlag(string flagname, int flag)
    {
        if (!GlobalFlags.ContainsKey(flagname))
        {
            GlobalFlags.Add(flagname, flag);
            print("Flag " + flagname + " created and set to " + flag);
            return;
        }
        GlobalFlags[flagname] = flag;
    }

    public static int GetGlobalFlag(string flagname)
    {
        if (!GlobalFlags.ContainsKey(flagname))
        {
            //print("Flag " + flagname + "does not exist.");
            return int.MinValue;
        }
        return GlobalFlags[flagname];
    }

    void Start ()
    {
        player = GameObject.Find("Player").GetComponent<o_plcharacter>();
    }

    public void LoadFlag(dat_globalflags flag)
    {
        GlobalFlags = flag.Flags;
    }

    public static void SaveData()
    {
        FileStream fs = new FileStream("Save.PD2", FileMode.Create);
        BinaryFormatter bin = new BinaryFormatter();
        s_leveledit lev = GameObject.Find("General").GetComponent<s_leveledit>();

        dat_save sav = new dat_save(
            new dat_globalflags(GlobalFlags), 
            3, 
            "Yes", 
            lev.maps, 
            GameObject.Find("Player").transform.position
            );

        bin.Serialize(fs, sav);
        fs.Close();
    }

    private void OnGUI()
    {
        if (DEBUG_MODE_ON)
        {
            if (GUI.Button(new Rect(0, 0, 80, 40), "Debug Mode Off"))
            {
                DEBUG_MODE_ON = false;
            }
            if (GUI.Button(new Rect(0, 60, 80, 40), EDIT.ToString()))
            {
                EDIT++;
                EDIT = (EDITOR_MODE)(int)Mathf.Clamp((float)EDIT, 0, 3);
                if ((int)EDIT == 3)
                {
                    EDIT = EDITOR_MODE.FLAG;
                }
            }
            int ind = 2;
            switch (EDIT)
            {
                case EDITOR_MODE.FLAG:

                    foreach (KeyValuePair<string, int> flag in GlobalFlags)
                    {
                        GUILayout.Label(flag.Key + " Value: " + flag.Value);
                        if (GUI.Button(new Rect(0, 50 * ind, 90, 40), "+"))
                        {
                            SetGlobalFlag(flag.Key, flag.Value + 1);
                        }
                        if (GUI.Button(new Rect(120, 50 * ind, 90, 40), "-"))
                        {
                            SetGlobalFlag(flag.Key, flag.Value - 1);
                        }
                    }
                    ind++;
                    break;

                case EDITOR_MODE.MAP_TRANS:

                    int x = 0, y = 0;
                    List<s_map> maps = s_leveledit.LevEd.maps;
                    for (int i = 0; i < maps.Count; i++)
                    {
                        s_map ma = maps[i];

                        if (curmap == ma)
                        {
                            if (GUI.Button(new Rect(150, 50 * 1, 160, 50), "Back"))
                            {
                                curmap = null;
                            }
                            foreach (s_map.s_tileobj ti in ma.tilesdata)
                            {
                                //scroll = GUI.BeginScrollView(new Rect(0, 0, 200, 400), scroll, new Rect(0, 0, 200, 300));
                                if (ti.TYPENAME == "teleport_object")
                                {
                                    if (y == 10)
                                    {
                                        y = 0;
                                        x++;
                                    }
                                    if (GUI.Button(new Rect(160 * x, 50 * (y + 2), 160, 50), ti.name))
                                    {
                                        s_levelloader.LevEd.TriggerSpawn(ma.name, ti.name);
                                    }
                                    ind++;
                                    y++;
                                }
                                //GUI.EndScrollView();
                            }
                        }
                        else
                        {
                            if (curmap == null)
                            {

                                if (y == 10)
                                {
                                    y = 0;
                                    x++;
                                }
                                if (GUI.Button(new Rect(160 * x, 50 * (y + 3), 160, 50), ma.name))
                                {
                                    curmap = ma;
                                }
                                y++;

                            }
                        }

                    }

                    break;

                case EDITOR_MODE.NOCLIP:
                    if (player.IS_KINEMATIC)
                    {
                        if (GUI.Button(new Rect(0, 100, 160, 50), "Disable"))
                        {
                            if (player.host != null)
                                player.host.IS_KINEMATIC = false;
                            player.IS_KINEMATIC = false;
                        }
                    }
                    else
                    {
                        if (GUI.Button(new Rect(0, 100, 160, 50), "Enable"))
                        {
                            if (player.host != null)
                                player.host.IS_KINEMATIC = true;
                            player.IS_KINEMATIC = true;
                        }
                    }
                    GUI.Label(new Rect(0, 100 * 2, 80, 30), "Offset: " + player.offsetCOL);

                    if (GUI.Button(new Rect(0, 140, 80, 30), "+Y"))
                    {
                        player.offsetCOL.y++;
                    }
                    if (GUI.Button(new Rect(90, 140, 80, 30), "-Y"))
                    {
                        player.offsetCOL.y--;
                    }
                    if (GUI.Button(new Rect(90, 180, 80, 30), "+X"))
                    {
                        player.offsetCOL.x++;
                    }
                    if (GUI.Button(new Rect(0, 180, 80, 30), "-X"))
                    {
                        player.offsetCOL.x--;
                    }

                    break;
            }

        }
        else
        {
            if (GUI.Button(new Rect(0, 0, 80, 40), "Debug Mode"))
            {
                DEBUG_MODE_ON = true;
            }
        }
    }
}
*/