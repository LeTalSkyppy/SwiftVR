using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Machine : MonoBehaviour
{
    public bool isPowered;
    public bool busy;
    public float timeToProduct;

    public float productTime;
    public List<string> queueProduct;

    public Text timerText;

    public RectTransform timerImage;

    public Text nomPiece;

    //Key = produit par la machine ; Value = produit nécessaire pour le produit de la machine, "none" dans le cas ou aucun produit n'est nécessaire.
    public Dictionary<string,string> productsMachine = new Dictionary<string, string>();

    public void AddQueueProduct()
    {
        foreach(KeyValuePair<string,string> product in productsMachine)
        {
            if(product.Value == "none")
            {
                if(!Production.products[product.Key] && !queueProduct.Contains(product.Key))
                {
                    queueProduct.Add(product.Key);
                }
            }
            else
            {
                if(!Production.products[product.Key] && !queueProduct.Contains(product.Key) && Production.products[product.Value])
                {
                    queueProduct.Add(product.Key);
                }
            }
            
        }
    }
    public void BusyOrNotBusy()
    {
        if(!busy && queueProduct.Count > 0)
        {
            busy = true;
            Production.inProduction[queueProduct[0]] = true;
            nomPiece.text = queueProduct[0];
            timerImage.gameObject.SetActive(true);
            timerText.gameObject.SetActive(true);
            if(productsMachine[queueProduct[0]] != "none")
            {
                // On utilise la pièce précédente, l'autre machine peut donc recommencer à produit ce type de pièce
                Production.products[productsMachine[queueProduct[0]]] = false;
            }
        }
        if(busy)
        {
            productTime -= Time.deltaTime;
            timerImage.sizeDelta = new Vector2(((timeToProduct-productTime)/timeToProduct)*300f,timerImage.sizeDelta.y);
            timerText.text = productTime.ToString("F1");
            if(productTime <= 0)
            {
                busy = false;
                Production.inProduction[queueProduct[0]] = false;
                Production.products[queueProduct[0]] = true;
                nomPiece.text = "";
                timerImage.gameObject.SetActive(false);
                timerText.gameObject.SetActive(false);
                queueProduct.RemoveAt(0);
                productTime = timeToProduct;
            } 
        }
    }

    public void AddProducts()
    {
        foreach(KeyValuePair<string,string> product in productsMachine)
        {
            Production.products.Add(product.Key,false);
            Production.inProduction.Add(product.Key,false);
        }
    }

}
