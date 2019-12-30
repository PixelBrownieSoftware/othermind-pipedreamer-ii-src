using MagnumFoudation;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public delegate void functionptr(Color colour);
[System.Serializable]
public struct gui_button
{
    public gui_button(Vector2Int vec, functionptr function, Color colour)
    {
        position = vec;
        funct = function;
        this.colour = colour;
    }
    public Vector2Int position;
    public Color colour;
    public functionptr funct;
}

public class s_gui : MonoBehaviour {

    public Vector2Int menuchoice;
    int menuchoice_1D;
    int[,] cells;
    public Texture2D text;
    static List<string> texttoWrite = new List<string>();

    public string gem_require_text { get; set; }
    public Text textstr;
    public PDII_character character;
    public static PDII_character[] othercharacter;
    public static PDII_character allycharacter;

    public static s_gui gui;
    static float timer = 0;

    int cell_x, cell_y;
    int inv_leng = 0;
    bool menu = false;
    bool x_limit;
    static bool stayOn;
    bool open_menu = false;
    public o_plcharacter play;

    static string TextShowNotification;
    public Text NotificationText;
    static Image NotificationBox;

    List<Vector2Int> cell_colours = new List<Vector2Int>();
    List<gui_button> buttons = new List<gui_button>();
    List<gui_element> gui_elements = new List<gui_element>();

	void Awake ()
    {
        cell_x = 20;
        cell_y = 10;
        gui = GetComponent<s_gui>();
        play = GameObject.Find("Player").GetComponent<o_plcharacter>();
        buttons.Add(new gui_button(new Vector2Int(4,6), ChangeCOlour, Color.red));
        buttons.Add(new gui_button(new Vector2Int(12, 3), ChangeCOlour, Color.magenta));
        buttons.Add(new gui_button(new Vector2Int(9, 5), ChangeCOlour, Color.blue));
    }

    private void Start()
    {
        NotificationBox = GameObject.Find("NotificationBox").GetComponent<Image>();
    }

    public static void DisplayNotificationText(string n, float t)
    {
        TextShowNotification = n;

        if (t < 0)
            stayOn = true;
        else
            stayOn = false;

        timer = t;
        NotificationBox.enabled = true;
    }
    public static void HideNotificationText()
    {
        TextShowNotification = "";
        timer = 0;
        NotificationBox.enabled = false;
    }

    public static void RemoveCharacters(bool top) {
        if (top)
        {
            allycharacter = null;
        }
        else {
            othercharacter = null;
        }
    }

    public static void RemoveText(string text)
    {
        texttoWrite.Remove(texttoWrite.Find(x => x == text));
    }

    public void ChangeCOlour(Color colou)
    {
       character.GetComponent<SpriteRenderer>().color = colou;
    }

    public static void AddText(string str)
    {
        texttoWrite.Add(str);
    }

    public static void AddCharacter(List<PDII_character> cha)
    {
        othercharacter = cha.ToArray();
    }
    public static void AddCharacter(PDII_character cha, bool top)
    {
        if (top)
        {
            allycharacter = cha;
        }
        else {
            othercharacter = new PDII_character[1];
            othercharacter[0] = cha;
        }
    }

    private void OnGUI()
    {
        if (textstr != null)
        {
            texttoWrite.Clear();
            /*
            textstr.text = "Money : " + s_globals.Money //+ "\nCharacter Zpos:" + character.Z_offset + " Floor:" + character.Z_floor + "\n" +
           + "Press X to open menu. \n" + "Press N to activate colour.\n" + "Press E to interact with red thing";
            if (menuchoice.x + (int)Input.GetAxisRaw("Horizontal") >= 0 && menuchoice.x + (int)Input.GetAxisRaw("Horizontal") <= cell_x)
            

            texttoWrite.Add("Gems: " + s_globals.Money + "\n");

            foreach (string s in texttoWrite)
            {
                textstr.text += s + "\n";
            }
            */
            textstr.text = s_globals.Money + "\n";
            /*    if (menu)
                    DrawInMulti(new Vector2(cell_x, cell_y), 45);
                    */
            //DrawInARow(4, 15);

            if (cells != null)
                DrawInMulti(new Vector2Int(cell_x, cell_y), 20);

        }
    }

    public void CreateCells(int x, int y)
    {
        cell_x = x;
        cell_y = y;

        cells = new int[cell_x, cell_y];
    }

    public void CreateCells(int quantity, int lim, bool x_limit)
    {
        int q = Mathf.CeilToInt(quantity/ lim);
        this.x_limit = x_limit;
        /*
        if (this.x_limit)
        {
        }*/

        cell_x = lim;
        cell_y = q;

        cells = new int[cell_x, cell_y];
    }

