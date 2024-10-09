using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BookDetail : MonoBehaviour
{
    public Book book;
    public TMP_Text price;
    public TMP_Text stock;
    public TMP_Text bookID;
    public TMP_Text name;
    public TMP_Dropdown status;
    public Image image;


    public void Show()
    {
        name.text = book.Name;
        bookID.text = book.Id.ToString();
        price.text = book.Price.ToString();
        stock.text = book.Stock.ToString();
        status.value = int.Parse(book.Status);
        Texture2D myTexture2D = book.imgBook;

        if(myTexture2D!=null)image.sprite = Sprite.Create(myTexture2D, new Rect(0.0f, 0.0f, myTexture2D.width, myTexture2D.height), new Vector2(0.5f, 0.5f), 100.0f);
    }

    public void detail()
    {
        PageManager pM=FindFirstObjectByType<PageManager>();
        pM.ShowBookDetail(book, this.gameObject);

    }
}
