using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialHandler : MonoBehaviour
{

    [SerializeField] static bool tutorialTaken = false;
    [SerializeField] GameObject tutorialEmptyGameobject;
    [SerializeField] GameObject welcome;
    [SerializeField] GameObject clickToSelect;
    [SerializeField] GameObject clickToConfirm;
    [SerializeField] GameObject nextLevel;

    public static int tutorialProgress = 0;

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
        if (tutorialProgress > 0)
        {
            NextTutorialStep();
        }
    }

    public void NextTutorialStep()
    {
        tutorialProgress += 1;

        switch (tutorialProgress)
        {
            case 0:
                welcome.SetActive(true);
                clickToSelect.SetActive(false);
                clickToConfirm.SetActive(false);
                nextLevel.SetActive(false);
                break;
            case 1:
                welcome.SetActive(false);
                clickToSelect.SetActive(true);
                clickToConfirm.SetActive(false);
                nextLevel.SetActive(false);
                break;
            case 2:
                welcome.SetActive(false);
                clickToSelect.SetActive(false);
                clickToConfirm.SetActive(true);
                nextLevel.SetActive(false);
                break;
            case 3:
                welcome.SetActive(false);
                clickToSelect.SetActive(false);
                clickToConfirm.SetActive(false);
                nextLevel.SetActive(true);
                break;
            default:
                welcome.SetActive(false);
                clickToSelect.SetActive(false);
                clickToConfirm.SetActive(false);
                nextLevel.SetActive(false);
                tutorialTaken = true;
                break;
        }

    }
}
