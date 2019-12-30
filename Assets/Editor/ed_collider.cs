using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using MagnumFoudation;

[CanEditMultipleObjects]
[CustomEditor(typeof(o_generic))]
public class ed_collider : Editor
{
    s_map mapdat;
    s_leveledit lev;
    o_trigger[] triggers;

    o_generic tar;

    public override void OnInspectorGUI()
    {
        if (GameObject.Find("Main Camera") != null) {
            if (GameObject.Find("Main Camera").GetComponent<s_leveledit>() != null) {
                if (mapdat == null)
                    mapdat = GameObject.Find("Main Camera").GetComponent<s_leveledit>().mapDat;
                if (lev == null)
                    lev = GameObject.Find("Main Camera").GetComponent<s_leveledit>();
            }
        }

        base.OnInspectorGUI();

        tar = (o_generic)target;

        //EditorGUILayout.LabelField("Exeption character");
        //tar.character = EditorGUILayout.TextArea(tar.character);

        switch (tar.name)
        {
            case "BoundTile":
                if (GUILayout.Button("Re-search triggers"))
                {
                    triggers = FindTriggers();
                }
                if (triggers == null)
                    triggers = FindTriggers();
                else
                {
                    List<u_boundary> bounds = FindBoundariesInTriggers();
                    foreach (u_boundary b in bounds)
                    {
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("    " + b.name);

                        GameObject bo = b.bounds.Find(x => tar.gameObject == x);
                        if (bo == null)
                        {
                            if (GUILayout.Button("Assign to boundary"))
                            {
                                b.bounds.Add(tar.gameObject);
                            }
                        }
                        else
                        {
                            if (GUILayout.Button("Remove from boundary"))
                            {
                                b.bounds.Remove(bo);
                            }
                        }
                    }
                }
                break;
        }

        EditorGUILayout.Space();
    }

    public o_trigger[] FindTriggers()
    {
        o_trigger[] triggersInMap = null;
        triggersInMap = lev.SceneLevelObject.transform.Find("Triggers").GetComponentsInChildren<o_trigger>();
        return triggersInMap;
    }

    public List<u_boundary> FindBoundariesInTriggers()
    {
        List<u_boundary> bound = new List<u_boundary>();
        foreach (o_trigger t in triggers)
        {
            if (t.GetComponent<u_boundary>()) {
                bound.Add(t.GetComponent<u_boundary>());
            }
        }
        return bound;
    }
}
