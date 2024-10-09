using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum orderStatus:int
{
    Cancelled=2,
    To_ship=0,
    complete=1
}
[System.Serializable]
public class Order 
{
    private int ordarID;
    private int userID;
    private string orderDetail;
    public orderStatus status;
    private int cartID;

    public Order(int ordarID, string orderDetail, int cartID, string status, int userID)
    {
        OrdarID = ordarID;
        OrderDetail = orderDetail;
        CartID = cartID;
        Status = status;
        UserID = userID;
    }

    public Dictionary<Book,int> booksOrder = new Dictionary<Book, int>();// int is amount
    public int OrdarID { get => ordarID; set => this.ordarID = value; }
    
    public string OrderDetail { get => orderDetail; set => this.orderDetail = value; }
    public int CartID { get => cartID; set => this.cartID = value; }
    public string Status
    {
        get => status.ToString(); 
        set
        {
            if (value == "Cancelled") status = orderStatus.Cancelled;
            else if (value == "To ship"||value== "To_ship") status = orderStatus.To_ship;
            else if (value == "complete") status = orderStatus.complete;
        }
    }
    public int UserID { get => userID; set => userID = value; }

    public Order(int ordarID, int UserID, string orderDetail, int cartID)
    {
        OrdarID = ordarID;
        this.UserID = UserID;
        OrderDetail = orderDetail;
        this.CartID = cartID;
    }
    public void setBookOrder(int bookid,int amount)
    {
        this.booksOrder.Add(DataManager.book[bookid], amount);
        
    }

}