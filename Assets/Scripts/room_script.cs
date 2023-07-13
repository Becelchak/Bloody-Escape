using System.Collections.Generic;
using UnityEngine;

public class Room_script : MonoBehaviour
{
    public bool isFinalRoom;

    public GameObject door;

    public List<Collider2D> allNPCList;

    public GameObject nextLevelStartedPoint;

    public GameObject player;

    void Update()
    {
        allNPCList.RemoveAll(item => item == null);
        for (var i = 0; i < allNPCList.Count; i++)
        {
            var enemy = allNPCList[i];
            allNPCList[i] = enemy.gameObject.GetComponent<Enemy_parameter>().EnemyAlive() ? enemy : null;
        }
        if (allNPCList.Count == 0)
            door.SetActive(false);
        // if final room level complete -> teleport player in next level
        if(isFinalRoom && allNPCList.Count == 0)
        {
            player.transform.position = nextLevelStartedPoint.transform.position;
            isFinalRoom = false;
        }
    }

}
