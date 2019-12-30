using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MagnumFoudation;

public class o_itemObj : MagnumFoudation.o_itemObj {

    float floatingNum = 0;
    public List<Sprite> itemTextures = new List<Sprite>();
    public AudioClip collectSound;

    new void Start ()
    {
        
        base.Start();
    }
	
	new void Update ()
    {
        if (floatingNum > Mathf.PI * 2)
            floatingNum = 0;
        else
            floatingNum += 0.04f;

        rendererObj.transform.position = new Vector3(transform.position.x, transform.position.y +(( Mathf.Sin(floatingNum) * 25) * Time.deltaTime));
        Collider2D col = IfTouchingGetCol<o_character>(collision);

        if (col != null)
        {
            o_character p = col.GetComponent<o_character>();

            if (p != null)
            {
                switch (it)
                {

                    case ITEM_TYPE.COLLECTIBLE:
                        /*
                        npc_customer mask = p.GetComponent<npc_customer>();
                        if (mask != null)
                        {
                            if (mask.desiredItemName == name)
                            {
                                mask.satisfied = true;
                                DespawnObject();
                            }
                        }
                        */
                        break;

                }
                if (!p.AI)
                switch (it)
                {
                    case ITEM_TYPE.MONEY:
                            s_globals.Money += 1;
                            s_map mp = GameObject.Find("General").GetComponent<s_leveledit>().mapDat;
                            s_leveledit.LevEd.GetComponent<s_leveledit>().SetItemData(name, false);
                            mp.gemCount++;
                            s_save_item it = mp.itemdat.Find(x => x.ID == indexID);
                            it.iscollected = true;
                            s_soundmanager.sound.PlaySound(ref collectSound, false);
                            DespawnObject();
                            break;

                        case ITEM_TYPE.COLLECTIBLE:
                            s_soundmanager.sound.PlaySound(ref collectSound, false);
                            DespawnObject();
                            break;

                    }
            }
            //DespawnObject();
        }
	}
}
