using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MagnumFoudation;

[System.Serializable]
public class o_weapon : o_item
{
    public o_weapon(int attackPow, string name, WEAPON_TYPE weap) : base(name, ITEM_TYPE.WEAPON,1)
    {
        level = 1;
        this.attackPow = attackPow;
        weapon_type = weap;
    }
    public enum WEAPON_TYPE
    {
        WEAPON_MELEE,
        WEAPON_RANGED
    }
    public WEAPON_TYPE weapon_type;
    public int attackPow;
    public int level;
};

[System.Serializable]
public struct o_shopItem
{
    public o_shopItem(o_item item, int price)
    {
        this.item = item;
        this.price = price;
    }
    public o_item item;
    public int price;
}


/*
public class u_shop : s_utility {
//Items
public o_shopItem[] items;

s_gui Gui;
o_plcharacter chara;
Text Txt;

private new void Start()
{
    base.Start();
    Txt = GameObject.Find("ShopText").GetComponent<Text>();
    Gui = GameObject.Find("General").GetComponent<s_gui>();
    chara = GameObject.Find("Player").GetComponent<o_plcharacter>();

    items = new o_shopItem[8];
    items[0] = new o_shopItem(new o_item("Kaj's magazine", o_item.ITEM_TYPE.KEY_ITEM), 5);
    items[1] = new o_shopItem(new o_item("Hamlet's costume", o_item.ITEM_TYPE.KEY_ITEM), 10);
    items[2] = new o_shopItem(new o_item("Nina's microphone", o_item.ITEM_TYPE.KEY_ITEM), 15);
    items[3] = new o_shopItem(new o_item("Loki's cushion", o_item.ITEM_TYPE.KEY_ITEM), 15);
    items[4] = new o_shopItem(new o_item("Okami's stew", o_item.ITEM_TYPE.CONSUMABLE, 2), 15);
    items[5] = new o_shopItem(new o_item("Sinro's cookie", o_item.ITEM_TYPE.CONSUMABLE, 1), 5);
    items[6] = new o_shopItem(new o_item("Carl's diary", o_item.ITEM_TYPE.KEY_ITEM), 20);
    items[7] = new o_shopItem(new o_item("Nak's cape", o_item.ITEM_TYPE.KEY_ITEM), 3);

}

public enum SHOPSTATES
{
    INITIALIZE,
    BUYING,
    EXIT
}
public SHOPSTATES SHOPSTATE = SHOPSTATES.INITIALIZE;

public int menuchoice = 0;
public int a = 0;

public override void EventUpdate()
{
    base.EventUpdate();
    if (Input.GetKeyDown(KeyCode.DownArrow))
    {
        menuchoice += 1;
    }
    if (Input.GetKeyDown(KeyCode.UpArrow))
    {
        menuchoice -= 1;
    }
    menuchoice = Mathf.Clamp(menuchoice, 0, items.Length - 1);


    Txt.text = "";
    for (int i = 0; i < items.Length; i++)
    {
        o_shopItem it = items[i];
        if (it.price > s_globals.Money)
            Txt.text += "<color=red>";
        if (i == menuchoice)
            Txt.text += "-> ";
        Txt.text += "Item: " + it.item.name + " Price: " + it.price;
        if (it.price > s_globals.Money)
            Txt.text += "</color>";
        Txt.text += "\n";
    }
    Txt.text += "\n";
    Txt.text += "Press Z to purchase" + "\n";
    Txt.text += "Press X to quit";

    if (Input.GetKeyDown(KeyCode.Z))
    {
        if (items[menuchoice].price <= s_globals.Money)
        {
            s_globals.AddItem(items[menuchoice].item);
            s_globals.Money -= items[menuchoice].price;
        }
    }
    if (Input.GetKeyDown(KeyCode.X))
    {
        Txt.text = "";
        chara.CHARACTER_STATE = o_character.CHARACTER_STATES.STATE_IDLE;
        istriggered = false;
    }
}

public override IEnumerator EventTrigger()
{
    //print(a);
    switch (SHOPSTATE)
    {
        case SHOPSTATES.INITIALIZE:
            a = 0;

            print(a);
            SHOPSTATE = SHOPSTATES.BUYING;
            break;

        case SHOPSTATES.BUYING:
            a = Gui.PickFromList(6);
            if (Input.GetKey(KeyCode.Space))
            {
                Buy();
            }
            if (Input.GetKey(KeyCode.E))
            {
                SHOPSTATE = SHOPSTATES.EXIT;
            }
            break;

        case SHOPSTATES.EXIT:
            eventState = 0;
            Gui.ResetData();
            yield return base.EventTrigger();
            SHOPSTATE = SHOPSTATES.INITIALIZE;
            eventState = 1;
            break;

    }
    yield return null;
}
}
*/
