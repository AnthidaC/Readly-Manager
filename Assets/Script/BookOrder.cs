using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BookOrder : MonoBehaviour
{
    public TMP_Text numr;
    public TMP_Text name;
    public TMP_Text amount;
    // Start is called before the first frame update
    public void ShowDetail(int num,string bookname,int amountt)
    {
        numr.text = num.ToString();
        name.text = bookname;
        amount.text = amountt.ToString();
        

    }
}
