using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCursor : MonoBehaviour
{
    [SerializeField] GameObject cursor;

    [SerializeField] List<GameObject> refColorObject;

    private float timeInterval = 0.5f;
    private float startTime;
    private float currentTime;
    private int index = 0;


    private void Awake()
    {
        startTime = Time.time;
        cursor.transform.position = refColorObject[index].transform.position;
    }
    private void FixedUpdate()
    {
        currentTime = Time.time;
        Debug.Log(currentTime - startTime);
        if (Mathf.Abs(currentTime - startTime) > timeInterval)
        {
            index += 1;
            if (index == refColorObject.Count)
            {
                index = 0;
            }
            cursor.transform.position = refColorObject[index].transform.position + new Vector3(0.2f, -0.5f, 0f);
            startTime = Time.time;

        }

    }
}
