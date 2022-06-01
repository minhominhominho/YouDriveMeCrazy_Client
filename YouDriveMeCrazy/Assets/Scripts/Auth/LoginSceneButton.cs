using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginSceneButton : MonoBehaviour
{
    public GameObject createAccountHolder;
    public GameObject loginHolder;
    public GameObject dialogHolder;
    public GameObject nickWarningHolder;
    public GameObject successMessageHolder;
    public GameObject wrongmatchPassHolder;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void CreateAccountScene(){
        createAccountHolder.SetActive(true);
        loginHolder.SetActive(false);

    }
    public  void LoginScene(){
        createAccountHolder.SetActive(false);
        loginHolder.SetActive(true);

    }
    //dialog message ok button
    public void dialogOffButton(){
            dialogHolder.SetActive(false);
    }
    public void nickWarningOffButton(){
            nickWarningHolder.SetActive(false);
    }
    public void wrongpassOffButton(){
        wrongmatchPassHolder.SetActive(false);
    }
    public void successMessageOffButton(){
            successMessageHolder.SetActive(false);
            createAccountHolder.SetActive(false);
            loginHolder.SetActive(true);
    }
}
