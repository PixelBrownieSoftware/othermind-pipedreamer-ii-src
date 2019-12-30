using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MagnumFoudation;

/*
public class u_delivery : s_utility
{
    public List<o_itemObj> itemsRequire = new List<o_itemObj>();
    public List<Vector2> itemPositions = new List<Vector2>();   //So that when the minigmae fails the items can be respawned

    public string[] deliveryItems;
    public string[] foodTypes;

    public npc_customer[] characters;
    public s_object boundary;
    float timer = 0;
    const float timeSet = 45f;
    int index = 0;
    public delegate void OnSatisfy();
    OnSatisfy os;

    public enum MODE
    {
        DELIVERY_SERVICE,
        SERVING_FOOD
    }
    public MODE mode;
    
    enum state
    {
        INACTIVE,
        ONGOING,
        FAIL,
        WIN,
        REWARD
    }
    o_plcharacter p;
    public npc_customer currentPerson;
    bool foodCooked = false;
    npc_maskperson mc;

    public void CheckIfSatisfied()
    {
    }

    public void RespawnItems()
    {
        for(int i =0;i < deliveryItems.Length; i++)
        {
            Vector2 o = itemPositions[i];
            string del = deliveryItems[i];
            o_itemObj go = s_levelloader.LevEd.SpawnObject<o_itemObj>("item", o, Quaternion.identity).GetComponent<o_itemObj>();
            go.ItemContain.name = del;
            itemsRequire.Add(go);
            go.transform.SetParent(GameObject.Find("Items").transform);
        }
    }

    public override void EventStart()
    {
        base.EventStart();
        for (int i = 0; i < characters.Length; i++)
        {
            npc_customer c = characters[i];
            c.satisfied = false;
        }
        index = 0;
       // p = GameObject.Find("Player").GetComponent<o_plcharacter>();
        timer = timeSet;
        currentPerson = characters[index];
        o_itemObj it = s_levelloader.LevEd.SpawnObject<o_itemObj>("ticket", transform.position, Quaternion.identity).GetComponent<o_itemObj>();
        if (index < characters.Length - 1)
        {
            currentPerson = characters[index];
            it.name = currentPerson.desiredItemName;
            timer = timeSet;
            eventState = 2;
        }
    }

    public void CookFood()
    {
        Vector2 pos = new Vector2(0,-20);
        foreach (string nam in foodTypes)
        {
            o_itemObj it = s_levelloader.LevEd.SpawnObject<o_itemObj>("ticket", transform.position + (Vector3)pos, Quaternion.identity).GetComponent<o_itemObj>();
            it.ItemContain.name = nam;
            pos += new Vector2(10, 0);
        }
    }

    public override void EventUpdate()
    {
        if (eventState == 2)
        {
            timer -= Time.deltaTime;
            string str = "";
            if (mc != null)
            {
                if (mc.currentItem != null)
                    str += "Time: " + timer + " Wanted item: " + currentPerson.desiredItemName;
                else
                {
                    if (foodCooked)
                    {
                        CookFood();
                        foodCooked = true;
                    }
                    timer = timeSet;
                    str += "Come back to me to get the next item...";
                }
            }

            switch (mode)
            {
                case MODE.SERVING_FOOD:

                    break;
            }
            s_gui.DisplayNotificationText(str, -1);
            if (currentPerson.satisfied)
            {
                index++;
                o_itemObj it = s_levelloader.LevEd.SpawnObject<o_itemObj>("ticket", transform.position, Quaternion.identity).GetComponent<o_itemObj>();
                timer = timeSet;
                if (index == characters.Length)
                {
                    eventState = 3;
                    s_gui.DisplayNotificationText("Congrats! You can go into the mall now!", -1);
                    return;
                }
                currentPerson = characters[index];
                it.name = currentPerson.desiredItemName;
            }
            if (timer <= 0)
            {
                s_gui.DisplayNotificationText("You were too slow, talk to the delivery man to play again.", 4.5f);
                eventState = 1;
                istriggered = false;
            }
        }


    }
    
    private new void Update()
    {
        if (mc == null)
        {
            if (p != null)
            {
                if (p.host != null)
                {
                    mc = p.host.GetComponent<npc_maskperson>();
                    foreach (npc_customer c in characters)
                    {
                        c.SetDeliverer(mc);
                    }
                }
            }
        }
        base.Update();
    }
}
*/