using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MagnumFoudation;

/*
public class u_teleport : s_utility
{
    public struct teleporter_data {
        public teleporter_data(string area, string map)
        {
            unlocked = false;
            this.area = area;
            this.map = map;
        }
        public bool unlocked;
        public string map;
        public string area;
    }
    public List<teleporter_data> teleportarea = new List<teleporter_data>();
    int choice = 0, finalchoice = -1;
    List<s_map> maps;

    void GetTeleporterData()
    {
        foreach (s_map m in maps) {
            List<s_map.s_tileobj> tiles = m.tilesdata;
            List<s_map.s_tileobj> teleporters = tiles.FindAll(x=> x.TYPENAME == "teleport_object");
            foreach (s_map.s_tileobj o in teleporters)
            {

            }
        }
    }

    new private void Start()
    {
        
    }

    public override void EventStart()
    {
        finalchoice = -1;
        choice = 0;
        maps = s_leveledit.LevEd.maps;

        base.EventStart();
    }

    private gui_dialogue Dialogue;
    public override void EventUpdate()
    {
        print(choice);
        if (finalchoice == -1)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
                choice--;

            if (Input.GetKeyDown(KeyCode.DownArrow))
                choice++;

            if (Input.GetKeyDown(KeyCode.Z))
            {
                print("Chosen");
                finalchoice = choice;
            }
            Dialogue.textthing.text = "";
            Dialogue.textthing.text += "Where?" + "\n";
            for (int i = 0; i < teleportarea.Count; i++)
            {
                if (choice == i)
                    Dialogue.textthing.text += "-> ";
               // if(teleportarea[])
                Dialogue.textthing.text += teleportarea[i] + "\n";
            }
            print(choice);
            choice = Mathf.Clamp(choice, 0, teleportarea.Count - 1);
        }
        Dialogue.textthing.text = "";
        base.EventUpdate();
    }

}
*/