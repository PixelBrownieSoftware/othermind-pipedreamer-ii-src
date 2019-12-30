using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class s_bgm : MonoBehaviour
{
    [System.Serializable]
    public struct area_music
    {
        public AudioClip music;
        public List<string> area;
    }

    public AudioSource bgm;
    public List<area_music> music = new List<area_music>();

    public void PlayMusic(AudioClip clip)
    {
        bgm.Stop();
        bgm.clip = clip;
        bgm.loop = true;
        bgm.Play();
    }

    public AudioClip GetMusic(string levname)
    {
        return music.Find(x=> x.area.Find(y => y == levname) != null).music;
    }
}
