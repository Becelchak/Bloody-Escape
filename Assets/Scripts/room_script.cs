using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class room_script : MonoBehaviour
{
    public bool isFinalRoom;

    public GameObject door;

    public List<Collider2D> allNPCList;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        allNPCList.RemoveAll(item => item == null);
        if (allNPCList.Count == 0)
            door.SetActive(false);
    }

}
