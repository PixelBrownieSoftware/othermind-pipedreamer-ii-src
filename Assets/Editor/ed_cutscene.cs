using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using MagnumFoudation;
using System;

public class ed_cutscene : MagnumFoundationEditor.ed_cutscene
{
    s_object objectItem;
    s_object[] objlist = null;
    o_character selectedCharacter = null;
    Vector2 scrollview;
    Vector2 scrollview2;    //Used for individual events 
    TextAsset utilityobj;
    TextAsset textobj;
    Vector2 mousepos;

    GameObject mouseArea;
    Vector2 dialogue_scroll;
    string label_text;
    List<string> locations;
    List<string> posLocation;
    int label_point;
    int leng;
    s_map eventMap;
    bool eventMapDisp = false;
    bool putlabel = false;
    
    Color evcolour; Event e;
    s_leveledit ed;

    Tool lasttool = Tool.None;

    //Pixel's Outstandingly Ultumate Cutscene Handler
    [MenuItem("Brownie/POUCH")]
    static void init()
    {
        GetWindow<ed_cutscene>("POUCH");
    }

    private void OnEnable()
    {
        if (mouseArea == null)
            mouseArea = GameObject.Find("Gizmo");
        
        SceneView.onSceneGUIDelegate += SceneGUI;
        lasttool = Tools.current;
    }