    public void ResetData()
    {
        cells = null;
    }

    public void DrawInARow(int count, float spacing, float y_offset)
    {
        for (int i = 0; i < count; i++)
        {
            DrawObject(new Rect(30 + spacing * i, 30 + y_offset, 30, 30), text, Color.white);
        }
    }

    /*
    public void DrawInMulti(int quantity ,float spacing)
    {
        int tot = cell_x * cell_y - quantity;
        int x_y = 0;
        int y_x = 0;
        for (int i = 0; i < tot; i++)
        {
            if (i % cell_y == 0)
            {
                y_x = 0;
                x_y++;
            }
            y_x++;

            if (menuchoice.x * menuchoice.y > tot)
            {
                menuchoice.x = 0;
                menuchoice.y = 0;
            }

            print(menuchoice);
            DrawObject(new Rect(30 + spacing * x_y,  spacing * y_x, 30, 30), text, Color.white);
            if (menuchoice.x == x_y && menuchoice.y == y_x)
            {
                DrawObject(new Rect(30 + spacing * x_y,  spacing * y_x, 30, 30), text, Color.red);
            }


        }
    }
    */
    public int PickFromList(int amount)
    {
        if (cells == null)
        {
            cell_x = 1;
            cell_y = amount;
            cells = new int[cell_x, cell_y];
        }

        return menuchoice.y;
    }

    public void DrawInMulti(Vector2 count, float spacing)
    {
        for (int x = 0; x < count.x; x++)
        {
            for (int y = 0; y < count.y; y++)
            {
                if (menuchoice == new Vector2Int(x, y))
                {
                    DrawObject(new Rect(spacing * x, spacing * y, 30, 30), text, Color.red);
                    if (Input.GetKeyDown(KeyCode.N))
                    {
                        foreach (gui_button butt in buttons)
                        {
                            if (butt.position == menuchoice)
                            {
                                butt.funct(butt.colour);
                            }
                        }


                        cell_colours.Add(new Vector2Int(x, y));
                        DrawObject(new Rect(spacing * x, spacing * y, 30, 30), text, Color.blue);
                    }
                }
                else
                {
                    DrawObject(new Rect(spacing * x, spacing * y, 30, 30), text, Color.white);

                    foreach (Vector2Int cell in cell_colours)
                    {
                        if (x == cell.x && y == cell.y)
                        {
                            DrawObject(new Rect(spacing * x, spacing * y, 30, 30), text, Color.green);
                        }
                    }
                    foreach (gui_button button in buttons)
                    {
                        if (x == button.position.x && y == button.position.y)
                        {
                            DrawObject(new Rect(spacing * x, spacing * y, 30, 30), text, button.colour);
                        }
                    }
                }


            }
        }
    }

    public void DrawTextGUI(string letter)
    {
        GUI.TextArea(new Rect(90, 90, 140, 80), letter);
    }

    public void DrawText(string letter)
    {
        GetComponent<Text>().text = letter;
    }

    public void DrawObject(Rect rectan, Texture2D thing, Color colour)
    {
        GUI.color = colour;
        GUI.DrawTexture(rectan, text);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
            menuchoice.y += 1;

        if (Input.GetKeyDown(KeyCode.UpArrow))
            menuchoice.y -= 1; //(int)Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.RightArrow))
            menuchoice.x += 1; //(int)Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.LeftArrow))
            menuchoice.x -= 1;
        
        menuchoice.x = Mathf.Clamp(menuchoice.x, 0, cell_x);

        if(NotificationText.text != null)
            NotificationText.text = TextShowNotification;

        if (!stayOn)
        {
            if (timer > 0)
            {
                NotificationBox.enabled = true;
                timer = timer - Time.deltaTime;
            }
            else
            {
                NotificationBox.enabled = false;
                TextShowNotification = "";
            }
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (!open_menu)
            {
                Time.timeScale = 0;
                open_menu = true;
            }
            else
            {
                Time.timeScale = 1;
                open_menu = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            menu = !menu;
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            cell_colours.Clear();
        }
        texttoWrite.Clear();
    }
}

[System.Serializable]
public class gui_element
{
    public Rect rectan;

    public gui_element(Rect rectan)
    {
        this.rectan = rectan;
    }

    void idk() { //DrawObject(new Rect(new Vector2(90, 90), new Vector2(90, 90)), text); 
    }

    public void DrawObject(Texture2D thing)
    {
        GUI.DrawTexture(rectan, thing);
    }

}
