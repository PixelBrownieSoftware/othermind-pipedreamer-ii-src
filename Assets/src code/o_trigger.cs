using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using MagnumFoudation;

[System.Serializable]
public class trigger_obj
{
    public enum DIRECTION
    {
        NONE,
        LEFT,
        UP_LEFT,
        UP_RIGHT,
        DOWN_LEFT,
        DOWN_RIGHT,
        DOWN,
        UP,
        RIGHT
    }
    public DIRECTION dir;
    public int steps;
    public bool is_Teleport;

    public Vector2 direcion
    {
        get {
            switch (dir)
            {
                default:
                    return new Vector2(0, 0);

                case DIRECTION.DOWN:
                    return new Vector2(0,-1);

                case DIRECTION.DOWN_LEFT:
                    return new Vector2(-1, -1);

                case DIRECTION.DOWN_RIGHT:
                    return new Vector2(1, -1);

                case DIRECTION.RIGHT:
                    return new Vector2(1, 0);

                case DIRECTION.UP:
                    return new Vector2(0, 1);

                case DIRECTION.UP_LEFT:
                    return new Vector2(-1, 1);

                case DIRECTION.UP_RIGHT:
                    return new Vector2(1, 1);

                case DIRECTION.LEFT:
                    return new Vector2(-1, 0);
            }
        }
    }


}

