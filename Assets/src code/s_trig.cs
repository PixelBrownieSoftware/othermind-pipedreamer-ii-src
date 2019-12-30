using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MagnumFoudation;
using UnityEngine.UI;

[System.Serializable]
public struct s_dialogue_choice
{
    public s_dialogue_choice(string option, int flagTojump)
    {
        this.option = option;
        this.flagTojump = flagTojump;
    }

    public string option;
    public int flagTojump;
}

public class s_trig : s_triggerhandler
{
    public List<s_dialogue_choice> dialogueChoices = new List<s_dialogue_choice>();
    public Image fade;
    const float shutterdepth = 1.55f;
    string current_label;
    
    public PDII_character host;

    s_leveledit leveled;
    

    void Start()
    {
        trig = this;
    }

    public override IEnumerator EventPlay()
    {
        switch ((EVENT_TYPES)current_ev.eventType)
        {
            default:
                yield return StartCoroutine(base.EventPlay());
                break;

            case EVENT_TYPES.CAMERA_MOVEMENT:

                GameObject ca = GameObject.Find("Main Camera");
                ca.GetComponent<s_camera>().focus = false;
                ca.GetComponent<s_camera>().ResetSpeedProg();
                ca.GetComponent<s_camera>().lerping = true;

                float spe = current_ev.float0; //SPEED
                float s = 0;

                s_object obje = null;

                if (current_ev.string0 != "o_player")
                {
                    if (GameObject.Find(current_ev.string0) != null)
                        obje = GameObject.Find(current_ev.string0).GetComponent<s_object>();
                }
                else {
                    if (host != null)
                        obje = host;
                    else
                        obje = player;
                }

                Vector2 pos = new Vector2(0, 0);
                if (obje != null)
                    pos = obje.transform.position;

                if (obje != null)
                    ca.GetComponent<s_camera>().targetPos = obje.transform.position;
                else
                    ca.GetComponent<s_camera>().targetPos = new Vector2(current_ev.pos.x, current_ev.pos.y);

                if (current_ev.boolean)
                {
                    float dista = Vector2.Distance(ca.transform.position, new Vector3(pos.x, pos.y));

                    while (Vector2.Distance(ca.transform.position, new Vector3(pos.x, pos.y))
                        > dista * 0.05f)
                    {
                        // s += spe * Time.deltaTime * travSpeed;
                        // ca.transform.position = Vector3.Lerp(ca.transform.position, new Vector3(pos.x, pos.y, -15), s);
                        yield return new WaitForSeconds(Time.deltaTime);
                    }
                    if (current_ev.boolean1)
                    {
                        ca.GetComponent<s_camera>().focus = true;
                        // ca.GetComponent<s_camera>(). = obje.GetComponent<o_character>();

                    }
                }
                else
                {

                    float dista = Vector2.Distance(ca.transform.position, new Vector3(current_ev.pos.x, current_ev.pos.y));
                    while (Vector2.Distance(ca.transform.position, new Vector3(current_ev.pos.x, current_ev.pos.y))
                        > dista * 0.05f)
                    {
                        // s += spe * Time.deltaTime * travSpeed;
                        // ca.transform.position = Vector2.Lerp(ca.transform.position, new Vector3(current_ev.pos.x, current_ev.pos.y, -15), s);
                        //ca.transform.position = new Vector3(ca.transform.position.x, ca.transform.position.y, -15);
                        yield return new WaitForSeconds(Time.deltaTime);
                    }
                }
                ca.GetComponent<s_camera>().lerping = false;
                break;

            case EVENT_TYPES.MOVEMNET:

                float timer = 1.02f;
                o_character charaMove = null;

                s_map.s_tileobj to = s_levelloader.LevEd.mapDat.tilesdata.Find(
                    x => x.TYPENAME == "teleport_object" &&
                    x.name == current_ev.string1);

                if (current_ev.string0 == "o_player")
                {
                    if (host == null)
                        charaMove = player;
                    else
                        charaMove = host;
                }
                else {
                    charaMove = GameObject.Find(current_ev.string0).GetComponent<o_character>();
                }

                Vector2 newpos = charaMove.transform.position;

                if (current_ev.boolean)
                {
                    newpos = new Vector2(to.pos_x, to.pos_y);
                    charaMove.transform.position = new Vector3(newpos.x, newpos.y, 0);
                    break;
                }

                float dist = Vector2.Distance(charaMove.transform.position, newpos);
                Vector2 dir = (newpos - new Vector2(charaMove.transform.position.x, charaMove.transform.position.y)).normalized;
                print(newpos);

                while (Vector2.Distance(charaMove.transform.position, newpos)
                    > dist * 0.01f)
                {
                    charaMove.transform.position += (Vector3)(dir * current_ev.float0 * current_ev.float1) * 0.007f;
                    yield return new WaitForSeconds(Time.deltaTime);
                }
                break;

            case EVENT_TYPES.CHANGE_MAP:

                dialogueChoices.Clear();
                if (player != null)
                {
                    player.direction = new Vector2(0, 0);
                    player.rbody2d.velocity = Vector2.zero;
                    player.control = false;
                    player.SetAnimation("idle_d", false);
                    player.CHARACTER_STATE = o_character.CHARACTER_STATES.STATE_IDLE;
                }
                if (host != null)
                {
                    host.direction = new Vector2(0, 0);
                    host.rbody2d.velocity = Vector2.zero;
                    host.control = false;
                    host.SetAnimation("idle_d",false);
                    host.CHARACTER_STATE = o_character.CHARACTER_STATES.STATE_IDLE;
                }

                float t2 = 0;
                while (fade.color != Color.black)
                {
                    t2 += Time.deltaTime;
                    fade.color = Color.Lerp(Color.clear, Color.black, t2);
                    yield return new WaitForSeconds(Time.deltaTime);
                }

                if (host == null)
                    s_levelloader.LevEd.TriggerSpawn(current_ev.string0, current_ev.string1, player);
                else
                    s_levelloader.LevEd.TriggerSpawn(current_ev.string0, current_ev.string1, host);
                
                t2 = 0;
                while (fade.color != Color.clear)
                {
                    t2 += Time.deltaTime;
                    fade.color = Color.Lerp(Color.black, Color.clear, t2);
                    yield return new WaitForSeconds(Time.deltaTime);
                }

                if (player != null)
                    player.control = true;
                if (host != null)
                    host.control = true;

                pointer = -1;
                doingEvents = false;
                break;

            case EVENT_TYPES.DEPOSSES:

                if (host != null)
                {
                    host.OnDeposess();
                    host = null;
                }
                break;

            case EVENT_TYPES.ADD_CHOICE_OPTION:
                s_dialogue_choice dialo = new s_dialogue_choice(current_ev.string0, FindLabel(current_ev.string1));

                if (dialogueChoices != null)
                    dialogueChoices.Add(dialo);
                else
                {
                    dialogueChoices = new List<s_dialogue_choice>();
                    dialogueChoices.Add(dialo);
                }
                break;

            case EVENT_TYPES.CHECK_FLAG:

                int integr = s_globals.GetGlobalFlag(current_ev.string0);

                int labelNum = FindLabel(current_ev.string1);

                if (labelNum == int.MinValue)
                    labelNum = current_ev.int1 - 1;

                switch ((LOGIC_TYPE)current_ev.logic)
                {
                    default:
                        yield return StartCoroutine(base.EventPlay());
                        break;

                    case LOGIC_TYPE.CHECK_CHARACTER:
                        if (host != null)
                        {
                            if (host.ID == current_ev.string0)  //Check if it is equal to the value
                            {
                                pointer = labelNum;   //Label to jump to
                            }
                        }
                        break;

                    case LOGIC_TYPE.CHECK_CHARACTER_NOT:
                        if (host != null)
                        {
                            if (host.ID != current_ev.string0)
                            {
                                pointer = labelNum;   //Label to jump to
                            }
                        }
                        else
                            pointer = labelNum;
                        break;
                        

                        /*
                    case LOGIC_TYPE.CHECK_UTILITY_RETURN_NUM:

                        //This checks utilities after the INITIALIZE function
                        if (GetComponent<s_utility>().eventState == current_ev.int0)
                        {
                            pointer = current_ev.int1 - 1;   //Label to jump to
                        }
                        else
                        {
                            pointer = current_ev.int2 - 1;
                        }
                        break;
                        */
                }
                break;

            case EVENT_TYPES.CHOICE:
                int choice = 0, finalchoice = -1;
                print(choice);

                while (finalchoice == -1)
                {
                    if (Input.GetKeyDown(KeyCode.UpArrow))
                        choice--;

                    if (Input.GetKeyDown(KeyCode.DownArrow))
                        choice++;

                    choice = Mathf.Clamp(choice, 0, dialogueChoices.Count - 1);

                    if (Input.GetKeyDown(KeyCode.Z))
                    {
                        print("Chosen");
                        finalchoice = choice;
                    }
                    Dialogue.text = "Arrow keys to scroll, Z to select" + "\n";
                    Dialogue.text += current_ev.string0 + "\n";
                    for (int i = 0; i < dialogueChoices.Count - 1; i++)
                    {
                        if (choice == i)
                            Dialogue.text += "-> ";

                        Dialogue.text += dialogueChoices[i].option + "\n";
                    }
                    print(choice);
                    yield return new WaitForSeconds(Time.deltaTime);
                }
                Dialogue.text = "";
                pointer = dialogueChoices[finalchoice].flagTojump - 1;
                break;

            case EVENT_TYPES.CLEAR_CHOICES:
                dialogueChoices.Clear();
                break;

            case EVENT_TYPES.RUN_CHARACTER_SCRIPT:


                if (current_ev.string0 == "o_player")
                {
                    o_character ch = null;
                    if (host == null)
                        ch = player.GetComponent<o_character>();
                    else
                        ch = host.GetComponent<o_character>();

                    if (ch.rbody2d != null)
                        ch.rbody2d.velocity = Vector2.zero;
                    ch.control = current_ev.boolean;
                    ch.CHARACTER_STATE = o_character.CHARACTER_STATES.STATE_IDLE;
                }
                else
                {
                    GameObject obj = GameObject.Find(current_ev.string0);
                    if (obj != null)
                    {
                        o_character ch = obj.GetComponent<o_character>();
                        if (ch != null)
                        {
                            if (ch.rbody2d != null)
                                ch.rbody2d.velocity = Vector2.zero;
                            ch.control = current_ev.boolean;
                            ch.CHARACTER_STATE = o_character.CHARACTER_STATES.STATE_IDLE;
                        }
                    }
                }
                break;
        }
    }
    /*
    switch ((EVENT_TYPES)current_ev.eventType)
    {

        case EVENT_TYPES.DEPOSSES:
            if (host != null)
            {
                host.OnDeposess();
                host.DespawnObject();
            }
            break;


        case EVENT_TYPES.BREAK_EVENT:
            dialogueChoices.Clear();
            pointer = -1;
            break;

        case EVENT_TYPES.SET_UTILITY_FLAG:
            GameObject utGO = GameObject.Find(current_ev.string0);
            if (utGO)
            {
                s_utility utility = utGO.GetComponent<s_utility>();

                if (utility != null)
                    utility.eventState = current_ev.int0;
                else
                    break;
            }

            break;

        case EVENT_TYPES.CAMERA_MOVEMENT:

            GameObject ca = GameObject.Find("Main Camera");
            ca.GetComponent<s_camera>().focus = false;

            float spe = current_ev.float0 / 22.5f; //SPEED
            float s = 0;

            s_object obje = null;

            if (GameObject.Find(current_ev.string0) != null)
                obje = GameObject.Find(current_ev.string0).GetComponent<s_object>();

            Vector2 pos = new Vector2(0, 0);
            if (obje != null)
                pos = obje.transform.position;

            if (current_ev.boolean)
            {
                float dista = Vector2.Distance(ca.transform.position, new Vector3(pos.x, pos.y));

                while (Vector2.Distance(ca.transform.position, new Vector3(pos.x, pos.y))
                    > dista * 0.05f)
                {
                    s += spe * Time.deltaTime;
                    ca.transform.position = Vector3.Lerp(ca.transform.position, new Vector3(pos.x, pos.y, -15), s);
                    yield return new WaitForSeconds(Time.deltaTime);
                }
                if (current_ev.boolean1)
                {
                    ca.GetComponent<s_camera>().focus = true;
                    ca.GetComponent<s_camera>().player = obje.GetComponent<PDII_character>();

                }
            }
            else
            {

                float dista = Vector2.Distance(ca.transform.position, new Vector3(current_ev.pos.x, current_ev.pos.y));
                while (Vector2.Distance(ca.transform.position, new Vector3(current_ev.pos.x, current_ev.pos.y))
                    > dista * 0.05f)
                {
                    s += spe * Time.deltaTime;
                    ca.transform.position = Vector2.Lerp(ca.transform.position, new Vector3(current_ev.pos.x, current_ev.pos.y, -15), s);
                    ca.transform.position = new Vector3(ca.transform.position.x, ca.transform.position.y, -15);
                    yield return new WaitForSeconds(Time.deltaTime);
                }
            }
            break;

        //Make this like the conditional statements where it checks if the utility is in a certain state
        case EVENT_TYPES.UTILITY_CHECK:

            GameObject utGO2 = GameObject.Find(current_ev.string0);
            if (utGO2)
            {
                s_utility ut = utGO2.GetComponent<s_utility>();
                if (ut != null)
                {
                    if (ut.eventState == current_ev.int0)
                    {
                        pointer = current_ev.int1;
                    }
                }
            }

            break;

        case EVENT_TYPES.UTILITY_INITIALIZE:
            GameObject utGO1 = GameObject.Find(current_ev.string0);
            if (utGO1)
            {
                s_utility ut = utGO1.GetComponent<s_utility>();
                print("Okay!");
                if (ut != null)
                {
                    ut.istriggered = true;
                    ut.EventStart();
                }
                else
                    break;
            }

            break;

        case EVENT_TYPES.FADE:

            Color col = new Color(current_ev.colour.a, current_ev.colour.g, current_ev.colour.b, current_ev.colour.a);
            float t = 0;

            while (fade.color != col)
            {
                t += Time.deltaTime;
                fade.color = Color.Lerp(fade.color, col, t);
                yield return new WaitForSeconds(Time.deltaTime);
            }
            break;

        case EVENT_TYPES.WAIT:

            yield return new WaitForSeconds(current_ev.float0);

            break;


        case EVENT_TYPES.SET_OBJ_COLLISION:
            s_object ob2 = GameObject.Find(current_ev.string0).GetComponent<s_object>();
            ob2.collision.size = new Vector2(current_ev.int0, current_ev.int1);
            ob2.collision.offset = new Vector2(current_ev.float0, current_ev.float1);
            break;

        case EVENT_TYPES.CREATE_OBJECT:

            s_object ob = leveled.SpawnObject<s_object>(current_ev.string0, new Vector3(current_ev.float0, current_ev.float1), Quaternion.identity);
            if (current_ev.string1 != "")
                ob.name = current_ev.string1;
            break;

        case EVENT_TYPES.DELETE_OBJECT:
            s_object o = GameObject.Find(current_ev.string0).GetComponent<s_object>();
            o.DespawnObject();
            break;

        case EVENT_TYPES.DISPLAY_CHARACTER_HEALTH:

            bool check = current_ev.stringList.Length < 2;
            if (current_ev.boolean)
            {
                if (check)
                {
                    if (current_ev.boolean)
                        s_gui.AddCharacter(GameObject.Find(current_ev.stringList[0]).GetComponent<PDII_character>(), false);
                    else
                        s_gui.AddCharacter(GameObject.Find(current_ev.stringList[0]).GetComponent<PDII_character>(), true);
                }
                else
                {
                    List<PDII_character> cha = new List<PDII_character>();
                    foreach (string st in current_ev.stringList)
                    {
                        cha.Add(GameObject.Find(st).GetComponent<PDII_character>());
                    }
                    s_gui.AddCharacter(cha);
                }

            }
            break;


        case EVENT_TYPES.SET_FLAG:
            s_globals.SetGlobalFlag(current_ev.string0, current_ev.int0);
            break;

        case EVENT_TYPES.CHANGE_MAP:

            dialogueChoices.Clear();
            if (player != null)
            {
                player.direction = new Vector2(0, 0);
                player.rbody2d.velocity = Vector2.zero;
                player.control = false;
            }
            if (host != null)
            {
                host.direction = new Vector2(0, 0);
                host.rbody2d.velocity = Vector2.zero;
                host.control = false;
            }

            float t2 = 0;
            while (fade.color != Color.black)
            {
                t2 += Time.deltaTime;
                fade.color = Color.Lerp(Color.clear, Color.black, t2);
                yield return new WaitForSeconds(Time.deltaTime);
            }
            if(host != null)
                s_levelloader.LevEd.TriggerSpawn(current_ev.string0, current_ev.string1, host);
            else
                s_levelloader.LevEd.TriggerSpawn(current_ev.string0, current_ev.string1, player);

            t2 = 0;
            while (fade.color != Color.clear)
            {
                t2 += Time.deltaTime;
                fade.color = Color.Lerp(Color.black, Color.clear, t2);
                yield return new WaitForSeconds(Time.deltaTime);
            }

            if (player != null)
                player.control = true;
            if (host != null)
                host.control = true;

            pointer = -1;
            doingEvents = false;
            break;

        case EVENT_TYPES.ADD_CHOICE_OPTION:
            if (dialogueChoices != null)
                dialogueChoices.Add(new s_dialogue_choice(current_ev.string0, current_ev.int0));
            else
            {
                dialogueChoices = new List<s_dialogue_choice>();
                dialogueChoices.Add(new s_dialogue_choice(current_ev.string0, current_ev.int0));
            }
            break;

        case EVENT_TYPES.CHOICE:
            int choice = 0, finalchoice = -1;
            print(choice);

            while (finalchoice == -1)
            {
                if (Input.GetKeyDown(KeyCode.UpArrow))
                    choice--;

                if (Input.GetKeyDown(KeyCode.DownArrow))
                    choice++;

                choice = Mathf.Clamp(choice, 0, dialogueChoices.Count - 1);

                if (Input.GetKeyDown(KeyCode.Z))
                {
                    print("Chosen");
                    finalchoice = choice;
                }
                Dialogue.text = "";
                Dialogue.text += current_ev.string0 + "\n";
                for (int i = 0; i < dialogueChoices.Count - 1; i++)
                {
                    if (choice == i)
                        Dialogue.text += "-> ";

                    Dialogue.text += dialogueChoices[i].option + "\n";
                }
                print(choice);
                yield return new WaitForSeconds(Time.deltaTime);
            }
            Dialogue.text = "";
            pointer = dialogueChoices[finalchoice].flagTojump - 1;
            break;
    }
    */
    //yield return new WaitForSeconds(0.5f);

    private void Update()
    {
        host = player.GetComponent<o_plcharacter>().host;
    }

}