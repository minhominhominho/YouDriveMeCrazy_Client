using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginSceneButton : MonoBehaviour
{
    public GameObject createAccountHolder;
    public GameObject loginHolder;
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
}
