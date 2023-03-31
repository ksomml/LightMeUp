using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackToSpawnScript : MonoBehaviour
{
    public static bool isTriggered = false;

    public GameObject spawnRoom;
    public GameObject player;
    public Animator fadeAnimator;

    private void OnTriggerEnter(Collider controller)
    {
        if (controller.name != "LeftHand Controller" && controller.name != "RightHand Controller")
            return;

        isTriggered = true;
        StartCoroutine(FadingTeleportToSpawnRoom());
    }

    private void OnTriggerExit(Collider controller)
    {
        if (controller.name != "LeftHand Controller" && controller.name != "RightHand Controller")
            return;

        isTriggered = false;
    }

    IEnumerator FadingTeleportToSpawnRoom()
    {
        fadeAnimator.SetTrigger("FadeOut");
        yield return new WaitForSeconds(1);
        staticTeleporter.SendPlayerToSpawn(player, spawnRoom);
        fadeAnimator.ResetTrigger("FadeOut");
        fadeAnimator.SetTrigger("FadeIn");
        yield return new WaitForSeconds(1);
        fadeAnimator.ResetTrigger("FadeIn");
    }
}
