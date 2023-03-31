using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitGameScript : MonoBehaviour
{
    public Animator fadeAnimator;

    private void OnTriggerEnter(Collider controller)
    {
        if (controller.name != "LeftHand Controller" && controller.name != "RightHand Controller")
            return;

        StartCoroutine(QuitGame());
    }

    IEnumerator QuitGame()
    {
        fadeAnimator.SetTrigger("FadeOut");
        yield return new WaitForSeconds(2);
        Debug.Log("GAME STOPPED!");
        Application.Quit();
    }

}