/*
public class o_trigger : s_object {
    
    public LayerMask layer;
    [HideInInspector]
    private gui_dialogue Dialogue;
    public enum TRIGGER_TYPE {
        CONTACT,
        CONTACT_INPUT,
        NONE
    }
    public BoxCollider2D colider;
    public TRIGGER_TYPE TRIGGER_T;
    [HideInInspector]
    public bool isactive = false;
    [HideInInspector]
    const float shutterdepth = 1.55f;
    [HideInInspector]
    public o_character charac;
    public string LabelToJumpTo;

    [HideInInspector]
    bool doingEvents = false;
    [HideInInspector]
    Image fade;
    [HideInInspector]
    public s_events Events;
    bool first_move_event = true;
    bool isskipping = false;

    [HideInInspector]
    public o_character[] characters;

    [HideInInspector]
    public int ev_num = 0;
    [HideInInspector]
    public List<trigger_obj> movesteps = new List<trigger_obj>();
    s_object selobj;

    private void Awake()
    {
        fade = GameObject.Find("GUIFADE").GetComponent<Image>();
        colider = GetComponent<BoxCollider2D>();
        if (GameObject.Find("Dialogue"))
            Dialogue = GameObject.Find("Dialogue").GetComponent<gui_dialogue>();
    }

    new void Update ()
    {
        base.Update();
        if (!isactive)
        {
            switch (TRIGGER_T)
            {
                case TRIGGER_TYPE.CONTACT:

                    Collider2D c = IfTouchingGetCol(collision, "o_character");
                    if (c != null)
                    {
                        print("Got something");
                        if (c.GetComponent<o_character>() != null)
                        {
                            selobj = c.gameObject.GetComponent<s_object>();
                            print(name + c.name);
                            if (selobj)
                            {
                                o_character posses = selobj.GetComponent<o_character>();
                                print(name + c.name);
                                GameObject obj = GameObject.Find("Player");
                                o_plcharacter ch = obj.GetComponent<o_plcharacter>();
                                if (ch || posses != null && !posses.AI)
                                {
                                    print("Activating trigger");
                                    //s_triggerhandler.trig.selobj = selobj;
                                    if(posses == null)
                                        s_triggerhandler.trig.JumpToEvent(LabelToJumpTo);
                                    else
                                        s_triggerhandler.trig.JumpToEvent(LabelToJumpTo);
                                }
                            }
                        }
                    }



                    break;

                case TRIGGER_TYPE.CONTACT_INPUT:
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        c = IfTouchingGetCol(collision, "o_character");
                        if (c != null)
                        {
                            print("Got something");
                            if (c.GetComponent<o_character>() != null)
                            {
                                selobj = c.gameObject.GetComponent<s_object>();
                                print(name + c.name);
                                if (selobj)
                                {
                                    o_character posses = selobj.GetComponent<o_character>();
                                    GameObject obj = GameObject.Find("Player");
                                    o_plcharacter ch = obj.GetComponent<o_plcharacter>();
                                    if (ch || posses != null && !posses.AI)
                                    {
                                        print("Activating trigger");
                                        //s_triggerhandler.trig.selobj = selobj;
                                        if (posses == null)
                                            s_triggerhandler.trig.JumpToEvent(LabelToJumpTo);
                                        else
                                            s_triggerhandler.trig.JumpToEvent(LabelToJumpTo);
                                        //isactive = true;
                                    }
                                }
                            }
                        }
                    }
                    break;
            }
        }

    }

    public void CallTrigger()
    {
        s_triggerhandler.trig.JumpToEvent(LabelToJumpTo);
        isactive = true;
    }

    private void OnDrawGizmos()
    {
#if UNITY_EDITOR
        Vector2 pos = transform.position;
        Vector2 lastpos = transform.position;
        

        foreach (trigger_obj trig in movesteps)
        {
            pos += (trig.direcion.normalized * trig.steps * speed);
            Gizmos.DrawLine(lastpos, pos);
            lastpos = pos;
        }
#endif
    }

    TextAsset Loadtextasset(string pathname)
    {
        return (TextAsset)Resources.Load("dialogue/" + pathname);
    }

    public void IncrementEvent()
    {
        ev_num++;
    }
    public IEnumerator EventPlayMast()
    {
        doingEvents = true;
        Image sh1 = GameObject.Find("Shutter1").GetComponent<Image>();
        Image sh2 = GameObject.Find("Shutter2").GetComponent<Image>();

        for (int i = 0; i < 30; i++) {
            sh1.rectTransform.position += new Vector3(0, -shutterdepth);
            sh2.rectTransform.position += new Vector3(0, shutterdepth);
            yield return new WaitForSeconds(Time.deltaTime);
        }

        while (ev_num < Events.ev_Details.Length)
        {
            ev_details current_ev = Events.ev_Details[ev_num];
            if (current_ev.simultaneous)
            {
                StartCoroutine(EventPlay(current_ev));
            } else {
                yield return StartCoroutine(EventPlay(current_ev));
            }
            if (!isactive)
            {
                break;
            }

            print("Increment");
            ev_num++;
        }
        for (int i = 0; i < 30; i++)
        {
            sh1.rectTransform.position += new Vector3(0, shutterdepth);
            sh2.rectTransform.position += new Vector3(0, -shutterdepth);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        doingEvents = false;
        isskipping = false;
        Time.timeScale = 1;
        first_move_event = true;
    }

    IEnumerator EventPlay(ev_details currnet_ev)
    {
        ev_details current_ev = Events.ev_Details[ev_num];
        switch (current_ev.eventType)
        {

            case ev_details.EVENT_TYPES.ANIMATION:

                Animator character = selobj.GetComponent<Animator>();
                character.Play(current_ev.int0);
                character.speed = current_ev.float0;
                break;

            case ev_details.EVENT_TYPES.MOVEMNET:

                float timer = 1.02f;

                Vector2 newpos = selobj.positioninworld;
                if (first_move_event)
                    newpos = positioninworld;

                first_move_event = false;

                while (timer > 0)
                {
                    newpos += (current_ev.direcion.normalized * current_ev.float0 * current_ev.float1) * 0.007f;

                    timer -= 0.007f;
                }
                
                float dist = Vector2.Distance(selobj.positioninworld, newpos);
                Vector2 dir = (newpos - new Vector2(selobj.positioninworld.x, selobj.positioninworld.y)).normalized;
                print(newpos);


                while (Vector2.Distance(selobj.positioninworld, newpos)
                    > dist * 0.01f)
                {
                    selobj.positioninworld += (Vector3)(dir * current_ev.float0 * current_ev.float1) * 0.007f;
                    yield return new WaitForSeconds(Time.deltaTime);
                }
                break;

            case ev_details.EVENT_TYPES.DIALOGUE:
                Dialogue.done_event = false;
                StartCoroutine(Dialogue.DisplayDialogue(current_ev.string0));
                while (!Dialogue.done_event)
                {
                    yield return new WaitForSeconds(Time.deltaTime);
                }
                break;

            case ev_details.EVENT_TYPES.BREAK_EVENT:
                selobj.GetComponent<o_plcharacter>().control = true;
                isactive = false;
                if (current_ev.boolean)     //Does this cutscene reset?
                {
                    ev_num = 0;   //Reset the number
                }
                break;

            case ev_details.EVENT_TYPES.CAMERA_MOVEMENT:

                GameObject ca = GameObject.Find("Main Camera");
                ca.GetComponent<s_camera>().focus = false;

                float spe = current_ev.float0; //SPEED
                float s = 0;
                float dista = Vector2.Distance(ca.transform.position, new Vector3(current_ev.pos.x, current_ev.pos.y));
                while (Vector2.Distance(ca.transform.position, new Vector3(current_ev.pos.x, current_ev.pos.y))
                    > dista * 0.05f)
                {
                    s += spe * 0.0001f;
                    ca.transform.position = Vector2.Lerp(ca.transform.position, new Vector2(current_ev.pos.x, current_ev.pos.y), s);
                    yield return new WaitForSeconds(Time.deltaTime);
                }
                break;

            case ev_details.EVENT_TYPES.CHECK_FLAG:
                int integr = s_globals.GetGlobalFlag(current_ev.string0);

                switch (current_ev.logic)
                {
                    case ev_details.LOGIC_TYPE.ITEM_OWNED:
                        if (s_globals.CheckItem(new o_item(current_ev.string0, (o_item.ITEM_TYPE)current_ev.int0)))
                        {
                            ev_num = current_ev.int1 - 1;
                        }
                        else
                        {
                            if (current_ev.boolean)     //Does this have an "else if"?
                                ev_num = current_ev.int2;      //Other Label to jump to
                        }
                        break;

                    case ev_details.LOGIC_TYPE.VAR_EQUAL:
                        if (integr== current_ev.int0)  //Check if it is equal to the value
                        {
                            ev_num = current_ev.int1 - 1;   //Label to jump to
                        }
                        else
                        {
                            if (current_ev.boolean)     //Does this have an "else if"?
                            {
                                ev_num = current_ev.int2;      //Other Label to jump to
                            }
                        }
                        break;

                    case ev_details.LOGIC_TYPE.VAR_GREATER:
                        if (integr > current_ev.int0)  //Check if it is greater
                        {
                            ev_num = current_ev.int1;   //Label to jump to
                        }
                        else
                        {
                            if (current_ev.boolean)     //Does this have an "else if"?
                            {
                                ev_num = current_ev.int2;      //Other Label to jump to
                            }
                        }
                        break;


                    case ev_details.LOGIC_TYPE.VAR_LESS:
                        if (integr < current_ev.int0)  //Check if it is less
                        {
                            ev_num = current_ev.int1;   //Label to jump to
                        }
                        else
                        {
                            if (current_ev.boolean)     //Does this have an "else if"?
                            {
                                ev_num = current_ev.int2;      //Other Label to jump to
                            }
                        }
                        break;
                }
                if (integr == current_ev.int0)
                {

                }
                break;

            case ev_details.EVENT_TYPES.UTILITY_FOCUS:
                s_utility utility;
                o_plcharacter player = GameObject.Find("Player").GetComponent<o_plcharacter>();

                if (GetComponent<s_utility>() != null)
                {
                    player.CHARACTER_STATE = o_character.CHARACTER_STATES.STATE_NOTHING;
                    utility = GetComponent<s_utility>();
                }
                else {
                    break;
                }

                utility.istriggered = true;
                while (utility.istriggered)
                {
                    yield return new WaitForSeconds(Time.deltaTime);
                }
                break;
                

            case ev_details.EVENT_TYPES.SET_HEALTH:

                o_character c = GameObject.Find(current_ev.string0).GetComponent<o_character>();
                if (c != null)
                    c.health = current_ev.float0;

                break;

            case ev_details.EVENT_TYPES.UTILITY_CHECK:

                //Make this like the conditional statements where it checks if the utility is still active

                break;

            case ev_details.EVENT_TYPES.UTILITY_INITIALIZE:
                s_utility ut;

                if (GetComponent<s_utility>() != null)
                {
                    ut = GetComponent<s_utility>();
                    ut.istriggered = true;
                }
                else
                {
                    break;
                }

                break;

            case ev_details.EVENT_TYPES.FADE:

                Color col = new Color(current_ev.colour.a, current_ev.colour.g, current_ev.colour.b, current_ev.colour.a);
                float t = 0;

                while (fade.color != col)
                {
                    t += Time.deltaTime;
                    fade.color = Color.Lerp(fade.color, col, t);
                    yield return new WaitForSeconds(0.01f);
                }
                break;

            case ev_details.EVENT_TYPES.WAIT:

                yield return new WaitForSeconds(current_ev.float0);

                break;

            case ev_details.EVENT_TYPES.OBJECT:

                foreach (string str in current_ev.stringList)
                {
                    GameObject o = GameObject.Find(str);
                    if (o == null)
                        continue;
                    s_object ob = o.GetComponent<s_object>();
                    if (ob == null)
                        continue;

                    if (current_ev.boolean)
                    {
                        ob.DespawnObject();
                    }
                    else
                    {
                        o.SetActive(true);
                    }
                }
                break;

            case ev_details.EVENT_TYPES.DISPLAY_CHARACTER_HEALTH:

                bool check = current_ev.stringList.Length < 2;
                if (current_ev.boolean) {
                    
                    if (check)
                    {
                        if (current_ev.boolean)
                            s_gui.AddCharacter(GameObject.Find(current_ev.stringList[0]).GetComponent<o_character>(), false);
                        else
                            s_gui.AddCharacter(GameObject.Find(current_ev.stringList[0]).GetComponent<o_character>(), true);
                    }
                    else
                    {
                        List<o_character> cha = new List<o_character>();
                        foreach (string st in current_ev.stringList)
                        {
                            cha.Add(GameObject.Find(st).GetComponent<o_character>());
                        }
                        s_gui.AddCharacter(cha);
                    }

                }
                break;

            case ev_details.EVENT_TYPES.END_EVENT:

                GameObject cam = GameObject.Find("Main Camera");
                cam.GetComponent<s_camera>().focus = true;
                if (selobj)
                {
                    if (selobj.GetComponent<o_plcharacter>())
                    {
                        selobj.GetComponent<o_plcharacter>().control = true;
                    }
                }
                if (current_ev.boolean)
                {
                    string[] cha = new string[characters.Length];

                int ind = 0;
                foreach (o_character chara in characters)
                {
                    cha[ind] = chara.name;
                    ind++;
                }
                s_utility u = GetComponent<s_utility>();
                u_boundary u_b = GetComponent<u_boundary>();
                    if (u != null)
                    {
                        string uti = u.GetType().ToString();
                        s_leveledit.LevEd.savedtriggerdatalist.Add(new s_map.s_trig(name,transform.position, Events.ev_Details, uti, u_b.bounds, cha, TRIGGER_T, collision.size, true));
                    }
                    else
                        s_leveledit.LevEd.savedtriggerdatalist.Add(new s_map.s_trig(name, transform.position, Events.ev_Details, TRIGGER_T, collision.size, true));
                }
                doingEvents = false;

                Image sh1 = GameObject.Find("Shutter1").GetComponent<Image>();
                Image sh2 = GameObject.Find("Shutter2").GetComponent<Image>();
                for (int i = 0; i < 30; i++)
                {
                    sh1.rectTransform.position += new Vector3(0, shutterdepth);
                    sh2.rectTransform.position += new Vector3(0, -shutterdepth);
                    yield return new WaitForSeconds(Time.deltaTime);
                }
                DespawnObject();
                break;

            case ev_details.EVENT_TYPES.SET_FLAG:
                s_globals.SetGlobalFlag(current_ev.string0, current_ev.int0);
                break;

            case ev_details.EVENT_TYPES.CHOICE:
                int choice = 0, finalchoice = -1;
                print(choice);
                while (finalchoice == -1)
                {
                    if (Input.GetKeyDown(KeyCode.UpArrow))
                        choice--;

                    if (Input.GetKeyDown(KeyCode.DownArrow))
                        choice++;

                    if (Input.GetKeyDown(KeyCode.Z))
                    {
                        print("Chosen");
                        finalchoice = choice;
                    }
                    Dialogue.textthing.text = "";
                    Dialogue.textthing.text += current_ev.string0 + "\n";
                    for (int i = 0; i < current_ev.stringList.Length; i++)
                    {
                        if(choice == i)
                            Dialogue.textthing.text += "-> ";

                        Dialogue.textthing.text += current_ev.stringList[i] + "\n";
                    }
                    print(choice);
                    choice = Mathf.Clamp(choice, 0, current_ev.stringList.Length - 1);
                    yield return new WaitForSeconds(Time.deltaTime);
                }
                Dialogue.textthing.text = "";
                ev_num = current_ev.intList[finalchoice]-1;
                break;
        }
        //yield return new WaitForSeconds(0.5f);
    }

}

    */
