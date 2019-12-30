using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
public class s_object : MagnumFoudation.s_object
{

    public string ID; //To return back to the object pooler
    public GameObject shadow;
    public float Z_offset;  //To show the character jumping
    public int Z_floor;
    public BoxCollider2D collision;
    s_nodegraph nd = null;
    public Vector3 positioninworld;
    public Vector2 velocity;
    public float speed, terminalspd;
    public float gravity;
    public float height;
    public bool is_kinematic = true;

    
    public void Start ()
    {
        nd = GameObject.Find("General").GetComponent<s_nodegraph>();
        collision = GetComponent<BoxCollider2D>();
    }
	

    public void DespawnObject()
    {
        s_leveledit.LevEd.DespawnObject(this);
    }

    public s_node CheckNode(float x, float y)
    {
        return CheckNode(new Vector2(x, y));
    }
    public s_node CheckNode(Vector2 v)
    {
        if (nd == null)
            return null;
       s_node no =  nd.PosToNode(v);
        if (no == null)
            return null;
        else
            return no;
    }

    protected void Update ()
    {
        if (shadow != null)
            shadow.transform.position = new Vector2(positioninworld.x, positioninworld.y);
    }

    internal bool IfTouching(BoxCollider2D collisn)
    {
        if (collisn == null)
            return false;

        return Physics2D.OverlapBox(positioninworld, collisn.size, 0);
    }

    public Collider2D GetCharacter(BoxCollider2D collisn) { return Physics2D.OverlapBox(positioninworld, collisn.size, 0); }
    public Collider2D GetCharacter(BoxCollider2D collisn, string name)
    {
        Collider2D col = Physics2D.OverlapBox(positioninworld, collisn.size, 0);
        if (col == null)
            return null;

        if (col.name == name)
            return col;
        else return null;
    }
    public Collider2D GetCharacter(BoxCollider2D collisn, int lay)
    {
        return Physics2D.OverlapBox(positioninworld, collisn.size, 0, lay);
    }
    public Collider2D GetCharacter(BoxCollider2D collisn, int lay, string nameofobj)
    {
        Collider2D col = Physics2D.OverlapBox(positioninworld, collisn.size, 0, lay);

        if (col == null)
            return null;
        //print(col.name);
        if (col.name == nameofobj)
            return col;
        else return null;
    }

    public Collider2D GetAllCharacters(BoxCollider2D collisn) { return Physics2D.OverlapBox(positioninworld, collisn.size, 0); }

    internal bool IfTouching(BoxCollider2D collisn, string nameofobj)
    {
        Collider2D col = Physics2D.OverlapBox(positioninworld, collisn.size, 0);
        if (collisn == null)
            return false;

        if (col == null)
            return false;
        
        if (col.gameObject == gameObject)
            return false;
        if (col)
            return col.name == nameofobj;

        return false;
    }



}
*/
