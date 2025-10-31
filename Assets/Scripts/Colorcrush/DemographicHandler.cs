using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Colorcrush.Logging;
using TMPro;
using Colorcrush.Util;

public class DemographicHandler : MonoBehaviour
{
    [Header("Age objects")]
    [SerializeField] private TMP_InputField ageInputField;

    [Header("Sex object")]
    [SerializeField] private TMP_Dropdown sexDropdown;


    [Header("Eye color object")]
    [SerializeField] private TMP_InputField eyeColor;

    [Header("Type of colorblindness")]
    [SerializeField] private TMP_Dropdown colorblindnessDropdown;


    [Header("Other vision deficiencies")]
    [SerializeField] private TMP_InputField visionDeficiency;

    private FirebaseLogger logger;
    private Dictionary<string, string> demographicData = new Dictionary<string, string>();

    public void SubmitDemographics()
    {

        logger = FindObjectOfType<FirebaseLogger>();


        string age = ageInputField.text;
        string sex = sexDropdown.options[sexDropdown.value].text;
        string eye_color = eyeColor.text;
        string colorblindness = colorblindnessDropdown.options[colorblindnessDropdown.value].text;
        string other_vision_deficiency = visionDeficiency.text;

        Debug.Log("DemographicHandler: Age: " + age);
        Debug.Log("DemographicHandler: Sex: " + sex);
        Debug.Log("DemographicHandler: Eye Color: " + eye_color);
        Debug.Log("DemographicHandler: Colorblindness: " + colorblindness);
        Debug.Log("DemographicHandler: Other Vision Deficiency: " + other_vision_deficiency);

        demographicData["age"] = age;
        demographicData["biologicalSex"] = sex;
        demographicData["eyeColor"] = eye_color;
        demographicData["colorBlindness"] = colorblindness;
        demographicData["otherVisionDeficiency"] = other_vision_deficiency;
        logger.WriteDemographicDataToDatabase(demographicData);

        SceneManager.LoadSceneAsync("TutorialScene", SceneManager.ActivateLoadedScene);

    }
}
