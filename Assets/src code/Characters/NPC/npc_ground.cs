using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class npc_ground : PDII_character
{
    new void Start()
    {
        Initialize();
    }

    new void Update()
    {
        base.Update();
        Collider2D col = IfTouchingGetCol(collision, "o_npcharacter");
        if (col != null) {
            DespawnObject();
        }
    }
}
