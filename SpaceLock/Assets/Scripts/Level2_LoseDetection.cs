using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level2_LoseDetection : MonoBehaviour
{
    public GameObject player;
    public Canvas cv;
    public ScreenFlickerController screenFlickerController;

    private float wall1Z = -105.5f;
    private float wall2Z = 82.4f;
    public float loseDistanceThreshold = 0.5f;


    void FixedUpdate()
    {
        if(player == null) {  return; }
        float playerZ = player.transform.position.z;

        float distanceToLeftWall = Mathf.Abs(playerZ - wall1Z);
        float distanceToRightWall = Mathf.Abs(playerZ - wall2Z);

        // Debug.Log($"Player Z position: {playerZ}, Distance to Left Wall: {distanceToLeftWall}, Distance to Right Wall: {distanceToRightWall}");

        if (distanceToLeftWall < loseDistanceThreshold || distanceToRightWall < loseDistanceThreshold)
        {
            Debug.Log("Player is near a wall.");
            screenFlickerController.StopFlickering();
            cv.PlayerLose(1);
            return;  // Early exit after detecting a loss condition
        }
    }
}
