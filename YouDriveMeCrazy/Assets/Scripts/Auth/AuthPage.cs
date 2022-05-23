using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.SceneManagement;
public class AuthPage : MonoBehaviour
{
    [Header("LoginPanel")]
   [SerializeField] public TMP_InputField IDinput;
   [SerializeField] public TMP_InputField Passinput;
    //계정 생성창의 인풋
    [Header("CreateAccountPanel")]
   [SerializeField] public TMP_InputField NewIDinput;
  [SerializeField]  public TMP_InputField NewPassinput;
  [SerializeField]  public TMP_InputField NewCheckPassinput;
    [SerializeField]private string loginUrl = "http://localhost:8080/ymmc/auth/login/";

    private string createUrl = "http://localhost:8080/ymmc/auth/add";
    // public GameObject[] uis;
    // public GameObject backLoginObj;
    // public Text massage;
    // public Text countText;
    // public Text passwordText;

    private const string email = "id";
    private const string password = "password";
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void LoginButton(){
        StartCoroutine(LoginCo());
    }
    [System.Serializable]
    public class User
    {
        //public int id;
        public string email;
        public string password;
    }
    IEnumerator LoginCo(){
        //Debug.Log(IDinput.text);
       // Debug.Log(Passinput.text);
       //웨이팅 스크린에서 인풋 받지말고 여기서 저장받은 이메일 값 바로 넘겨주고 그 화면에는 넘겨받은 이메일 값 출력하게 하자
       Debug.Log(IDinput.text);
       Debug.Log(Passinput.text);
       User loginUser = new User();
       loginUser.email = IDinput.text;
       loginUser.password = Passinput.text;
       string userJson = JsonUtility.ToJson(loginUser);
       Debug.Log(userJson);
        string loginUrll = loginUrl+IDinput.text + "/"+ Passinput.text;
        Debug.Log(loginUrll);
        UnityWebRequest webRequest =  UnityWebRequest.Get(loginUrll);
      
        yield return webRequest.SendWebRequest();
        var textt = webRequest.downloadHandler.text;
        Debug.Log(textt);
        if(textt.Equals("true")){
            SceneManager.LoadScene("Title");
            //게임매니져 변수에 접근해서 저장하기
        }
        
       // User user = JsonUtility.FromJson<User>(textt);
        //Debug.Log(user.email);

    }
    IEnumerator CreateCo(){
        //Debug.Log(IDinput.text);
       // Debug.Log(Passinput.text);
       Debug.Log(NewIDinput.text);
       Debug.Log(NewPassinput.text);
    //    WWWForm form = new WWWForm();
    //     form.AddField(email, NewIDinput.text);
    //     form.AddField(password, NewPassinput.text);
    User createUser = new User(); 
        //createUser.id = 15;
       createUser.email = NewIDinput.text;
       createUser.password = NewPassinput.text;
       string createUserJson = JsonUtility.ToJson(createUser);

        using(UnityWebRequest webRequest =  UnityWebRequest.Post(createUrl,createUserJson)){
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(createUserJson);
            webRequest.uploadHandler = new UploadHandlerRaw(jsonToSend);
            webRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");

        //응답대기
        yield return webRequest.SendWebRequest();
        //에러처리
        if(webRequest.result != UnityWebRequest.Result.Success) {
            Debug.Log(webRequest.error);
            Debug.Log("뭔가 이상해");
        }
        else {
            Debug.Log("Form upload complete!");
            Debug.Log(createUserJson);
            //
            //저장완료시 로그인창으로 넘어가도록
            //LoginSceneButton.LoginScene();
            
        }
        }
        
       //응답대기
        // yield return webRequest;
        // //에러처리
        // if(webRequest.result != UnityWebRequest.Result.Success) {
        //     Debug.Log(webRequest.error);
        //     Debug.Log("뭔가 이상해");
        // }
        // else {
        //     Debug.Log("Form upload complete!");
        //     //저장완료시 로그인창으로 넘어가도록
        //     //LoginSceneButton.LoginScene();
            
        // }
    }
    public void CreateAccountButton(){

        if(NewPassinput.text.Equals(NewCheckPassinput.text)){
            StartCoroutine(CreateCo());
        }else{
            Debug.Log("user enter different password");
        }
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
