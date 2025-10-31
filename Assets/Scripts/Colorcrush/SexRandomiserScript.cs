using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SexRandomiserScript : MonoBehaviour
{

    List<string> sexList = new();
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.GetComponent<TMP_Dropdown>().ClearOptions();
        sexList.Add("");
        if (Random.Range(0, 2) == 0)//Female first, to be 0 was chosen by a female
        {
            Debug.Log("SexRandomiserScript: Female was chosen first");
            sexList.Add("Female");
            sexList.Add("Male");
        }
        else //Male first
        {
            Debug.Log("SexRandomiserScript: Male was chosen first");
            sexList.Add("Male");
            sexList.Add("Female");
        }
        sexList.Add("Prefer not to answer");

        this.gameObject.GetComponent<TMP_Dropdown>().AddOptions(sexList);
    }
}
