#if !UNITY_WEBGL || UNITY_EDITOR
using Firebase.Firestore;
using Firebase.Extensions;
#endif

using System;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class FirebaseLogger : MonoBehaviour
{
    private const string UserIdKey = "user_id";
    private static string userId;
    public static Color currentColorDatabase;
#if !UNITY_WEBGL || UNITY_EDITOR
    private static FirebaseFirestore firestore;
#endif
    private void Awake()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        GetOrCreateUserId();
#else
        InitializeFirebase();
#endif
        }

    private void InitializeFirebase()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        firestore = FirebaseFirestore.DefaultInstance;
#endif
        GetOrCreateUserId();
    }

    public static void CreateUserInDatabase()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        JS_CreateUser(userId);
#else
#if !UNITY_WEBGL || UNITY_EDITOR
        //Debug.Log("Creating user in Firestore...");

        Dictionary<string, object> userData = new Dictionary<string, object>
        {
            { "userId", userId },
            { "createdAt", System.DateTime.UtcNow.ToString("o") }
        };

        firestore.Collection("users").Document(userId).SetAsync(userData).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
                Debug.LogError($"Failed to create user: {task.Exception}");
            else
                Debug.Log($"User {userId} successfully created in Firestore.");
        });
#endif
#endif
    }
    /*
    private static void CreateColorInDatabase()
    {

        CollectionReference colorsRef = firestore
            .Collection("users")
            .Document(userId)
            .Collection("colors");

        foreach (Color color in ColorManager.TargetColors)
        {
            string colorString = ColorUtility.ToHtmlStringRGB(color);
            Dictionary<string, object> colorData = new Dictionary<string, object>
            {
                { "createdAt", System.DateTime.UtcNow.ToString("o") }
            };

            colorsRef.Document(colorString).SetAsync(colorData).ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                    Debug.LogError($"Failed to create color {colorString}: {task.Exception}");
                else
                    Debug.Log($"Color {colorString} created for user {userId}.");
            });
        }
    }
    */

    public void WriteDemographicDataToDatabase(Dictionary<string, string> demographicData)
    {
        Debug.Log("Writing demographic data to Firestore...");

#if UNITY_WEBGL && !UNITY_EDITOR
        // Convert dictionary to JSON for JavaScript bridge
        string jsonData = JsonUtility.ToJson(new SerializableDictionary<string, string>(demographicData));
        JS_WriteDemographicData(userId, jsonData);
#else
#if !UNITY_WEBGL || UNITY_EDITOR
    firestore.Collection("users")
        .Document(userId)
        .Collection("demographics")
        .AddAsync(demographicData)
        .ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
                Debug.LogError($"Failed to write demographic data: {task.Exception}");
            else
                Debug.Log($"Demographic data for user {userId} successfully written to Firestore.");
        });
#endif
#endif
    }
    [System.Serializable]
    public class SerializableDictionary<TKey, TValue>
    {
        public List<TKey> keys = new List<TKey>();
        public List<TValue> values = new List<TValue>();

        public SerializableDictionary(Dictionary<TKey, TValue> dict)
        {
            foreach (var kvp in dict)
            {
                keys.Add(kvp.Key);
                values.Add(kvp.Value);
            }
        }
    }


    public static void AppendColorData(string logData)   
    {

        //Debug.Log("Appending color log data to Firestore...");
        string colorName = ColorUtility.ToHtmlStringRGB(currentColorDatabase);
#if UNITY_WEBGL && !UNITY_EDITOR
        JS_AppendColorLog(userId, colorName, logData);
#else
#if !UNITY_WEBGL || UNITY_EDITOR
        Dictionary<string, object> update  = new Dictionary<string, object>
        {
            { "logs", FieldValue.ArrayUnion(logData) },
            { "timestamp", System.DateTime.UtcNow.ToString("o") }
        };
        firestore.Collection("users").Document(userId)
            .Collection("colors").Document(colorName)
            .SetAsync(update, SetOptions.MergeAll).ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                    Debug.LogError($"Failed to append log for {colorName}: {task.Exception}");
                else
                    Debug.Log($"Log entry added for color {colorName}.");
            });
#endif
#endif
    }
    public void GetOrCreateUserId()
    {
        if (!PlayerPrefs.HasKey(UserIdKey))
        {
            string newId = System.Guid.NewGuid().ToString();
            PlayerPrefs.SetString(UserIdKey, newId);
            PlayerPrefs.Save();
            Debug.Log($"Created new user ID: {newId}");
            userId = newId;
        }
        else
        {
            userId = PlayerPrefs.GetString(UserIdKey);
            Debug.Log($"Loaded existing user ID: {userId}");
        }

        CreateUserInDatabase();
    }
#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void JS_CreateUser(string userId);

    [DllImport("__Internal")]
    private static extern void JS_AppendColorLog(string userId, string colorName, string logData);

    [DllImport("__Internal")]
    private static extern void JS_WriteDemographicData(string userId, string jsonData);

#endif
}
