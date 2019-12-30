using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MagnumFoudation;

/*
[System.Serializable]
public class s_map : MagnumFoudation.s_map
{
    public int gemCount = 0;
    public List<string> characterFileNames = new List<string>();
    public List<s_trig> triggerdata = new List<s_trig>();
    public List<s_block> graphicTiles = new List<s_block>();
    public List<s_block> graphicTilesMiddle = new List<s_block>();
    public List<s_block> graphicTilesTop = new List<s_block>();
    public List<s_chara> objectdata = new List<s_chara>();
    public List<s_tileobj> tilesdata = new List<s_tileobj>();
    public List<s_save_item> itemdat = new List<s_save_item>();
    public s_save_vector spawnPoint;
    public s_save_vector mapsize;
    public string FlagNameCheck;
    public string mapscript;

    public List<MagnumFoudation.ev_details> Map_Script = new List<MagnumFoudation.ev_details>();
    public List<MagnumFoudation.ev_label> map_script_labels = new List<MagnumFoudation.ev_label>();
    
    public int id;
    public string name;
    public string zone;

    public s_map(string name)
    {
        this.name = name;
    }
    public s_map(string name, string script)
    {
        this.name = name;
        mapscript = script;
    }

    [System.Serializable]
    public class s_save_item
    {
        public s_save_item(Vector2 vec, int id, o_item it, o_itemObj.ITEM_TYPE type, string name)
        {
            pos = new s_save_vector(vec.x,vec.y);
            ID = id;
            it = item;
            this.type = type;
            this.name = name;
        }
        public string name;
        public o_itemObj.ITEM_TYPE type;
        public bool iscollected = false;
        public s_save_vector pos;
        public int ID;
        public o_item item;
    }

    [System.Serializable]
    public class s_block
    {
        public enum LAYER
        {
            TOP_LAYER,
            MIDDLE_LAYER,
            BOTTOM_LAYER
        };
        public s_block(string sprite, Vector2 position, LAYER layer)
        {
            this.layer = layer;
            this.position = new s_save_vector(position);
            this.sprite = sprite;
        }
        public LAYER layer = LAYER.BOTTOM_LAYER;
        public string sprite;
        public s_save_vector position;
    }
    



    [System.Serializable]
    public class s_trig
    {
        [System.Serializable]
        public struct bound
        {
            public s_save_vector pos;
            public bound(Vector2 vec)
            {
                pos = new s_save_vector(vec.x, vec.y);
            }
        }
        
        public void AddCustomTag(string tagname, string tagValue)
        {
            if (CustomTypes != null)
                CustomTypes.Add(new s_customType(tagname, tagValue));
            else
            {
                CustomTypes = new List<s_customType>();
                CustomTypes.Add(new s_customType(tagname, tagValue));
            }
        }
        public void AddCustomTag(string tagname, int tagValue)
        {
            if (CustomTypes != null)
                CustomTypes.Add(new s_customType(tagname, tagValue));
            else
            {
                CustomTypes = new List<s_customType>();
                CustomTypes.Add(new s_customType(tagname, tagValue));
            }
        }
        
        public s_trig(string name, Vector2 pos)
        {
            trigSize = new s_save_vector(0, 0);
            this.name = name;
            util = null;
            pos_x = (int)pos.x;
            pos_y = (int)pos.y;
            listofevents = null;
            characternames = null;
            boundaryobj = null;
            trigtye = 0;
            IsPermanentlyDisabled = false;
        }

        public s_trig(string name, Vector2 pos, o_trigger.TRIGGER_TYPE trig, Vector2 size, bool disable)
        {
            trigSize = new s_save_vector(size.x, size.y);
            this.name = name;
            util = null;
            pos_x = (int)pos.x;
            pos_y = (int)pos.y;
            listofevents = null;
            characternames = null;
            boundaryobj = null;
            trigtye = trig;
            IsPermanentlyDisabled = disable;
        }
        public s_trig(string name, Vector2 pos, ev_details[] ev, o_trigger.TRIGGER_TYPE trig, Vector2 size,bool disable)
        {
            trigSize = new s_save_vector(size.x, size.y);
            this.name = name;
            util = null;
            pos_x = (int)pos.x;
            pos_y = (int)pos.y;
            listofevents = ev;
            characternames = null;
            boundaryobj = null;
            trigtye = trig;
            IsPermanentlyDisabled = disable;
        }
        public s_trig(string name, Vector2 pos, ev_details[] ev, string util, o_trigger.TRIGGER_TYPE trig, bool disable)
        {
            this.name = name;
            pos_x = (int)pos.x;
            pos_y = (int)pos.y;
            listofevents = ev;
            this.util = util;
            characternames = null;
            boundaryobj = null;
            trigtye = trig;
            IsPermanentlyDisabled = disable;
        }

        public s_trig(string name ,Vector2 pos, ev_details[] ev, string util, GameObject[] bound, int gems, o_trigger.TRIGGER_TYPE trig,Vector2 trigSize, bool disable)
        {
            gemreq = gems;
            this.name = name;
            pos_x = (int)pos.x;
            pos_y = (int)pos.y;
            listofevents = ev;
            this.trigSize = new s_save_vector(trigSize.x, trigSize.y);
            this.util = util;
            boundaryobj = new bound[bound.Length];
            for (int i = 0; i < bound.Length; i++)
            {
                boundaryobj[i] = new bound(bound[i].transform.position);
            }
            trigtye = trig;
            IsPermanentlyDisabled = disable;
        }
        public s_trig(string name, Vector2 pos, ev_details[] ev, string util, string[] charnames, int[] intlist, o_trigger.TRIGGER_TYPE trig, Vector2 trigSize, bool disable)
        {
            this.name = name;
            pos_x = (int)pos.x;
            pos_y = (int)pos.y;
            listofevents = ev;
            this.intlist = intlist;
            this.trigSize = new s_save_vector(trigSize.x, trigSize.y);
            this.util = util;
            characternames = charnames;
            trigtye = trig;
            IsPermanentlyDisabled = disable;
        }
        public s_trig(Vector2 trigSize)
        {
            this.trigSize = new s_save_vector(trigSize.x, trigSize.y);
        }
        public bool IsPermanentlyDisabled; // Set to false by default
        public bool unlocked = true;
        public string labelToJumpTo;

        public int pos_x, pos_y;
        public string util;
        public string name;
        public int gemreq;
        public o_trigger.TRIGGER_TYPE trigtye;
        public ev_details[] listofevents;
        public string[] characternames;
        public int[] intlist;
        public bound[] boundaryobj;
        public s_save_vector trigSize;

        public List<s_customType> CustomTypes;

        public bool utilbool;
        public int utilint;
        public string utilstring;
    }

    [System.Serializable]
    public struct s_customType
    {
        public s_customType(string name, int type, string type2, object type3)
        {
            this.name = name;
            this.type = type;
            this.type2 = type2;
            this.type3 = type3;
        }
        public s_customType(string name, int type, string type2)
        {
            this.name = name;
            this.type = type;
            this.type2 = type2;
            type3 = null;
        }
        public s_customType(string name, string type2)
        {
            this.name = name;
            type = 0;
            this.type2 = type2;
            type3 = null;
        }
        public s_customType(string name, int type)
        {
            this.name = name;
            this.type = type;
            type2 = null;
            type3 = null;
        }
        public string name;
        public int type;
        public string type2;
        public object type3;

    }

    [System.Serializable]
    public struct s_customType<T>
    {
        public s_customType(string name, T type)
        {
            this.name = name;
            this.type = type;
        }
        public string name;
        public T type;
    }
    [System.Serializable]
    public struct s_customType<T, T1>
    {
        public s_customType(string name, T type, T1 type2)
        {
            this.name = name;
            this.type = type;
            this.type2 = type2;
        }
        public string name;
        public T type;
        public T1 type2;
    }
    [System.Serializable]
    public struct s_customType<T, T1, T2>
    {
        public s_customType(string name, T type, T1 type2, T2 type3)
        {
            this.name = name;
            this.type = type;
            this.type2 = type2;
            this.type3 = type3;
        }
        public string name;
        public T type;
        public T1 type2;
        public T2 type3;
    }

    [System.Serializable]
    public struct s_customType2<T,T1,T2>
    {
        public s_customType2(string name,T type, T1 type2, T2 type3)
        {
            this.name = name;
            this.type = type;
            this.type2 = type2;
            this.type3 = type3;
        }
        public string name;
        public T type;
        public T1 type2;
        public T2 type3;

    }

    [System.Serializable]
    public class s_chara
    {
        public s_chara(Vector2 pos, string mapname, string charname, string charType, bool enabled, bool spawnthis, bool disableStatic, string faction)
        {
            this.faction = faction;
            pos_x = (int)pos.x;
            pos_y = (int)pos.y;
            this.disableStatic = disableStatic;
            this.possesed = spawnthis;
            this.charname = charname;
            this.mapname = mapname;
            this.charType = charType;
        }
        public string faction;
        public string mapname;
        public string charname;
        public string charType; //Checks for the type of the character
        public bool possesed = false;  //Checks if the character is defeated or not present
        public bool disableStatic;
        public bool enabled;    //Checks if the character's control is enabled
        public int pos_x, pos_y;
    }

    [System.Serializable]
    public class s_tileobj
    {
        public s_tileobj(Vector3 pos, string objname)
        {
            pos_x = (int)pos.x;
            pos_y = (int)pos.y;
            pos_z = (int)pos.z;
            enumthing = 0;
            TYPENAME = objname;
        }
        public s_tileobj(Vector3 pos, string objname, int enumth, string cha)
        {
            exceptionChar = cha;
            pos_x = (int)pos.x;
            pos_y = (int)pos.y;
            pos_z = (int)pos.z;
            enumthing = enumth;
            TYPENAME = objname;
        }
        public s_tileobj(Vector3 pos, string objname, int enumth)
        {
            pos_x = (int)pos.x;
            pos_y = (int)pos.y;
            pos_z = (int)pos.z;
            enumthing = enumth;
            TYPENAME = objname;
        }
        public s_tileobj(Vector2 pos, string objname, Vector2 teleportpos, string mapname)
        {
            pos_x = (int)pos.x;
            pos_y = (int)pos.y;
            this.teleportpos.x = teleportpos.x;
            this.teleportpos.y = teleportpos.y;
            this.mapname = mapname;
            TYPENAME = objname;
        }

        public s_tileobj(Vector2 pos, string objname, Vector2 teleportpos, string mapname, string flagname, int[] flagchecks, string[] mapnames)
        {
            pos_x = (int)pos.x;
            pos_y = (int)pos.y;
            this.teleportpos = new s_save_vector(teleportpos.x, teleportpos.y);
            this.mapname = mapname;
            TYPENAME = objname;
            this.flagname = flagname;
            this.flagchecks = flagchecks;
            this.mapnames = mapnames;
        }

        public string flagname;
        public int[] flagchecks;
        public string[] mapnames;

        public s_save_vector tilemapPos;
        public s_save_vector size;
        public s_save_vector scale;
        public s_save_vector teleportpos;
        public string mapname;
        public string areaname;
        public string TYPENAME;
        public string exceptionChar;
        public string name;
        public string cannotPassChar;
        public bool issolid = true;

        public int enumthing = -1;
        public int pos_x, pos_y, pos_z;
        public float height;
    }
}
*/
