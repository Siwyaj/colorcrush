using Colorcrush.Game;
using Colorcrush.Logging;
using Colorcrush.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static Colorcrush.Util.ShaderManager;

public class TutorialHandler : MonoBehaviour
{

    static bool isTutorial = true;

    [Header("Tutorial stages")]
    [SerializeField] GameObject tutorialEmptyGameobject;
    [SerializeField] GameObject welcome;
    [SerializeField] GameObject clickToSelect;
    [SerializeField] GameObject clickToConfirm;
    [SerializeField] GameObject clickToDeselect;
    [SerializeField] GameObject nextLevel;
    [SerializeField] GameObject spiderWeb;

    //Colors for select tutorial
    [Header("Buttons to select")]
    [SerializeField] GameObject colorButton5;
    [SerializeField] GameObject colorButton10;
    [SerializeField] GameObject colorButton4;

    //The rest of the colors needs to be target color
    [Header("The rest of the colors")]
    [SerializeField] GameObject colorButton1;
    [SerializeField] GameObject colorButton2;
    [SerializeField] GameObject colorButton3;
    [SerializeField] GameObject colorButton6;
    [SerializeField] GameObject colorButton7;
    [SerializeField] GameObject colorButton8;
    [SerializeField] GameObject colorButton9;
    [SerializeField] GameObject colorButton11;
    [SerializeField] GameObject colorButton12;

    //Commit button
    [Header("Commit button")]
    [SerializeField] GameObject commitButton;


    //Cursor
    [Header("Cursor")]
    [SerializeField] GameObject cursor;


    //GameSceneController
    [Header("GameSceneController")]
    [SerializeField] GameObject gameSceneController;


    Color tutorialTargetColor = new Color(189f / 255f, 121f / 255f, 117f / 255f);
    Color tutorialDifferentColor = new Color(255f / 255f, 121f / 255f, 111f / 255f);


    public static int tutorialProgress = 0;


    private void Start()
    {

        Debug.Log("ran");
        NextTutorialStep();
        SetAllColorsBase();

        SetShaderColor(colorButton5, "_TargetColor", tutorialDifferentColor);
        SetShaderColor(colorButton10, "_TargetColor", tutorialDifferentColor);
        SetShaderColor(colorButton4, "_TargetColor", tutorialDifferentColor);
        colorButton5.GetComponent<Button>().interactable = true;
        colorButton10.GetComponent<Button>().interactable = true;
        colorButton4.GetComponent<Button>().interactable = true;
    }
    private void SetAllColorsBase()
    {
        SetShaderColor(colorButton1, "_TargetColor", tutorialTargetColor);
        SetShaderColor(colorButton2, "_TargetColor", tutorialTargetColor);
        SetShaderColor(colorButton3, "_TargetColor", tutorialTargetColor);
        SetShaderColor(colorButton4, "_TargetColor", tutorialTargetColor);
        SetShaderColor(colorButton5, "_TargetColor", tutorialTargetColor);
        SetShaderColor(colorButton6, "_TargetColor", tutorialTargetColor);
        SetShaderColor(colorButton7, "_TargetColor", tutorialTargetColor);
        SetShaderColor(colorButton8, "_TargetColor", tutorialTargetColor);
        SetShaderColor(colorButton9, "_TargetColor", tutorialTargetColor);
        SetShaderColor(colorButton10, "_TargetColor", tutorialTargetColor);
        SetShaderColor(colorButton11, "_TargetColor", tutorialTargetColor);
        SetShaderColor(colorButton12, "_TargetColor", tutorialTargetColor);

        colorButton1.GetComponent<Button>().interactable = false;
        colorButton2.GetComponent<Button>().interactable = false;
        colorButton3.GetComponent<Button>().interactable = false;
        colorButton4.GetComponent<Button>().interactable = false;
        colorButton5.GetComponent<Button>().interactable = false;
        colorButton6.GetComponent<Button>().interactable = false;
        colorButton7.GetComponent<Button>().interactable = false;
        colorButton8.GetComponent<Button>().interactable = false;
        colorButton9.GetComponent<Button>().interactable = false;
        colorButton10.GetComponent<Button>().interactable = false;
        colorButton11.GetComponent<Button>().interactable = false;
        colorButton12.GetComponent<Button>().interactable = false;


    }
    public void TutorialStartButton()
    {
        if (tutorialProgress == 0)
        {
            NextTutorialStep();
        }
    }
    public void TutorialReferenceColorButton()
    {
        if (tutorialProgress == 1)
        {
            NextTutorialStep();
        }
    }

