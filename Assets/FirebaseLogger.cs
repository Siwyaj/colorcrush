using Firebase.Firestore;
using Firebase.Extensions;
using System.Collections.Generic;
using UnityEngine;


public class FirebaseLogger : MonoBehaviour
{
    private FirebaseFirestore _db;
    private string _participantId;

    public FirebaseLogger()
    {
        _db = FirebaseFirestore.DefaultInstance;
        _participantId = SystemInfo.deviceUniqueIdentifier; // or your participant ID
    }

    public void SendParticipantData(
        string name,
        string participantId,
        string age,
        string gender,
        string eyeColor,
        string visionDeficiency,
        bool colorblind,
        bool isPrivate,
        string deviceInfo,
        Dictionary<string, List<string>> colorEvents
    )
    {
        // Build top-level document
        var doc = new Dictionary<string, object>
        {
            { "name", name },
            { "id", participantId },
            { "Participant Age", age },
            { "Biological Gender", gender },
            { "Eye Color", eyeColor },
            { "Vision Deficiency", visionDeficiency },
            { "Participant Colorblind", colorblind },
            { "private", isPrivate },
            { "device info", deviceInfo }
        };

        // Add nested color events
        foreach (var color in colorEvents)
        {
            var colorDict = new Dictionary<string, object>();
            for (int i = 0; i < color.Value.Count; i++)
            {
                colorDict.Add($"event{i + 1}", color.Value[i]);
            }

            doc.Add(color.Key, colorDict); // e.g., "Color1", "Color2"
        }

        // Save to Firestore
        _db.Collection("participants").Document(participantId)
            .SetAsync(doc)
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsCompletedSuccessfully)
                    Debug.Log($"✅ Uploaded participant {participantId} data to Firestore");
                else
                    Debug.LogError($"❌ Firebase upload failed: {task.Exception}");
            });
    }
}