    void SceneGUI(SceneView sv)
    {
        e = Event.current;
        if (e.keyCode == KeyCode.S)
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            pos = ray.origin;
        }
    }

    List<string> LocationsMap()
    {
        if (ed == null)
            return null;
        if (ed.jsonMaps == null)
            return null;
        List<TextAsset> te = ed.jsonMaps;

        List<string> maploc = new List<string>();
        for (int i = 0; i < te.Count; i++)
        {
            maploc.Add(te[i].name);
        }
        return maploc;
    }

    List<string> PositionsOnMap(string n)
    {
        List<string> maploc = new List<string>();
        TextAsset te = ed.jsonMaps.Find(x => x.name == n);

        s_map m = JsonUtility.FromJson<s_map>(te.text);

        for (int i = 0; i < m.tilesdata.Count; i++)
        {
            //print((s_map.s_mapteleport)mapdat.tilesdata[i]);
            switch (m.tilesdata[i].TYPENAME)
            {
                case "teleport_object":
                    maploc.Add(m.tilesdata[i].name);
                    break;
            }
        }
        return maploc;
    }

    Vector2 TeleporterPos(string n, string t)
    {
        List<string> maploc = new List<string>();
        TextAsset te = ed.jsonMaps.Find(x => x.name == n);

        s_map m = JsonUtility.FromJson<s_map>(te.text);

        s_map.s_tileobj til = m.tilesdata.Find(x => x.TYPENAME == "teleport_object" && x.name == t);
        if (til != null)
            return new Vector2(til.pos_x, til.pos_y);
        return new Vector2(0,0);
    }

    bool FindLabel(int labelPt)
    {
        for (int l = 0; l < ed.mapDat.map_script_labels.Count; l++)
        {
            if (ed.mapDat.map_script_labels[l].index == labelPt)
            {
                return true;
            }
        }
        return false;
    }
    
    private new void OnGUI()
    {
        ed = GameObject.Find("General").GetComponent<s_leveledit>();
        base.OnGUI();
    }

    void WriteCode()
    {
        s_map map = ed.mapDat;
        string str = "";
        EditorGUILayout.Space();
        int ind = 0;
        foreach (ev_details d in map.Map_Script)
        {
            bool findLabel = FindLabel(ind);
            if (findLabel)
            {
                ev_label l = map.map_script_labels.Find(x => x.index == ind);
                EditorGUILayout.LabelField(l.name + ": \n");
            }
            ind++;
            EditorGUILayout.LabelField(ind + ". " + str + "\n");
        }
    }

    void SetStringToObjectName(ref ev_details det)
    {
        objectItem = (s_object)EditorGUILayout.ObjectField(objectItem, typeof(s_object), true);
        if (GUILayout.Button("Set string to object"))
        {
            if (objectItem != null)
                det.string0 = objectItem.name;
        }
    }
    
    public override string DisplayCode(int eventType, ev_details d)
    {
        string str = "";
        switch ((EVENT_TYPES)d.eventType)
        {
            default:
                return base.DisplayCode(eventType, d);
            case EVENT_TYPES.CHECK_FLAG:
                
                switch ((LOGIC_TYPE)d.logic)
                {
                    default:
                        return base.DisplayCode(eventType, d);

                    case LOGIC_TYPE.NUM_OF_GEMS:
                        str = "    JIGG " + d.int1;
                        break;
                    case LOGIC_TYPE.CHECK_UTILITY_RETURN_NUM:
                        str = "    If utility '" + d.string0 + "' number is equal, jump to point " + d.int1;
                        break;
                    case LOGIC_TYPE.CHECK_CHARACTER:
                        str = "    If chracter is " + d.string0 + "jump to label " + d.int1;
                        break;
                    case LOGIC_TYPE.CHECK_CHARACTER_NOT:
                        str = "    If chracter is not " + d.string0 + "jump to label " + d.int1;
                        break;
                }
                break;

            case EVENT_TYPES.DEPOSSES:
                str = "    Deposess character.";
                break;
            case EVENT_TYPES.CHANGE_SCENE:
                if (d.boolean)
                    str = "    Allow deposses";
                else
                    str = "    Disallow deposses";
                break;
            case EVENT_TYPES.UTILITY_INITIALIZE:
                str = "    Initialize utility '" + d.string0 + "'";
                break;
            case EVENT_TYPES.SET_UTILITY_FLAG:
                str = "    Set utility '" + d.string0 + "' flag to " + d.int0;
                break;
            case EVENT_TYPES.UTILITY_CHECK:
                str = "    Check if utility '" + d.string0 + "' event state is " + d.int0;
                break;

            case EVENT_TYPES.ADD_CHOICE_OPTION:
                str = "    Add choice '" + d.string0 + "' that jumps to line " + d.int0;
                break;
            case EVENT_TYPES.CHOICE:
                str = "    Present Choices";
                break;
        }
        return str;
    }

    public override void DisplayCutsceneEditor(int eventType, ref ev_details ev, int i)
    {
        switch ((EVENT_TYPES)eventType)
        {
            default:
                base.DisplayCutsceneEditor(eventType, ref ev, i);
                break;

            #region CHECK FLAG
            case EVENT_TYPES.CHECK_FLAG:
                ev.logic = (int)(LOGIC_TYPE)EditorGUILayout.EnumPopup("Logic Type", (LOGIC_TYPE)ev.logic);
                switch ((LOGIC_TYPE)ev.logic)
                {
                    default:
                        base.DisplayCutsceneEditor(eventType, ref ev, i);
                        break;

                    case LOGIC_TYPE.NUM_OF_GEMS:

                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("IF NUMBER OF GEMS  =< ");
                        ev.int0 = EditorGUILayout.IntField(ev.int0);
                        EditorGUILayout.Space();
                        EditorGUILayout.EndHorizontal();

                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("THEN JUMP TO ");
                        ev.int1 = EditorGUILayout.IntField(ev.int1);
                        EditorGUILayout.LabelField(" OR ");
                        ev.string1 = EditorGUILayout.TextField(ev.string1);
                        EditorGUILayout.EndHorizontal();
                        break;
                        
                    case LOGIC_TYPE.ITEM_OWNED:

                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("IF "); EditorGUILayout.LabelField("ITEM ");
                        ev.string0 = EditorGUILayout.TextField(ev.string0);
                        EditorGUILayout.LabelField(" WITH TYPE ");
                        ev.int0 = EditorGUILayout.IntField(ev.int0);
                        EditorGUILayout.LabelField(" (" + (o_item.ITEM_TYPE)ev.int0 + ")");
                        EditorGUILayout.LabelField(" POSSESED.");
                        EditorGUILayout.EndHorizontal();

                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("THEN JUMP TO ");
                        ev.int1 = EditorGUILayout.IntField(ev.int1);
                        EditorGUILayout.EndHorizontal();
                        break;

                    case LOGIC_TYPE.CHECK_CHARACTER:

                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("IF CHARACTER IS ");
                        ev.string0 = EditorGUILayout.TextField(ev.string0);
                        EditorGUILayout.EndHorizontal();

                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("THEN JUMP TO ");
                        ev.int1 = EditorGUILayout.IntField(ev.int1);
                        EditorGUILayout.EndHorizontal();
                        break;

                    case LOGIC_TYPE.CHECK_CHARACTER_NOT:

                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("IF CHARACTER IS NOT");
                        ev.string0 = EditorGUILayout.TextField(ev.string0);
                        EditorGUILayout.EndHorizontal();

                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("THEN JUMP TO ");
                        ev.int1 = EditorGUILayout.IntField(ev.int1);
                        EditorGUILayout.LabelField(" OR ");
                        ev.string1 = EditorGUILayout.TextField(ev.string1);
                        EditorGUILayout.EndHorizontal();
                        break;

                    case LOGIC_TYPE.CHECK_UTILITY_RETURN_NUM:

                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("IF UITILITY ");
                        SetStringToObjectName(ref ev);
                        EditorGUILayout.LabelField(ev.string0);
                        EditorGUILayout.LabelField(" RETURN NUMBER IS ");
                        ev.int0 = EditorGUILayout.IntField(ev.int0);
                        EditorGUILayout.EndHorizontal();

                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("THEN JUMP TO ");
                        ev.int1 = EditorGUILayout.IntField(ev.int1);
                        EditorGUILayout.LabelField(" OR ");
                        ev.string1 = EditorGUILayout.TextField(ev.string1);
                        EditorGUILayout.EndHorizontal();
                        break;

                    case LOGIC_TYPE.VAR_NOT_EQUAL:

                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("IF ");
                        ev.string0 = EditorGUILayout.TextField(ev.string0);
                        EditorGUILayout.LabelField(" != ");
                        ev.int0 = EditorGUILayout.IntField(ev.int0);
                        EditorGUILayout.Space();
                        EditorGUILayout.EndHorizontal();

                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("THEN JUMP TO LABEL");
                        ev.string1 = EditorGUILayout.TextField(ev.string1);
                        EditorGUILayout.EndHorizontal();
                        break;
                }
                break;
            #endregion

            #region CREATE OBJECT
            case EVENT_TYPES.CREATE_OBJECT:
                
                EditorGUILayout.LabelField("Object type:");
                ev.string0 = EditorGUILayout.TextField(ev.string0);
                EditorGUILayout.LabelField("Object name: " + ev.string1);
                ev.string1 = EditorGUILayout.TextField(ev.string1);
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Object position:");
                EditorGUILayout.BeginHorizontal();
                ev.float0 = EditorGUILayout.FloatField(ev.float0);
                ev.float1 = EditorGUILayout.FloatField(ev.float1);
                EditorGUILayout.EndHorizontal();
                break;
            #endregion

            #region DELETE OBJECT
            case EVENT_TYPES.DELETE_OBJECT:

                EditorGUILayout.LabelField("Object type: " + ev.string0);
                EditorGUILayout.LabelField("Object name: " + ev.string1);
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Object position:");
                EditorGUILayout.BeginHorizontal();
                ev.float0 = EditorGUILayout.FloatField(ev.float0);
                ev.float1 = EditorGUILayout.FloatField(ev.float1);
                EditorGUILayout.EndHorizontal();
                break;
            #endregion

            #region SET OBJECT COLLISION SIZE
            case EVENT_TYPES.SET_OBJ_COLLISION:

                EditorGUILayout.LabelField("Name:");
                ev.string0 = EditorGUILayout.TextField(ev.string0);
                EditorGUILayout.BeginHorizontal();
                ev.float0 = EditorGUILayout.FloatField(ev.float0);
                ev.float1 = EditorGUILayout.FloatField(ev.float1);
                EditorGUILayout.EndHorizontal();
                break;
            #endregion

            #region DISPLAY CHARACTER HEALTH
            case EVENT_TYPES.DISPLAY_CHARACTER_HEALTH:
                leng = EditorGUILayout.IntField(leng);
                if (GUILayout.Button("New list"))
                {
                    ev.stringList = new string[leng];
                    objlist = new s_object[leng];
                }

                if (ev.stringList != null || ev.stringList.Length > 0)
                {
                    if (objlist == null)
                        objlist = new s_object[leng];
                    for (int o = 0; o < ev.stringList.Length; o++)
                    {
                        objlist[o] = (o_character)EditorGUILayout.ObjectField(objlist[o], typeof(o_character), true);

                        if (objlist[o] != null)
                            ev.stringList[o] = objlist[o].name;
                    }
                    for (int o = 0; o < ev.stringList.Length; o++)
                    {
                        EditorGUILayout.LabelField(ev.stringList[o]);
                    }
                }
                //There will be one GUI for the top (near main character health) and one in the bottom (boss health)
                ev.boolean1 = EditorGUILayout.Toggle("Top?", ev.boolean1);
                //
                ev.boolean = EditorGUILayout.Toggle("Show?", ev.boolean);

                break;
            #endregion

            #region WAIT
            case EVENT_TYPES.WAIT:

                ev.float0 = EditorGUILayout.FloatField("Length: ", ev.float0);

                break;

            #endregion

            #region SET_HEALTH
            case EVENT_TYPES.SET_HEALTH:

                selectedCharacter = (o_character)EditorGUILayout.ObjectField(selectedCharacter, typeof(o_character), true);
                if (selectedCharacter != null)
                    ev.string0 = selectedCharacter.name;
                ev.float0 = EditorGUILayout.FloatField("Set health to: ", ev.float0);
                break;
            #endregion

            #region CHOICE 
            case EVENT_TYPES.CHOICE:
                ev.string0 = EditorGUILayout.TextField("Question label:", ev.string0);
                /*
                leng = EditorGUILayout.IntField(leng);
                if (GUILayout.Button("New list"))
                {
                    ev.stringList = new string[leng];
                    ev.intList = new int[leng];
                }
                if (ev.stringList != null)
                {
                    //EditorGUILayout.BeginScrollView(scrollview2);
                    for (int o = 0; o < ev.stringList.Length; o++)
                    {
                        ev.stringList[o] = EditorGUILayout.TextField("Name of choice" + o + " :", ev.stringList[o]);
                        ev.intList[o] = EditorGUILayout.IntField("Event position to jump to: ", ev.intList[o]);
                        EditorGUILayout.Space();
                    }
                    //EditorGUILayout.EndScrollView();

                    for (int o = 0; o < ev.stringList.Length; o++)
                    {
                        EditorGUILayout.LabelField(ev.stringList[o]);
                        EditorGUILayout.LabelField("" + ev.intList[o]);
                    }
                }
                */
                break;
            #endregion

            #region ADD CHOICE 
            case EVENT_TYPES.ADD_CHOICE_OPTION:
                ev.string0= EditorGUILayout.TextField("Name of choice: ", ev.string0);
                ev.int0 = EditorGUILayout.IntField("Event position to jump to: ", ev.int0);
                ev.string1 = EditorGUILayout.TextField("or label name: ", ev.string1);
                break;
            #endregion

            #region SHOW_TEXT
            case EVENT_TYPES.SHOW_TEXT:

                ev.float0 = EditorGUILayout.FloatField(ev.float0);
                ev.string0 = EditorGUILayout.TextArea(ev.string0);
                break;
            #endregion
                
        }
    }

    public override void EnumChange(ref ev_details ev)
    {
        ev.eventType = (int)(EVENT_TYPES)EditorGUILayout.EnumPopup((EVENT_TYPES)ev.eventType);
    }
    /*
    void DrawDetailsTrigger(o_trigger trig)
    {
        int EVNUM = 0;
        ev_details[] details = trig.Events.ev_Details;
        for (int i = 0; i < details.Length; i++)
        {
            ev_details ev = details[i];
            if (ev != null)
            {
                EditorGUILayout.BeginHorizontal();
                foldoutlist[i] = EditorGUILayout.Foldout(foldoutlist[i], "Event Numeber: " + EVNUM, true);
                

                EditorGUILayout.EndHorizontal();
            }

            if (foldoutlist[i])
            {
                switch (ev.eventType)
                {


                }
                EditorGUILayout.Space();
            }
            EditorGUILayout.Space();
            EVNUM++;

        }
    }
    
    void LoadTempLevel(string dir)
    {
        s_leveledit ed = GameObject.Find("Main Camera").GetComponent<s_leveledit>();
        ed.LoadTempMap(ed.JsonToObj(dir));
    }
    */
    /*
    void DrawDetailsTrigger()
    {
        int EVNUM = 0;
        List<ev_label> labels = eventMap.map_script_labels;
        List<ev_details> details = eventMap.Map_Script;
        if (foldoutlist2 != null)
        {
            for (int i = 0; i < details.Count; i++)
            {
                ev_details ev = details[i];
                if (ev != null)
                {
                    bool findLabel = false;
                    ev_label lab = new ev_label(null, -1);
                    int l = 0;
                    for (l = 0; l < labels.Count; l++)
                    {
                        if (labels[l].index == i)
                        {
                            lab = labels[l];
                            findLabel = true;
                            break;
                        }
                    }
                    if (findLabel)
                    {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Label: ");
                        lab.name = EditorGUILayout.TextArea(lab.name);
                        labels[l] = lab;
                        EditorGUILayout.Space();
                        EditorGUILayout.EndHorizontal();
                    }

                    EditorGUILayout.BeginHorizontal();
                    //EditorGUILayout.LabelField("Event Numeber: " + EVNUM);
                    

                    foldoutlist2[i] = EditorGUILayout.Foldout(foldoutlist2[i], "Event Numeber: " + EVNUM, true);

                    #region REMOVE EVENT
                    if (GUILayout.Button("-"))
                    {
                        details.RemoveAt(i);
                        foldoutlist2 = new bool[details.Count];
                    }
                    #endregion

                    #region MOVE EVENT
                    if (0 < i)
                    {
                        if (GUILayout.Button("^"))
                        {
                            int index = i - 1; //For the index of the new object above
                            ev_details det = details[i];
                            details.RemoveAt(i);
                            details.Insert(index, det);
                            foldoutlist2 = new bool[details.Count];
                        }
                    }
                    if (details.Count > i + 1)
                    {
                        if (GUILayout.Button("v"))
                        {
                            int index = i + 1;
                            ev_details det = details[i];
                            details.RemoveAt(i);
                            details.Insert(index, det);
                            foldoutlist2 = new bool[details.Count];
                        }
                    }
                    #endregion

                    //ev.simultaneous = EditorGUILayout.Toggle("Simultaneous?", ev.simultaneous);

                    ev.eventType = (EVENT_TYPES)EditorGUILayout.EnumPopup(ev.eventType);

                    EditorGUILayout.EndHorizontal();
                }
                if (foldoutlist2[i])
                {
                    switch (ev.eventType)
                    {

                    }
                    EditorGUILayout.Space();
                }

                EVNUM++;

            }

        }
    }
    */
}