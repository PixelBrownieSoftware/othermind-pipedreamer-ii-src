using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
public class s_camera : MonoBehaviour {

    public PDII_character player;
    Vector2 offset;
    float offset_multiplier;
    float orthGraphicSize = 0;

    public bool focus = true;

    void Start ()
    {
        orthGraphicSize = GetComponent<Camera>().orthographicSize * 2;
    }

    public void SetPlayer(PDII_character cha)
    {
        player = cha;
    }

	void FixedUpdate ()
    {
        if (focus)
        {
            Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            offset = (mouse - player.transform.position).normalized;

            if (Vector2.Distance(mouse, player.transform.position) > 9)
            {
                offset_multiplier = Vector3.Distance(player.transform.position, mouse) * 0.35f;
            }
            

            offset_multiplier = Mathf.Clamp(offset_multiplier, 0, 50f);
            offset *= offset_multiplier;

            Vector3 vec = Vector3.Lerp(player.transform.position, transform.position, 0.6f);

            vec.y = Mathf.RoundToInt(vec.y);
            vec.x = Mathf.RoundToInt(vec.x);

            if (vec.x < orthGraphicSize)
                vec.x = orthGraphicSize;
            if (vec.x > s_leveledit.LevEd.mapsizeToKeep.x * s_leveledit.LevEd.tilesize - orthGraphicSize)
                vec.x = s_leveledit.LevEd.mapsizeToKeep.x * s_leveledit.LevEd.tilesize - orthGraphicSize;

            if (vec.y < orthGraphicSize)
                vec.y = orthGraphicSize;
            if (vec.y > s_leveledit.LevEd.mapsizeToKeep.y * s_leveledit.LevEd.tilesize - orthGraphicSize)
                vec.y = s_leveledit.LevEd.mapsizeToKeep.y * s_leveledit.LevEd.tilesize - orthGraphicSize;

            transform.position = new Vector3(vec.x, vec.y, -10);
        }


        //print(Vector2.Distance(mouse, player.transform.position));
        //print("offset mult = " + offset_multiplier);
        //print("offset = " + 
        //player.positioninworld + 
        //(Vector3)offset);

    }
}
*/