using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MagnumFoudation;
/*
public class u_save : s_utility
{
    Text Txt;
    o_plcharacter chara;
    int menuchoice = 0;
    string[] menus = {"Save", "Exit" };
    
    private void Awake()
    {
        chara = GameObject.Find("Player").GetComponent<o_plcharacter>();
        Txt = GameObject.Find("ShopText").GetComponent<Text>();
    }

    public override void EventUpdate()
    {
        base.EventUpdate();

        if (Input.GetKeyDown(KeyCode.DownArrow))
            menuchoice += 1;
        if (Input.GetKeyDown(KeyCode.UpArrow))
            menuchoice -= 1;
        menuchoice = Mathf.Clamp(menuchoice, 0, menus.Length - 1);

        Txt.text = "";
        Txt.text += "Press Z to confirm" + "\n";
        for (int i = 0; i < menus.Length; i++)
        {
            if (menuchoice == i)
                Txt.text += "-> ";
            Txt.text += menus[i] + "\n";
            if (Input.GetKeyDown(KeyCode.Z))
            {
                switch (menus[menuchoice])
                {
                    case "Save":
                        s_globals.SaveData();
                        chara.CHARACTER_STATE = o_character.CHARACTER_STATES.STATE_IDLE;
                        istriggered = false;
                        StartCoroutine(DispText());
                        break;

                    case "Exit":
                        Txt.text = "";
                        chara.CHARACTER_STATE = o_character.CHARACTER_STATES.STATE_IDLE;
                        istriggered = false;
                        break;
                }
            }
        }
    }
    IEnumerator DispText() {

        Txt.text = "Saved...";
        yield return new WaitForSeconds(0.5f);
        Txt.text = "";
    }

}
*/