using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Colorcrush.Logging;
using TMPro;

using Colorcrush.Util;

public class FirstTimeLaunchManager : MonoBehaviour
{
    [SerializeField] private Image ConsentImage;
    [SerializeField] private Image demographicImage;
    private Dictionary<string, string> demographicData = new Dictionary<string, string>();
    [SerializeField] private TMP_InputField ageInputField;
    [SerializeField] private TMP_InputField bGenderField;
    [SerializeField] private TMP_InputField eyeColor;
    [SerializeField] private TMP_InputField visionDeficiency;
    [SerializeField] private TMP_InputField colorBlind;


    FirebaseLogger logger;

    public void ConsentAllowButton()
    {
        logger = FindObjectOfType<FirebaseLogger>();
        Debug.Log("User consented to data collection.");
        ConsentImage.gameObject.SetActive(false);
        demographicImage.gameObject.SetActive(true);
        logger.GetOrCreateUserId();

    }

    public void ConsentDenyButton()
    {
        Debug.Log("User denied consent. Exiting application.");
        Application.Quit();
    }
    public void SubmitDemographicDataButton()
    {
        demographicData["age"] = ageInputField.text;
        demographicData["biologicalGender"] = bGenderField.text;
        demographicData["eyeColor"] = eyeColor.text;
        demographicData["visionDeficiency"] = visionDeficiency.text;
        demographicData["colorBlind"] = colorBlind.text;
        logger.WriteDemographicDataToDatabase(demographicData);
        demographicImage.gameObject.SetActive(false);
        SceneManager.LoadSceneAsync("TutorialScene", SceneManager.ActivateLoadedScene);
    }
}
