using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class npc_venusboss : PDII_character
{
    npc_ground[] grounds = new npc_ground[3];
    new void Start()
    {
        string[] g = {"ground_1" , "ground_2" , "ground_3" };
        for (int i = 0; i < grounds.Length; i++) {
            GameObject gr = GameObject.Find(g[i]);
            if (gr == null)
                continue;
            npc_ground attch = gr.GetComponent<npc_ground>();
            if (attch)
                grounds[i] = attch; 
        }
        base.Start();
    }
    
    new void Update()
    {
        int add = 0;
        foreach (npc_ground g in grounds) {
            if (g == null)
                add++;
        }
        if (add == grounds.Length) {
            DespawnObject();
        }
        base.Update();
    }
}
