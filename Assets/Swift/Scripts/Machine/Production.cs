using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Production : MonoBehaviour
{
    public static Dictionary<string,bool> products = new Dictionary<string, bool>();

    public static Dictionary<string,bool> inProduction = new Dictionary<string, bool>();

    public  int A = 0;
    public int B = 0;
    public int C = 0;
    public  int D = 0;
    public int E = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    // A = G1A
    // B = P2B
    // C = G2C
    // D = G2D
    // E = P3E
    void Update()
    {
        if(products["G1A"])
        {
            products["G1A"] = false;
            A++;
        }

        if(products["P2B"])
        {
            products["P2B"] = false;
            B++;
        }

        if(products["G2C"])
        {
            products["G2C"] = false;
            C++;
        }

        if(products["G2D"])
        {
            products["G2D"] = false;
            D++;
        }

        if(products["P3E"])
        {
            products["P3E"] = false;
            E++;
        }
    }
}
