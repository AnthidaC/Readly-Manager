using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Cart
{
    private int cartID;
    private Dictionary<Book, int> booksInCart=new Dictionary<Book, int>();

    public int CartID { get => cartID; set => cartID = value; }
    public Dictionary<Book, int> BooksInCart { get => booksInCart; set => booksInCart = value; }

    public Cart(int cartID)
    {
        this.CartID = cartID;
    }

    public bool AddBookToCart(Book book,int amount)
    {
        for (int i = 0; i < this.booksInCart.Count; i++)
        {
            if (booksInCart.ElementAt(i).Key.Id == book.Id)
            {
                Debug.Log("book is exist");
                return true;
            }
        }
        booksInCart.Add(book, amount);
        return true;
    }
    public void ChangAmountBookFromCart(int bookId,int amount) {
        for (int i = 0; i < this.booksInCart.Count; i++)
        {
            if (booksInCart.ElementAt(i).Key.Id == bookId)
            {
                booksInCart[(booksInCart.ElementAt(i).Key)]=amount;
            }
        }
    }
    public void RemoveBookFromCart(int bookID) {
        for (int i = 0; i < this.booksInCart.Count; i++)
        {
            if (booksInCart.ElementAt(i).Key.Id == bookID)
            {
                booksInCart.Remove(booksInCart.ElementAt(i).Key);
            }
        }
    }
    public void Clear() { 
        this.booksInCart = null;
    }
}
