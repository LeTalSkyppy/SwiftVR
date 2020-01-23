using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Production : MonoBehaviour
{
    public static Dictionary<string,bool> products = new Dictionary<string, bool>();

    public static Dictionary<string,bool> inProduction = new Dictionary<string, bool>();

    public  int A = 0;
    public Text Atext;
    public int B = 0;
    public Text Btext;
    public int C = 0;
    public Text Ctext;
    public  int D = 0;
    public Text Dtext;
    public int E = 0;
    public Text Etext;

    public static List<string> Alist = new List<string>() {"F1A","F2A","T1A","P1A","F3A","G1A"};
    public static List<string> Blist = new List<string>() {"G2B", "T3B" , "F2B" , "F4B", "P2B"};
    public static List<string> Clist = new List<string>() {"F3C", "T2C" , "G1C" , "F4C", "G2C"};
    public static List<string> Dlist = new List<string>() {"F1D", "T1D" , "F3D" , "F2D", "G2D"};
    public static List<string> Elist = new List<string>() {"F3E", "G1E" , "F4E" , "P3E"};

    public List<Text> TextProducts = new List<Text>();
    public List<Text> TextinProduction = new List<Text>();

    private const float ALPHA_MAX = 255f;
    private const float ALPHA_DISABLED = 140f/ALPHA_MAX;
    // Start is called before the first frame update
    void Start()
    {
        foreach(Text text in TextProducts)
        {
            text.color = new Color(text.color.r,text.color.g,text.color.b,ALPHA_DISABLED);
        }

        foreach(Text text in TextinProduction)
        {
            text.color = new Color(text.color.r,text.color.g,text.color.b,ALPHA_DISABLED);
        }
    }

    // Update is called once per frame
    // A = G1A
    // B = P2B
    // C = G2C
    // D = G2D
    // E = P3E
    void Update()
    {
        TString(Alist);
        TString(Blist);
        TString(Clist);
        TString(Dlist);
        TString(Elist);

        if(products["G1A"])
        {
            products["G1A"] = false;
            A++;
            Atext.text = "A: " + A.ToString();
        }

        if(products["P2B"])
        {
            products["P2B"] = false;
            B++;
            Btext.text = "B: " + B.ToString();
        }

        if(products["G2C"])
        {
            products["G2C"] = false;
            C++;
            Ctext.text = "C: " + C.ToString();
        }

        if(products["G2D"])
        {
            products["G2D"] = false;
            D++;
            Dtext.text = "D: " + D.ToString();
        }

        if(products["P3E"])
        {
            products["P3E"] = false;
            E++;
            Etext.text = "E: " + E.ToString();
        }
    }

    public void TString(List<string> tstring)
    {
        foreach(string tstr in tstring)
        {
            if(products[tstr])
            {
                foreach(Text text in TextProducts)
                {
                    if(text.name == tstr)
                    {
                        text.color = new Color(text.color.r,text.color.g,text.color.b, ALPHA_MAX);
                    }
                }
            }

            if(!products[tstr])
            {
                foreach(Text text in TextProducts)
                {
                    if(text.name == tstr)
                    {
                        text.color = new Color(text.color.r,text.color.g,text.color.b, ALPHA_DISABLED);
                    }
                }
            }


            if(inProduction[tstr])
            {
                foreach(Text text in TextinProduction)
                {
                    if(text.name == tstr)
                    {
                        text.color = new Color(text.color.r,text.color.g,text.color.b, ALPHA_MAX);
                    }
                }
            }

            if(!inProduction[tstr])
            {
                foreach(Text text in TextinProduction)
                {
                    if(text.name == tstr)
                    {
                        text.color = new Color(text.color.r,text.color.g,text.color.b, ALPHA_DISABLED);
                    }
                }
            }
        }
    }
}
