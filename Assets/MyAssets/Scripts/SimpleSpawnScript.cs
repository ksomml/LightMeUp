using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleSpawnScript : MonoBehaviour
{
    // for debugging purposes only

    public bool playerSpawnsAtSpawn;
    public bool keepDebuggerLighting;
    public GameObject player;
    public GameObject spawnRoom;

    void Awake()
    {
        if (!keepDebuggerLighting)
        {
            gameObject.transform.Find("DebugLighting").GetComponent<Light>().enabled = false;
            Debug.Log("triggered");
        }


        if (playerSpawnsAtSpawn)
            staticTeleporter.SendPlayerToSpawn(player, spawnRoom);
    }

}
