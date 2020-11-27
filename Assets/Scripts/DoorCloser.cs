using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorCloser : MonoBehaviour
{
    [SerializeField]
    private GameObject Door;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {

        }
    }

    private void closeDoor()
    {
        Door.GetComponent<Transform>().Rotate(0, 0, -90, Space.Self);
        Door.GetComponent<BoxCollider>().enabled = true;
    }
}
