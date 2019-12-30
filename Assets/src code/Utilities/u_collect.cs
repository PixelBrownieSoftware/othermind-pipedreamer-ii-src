using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MagnumFoudation;

public class u_collect : s_utility
{
    public List<o_itemObj> itemsRequire = new List<o_itemObj>();
    public List<Vector2> itemPositions;  //So that when the minigmae fails the items can be respawned
    public float timer = 1f;
    const float timerStart = 85f;
    
    enum EVENTSTATE
    {
        INACTIVE = 1,
        ACTIVE = 2,
        WIN,
        DONE,
        START
    };

    private new void Start()
    {
        base.Start();
        eventState = 5;
        //DespawnItems();
        //itemsRequire.Clear();
    }
    
    public void DespawnItems()
    {
        foreach (o_itemObj o in itemsRequire)
        {
            o.DespawnObject();
        }
        itemsRequire.Clear();
    }

    public void RespawnItems()
    {
        foreach (Vector2 o in itemPositions)
        {
            o_itemObj go = s_levelloader.LevEd.SpawnObject<o_itemObj>("seashell", o, Quaternion.identity).GetComponent<o_itemObj>();
            itemsRequire.Add(go);
            go.transform.SetParent(GameObject.Find("Items").transform);
        }
    }


    public new void Update()
    {
        switch ((EVENTSTATE)eventState)
        {
            default:

                if (itemsRequire.Count == 0)
                {
                    eventState = (int)EVENTSTATE.WIN;
                }
                o_itemObj o = itemsRequire.Find(x => !x.gameObject.activeSelf);
                if (o != null)
                {
                    itemsRequire.Remove(o);
                }
                break;

            /*
            case EVENTSTATE.ACTIVE:

            //s_gui.DisplayNotificationText("Time: " + timer + " Seashells: " + itemsRequire.Count + "/ " + itemPositions.Count, -1);

            if (timer >= 0)
                timer -= Time.deltaTime;
            else
            {
                DespawnItems();
                eventState = (int)EVENTSTATE.INACTIVE;
            }
            break;*/
            case EVENTSTATE.WIN:

                s_gui.DisplayNotificationText("The boundary has dissappeared...", 2.5f);
                s_globals.SetGlobalFlag("PS_MG_WIN", 1);
                //s_trig.trig.JumpToEvent
                eventState = (int)EVENTSTATE.DONE;
                DespawnObject();
                break;
        }
    }
}
