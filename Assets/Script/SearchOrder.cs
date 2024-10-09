using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SearchOrder : MonoBehaviour
{
    public Transform Orderlist;
    public TMP_InputField ser;
    public TMP_Dropdown dropdown;

    // Update is called once per frame
    public void search()
    {
        int l = 0;
        for (int i=0;i< Orderlist.childCount; i++)
        {
            if (string.IsNullOrEmpty(ser.text)&&dropdown.value == 0)
            {
                Orderlist.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                if (Orderlist.childCount > 6)
                {
                    Orderlist.GetComponent<ContentSizeFitter>().enabled = false;
                    Orderlist.GetComponent<RectTransform>().sizeDelta = new Vector2(1000, 790);
                }
                   
                
                GameObject g = Orderlist.GetChild(i).gameObject;
                if (g.GetComponent<OrderDetail>().order.OrdarID.ToString().Contains(ser.text)&&(((orderStatus)(dropdown.value-1)== g.GetComponent<OrderDetail>().order.status)||dropdown.value==0)) { 
                    g.SetActive(true);
                    l++;
                }
                else g.SetActive(false);

            }
        }
        if (l > 6)
        {
            
            Orderlist.GetComponent<ContentSizeFitter>().enabled = true;
        }
    }

}
