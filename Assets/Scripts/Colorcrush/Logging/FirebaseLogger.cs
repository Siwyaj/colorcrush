using System.Collections.Generic;
using UnityEngine;
using Firebase.Firestore;
using Firebase.Extensions;
using Colorcrush.Game;

public class FirebaseLogger : MonoBehaviour
{
    private const string UserIdKey = "user_id";
    private static string userId;
    private static FirebaseFirestore firestore;
    public static Color currentColorDatabase;

    private void Awake()
    {
        InitializeFirebase();
    }

    private void InitializeFirebase()
    {
        firestore = FirebaseFirestore.DefaultInstance;
        GetOrCreateUserId();
    }

    public static void CreateUserInDatabase()
    {
        Debug.Log("Creating user in Firestore...");

        Dictionary<string, object> userData = new Dictionary<string, object>
        {
            { "userId", userId },
            { "createdAt", System.DateTime.UtcNow.ToString("o") }
        };

        DocumentReference userDoc = firestore.Collection("users").Document(userId);
        userDoc.SetAsync(userData).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError($"Failed to create user: {task.Exception}");
            }
            else
            {
                Debug.Log($"User {userId} successfully created in Firestore.");
                CreateColorInDatabase();
            }
        });
    }

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

    public void WriteDemographicDataToDatabase(Dictionary<string, string> demographicData)
    {
        Debug.Log("Writing demographic data to Firestore...");

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
    }

    public static void AppendColorData(string logData)   
    {

        Debug.Log("Appending color log data to Firestore...");
        string colorName = ColorUtility.ToHtmlStringRGB(currentColorDatabase);

        Dictionary<string, object> logDataWithTime = new Dictionary<string, object>
        {
            { "logs", FieldValue.ArrayUnion(logData) },
            { "timestamp", System.DateTime.UtcNow.ToString("o") }
        };
        DocumentReference colorDocRef = firestore
        .Collection("users")
        .Document(userId)
        .Collection("colors")
        .Document(colorName);

        colorDocRef.SetAsync(logDataWithTime, SetOptions.MergeAll).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
                Debug.LogError($"Failed to append log for {colorName}: {task.Exception}");
            else
                Debug.Log($"Log entry added for color {colorName}.");
        });
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
}
