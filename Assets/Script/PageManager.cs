using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static Unity.Collections.AllocatorManager;
using System.Linq;
using System;
using Unity.VisualScripting;
using System.ComponentModel.Design;

public class PageManager : MonoBehaviour
{

    [Header("Home page")]
    public TMP_Text numToship;
    public TMP_Text numCC;
    public TMP_Text numCP;

    [Header("Order page")]
    public GameObject OrderPrefab;
    public Transform Orderlist;
    public GameObject OrderBookListPrefab;
    public GameObject orderDetail;
    public Transform orderBookList;
    public TMP_Text orderID;
    public TMP_Text orderAddress;
    public TMP_Text userID;
    public GameObject editAL;
    public Button editY;
    public Button editN;

    [Header("Book page")]
    public GameObject BookPrefab;
    public Transform Booklist;

    public GameObject bookDetailPage;
    public Image bookimg;
    public TMP_Text bookName;
    public TMP_Text content;
    public TMP_Text price;
    public TMP_Text stock;
    public TMP_Text status;
    public TMP_Text type;
    public TMP_Text Author;
    public TMP_Text Publisher;
    private GameObject bookObject;

    [Header("Book Edit")]
    public TMP_InputField bookNameIn;
    public TMP_InputField contentIn;
    public TMP_InputField priceIn;
    public TMP_InputField stockIn;
    public TMP_Dropdown statusIn;
    public TMP_Dropdown typeIn;
    public TMP_InputField AuthorIn;
    public TMP_InputField PublisherIn;
    public TMP_InputField bookImageIn;

    [Header("Book Edit alert")]
    public GameObject EditSS;
    public GameObject nameNULL2;
    public GameObject priceNULL2;
    public GameObject stockNULL2;
    public GameObject imgLoading2;
    public Image bookImageOut2;

    [Header("Book Add")]
    public TMP_InputField bookNameIn2;
    public TMP_InputField contentIn2;
    public TMP_InputField priceIn2;
    public TMP_InputField stockIn2;
    public TMP_Dropdown statusIn2;
    public TMP_Dropdown typeIn2;
    public TMP_InputField AuthorIn2;
    public TMP_InputField PublisherIn2;
    public TMP_InputField bookImageIn2;
    public Image bookImageOut;

    [Header("Book Add alert")]
    public GameObject AddSS;
    public GameObject nameNULL;
    public GameObject priceNULL;
    public GameObject stockNULL;
    public GameObject imgLoading;

    private ImageManager imgMana;
    private DataManager dataMana;

    private void Awake()
    {
        imgMana = FindFirstObjectByType<ImageManager>();
        dataMana= FindFirstObjectByType<DataManager>();
    }

    public void Home()
    {
        int tp = 0, cc = 0, cp = 0;
        foreach (Order order in DataManager.orders.Values)
        {
            if (order.Status == "To_ship") tp++;
            else if (order.Status == "Cancelled") cc++;
            else cp++;
        }
        numCC.text=cc.ToString();
        numCP.text=cp.ToString();
        numToship.text=tp.ToString();
    }
    public void loadingOrder()
    {
        while (Orderlist.childCount > 0)
        {
            DestroyImmediate(Orderlist.GetChild(0).gameObject);
        }
        foreach (Order order in DataManager.orders.Values)
        {
            GameObject clone = Instantiate(OrderPrefab);
            clone.transform.parent = Orderlist;
            clone.GetComponent<RectTransform>().sizeDelta = new Vector2(1000, 100);
            clone.GetComponent<OrderDetail>().order = order;
            clone.GetComponent<OrderDetail>().Show();
        }
        if (DataManager.orders.Count > 6)
        {
            Orderlist.gameObject.AddComponent<ContentSizeFitter>().verticalFit= ContentSizeFitter.FitMode.PreferredSize;
        }
        print("Hi Page Order");
        Home();
        
    }

