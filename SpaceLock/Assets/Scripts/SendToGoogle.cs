using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SendToGoogle : MonoBehaviour
{

    [SerializeField] private string URL;
    [SerializeField] Canvas cv;

    private long _sessionID;
    private int _noOfGrapples;
    private bool _testBool;
    private float _CompetionTime;
    private string Level_Name;
    private string _WonOrLose;
    private int powerups;

    private void Awake()
    {
        // Assign sessionID to identify playtests
        _sessionID = DateTime.Now.Ticks;
    }
    // Start is called before the first frame update
    public void Send(float completionTime, int Grapples, string Level , string WonORLose , int Powerups)
    {
        // Assign variables
        _noOfGrapples = Grapples;
        _CompetionTime = completionTime;
        Level_Name = Level;
        _WonOrLose = WonORLose;
        powerups = Powerups;

        StartCoroutine(Post(_sessionID.ToString(), _noOfGrapples.ToString(), _CompetionTime.ToString() , Level_Name.ToString() ,WonORLose.ToString(), powerups.ToString()  ));
        //Debug.LogWarning("wadawdwa : " + _noOfGrapples);
        //Debug.LogWarning("gdrgdrgrdg : " + _CompetionTime);
    }
    private IEnumerator Post(string sessionID, string testInt, string testFloat ,string Level , string WonOrLose , string Powerups)
    {
        // Create the form and enter responses
        WWWForm form = new WWWForm();
        form.AddField("entry.2137090667", sessionID);
        form.AddField("entry.56667914", testInt);
        form.AddField("entry.1338668664", testFloat);
        form.AddField("entry.1811587901", Level);
        form.AddField("entry.387450459", WonOrLose);
        form.AddField("entry.1936283403", Powerups);

        // Send responses and verify result
        using (UnityWebRequest www = UnityWebRequest.Post(URL, form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Form upload complete!");
            }
        }
    }


}
