using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class SignIn : MonoBehaviour
{
    public TMP_InputField nameInput;
    public TMP_InputField passwordInput;

    public void RegistorSubmit()
    {
        StartCoroutine(Registor());
    }
    public IEnumerator Registor()
    {
        WWWForm form = new WWWForm();
        form.AddField("name",nameInput.text);
        form.AddField("password", passwordInput.text);

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/Readly_Pj/Registor.php", form))
        {
            yield return www.SendWebRequest();


            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
            }
        }
    }
    // Start is called before the first frame update

}
