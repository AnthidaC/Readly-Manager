using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class UserRegistor : MonoBehaviour
{
    [Header("Sing In page")]
    public TMP_InputField nameInput;
    public TMP_InputField passwordInput;
    public TMP_InputField emailInput;
    public GameObject SingInPage;
    public GameObject successPage;

    [Header("Log In page")]
    public TMP_InputField Usernamelogin;
    public TMP_InputField passwordLogin;
    public GameObject LogInPage;
    
    [SerializeField]
    private DataManager dataManager;

    private void Awake()
    {
        dataManager = FindFirstObjectByType<DataManager>();
    }

    public void RegistorSubmit()
    {
        if (!string.IsNullOrEmpty(nameInput.text))
        {
            if(passwordInput.text.Length >= 8)
            {
                if (emailInput.text.Contains("@")){
                    StartCoroutine(Registor(ReturnValue =>
                    {
                        if (ReturnValue == 1)
                        {
                            successPage.SetActive(true);
                            SingInPage.SetActive(false);
                        }
                    }));
                }
                else { 
                    
                }

            }
        }

        
    }

    public void LogIn()
    {
        if (!string.IsNullOrEmpty(Usernamelogin.text) && !string.IsNullOrEmpty(passwordLogin.text))
        {
            StartCoroutine(dataManager.loginUser(Usernamelogin.text, passwordLogin.text, returnValue =>
            {
                if(returnValue == 1)
                {
                    print(DataManager.user.ToString()+" Wow");
                    SceneManager.LoadScene(1);
                }
                else
                {
                    print("log in faile");
                }
            }));
        }
    }


    public IEnumerator Registor(System.Action<int> callback = null)
    {
        WWWForm form = new WWWForm();
        form.AddField("name",nameInput.text);
        form.AddField("password", passwordInput.text);
        form.AddField("email", emailInput.text);

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/Readly_Pj/Registor.php", form))
        {
            yield return www.SendWebRequest();


            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                string tex = www.downloadHandler.text;
                if (tex != null) { 
                    if(tex.Equals("success"))
                    {
                        print("create user success");
                        if (callback != null) callback.Invoke(1);
                    }
                    print(tex);
                }

            }
        }
    }
    // Start is called before the first frame update

}
