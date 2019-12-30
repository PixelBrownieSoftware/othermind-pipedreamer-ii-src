using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
public class s_triggerhandler : MonoBehaviour {

    public static s_triggerhandler trig;
    public bool doingEvents = false;
    Image fade;
    private gui_dialogue Dialogue;
    const float shutterdepth = 1.55f;
    public List<ev_details> Events;
    public o_trigger current_trigger;
    string current_label;
    bool first_move_event = true;
    public o_character[] characters;

    public o_character host;

    s_leveledit leveled;
    public s_object selobj;
    public int pointer;
    List<ev_label> labels = new List<ev_label>();

    bool activated_shutters = false;

    private void Awake()
    {
        leveled = GetComponent<s_leveledit>();
        if (trig == null)
            trig = this;
        fade = GameObject.Find("GUIFADE").GetComponent<Image>();
        if (GameObject.Find("Dialogue"))
            Dialogue = GameObject.Find("Dialogue").GetComponent<gui_dialogue>();
    }


    public void StartEvent(string label)
    {
        int jump = labels.Find(x => x.name == label).index;
        pointer = jump;
        StartCoroutine(EventPlayMast());
    }

    public void JumpToEvent(string label, o_character host)
    {
        this.host = host;
        int jump = labels.Find(x => x.name == label).index;
        pointer = jump;
        StartCoroutine(EventPlayMast());
    }
    public void JumpToEvent(string label)
    {
        int jump = labels.Find(x => x.name == label).index;
        pointer = jump;
        StartCoroutine(EventPlayMast());
    }

    public void GetMapEvents()
    {
        labels = s_leveledit.LevEd.mapDat.map_script_labels;
        Events = s_leveledit.LevEd.mapDat.Map_Script;
    }
    
    
    public IEnumerator EventPlayMast()
    {
        doingEvents = true;
        Image sh1 = GameObject.Find("Shutter1").GetComponent<Image>();
        Image sh2 = GameObject.Find("Shutter2").GetComponent<Image>();

        while (pointer != -1)
        {
            yield return StartCoroutine(EventPlay());

            print("Pointer at: " + pointer);
            if (pointer == -1)
                break;
            pointer++;
        }
        if (activated_shutters)
        {
            for (int i = 0; i < 30; i++)
            {
                sh1.rectTransform.position += new Vector3(0, shutterdepth);
                sh2.rectTransform.position += new Vector3(0, -shutterdepth);
                yield return new WaitForSeconds(Time.deltaTime);
            }
        }
        current_trigger = null;
        doingEvents = false;
        //isskipping = false;
        Time.timeScale = 1;
        first_move_event = true;
        activated_shutters = false;
    }

