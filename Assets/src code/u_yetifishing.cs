using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using MagnumFoudation;

/*
public class u_yetifishing : s_utility
{
    public npc_yeti[] yetiCharacters;
    public float timer = 0;
    public List<npc_icefish> FishCount = new List<npc_icefish>();
    Vector3 pos;

    public int ThreadSleepr = 4000;

    Thread del;

    public void SpawnFish()
    {
        Thread.Sleep(ThreadSleepr);
        print("Sleep");
        del = null;
    }

    public void RemoveFish()
    {
        //FishCount.Remove();
    }

    public override void EventUpdate()
    {
        base.EventUpdate();
        s_gui.DisplayNotificationText("Timer: " + timer + 
            " Yeti 1:" + yetiCharacters[0].caughtFish +
            " Yeti 2:" + yetiCharacters[1].caughtFish +
            " Yeti 3:" + yetiCharacters[2].caughtFish, -1);
        timer -= Time.deltaTime;
        if (del == null)
        {
            if (FishCount.Count < 10)
            {
                pos = new Vector2(Random.Range(2100, 2580), Random.Range(520, 860));
                npc_icefish ice = s_levelloader.LevEd.SpawnObject<npc_icefish>("ice_fish", pos, Quaternion.identity);
                //Have a delay in spawning
                del = new Thread(SpawnFish);
                del.Start();
                //Reload all of the targets of the characters
                foreach (npc_yeti y in yetiCharacters)
                {
                    s_leveledit.LevEd.AddCharacter(ice);
                    y.ReAddFactions();
                }
            }
        }
        if (timer == 0)
        {

        }
    }
}
*/