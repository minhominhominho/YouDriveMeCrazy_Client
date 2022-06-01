using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginSceneButton : MonoBehaviour
{
    public GameObject createAccountHolder;
    public GameObject loginHolder;
    public GameObject dialogHolder;
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
    
}
