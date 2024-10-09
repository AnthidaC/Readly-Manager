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
    private int castID;
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

    public int CastID
    {
        get { return castID; }
        set { castID = value; }
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
