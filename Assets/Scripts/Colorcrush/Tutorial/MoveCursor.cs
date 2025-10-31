using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCursor : MonoBehaviour
{
    [SerializeField] GameObject cursor;
    [SerializeField] GameObject confirmButton;

    public List<GameObject> refColorObject;

    private float timeInterval = 1f;
    private float startTime;
    private float currentTime;
    private int index = 0;
    bool selectTutorial = true;
    Vector3 curserClickRotation = new Vector3(0, 0, 55f);

    Vector3 curserColorOffset = new Vector3(0.2f, -0.25f, -40f);
    private Vector3 curserRotation;

    private void Awake()
    {
        startTime = Time.time;
        cursor.transform.position = refColorObject[index].transform.position;
        curserRotation = curserRotation = cursor.transform.rotation.eulerAngles;
    }
    private void FixedUpdate()
    {
        currentTime = Time.time;
        //Debug.Log(currentTime - startTime);
        if (Mathf.Abs(currentTime - startTime) > timeInterval / 2f)
        {
            cursor.transform.rotation = Quaternion.Euler(curserClickRotation);

        }
        if (Mathf.Abs(currentTime - startTime) > timeInterval && refColorObject.Count != 0)
        {
            index = (index + 1) % refColorObject.Count;// index goes from 0 to 2 an cycles
            cursor.transform.position = refColorObject[index].transform.position + curserColorOffset;

        }
        if (Mathf.Abs(currentTime - startTime) > timeInterval)
        {
            cursor.transform.rotation = Quaternion.Euler(curserRotation);
            startTime = Time.time;
        }
    }
    public void ColorClicked(GameObject button)
    {
        if (selectTutorial)
        {
            if (refColorObject.Contains(button))
            {
                refColorObject.Remove(button);
            }
            else
            {
                refColorObject.Add(button);
            }

            if(refColorObject.Count == 0)
            {
                Debug.Log(this.gameObject.GetComponent<TutorialHandler>());
                this.gameObject.GetComponent<TutorialHandler>().NextTutorialStep();
                selectTutorial = false;
            }
        }
    }

    public void SetCurserToChangedColor(GameObject button)
    {
        cursor.transform.position = button.transform.position + curserColorOffset;
    }
    public void SetCurserToCommitButton(GameObject commitButton)
    {
        cursor.transform.position = confirmButton.transform.position+new Vector3(0f,0f,-40f);
    }
}
