using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public static Dictionary<int, Interactable> Interactables = new Dictionary<int, Interactable>();
    private static int nextInteractibleID = 1;

    public int InteractibleID;
    public bool InteractedWith = false;

    private void Start()
    {
        InteractedWith = false;
        InteractibleID = nextInteractibleID;
        nextInteractibleID++;
        Interactables.Add(InteractibleID, this);
    }

    private IEnumerator Interact(Player _player) {
        yield return new WaitForSeconds(2f);
        InteractedWith = true;
        ServerSend.InteractibleTouched(InteractibleID);
        _player.startMoving();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !InteractedWith) {
            Player _player = other.GetComponent<Player>();
            _player.stopMoving();
            StartCoroutine(Interact(_player));
            
        }

    }
}
