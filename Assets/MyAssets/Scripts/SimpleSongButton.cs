using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleSongButton : MonoBehaviour
{
    public static int songNumber = 0;
    public static bool isTriggered;

    public int number;
    public AudioClip song;
    public GameObject player;
    public GameObject spawnRoom;
    public GameObject mainRoomSpawn;
    public Animator fadeAnimator;

    private int totalHandsInside = 0;

    private void OnTriggerEnter(Collider controller)
    {
        if (controller.name != "LeftHand Controller" && controller.name != "RightHand Controller")
            return;

        totalHandsInside += 1;
        isTriggered = true;
        songNumber = number;
        StartCoroutine(FadingTeleportToMainRoom());

    }

    private void OnTriggerExit(Collider controller)
    {
        if (controller.name != "LeftHand Controller" && controller.name != "RightHand Controller")
            return;

        totalHandsInside -= 1;

        if (totalHandsInside == 0)
        {
            isTriggered = false;
        }
    }

    IEnumerator FadingTeleportToMainRoom()
    {
        fadeAnimator.SetTrigger("FadeOut");
        yield return new WaitForSeconds(1);
        staticTeleporter.SendPlayerToRoom(player, spawnRoom, mainRoomSpawn);
        fadeAnimator.ResetTrigger("FadeOut");
        fadeAnimator.SetTrigger("FadeIn");
        yield return new WaitForSeconds(1);
        fadeAnimator.ResetTrigger("FadeIn");
    }
}
