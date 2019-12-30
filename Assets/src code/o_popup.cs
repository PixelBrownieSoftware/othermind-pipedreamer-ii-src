using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MagnumFoudation;

public class o_popup : s_object
{
    public float timer;
    public s_object parent;
    public bool timed = true;
    public float offsetPos = 20;

    public Sprite notAllowed;
    public Sprite jumpOff;

    public enum NOTTYPE {
        NOALLOW,
        JUMPOFF
    }
    public NOTTYPE typeOfThing;

    public new void Update()
    {
        transform.position = parent.transform.position + new Vector3(0, offsetPos + parent.Z_offset);

        switch (typeOfThing)
        {
            case NOTTYPE.JUMPOFF:
                rendererObj.sprite = jumpOff;
                break;

            case NOTTYPE.NOALLOW:
                rendererObj.sprite = notAllowed;
                break;
        }

        if (timed)
        {
            if (timer > 0)
                timer -= Time.deltaTime;
            else
                DespawnObject();
        }
    }

}