    public void TutorialConfirmButton()
    {
        NextTutorialStep();
            //gameSceneController.GetComponent<GameSceneController>().TutorialEndColorExperiment(); //worked
            /*
            //if it did not work, add the line above to the CommitButton2nd(); and uncomment this block
            commitButton.GetComponent<Button>().onClick.AddListener(() =>
            {
                CommitButton2nd();
        });*/
        
    }

    public void DeselectbuttonClicked()
    {
        NextTutorialStep();
        
    }
    
    void TutorialDeselectStep()
    {
        //Make one color normal again and add listener to it
        SetShaderColor(colorButton10, "_TargetColor", tutorialTargetColor);
        this.gameObject.GetComponent<MoveCursor>().SetCurserToChangedColor(colorButton10);
        colorButton10.GetComponent<Button>().onClick.AddListener(() =>
        {
            DeselectbuttonClicked();
        });
    }

    public void CommitButton2nd()
    {
        //End stage or set batch to 0 in gamescenecontroller/colormanager
    }
    
    public void DeselectButton()
    {
        NextTutorialStep();
    }

    public void NextTutorialStep()
    {
        tutorialProgress += 1;

        switch (tutorialProgress)
        {
            case 1:
                //introduction
                Debug.Log("Tutrial manager: introduction");
                SetAllInactive();
                welcome.SetActive(true);
                break;
            case 2:
                //Select 3 colors
                Debug.Log("Tutrial manager: press 3 colors");
                SetAllInactive();
                cursor.SetActive(true);
                clickToSelect.SetActive(true);
                commitButton.GetComponent<Button>().interactable = false;
                break;
            case 3:
                //Confirm
                Debug.Log("Tutrial manager: press confirm");
                SetAllInactive();
                this.GetComponent<MoveCursor>().SetCurserToCommitButton(commitButton);
                commitButton.GetComponent<Button>().interactable = true;
                break;
            case 4:
                //select 1 color (deselect tutorial)
                Debug.Log("Tutrial manager: Select 1 color");
                SetAllInactive();
                clickToDeselect.SetActive(true);

                colorButton10.GetComponent<Button>().onClick.AddListener(() =>
                {
                    DeselectButton();
                });
                commitButton.GetComponent<Button>().interactable = false;

                StartCoroutine(TempName());

                break;
            case 5:
                Debug.Log("Tutrial manager: deselect 1 color");
                //deselect 1
                //Actually nothing to do here
                break;
            case 6:
                Debug.Log("Tutrial manager: select color again");
                //select 1 (it's still different)
                //Actually nothing to do here
                break;
            case 7:
                //confirm
                Debug.Log("Tutrial manager: press confirm");
                colorButton10.GetComponent<Button>().interactable = false;
                commitButton.GetComponent<Button>().interactable = true;
                this.GetComponent<MoveCursor>().SetCurserToCommitButton(commitButton);
                break;

            case 8:
                //Tiny explenation
                Debug.Log("Tutrial manager: tiny explenation");
                SetAllInactive();
                cursor.SetActive(false);
                nextLevel.SetActive(true);
                break;

            case 9:
                //Spiderweb
                Debug.Log("Tutrial manager: Spiderweb");
                SetAllInactive();
                spiderWeb.SetActive(true);
                break;
            case 10://if needed

                Debug.Log("Tutrial manager: load menu");
                LoggingManager.isTutorial = false;
                SceneManager.LoadSceneAsync("MenuScene", SceneManager.ActivateLoadedScene);
                break;
            default:
                SetAllInactive();
                break;
        }



    }

    IEnumerator TempName()
    {
        Debug.Log("Coroutine started");
        yield return new WaitForSeconds(2f);
        Debug.Log("Coroutine pause over");
        this.GetComponent<MoveCursor>().SetCurserToChangedColor(colorButton10);
        SetAllColorsBase();
        SetShaderColor(colorButton10, "_TargetColor", tutorialDifferentColor);
    }
    private void SetAllInactive()
    {
        welcome.SetActive(false);
        clickToSelect.SetActive(false);
        clickToDeselect.SetActive(false);
        clickToConfirm.SetActive(false);
        nextLevel.SetActive(false);
        spiderWeb.SetActive(false);
    }

}
