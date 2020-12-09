using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using TMPro;

public class GameManager : MonoBehaviour
{
    //XML variables
    [Header("Levels")]
    public TextAsset levelsAsset;
    public int levelChosen = 0;
    private List<Dictionary<string, string>> levels = new List<Dictionary<string, string>>();
    private Dictionary<string, string> obj;

    //Variables from CircleSegmentManager.cs
    [Header("Level parameters")]
    public int nLayer = 6;
    public int nSlice = 6;
    public int numberLane = 4; // Number of lanes to play with
    public Color[] segmentColors;
    public int heightFilling = 5;
    public int probabilityBlackLastLayer = 3;
    public string fillingMode = "normal";//easy": maximize the chance to have two blocks next to each other
                                         //"normal": 2 blocks of the same color can be next to each other
                                         //"hard": 2 blocks of the same color can't be next to each other
    
    [Header("Objects")]
    public GameObject planetTop;
    public GameObject planetCore;
    public GameObject projectile;
    public TextMeshProUGUI gameOverText;

    //Variables from PlayerController.cs
    [Header("Player")]
    public bool enableGravity = true;
    public float gravity = 2000;
    public float jumpForce = 5000;
    public float speed = 2;
    public float throwForce = 2;  // For throwing a projectile

    

    // Start is called before the first frame update
    void Start()
    {
        GetLevel();

        //Level initialization
        Dictionary<string, string> level = levels[levelChosen];
        nSlice = int.Parse(level["nSlice"]);
        nLayer = int.Parse(level["nLayer"]);

        /*
        foreach (string key in levels[0].Keys)
        {
            Debug.Log(key);
        }
        */
    }

    //Script to load the levels from the XML file
    public void GetLevel()
    {
        XmlDocument xmlDoc = new XmlDocument(); // xmlDoc is the new xml document.
        xmlDoc.LoadXml(levelsAsset.text); // load the file.
        XmlNodeList levelsList = xmlDoc.GetElementsByTagName("level"); // array of the level nodes.

        foreach (XmlNode levelInfo in levelsList)
        {
            XmlNodeList levelcontent = levelInfo.ChildNodes;
            obj = new Dictionary<string, string>(); // Create a object(Dictionary) to colect the both nodes inside the level node and then put into levels[] array.

            foreach (XmlNode levelsItens in levelcontent) // levels itens nodes.
            {

                if (levelsItens.Name == "player")
                {
                    obj.Add("enableGravity", levelsItens.Attributes["enableGravity"].Value); // put this in the dictionary.
                    obj.Add("gravity", levelsItens.Attributes["gravity"].Value); // put this in the dictionary.
                    obj.Add("jumpForce", levelsItens.Attributes["jumpForce"].Value); // put this in the dictionary.
                    obj.Add("speed", levelsItens.Attributes["speed"].Value);; // put this in the dictionary.
                    obj.Add("throwForce", levelsItens.Attributes["throwForce"].Value); ; // put this in the dictionary.
                }

                else
                {
                    obj.Add(levelsItens.Name, levelsItens.Attributes["value"].Value); // put this in the dictionary.
                }
            }
            levels.Add(obj); // add whole obj dictionary in the levels[].
        }
    }

}
