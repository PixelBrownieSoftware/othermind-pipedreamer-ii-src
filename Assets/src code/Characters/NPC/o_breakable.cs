using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class o_breakable : PDII_character
{
    string character_can_break;

    new void Start()
    {
        base.Start();
        isControllable = false;
        icetolerant = true;
        character_can_break = "Yeti";
        Initialize();
    }

    new void Update()
    {

        Collider2D c = IfTouchingGetCol(collision, "o_bullet");
        if (c != null) {
            PDII_bullet bul = c.GetComponent<PDII_bullet>();
            if (bul != null)
            {
                print("fjdksjfljsk");
                if (bul.parent.ID == character_can_break)
                    DespawnObject();
            }
        }
    }
}
