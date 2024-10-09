using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;

public class DataManager : MonoBehaviour
{
    public static User user=new User();
    [SerializeField]public static Dictionary<int,Book> book = new Dictionary<int,Book>();//bookid,book
    public static Dictionary<int,Order> orders = new Dictionary<int,Order>();
    public static Cart userCart;

    public PageManager pageOrder;
    //public static List<> 

    private void Awake()
    {
            DataManager[] objs = FindObjectsOfType<DataManager>();
            print("5555555 "+objs.Length);
            if (objs.Length > 1)
            {
                Destroy(this.gameObject);
            }

            DontDestroyOnLoad(this.gameObject);
            StartCoroutine(GetNormalData(v =>
            {
                if (v == 1)
                {
                   
                }
            }));
            
    }
    private void Start()
    {
       
        //StartCoroutine(loginUser("yyyyy", "5555"));
    }
    public IEnumerator loginUser(String name, String pass, System.Action<int> callback = null)
    {
        WWWForm form = new WWWForm();
        form.AddField("name", name);
        form.AddField("password", pass);

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/Readly_Pj/Login.php",form))
        {
            yield return www.SendWebRequest();

            Debug.Log("on login");
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                string tex = www.downloadHandler.text;
                if (tex != null)
                {
                    if (tex.Equals("0") || tex.Equals(" password  not correct") || tex.Equals("3: Name don't exists") || tex.Equals("1: Connect failed"))
                    {
                        if (callback != null) callback.Invoke(0);

                    }
                    else
                    {
                        tex = tex.Remove(0, 2);
                        tex = tex.Remove((tex.Length) - 2, 2);
                        Debug.Log(tex);
                        string[] data = tex.Split('-');
                        user.UserID = int.Parse(data[0]);
                        user.UserName = data[1];
                        user.Email = data[2];
                        user.CastID = int.Parse(data[3]);
                        userCart = new Cart(user.CastID);
                        Debug.Log("Login success Userid :"+user.UserID);
                        StartCoroutine(GetOrderData());
                        StartCoroutine(GetCartData());
                        if (callback != null) callback.Invoke(1);

                    }
                    
                }

            }
        }
    }

    public IEnumerator GetNormalData(System.Action<int> callback = null)
    {
        WWWForm form = new WWWForm();

        using (UnityWebRequest www = UnityWebRequest.Get("http://localhost/Readly_Pj/GetBookData.php"))
        {
            yield return www.SendWebRequest();


            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                string tex = www.downloadHandler.text;
                if (tex != null)
                {
                    if (!tex.Equals("0"))
                    {
                        StartCoroutine( GetBook(tex));
                    }
                    print(tex);
                }
                

            }
        }
    }

    IEnumerator GetBook(string tex)
    {
        tex=tex.Remove(0,1);
        print(tex);
        print((tex.Length));
        tex=tex.Remove((tex.Length)-1,1);
        string[] books = tex.Split(',');
        for (int i = 0; i < books.Length; i++)
        {
            books[i]=books[i].Remove(0,1);
            string v = books[i].Remove(books[i].Length - 1, 1);
            books[i] = v;
           string[] detail = books[i].Split("-");
            WWWForm form = new WWWForm();
            book.Add(int.Parse(detail[0]), new Book(detail[1], int.Parse(detail[0]), int.Parse(detail[5]), detail[8], detail[4], detail[2], int.Parse(detail[6]), detail[3], detail[7]));
            form.AddField("bookID",detail[0]) ;

            using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/Readly_Pj/GetImgBook.php",form))
            {
                yield return www.SendWebRequest();


                if (www.isNetworkError || www.isHttpError)
                {
                    Debug.Log(www.error);
                }
                else
                {
                    string texe = www.downloadHandler.text;
                    if (texe != null)
                    {
                        if (!texe.Equals("0"))
                        {
                            book[book.ElementAt(book.Count-1).Key].setImg(texe);
                        }
                        else
                        {
                            print("cann't get img");
                        }

                    }


                }
            }

        }pageOrder.loadingBook();
        StartCoroutine(GetOrderData(value =>
        {
            if (value == 1)
            {
                pageOrder.loadingOrder();
            }
        }));
        print("this is status book 1 :" + book[1].Status);
    }
    public IEnumerator GetOrderDetail(int id)
    {
        WWWForm form = new WWWForm();
        form.AddField("id", id);

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/Readly_Pj/GetBookFromOrder.php", form))
        {
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                string tex = www.downloadHandler.text;
                if (tex != null)
                {
                    if (tex.Equals("0") || tex.Equals(" password  not correct") || tex.Equals("3: Name don't exists") || tex.Equals("1: Connect failed"))
                    {
                        print("Error get order data or have null order data");
                        yield return null;
                    }
                    else
                    {
                        if (tex.Equals("0") || tex.Equals(" password  not correct") || tex.Equals("3: Name don't exists") || tex.Equals("1: Connect failed"))
                        {
                            print("Error get order data or have null order data");
                            yield return null;
                        }
                        else
                        {
                            tex = tex.Remove(0, 1);
                            print(tex);
                            print((tex.Length));
                            tex = tex.Remove((tex.Length) - 1, 1);
                            string[] detail = tex.Split(',');
                            for (int i = 0; i < detail.Length; i++)
                            {
                                detail[i] = detail[i].Remove(0, 1);
                                string v = detail[i].Remove(detail[i].Length - 1, 1);
                                detail[i] = v;
                                string[] detail2 = detail[i].Split("-");
                                orders[id].setBookOrder(int.Parse(detail2[0]), int.Parse(detail2[1]));
                            }
                                
                        }
                    }

                }
            }
        }

    }
    public IEnumerator GetOrderData(System.Action<int> callback = null)
    {
        WWWForm form = new WWWForm();

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/Readly_Pj/Manager/GetAllOrder.php", form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
                if (callback != null) callback.Invoke(0);
            }
            else
            {
                string tex = www.downloadHandler.text;
                if (tex != null)
                {
                    if (tex.Equals("0") || tex.Equals(" password  not correct") || tex.Equals("3: Name don't exists") || tex.Equals("1: Connect failed"))
                    {
                        print("Error get order data or have null order data");
                        yield return null;
                    }
                    else
                    {
                        tex = tex.Remove(0, 1);
                        print(tex);
                        print((tex.Length));
                        tex = tex.Remove((tex.Length) - 1, 1);
                        string[] order = tex.Split(',');
                        for (int i = 0; i < order.Length; i++)
                        {
                            order[i] = order[i].Remove(0, 1);
                            string v = order[i].Remove(order[i].Length - 1, 1);
                            order[i] = v;
                            string[] detail = order[i].Split("-");

                            orders.Add(int.Parse(detail[0]), new Order(int.Parse(detail[0]), detail[2], int.Parse(detail[4]), detail[1], int.Parse(detail[3])));
                            StartCoroutine(GetOrderDetail(int.Parse(detail[0])));
                        }
                        
                    }
                    if (callback != null) callback.Invoke(1);

                }

            }
        }
    }
    public IEnumerator CreateOrder(string address,Dictionary<Book,int> addBook, System.Action<int> callback = null)
    {
        WWWForm form = new WWWForm();
        form.AddField("id", user.UserID);
        form.AddField("detail", address);
        form.AddField("cartID", user.CastID);

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/Readly_Pj/SetOrderData.php", form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
                if (callback != null) callback.Invoke(0);
            }
            else
            {
                string tex = www.downloadHandler.text;
                if (tex != null)
                {
                    if (tex.Equals("0") || tex.Equals(" password  not correct") || tex.Equals("3: Name don't exists") || tex.Equals("1: Connect failed"))
                    {
                        print("Error create order data or have null order data");
                        if (callback != null) callback.Invoke(0);
                        yield return null;
                    }
                    else
                    {
                        WWWForm form2 = new WWWForm();
                        form2.AddField("orderID", tex);
                        for (int i = 0; i < addBook.Count; i++) {
                            form2.AddField("amount", addBook.ElementAt(i).Value);
                            form2.AddField("bookID", addBook.ElementAt(i).Key.Id);

                            using (UnityWebRequest www2 = UnityWebRequest.Post("http://localhost/Readly_Pj/SetBookInOrder.php", form2))
                            {
                                yield return www2.SendWebRequest();
                                if (www.isNetworkError || www.isHttpError)
                                {
                                    Debug.Log(www.error);
                                }
                                else
                                {
                                    string text=www.downloadHandler.text;
                                    if (text == "ss")
                                    {
                                        Debug.Log("add book to order ss");
                                    }

                                }
                            }
                        }
                        
                        print("order sussess");
                        StartCoroutine(GetOrderData());
                        if (callback != null) callback.Invoke(1);
                    }

                }

            }
        }
    }
    public IEnumerator AddCartData(int bookID ,int num, System.Action<int> callback = null)
    {
        WWWForm form = new WWWForm();
        form.AddField("cartID", user.CastID);
        form.AddField("amount", num);
        form.AddField("bookID", bookID);

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/Readly_Pj/SetBookInCart.php", form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                string tex = www.downloadHandler.text;
                if (tex != null)
                {
                    if (tex.Equals("0") || tex.Equals(" password  not correct") || tex.Equals("3: Name don't exists") || tex.Equals("1: Connect failed"))
                    {
                        print("Error get cart data or have null order data");
                        callback?.Invoke(0);
                        yield return null;
                    }
                    else
                    {
                        
                        print("add cart data ss");
                        callback?.Invoke(1);
                    }

                }

            }
        }
    }
    public IEnumerator ChangeNumBookInCart(int bookID, int num, System.Action<int> callback = null)
    {
        WWWForm form = new WWWForm();
        form.AddField("cartID", user.CastID);
        form.AddField("amount", num);
        form.AddField("bookID", bookID);

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/Readly_Pj/AddMoreBookToCart.php", form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                string tex = www.downloadHandler.text;
                if (tex != null)
                {
                    if (tex.Equals("0") || tex.Equals(" password  not correct") || tex.Equals("3: Name don't exists") || tex.Equals("1: Connect failed"))
                    {
                        print("Error get cart data or have null order data");
                        if (callback != null) callback.Invoke(0);
                        yield return null;
                    }
                    else
                    {
                        userCart.ChangAmountBookFromCart(bookID, num);
                        print("add cart data ss");
                        if (callback != null) callback.Invoke(1);
                    }

                }

            }
        }
    }
    public IEnumerator DeleteBookInCart(int bookID, System.Action<int> callback = null)
    {
        WWWForm form = new WWWForm();
        form.AddField("cartID", user.CastID);
        form.AddField("bookID", bookID);

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/Readly_Pj/DeleteBookInCart.php", form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                string tex = www.downloadHandler.text;
                if (tex != null)
                {
                    if (tex.Equals("0") || tex.Equals(" password  not correct") || tex.Equals("3: Name don't exists") || tex.Equals("1: Connect failed"))
                    {
                        print("Error get cart data or have null order data");
                        if (callback != null) callback.Invoke(0);
                        yield return null;
                    }
                    else
                    {
                        userCart.RemoveBookFromCart(bookID);
                        print("add cart data ss");
                        if (callback != null) callback.Invoke(1);
                    }

                }

            }
        }
    }
    public IEnumerator DeleteAllBookInCart(System.Action<int> callback = null)
    {
        WWWForm form = new WWWForm();
        form.AddField("cartID", user.CastID);

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/Readly_Pj/DeleteAllBookInCart.php", form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                string tex = www.downloadHandler.text;
                if (tex != null)
                {
                    if (tex.Equals("0") || tex.Equals(" password  not correct") || tex.Equals("3: Name don't exists") || tex.Equals("1: Connect failed"))
                    {
                        print("Error get cart data or have null order data");
                        if (callback != null) callback.Invoke(0);
                        yield return null;
                    }
                    else
                    {
                        userCart.Clear();
                        print("delete cart data ss");
                        if (callback != null) callback.Invoke(1);
                    }

                }

            }
        }
    }
    public IEnumerator GetCartData()
    {
        WWWForm form = new WWWForm();
        form.AddField("id", user.CastID);

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/Readly_Pj/GetCartData.php", form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                string tex = www.downloadHandler.text;
                if (tex != null)
                {
                    if (tex.Equals("0") || tex.Equals(" password  not correct") || tex.Equals("3: Name don't exists") || tex.Equals("1: Connect failed"))
                    {
                        print("Error get cart data or have null order data");
                        yield return null;
                    }
                    else
                    {
                        tex = tex.Remove(0, 1);
                        print(tex);
                        print((tex.Length));
                        tex = tex.Remove((tex.Length) - 1, 1);
                        string[] order = tex.Split(',');
                        for (int i = 0; i < order.Length; i++)
                        {
                            order[i] = order[i].Remove(0, 1);
                            string v = order[i].Remove(order[i].Length - 1, 1);
                            order[i] = v;
                            string[] detail = order[i].Split("-");
                            userCart.AddBookToCart(book[int.Parse(detail[0])],int.Parse(detail[1]));
       
                        }
                        print("get cart data ss");
                    }

                }

            }
        }
    }

    public IEnumerator AddBook(Book b, System.Action<int> callback = null)
    {
        WWWForm form = new WWWForm();
        form.AddField("name",b.Name );
        form.AddField("price", b.Price);
        form.AddField("BookAuthor", b.Author);
        string t = b.TypeBook;
        if (t == "Self_help") t = "Self help";
        form.AddField("bookType", t);
        string s = ((statusBook)int.Parse(b.Status)).ToString();
        if (s == "on_shelf") s = "on shelf";
        else if (s == "Out_of_stock") s = "Out of stock";
        form.AddField("status", s);
        form.AddField("BookPublisher", b.Publisher);
        form.AddField("stock",b.Stock);
        form.AddField("content",b.Title);
        if(b.imgBook!=null)
        {var imgMana = FindFirstObjectByType<ImageManager>();
            form.AddField("bookImg", imgMana.TexttureTo64(b.imgBook));
        }


        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/Readly_Pj/Manager/AddBook.php", form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                string tex = www.downloadHandler.text;
                if (tex != null)
                {
                    if (tex.Equals("0") || tex.Equals(" password  not correct") || tex.Equals("3: Name don't exists") || tex.Equals("1: Connect failed"))
                    {
                        print("Error get cart data or have null order data");
                        callback?.Invoke(0);
                        yield return null;
                    }
                    else
                    {

                        print("add book data ss");
                        callback?.Invoke(1);
                    }

                }

            }
        }
    }
    public IEnumerator EditBook(Book b, System.Action<int> callback = null)
    {
        WWWForm form = new WWWForm();
        form.AddField("Bookid", b.Id);
        form.AddField("name", b.Name);
        form.AddField("price", b.Price);
        form.AddField("BookAuthor", b.Author);
        string t = b.TypeBook;
        if (t == "Self_help") t = "Self help";
        form.AddField("bookType", t);
        string s = ((statusBook)int.Parse(b.Status)).ToString();
        if (s == "on_shelf") s = "on shelf";
        else if (s == "Out_of_stock") s = "Out of stock";
        form.AddField("status", s);
        form.AddField("BookPublisher", b.Publisher);
        form.AddField("stock", b.Stock);
        form.AddField("content", b.Title);
        if (b.imgBook != null)
        {
            var imgMana = FindFirstObjectByType<ImageManager>();
            form.AddField("bookImg", imgMana.TexttureTo64(b.imgBook));
        }


        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/Readly_Pj/Manager/EditBook.php", form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                string tex = www.downloadHandler.text;
                if (tex != null)
                {
                    if (tex.Equals("0") || tex.Equals(" password  not correct") || tex.Equals("3: Name don't exists") || tex.Equals("1: Connect failed"))
                    {
                        print("Error get cart data or have null order data");
                        callback?.Invoke(0);
                        yield return null;
                    }
                    else
                    {

                        print("edit book data ss");
                        callback?.Invoke(1);
                    }

                }

            }
        }
    }
    public IEnumerator EditOrderStatus(Order b, System.Action<int> callback = null)
    {
        WWWForm form = new WWWForm();
        print(b.OrdarID);
        form.AddField("orderid", b.OrdarID);
        string s = (b.Status).ToString();
        if (s == "To_ship") s = "To ship";
        form.AddField("status", s);
        


        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/Readly_Pj/Manager/SetStatusOrder.php", form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                string tex = www.downloadHandler.text;
                if (tex != null)
                {
                    if (tex.Equals("0") || tex.Equals(" password  not correct") || tex.Equals("3: Name don't exists") || tex.Equals("1: Connect failed"))
                    {
                        print("Error edit status data");
                        callback?.Invoke(0);
                        yield return null;
                    }
                    else
                    {

                        print("edit status data ss");
                        callback?.Invoke(1);
                    }

                }

            }
        }
    }
}

    
