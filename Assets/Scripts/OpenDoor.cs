using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    public GameObject[] comps;
    private bool doorcheck;
    void Update()
    {
        
        for (int i = 0; i < comps.Length; i++) {
            if (!comps[i].GetComponent<Interactable>().InteractedWith) {
                doorcheck = false;
                break;
            }
            doorcheck = true;
        }
        if (doorcheck) {
            Destroy(gameObject);
        }
    }
}
