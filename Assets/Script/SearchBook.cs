using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SearchBook : MonoBehaviour
{
    public Transform Booklist;
    public TMP_InputField ser;
    public TMP_Dropdown status;
    public TMP_Dropdown type;

    // Update is called once per frame
    public void search()
    {
        int l = 0;
        for (int i = 0; i < Booklist.childCount; i++)
        {
            if (string.IsNullOrEmpty(ser.text) && status.value == 0&&type.value==0)
            {
                Booklist.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                if (Booklist.childCount > 6)
                    { Booklist.GetComponent<ContentSizeFitter>().enabled = false; 
                    Booklist.GetComponent<RectTransform>().sizeDelta = new Vector2(1925, 680);
                }
                GameObject g = Booklist.GetChild(i).gameObject;
                if (g.GetComponent<BookDetail>().book.ToString().Contains(ser.text) && (((status.value - 1) == int.Parse(g.GetComponent<BookDetail>().book.Status)) || status.value == 0)&&(((typeBook)(type.value-1)).ToString()== g.GetComponent<BookDetail>().book.TypeBook||type.value==0))
                {
                    g.SetActive(true);
                    l++;
                }
                else g.SetActive(false);

            }
        }
        if (l > 6)
        {

            Booklist.GetComponent<ContentSizeFitter>().enabled = true;
        }
    }
}
