using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MagnumFoudation;

public class u_boundary : s_utility {

    public int gemRequire;
    public bool IsOverworldRequire;
    public List<GameObject> bounds;
    public string flagname;
    public int flagCheck;
    s_leveledit edit;
    BoxCollider2D obj;
    bool playerIn = false;
    public bool checkFlag = false;

    public Text txt;
    public Image imageTxt;

    public string description;

    s_map.s_trig t;

    new private void Start()
    {
        base.Start();
        obj = transform.GetChild(1).GetComponent<BoxCollider2D>();
        edit = GameObject.Find("General").GetComponent<s_leveledit>();
        t = edit.mapDat.triggerdata.Find(x => x.name == name);
    }

    private new void Update()
    {
        if (checkFlag)
        {
            txt.text = description;
            if (s_globals.GetGlobalFlag(flagname) == flagCheck)
            {
                DespawnObject();
            }
        }
        else {

            if (s_globals.Money >= gemRequire)
            {
                DespawnObject();
            }
            txt.text = "Gems required: " + gemRequire;
            /*
            txt.transform.position = Camera.main.WorldToScreenPoint(transform.position);
            imageTxt.transform.position = Camera.main.WorldToScreenPoint(transform.position);
            */
            if (edit.mapDat.gemCount >= gemRequire)
            {
                for (int i = 0; i < bounds.Count; i++)
                {
                    bounds[i].GetComponent<o_generic>().DespawnObject();
                }
                if (t != null)
                    t.IsPermanentlyDisabled = true;
            }
        }

        /*
        Collider2D co = IfTouchingGetCol<o_plcharacter>(obj);

        if (co != null)
        {
            o_plcharacter pl = co.GetComponent<o_plcharacter>();
            if (pl != null)
            {
                if (!playerIn)
                {
                    txt.text = "Gems required: " + gemRequire;
                    playerIn = true;
                }
            }
        }
        else
        {
            if (playerIn)
            {
                txt.text = "";
                playerIn = false;
            }
        }
        */
    }
    
    /*
    public override IEnumerator EventTrigger()
    {
        istriggered = true;
        while (defeated.Count != characters.Length)
        {
            print("No.");
            yield return new WaitForSeconds(Time.deltaTime);
        }

        eventState = 0;
        print("Yes.");
        yield return base.EventTrigger();
    }
    */
}