    public void loadingBook()
    {
        while (Booklist.childCount > 0)
        {
            DestroyImmediate(Booklist.GetChild(0).gameObject);
        }
        foreach (Book b in DataManager.book.Values)
        {
            GameObject clone = Instantiate(BookPrefab);
            clone.transform.parent = Booklist;
            clone.GetComponent<RectTransform>().sizeDelta = new Vector2(1000, 100);
            clone.GetComponent<BookDetail>().book = b;
            clone.GetComponent<BookDetail>().Show();
        }
        if (DataManager.book.Count > 6)
        {
            Booklist.gameObject.AddComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        }
        print("Hi Page Order");
    }

    public void ShowBookDetail(Book book,GameObject t)
    {
        bookDetailPage.SetActive(true);
        Texture2D myTexture2D = book.imgBook;
        if (myTexture2D != null) bookimg.sprite = Sprite.Create(myTexture2D, new Rect(0.0f, 0.0f, myTexture2D.width, myTexture2D.height), new Vector2(0.5f, 0.5f), 100.0f);
        else bookimg.sprite = null;
        bookName.text = book.Name;
        price.text = book.Price.ToString();
        stock.text = book.Stock.ToString();
        type.text = book.TypeBook;
        status.text = ((statusBook)int.Parse(book.Status)).ToString();
        Author.text = book.Author;
        Publisher.text = book.Publisher;
        content.text = book.Title;
        bookObject = t;
    }

    public void ShowOrderDetail(Order order) {
        orderDetail.SetActive(true);
        orderAddress.text = order.OrderDetail;
        orderID.text = order.OrdarID.ToString();
        userID.text=order.UserID.ToString();
        while (orderBookList.childCount > 1)
        {
            DestroyImmediate(orderBookList.GetChild(1).gameObject);
        }
        for (int i=0;i<order.booksOrder.Count;i++)
        {
            Book book = order.booksOrder.ElementAt(i).Key;
            int a = order.booksOrder.ElementAt(i).Value;
            GameObject clone = Instantiate(OrderBookListPrefab);
            clone.transform.parent = orderBookList;
            clone.GetComponent<RectTransform>().sizeDelta = new Vector2(660, 50);
            clone.GetComponent<BookOrder>().ShowDetail(i+1,book.Name,a);
        }
        if (order.booksOrder.Count > 6)
        {
            orderBookList.gameObject.AddComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        }

    }
    public void edit()
    {
        Book book = bookObject.GetComponent<BookDetail>().book;
        bookNameIn.text = book.Name;
        priceIn.text = book.Price.ToString();
        stockIn.text = book.Stock.ToString();
        typeIn.value = (int)Enum.Parse(typeof(typeBook) ,book.TypeBook);
        statusIn.value = int.Parse(book.Status);
        AuthorIn.text = book.Author;
        PublisherIn.text = book.Publisher;
        contentIn.text = book.Title;
        
    }

    public void loadimgAdd()
    {
        if (!String.IsNullOrEmpty(bookImageIn2.text))
            StartCoroutine(imgMana.DownloadImage(bookImageIn2.text, Img =>
            {
                if (Img != null)
                {
                    print("have img");
                    bookImageOut.sprite = imgMana.TexttureToImg(Img);
                    imgLoading.SetActive(false);
                }
                else imgLoading.SetActive(true);

            }));
        else imgLoading.SetActive(true);
    }

    public void loadimgEdit()
    {
        if (!String.IsNullOrEmpty(bookImageIn2.text))
            StartCoroutine(imgMana.DownloadImage(bookImageIn2.text, Img =>
            {
                if (Img != null)
                {
                    print("have img");
                    bookImageOut2.sprite = imgMana.TexttureToImg(Img);
                    imgLoading2.SetActive(false);
                }
                else imgLoading2.SetActive(true);

            }));
        else imgLoading2.SetActive(true);
    }

