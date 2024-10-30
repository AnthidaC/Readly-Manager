using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

public class User
{
    private int userID;
    private int cartID;
    private string email;
    private string userName;
    public int UserID
    {
        get { return userID; }
        set { userID = value; }
    }
    public string UserName
    {
        get { return userName; }
        set { userName = value; }
    }

    public int CartID
    {
        get { return cartID; }
        set { cartID = value; }
    }

    public string Email { get => email; set => email = value; }

    public User(string name)
    {
        this.UserName = name;
    }

    public User()
    {

    }
}
