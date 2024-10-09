using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OrderDetail : MonoBehaviour
{
    public Order order;
    public TMP_Text orderID;
    public TMP_Text userID;
    public TMP_Dropdown status;

    public void Show()
    {
     
        orderID.text=order.OrdarID.ToString();
        userID.text=order.UserID.ToString();
        print(((orderStatus)order.status).ToString()+ order.OrdarID.ToString());
        status.value = (int)order.status;
    }
    public void detail()
    {
        PageManager pM = FindFirstObjectByType<PageManager>();
        pM.ShowOrderDetail(order);

    }

    public void changeStatus()
    {
        if (status.value != (int)order.status)
        {
            order.status=((orderStatus)(status.value));
            DataManager da = FindFirstObjectByType<DataManager>();
            StartCoroutine(da.EditOrderStatus(order, v =>
            {
                if (v == 1)
                {
                    this.Show();
                }
            }));
        }
    }
    public void Check()
    {
        if((orderStatus)status.value!=order.status)
        {PageManager pM = FindFirstObjectByType<PageManager>();
        pM.editY.onClick.RemoveAllListeners();
        pM.editY.onClick.AddListener(this.changeStatus);
        pM.editY.onClick.AddListener(pM.close);
        pM.editN.onClick.RemoveAllListeners();
        pM.editN.onClick.AddListener(this.Show);
        pM.editN.onClick.AddListener(pM.close);
            pM.open();
        }

    }


}