    public void AddBook()
    {
        if (String.IsNullOrEmpty(bookNameIn2.text))
        {
            nameNULL.SetActive(true);
        }
        else if (String.IsNullOrEmpty(priceIn2.text)||!int.TryParse(priceIn2.text, out _)) {
            nameNULL.SetActive(false);
            priceNULL.SetActive(true); }
        else if (String.IsNullOrEmpty(stockIn2.text)||!int.TryParse(stockIn2.text, out _)) {
            nameNULL.SetActive(false);
            priceNULL.SetActive(false);
            stockNULL.SetActive(true); }
        else
        {
            nameNULL.SetActive(false);
            priceNULL.SetActive(false);
            stockNULL.SetActive(false);
            Book book = new Book(bookNameIn2.text, int.Parse(priceIn2.text), contentIn2.text, PublisherIn2.text, AuthorIn2.text, int.Parse(stockIn2.text), ((typeBook)(typeIn2.value)).ToString(), ((statusBook)statusIn2.value).ToString());
            if (!String.IsNullOrEmpty(bookImageIn2.text))
                StartCoroutine(imgMana.DownloadImage(bookImageIn2.text, Img =>
                {
                    if (Img != null)
                    {
                        book.setImg(imgMana.TexttureTo64(Img));
                        print("have img");
                        bookImageOut.sprite = imgMana.TexttureToImg(Img);
                    }
                    StartCoroutine(dataMana.AddBook(book, v =>
                    {
                        if (v == 1)
                        {
                            DataManager.book.Clear();
                            DataManager.orders.Clear();
                            StartCoroutine(dataMana.GetNormalData(b =>
                            {
                                if (b == 1)
                                {
                                    print("hi");
                                    AddSS.SetActive(true);
                                }
                            }));
                        }
                    }));

                }));
            else
                StartCoroutine(dataMana.AddBook(book, v =>
                {
                    if (v == 1)
                    {
                        DataManager.book.Clear();
                        DataManager.orders.Clear();
                        StartCoroutine(dataMana.GetNormalData(b =>
                        {
                            if (b == 1)
                            {
                                AddSS.SetActive(true);
                            }
                        }));
                    }
                }));
        }
    }
    public void EditBook()
    {
        if (String.IsNullOrEmpty(bookNameIn.text))
        {
            nameNULL2.SetActive(true);
        }
        else if (String.IsNullOrEmpty(priceIn.text)||!int.TryParse(priceIn.text, out _))
        {
            nameNULL2.SetActive(false);
            priceNULL.SetActive(true);
        }
        else if (String.IsNullOrEmpty(stockIn.text)|| !int.TryParse(stockIn.text,out _))
        {
            nameNULL2.SetActive(false);
            priceNULL2.SetActive(false);
            stockNULL2.SetActive(true);
        }
        else
        {
            nameNULL2.SetActive(false);
            priceNULL2.SetActive(false);
            stockNULL2.SetActive(false);
            
                //print(bookNameIn2.text+  int.Parse(priceIn.text)+ contentIn2.text+ PublisherIn2.text+ AuthorIn2.text+ int.Parse(stockIn2.text)+ ((typeBook)(typeIn2.value)).ToString()+((statusBook)statusIn2.value).ToString());
                Book book = bookObject.GetComponent<BookDetail>().book;
                book.Name = bookNameIn.text;
                book.Price = int.Parse(priceIn.text);
                book.Title = contentIn.text;
                book.Publisher = PublisherIn.text;
                book.Author = AuthorIn.text;
                book.Stock = int.Parse(stockIn.text);
                book.TypeBook = ((typeBook)(typeIn.value)).ToString();
                book.Status = ((statusBook)statusIn.value).ToString();
                if (!String.IsNullOrEmpty(bookImageIn.text))
                    StartCoroutine(imgMana.DownloadImage(bookImageIn.text, Img =>
                    {
                        if (Img != null)
                        {
                            book.setImg(imgMana.TexttureTo64(Img));
                            print("have img");
                            StartCoroutine(dataMana.EditBook(book, v =>
                            {
                                if (v == 1)
                                {
                                    bookObject.GetComponent<BookDetail>().Show();
                                    EditSS.SetActive(true);
                                }
                            }));
                        }

                    }));
                else
                    StartCoroutine(dataMana.EditBook(book, v =>
                    {
                        if (v == 1)
                        {
                            bookObject.GetComponent<BookDetail>().Show();
                            EditSS.SetActive(true);
                        }
                    }));
            }
    }

    public void close()
    {
        editAL.SetActive(false);
    }
    public void open()
    {
        editAL.SetActive(true);
    }
}
