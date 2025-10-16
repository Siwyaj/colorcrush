using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Colorcrush.Game;


public class FireBaseInit : MonoBehaviour
{
    public static FirebaseDatabase Database;
    private static FireBaseInit _instance;
    public static bool isReady = false;


    private void Awake()
    {
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject); //Keeps it alive between scenes
                InitializeFirebase();
                isReady = true;
                CreateOrFindLogger();

            }
            else
            {
                Destroy(gameObject); //Prevent duplicates if scene reloads
            }
        }
    }


    private void InitializeFirebase()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                FirebaseApp app = FirebaseApp.DefaultInstance;
                Database = FirebaseDatabase.DefaultInstance;
                Debug.Log("Firebase is initialized and persistent across scenes!");
            }
            else
            {
                Debug.LogError($"Could not resolve Firebase dependencies: {dependencyStatus}");
            }
        });
    }

    
    private void CreateOrFindLogger()
    {
        // Check if there’s already a logger in the scene
        FirebaseLogger logger = FindObjectOfType<FirebaseLogger>();

        if (logger == null)
        {
            // 🔹 Create a new GameObject and attach the script
            GameObject loggerObj = new GameObject("FirebaseLogger");
            logger = loggerObj.AddComponent<FirebaseLogger>();
            DontDestroyOnLoad(loggerObj);
            Debug.Log("FirebaseLogger created automatically.");
        }
        else
        {
            // 🔹 Just make sure it persists
            DontDestroyOnLoad(logger.gameObject);
            Debug.Log("Existing FirebaseLogger found and marked DontDestroyOnLoad.");
        }

        //FirebaseLogger.dbRef = FirebaseDatabase.DefaultInstance.RootReference;
    }

}
