using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class myNameText : MonoBehaviour
{
    public TextMeshProUGUI myName;
    // Start is called before the first frame update
    void Start()
    {
        myName.text = SavingData.myName;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
