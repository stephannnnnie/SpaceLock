using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Analytics;
using UnityEngine.Analytics;
using Unity.Profiling;
using System;

public class AnalyticsManager : MonoBehaviour
{
    public static AnalyticsManager Instance;
    private int totalPowerUpsCollected = 0; // Counter for the total number of power-ups collected
    private string sessionId;

    private void Awake(){
        // Ensure that only one instance of AnalyticsManager exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist the AnalyticsManager across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate AnalyticsManager if one already exists
        }
    }
    // Start is called before the first frame update
    private async void Start()
    {
        await UnityServices.InitializeAsync();
        AnalyticsService.Instance.StartDataCollection();

        // Generate a unique session ID for the current game session using System.Guid
        sessionId = Guid.NewGuid().ToString(); // Generate a unique identifier
        Debug.Log("Session ID: " + sessionId);
    }

    public void NextLevel(int currentLevel){
        CustomEvent myEvent = new CustomEvent("next_level")
        {
            {"level_index", currentLevel}
        };
        AnalyticsService.Instance.RecordEvent(myEvent);
        AnalyticsService.Instance.Flush();
        Debug.Log("Analytics next_level sent");
    }

    // Function to track total number of power-ups acquired
    public void TrackPowerUpAcquired()
    {
        totalPowerUpsCollected++; // Increment the counter when a power-up is collected

        // Log the number of power-ups collected in the custom event
        CustomEvent powerUpEvent = new CustomEvent("power_up_acquired")
        {
            { "session_id", sessionId }, // Attach session ID to event
            { "total_power_ups_collected", totalPowerUpsCollected } // Track total number of power-ups collected
        };
        AnalyticsService.Instance.RecordEvent(powerUpEvent);
        AnalyticsService.Instance.Flush();
        Debug.Log("Analytics power_up_acquired sent with session ID: " + sessionId + ". Total power-ups collected: " + totalPowerUpsCollected);
    }
}
