using MagnumFoudation;
using UnityEngine;

public class npc_customer : PDII_character
{
    public string itemName;
    public bool hasGem;
    public GameObject speech;
    public SpriteRenderer spechSprite;

    public npc_maskperson mask;
    
    private new void Start()
    {
        spechSprite = speech.GetComponent<SpriteRenderer>();
        Initialize();
    }

    public void SetDeliverer(npc_maskperson msk)
    {
        mask = msk;
    }

    private new void Update()
    {
        base.Update();
        if (itemName == "gem")
        {
            spechSprite.color = Color.white;
        }
        else
        {
            spechSprite.color = Color.clear;
        }
        Collider2D c = IfTouchingGetCol<npc_maskperson>(collision);

        if (c != null)
        {
            if (!c.GetComponent<npc_maskperson>().AI)
                mask = c.GetComponent<npc_maskperson>();

            if (mask != null)
            {
                if (mask.isTalking)
                {
                    if (itemName == "gem")
                    {
                        s_globals.Money++;
                    }
                    itemName = "";
                    s_leveledit.LevEd.GetComponent<s_leveledit>().SetEntity(name, new MagnumFoudation.s_map.s_customType("", ""), "itemName");
                    speech.SetActive(false);
                }
            }
        }

    }
}
