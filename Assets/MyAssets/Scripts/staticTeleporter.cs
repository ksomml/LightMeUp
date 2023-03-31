using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class staticTeleporter
{
    public static bool playerIsAtSpawn = false;
    public Animator animator;

    public static void SendPlayerToSpawn(GameObject player, GameObject spawnRoom)
    {
        spawnRoom.SetActive(true);
        player.transform.position = spawnRoom.transform.Find("SPAWN_ROOM_SPAWN").GetComponent<Transform>().transform.position;
        playerIsAtSpawn = true;
    }

    public static void SendPlayerToRoom(GameObject player, GameObject spawnRoom, GameObject targetSpawn)
    {
        spawnRoom.SetActive(false);
        player.transform.position = targetSpawn.transform.position;
        playerIsAtSpawn = false;
    }

}
