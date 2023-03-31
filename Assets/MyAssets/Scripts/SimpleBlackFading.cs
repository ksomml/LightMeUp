using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleBlackFading : MonoBehaviour
{
    public Animator animator;

    public void FadeToSpawn (GameObject player, GameObject spawnRoom)
    {
        StartCoroutine(TeleportWithFading(player, spawnRoom));
    }

    public IEnumerator TeleportWithFading(GameObject player, GameObject spawnRoom)
    {
        animator.SetTrigger("FadeOut");
        yield return new WaitForSeconds(1);
        player.transform.position = spawnRoom.transform.Find("SPAWN_ROOM_SPAWN").GetComponent<Transform>().transform.position;
    }
}