    IEnumerator EventPlay()
    {
        ev_details current_ev = Events[pointer];
        switch (current_ev.eventType)
        {

            case ev_details.EVENT_TYPES.DEPOSSES:
                o_plcharacter pla = GameObject.Find("Player").GetComponent<o_plcharacter>();
                pla.Depossess();
                break;

            case ev_details.EVENT_TYPES.SHOW_TEXT:

                s_gui.DisplayNotificationText(current_ev.string0, current_ev.float0);
                break;

            case ev_details.EVENT_TYPES.ANIMATION:

                Animator character = selobj.GetComponent<Animator>();
                character.Play(current_ev.int0);
                character.speed = current_ev.float0;
                break;

            case ev_details.EVENT_TYPES.MOVEMNET:

                float timer = 1.02f;
                o_character charaMove = GameObject.Find(current_ev.string0).GetComponent<o_character>();

                Vector2 newpos = charaMove.transform.position;

                if (first_move_event)
                    if (!current_ev.boolean)
                    {

                        if (GameObject.Find(current_ev.string0).GetComponent<o_character>() == selobj)
                        {
                            newpos = current_trigger.positioninworld;
                            first_move_event = false;
                        }
                    }

                if (current_ev.boolean)
                {
                    charaMove.transform.position = new Vector3(current_ev.float0, current_ev.float1, 0);
                    break;
                }

                first_move_event = false;
                while (timer > 0)
                {
                    newpos += (current_ev.direcion.normalized * current_ev.float0 * current_ev.float1) * 0.007f;
                    timer -= 0.007f;
                }

                float dist = Vector2.Distance(charaMove.positioninworld, newpos);
                Vector2 dir = (newpos - new Vector2(charaMove.transform.position.x, charaMove.transform.position.y)).normalized;
                print(newpos);


                while (Vector2.Distance(charaMove.positioninworld, newpos)
                    > dist * 0.01f)
                {
                    charaMove.transform.position += (Vector3)(dir * current_ev.float0 * current_ev.float1) * 0.007f;
                    yield return new WaitForSeconds(Time.deltaTime);
                }



                break;

            case ev_details.EVENT_TYPES.DISPLAY_IMAGE:

                //Enable image via string
                //Maybe display via position
                
                yield return new WaitForSeconds(current_ev.float0);
                //Disable image

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
                if(selobj.GetComponent<o_character>() != null)
                    selobj.GetComponent<o_character>().control = true;

                pointer = -1;
                break;

            case ev_details.EVENT_TYPES.RUN_CHARACTER_SCRIPT:

                if (!current_ev.boolean)
                {
                    foreach (string characternam in current_ev.stringList)
                    {
                        GameObject.Find(characternam).GetComponent<o_character>().control = true;
                    }
                }
                else {
                    if (!current_ev.boolean1) {

                        GameObject obj = GameObject.Find("Player");
                        o_plcharacter ch = obj.GetComponent<o_plcharacter>();
                        ch.velocity = Vector2.zero;
                        ch.control = false;
                    }
                    //ch.positioninworld = new Vector3(transform.position.x, transform.position.y);
                    //ch.transform.position = new Vector3(transform.position.x, transform.position.y);
                }

                break;

            case ev_details.EVENT_TYPES.CAMERA_MOVEMENT:

                GameObject ca = GameObject.Find("Main Camera");
                ca.GetComponent<s_camera>().focus = false;

                float spe = current_ev.float0; //SPEED
                float s = 0;

                s_object obje = GameObject.Find(current_ev.string0).GetComponent<s_object>();

                Vector2 pos = obje.positioninworld;
                if (current_ev.boolean)
                {
                    float dista = Vector2.Distance(ca.transform.position, new Vector3(pos.x, pos.y));

                    while (Vector2.Distance(ca.transform.position, new Vector3(pos.x, pos.y))
                        > dista * 0.05f)
                    {
                        s += spe * 0.0001f;
                        ca.transform.position = Vector3.Lerp(ca.transform.position, new Vector3(pos.x, pos.y, -15), s);
                        yield return new WaitForSeconds(Time.deltaTime);
                    }
                    if (current_ev.boolean1)
                    {
                        ca.GetComponent<s_camera>().focus = true;
                        ca.GetComponent<s_camera>().player = obje.GetComponent<o_character>();

                    }
                }
                else {

                    float dista = Vector2.Distance(ca.transform.position, new Vector3(current_ev.pos.x, current_ev.pos.y));
                    while (Vector2.Distance(ca.transform.position, new Vector3(current_ev.pos.x, current_ev.pos.y))
                        > dista * 0.05f)
                    {
                        s += spe * 0.0001f;
                        ca.transform.position = Vector2.Lerp(ca.transform.position, new Vector2(current_ev.pos.x, current_ev.pos.y), s);
                        yield return new WaitForSeconds(Time.deltaTime);
                    }
                }
                break;

            case ev_details.EVENT_TYPES.CHECK_FLAG:
                int integr = s_globals.GetGlobalFlag(current_ev.string0);

                switch (current_ev.logic)
                {
                    case ev_details.LOGIC_TYPE.ITEM_OWNED:
                        if (s_globals.CheckItem(new o_item(current_ev.string0, (o_item.ITEM_TYPE)current_ev.int0)))
                        {
                            current_trigger.ev_num = current_ev.int1 - 1;
                        }
                        else
                        {
                            if (current_ev.boolean)     //Does this have an "else if"?
                                current_trigger.ev_num = current_ev.int2;      //Other Label to jump to
                        }
                        break;

                    case ev_details.LOGIC_TYPE.VAR_EQUAL:
                        if (integr == current_ev.int0)  //Check if it is equal to the value
                        {
                            current_trigger.ev_num = current_ev.int1 - 1;   //Label to jump to
                        }
                        else
                        {
                            if (current_ev.boolean)     //Does this have an "else if"?
                            {
                                current_trigger.ev_num = current_ev.int2;      //Other Label to jump to
                            }
                        }
                        break;

                    case ev_details.LOGIC_TYPE.VAR_GREATER:
                        if (integr > current_ev.int0)  //Check if it is greater
                        {
                            current_trigger.ev_num = current_ev.int1;   //Label to jump to
                        }
                        else
                        {
                            if (current_ev.boolean)     //Does this have an "else if"?
                            {
                                current_trigger.ev_num = current_ev.int2;      //Other Label to jump to
                            }
                        }
                        break;


                    case ev_details.LOGIC_TYPE.VAR_LESS:
                        if (integr < current_ev.int0)  //Check if it is less
                        {
                            current_trigger.ev_num = current_ev.int1;   //Label to jump to
                        }
                        else
                        {
                            if (current_ev.boolean)     //Does this have an "else if"?
                            {
                                current_trigger.ev_num = current_ev.int2;      //Other Label to jump to
                            }
                        }
                        break;

                    case ev_details.LOGIC_TYPE.CHECK_UTILITY_RETURN_NUM:

                        //This checks utilities after the INITIALIZE function
                        if (GetComponent<s_utility>().eventState == current_ev.int0) 
                        {
                            current_trigger.ev_num = current_ev.int1;   //Label to jump to
                        }
                        else
                        {
                            current_trigger.ev_num = current_ev.int2;
                        }
                        break;

                    case ev_details.LOGIC_TYPE.CHECK_CHARACTER:
                        if(host.ID == current_ev.string0)
                            current_trigger.ev_num = current_ev.int1;
                        break;

                    case ev_details.LOGIC_TYPE.CHECK_CHARACTER_NOT:
                        if (host.ID != current_ev.string0)
                            current_trigger.ev_num = current_ev.int1;
                        break;
                }
                if (integr == current_ev.int0)
                {

                }
                break;

            case ev_details.EVENT_TYPES.PUT_SHUTTERS:
                activated_shutters = true;
                Image sh1 = GameObject.Find("Shutter1").GetComponent<Image>();
                Image sh2 = GameObject.Find("Shutter2").GetComponent<Image>();

                for (int i = 0; i < 30; i++)
                {
                    sh1.rectTransform.position += new Vector3(0, -shutterdepth);
                    sh2.rectTransform.position += new Vector3(0, shutterdepth);
                    yield return new WaitForSeconds(Time.deltaTime);
                }
                break;

            case ev_details.EVENT_TYPES.SET_UTILITY_FLAG:
                GameObject utGO = GameObject.Find(current_ev.string0);
                s_utility utility = utGO.GetComponent<s_utility>();

                if (utility != null)
                    utility.eventState = current_ev.int0;
                else
                    break;

                break;
                
            case ev_details.EVENT_TYPES.UTILITY_CHECK:

                //Make this like the conditional statements where it checks if the utility is still active

                break;

            case ev_details.EVENT_TYPES.UTILITY_INITIALIZE:
                GameObject utGO1 = GameObject.Find(current_ev.string0);
                s_utility ut = utGO1.GetComponent<s_utility>();

                if (ut != null)
                    ut.istriggered = true;
                else
                    break;

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

            case ev_details.EVENT_TYPES.ALLOW_DEPOSSES:

                o_plcharacter pl = GameObject.Find("Player").GetComponent<o_plcharacter>();
                pl.canDeposses = current_ev.boolean;

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
                if (current_ev.boolean)
                {

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
                

            case ev_details.EVENT_TYPES.SET_FLAG:
                s_globals.SetGlobalFlag(current_ev.string0, current_ev.int0);
                break;

            case ev_details.EVENT_TYPES.CHANGE_MAP:

                if (selobj.GetComponent<o_character>() != null)
                    selobj.GetComponent<o_character>().control = true;

                pointer = -1;
                doingEvents = false;
                s_leveledit.LevEd.TriggerSpawn(current_ev.string0, new Vector2(current_ev.float0, current_ev.float1));
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
                        if (choice == i)
                            Dialogue.textthing.text += "-> ";

                        Dialogue.textthing.text += current_ev.stringList[i] + "\n";
                    }
                    print(choice);
                    choice = Mathf.Clamp(choice, 0, current_ev.stringList.Length - 1);
                    yield return new WaitForSeconds(Time.deltaTime);
                }
                Dialogue.textthing.text = "";
                current_trigger.ev_num = current_ev.intList[finalchoice] - 1;
                break;
        }
        //yield return new WaitForSeconds(0.5f);
    }
}
*/
