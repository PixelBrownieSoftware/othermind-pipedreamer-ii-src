using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using UnityEngine;

public class s_mainmenu : MonoBehaviour {

    public static dat_save save;
    public static bool isload = false;

    private void OnGUI()
    {
        if (GUILayout.Button("Start game"))
        {
            isload = false;
            UnityEngine.SceneManagement.SceneManager.LoadScene("Main Game");
        }

        if (File.Exists("Save.PD2"))
        {
            if (GUILayout.Button("Load game"))
            {
                isload = true;
                FileStream fs = new FileStream("Save.PD2", FileMode.Open);
                BinaryFormatter bin = new BinaryFormatter();

                save = (dat_save)bin.Deserialize(fs);
                
                fs.Close();
                UnityEngine.SceneManagement.SceneManager.LoadScene("Main Game");
            }
        }
        
    }
}
