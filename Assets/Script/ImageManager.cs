using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


public class ImageManager : MonoBehaviour
{
    public Texture2D img = null;


   
    public IEnumerator DownloadImage(string MediaUrl, System.Action<Texture2D> callback = null)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(MediaUrl);
        yield return request.SendWebRequest();
        if (request.isNetworkError || request.isHttpError)
            Debug.Log(request.error);
        else
            callback?.Invoke(((DownloadHandlerTexture)request.downloadHandler).texture);

    }

    public string TexttureTo64(Texture2D img) {
        return System.Convert.ToBase64String(img.EncodeToPNG());
    }
    public void ImgTotexture(Image img)
    {

    }
    public Sprite TexttureToImg(Texture2D te)
    {
 
        return Sprite.Create(te, new Rect(0.0f, 0.0f, te.width, te.height), new Vector2(0.5f, 0.5f), 100.0f);
    }
}
