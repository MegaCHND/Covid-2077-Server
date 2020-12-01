using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public static Dictionary<int, Interactable> Interactables = new Dictionary<int, Interactable>();
    private static int nextInteractibleID = 1;

    public int InteractibleID;
    public bool InteractedWith = false;
    public Transform telepoint;
    public Transform returnPoint;
    public GameObject Door;
    [SerializeField]
    private Player _player;

    private void Start()
    {
        InteractedWith = false;
        InteractibleID = nextInteractibleID;
        nextInteractibleID++;
        Interactables.Add(InteractibleID, this);
    }

    private void Update()
    {
        if (_player != null) {
            if (_player.getE())
            {
                if (gameObject.CompareTag("Vent") && !InteractedWith)
                {
                    _player.stopMoving();
                    StartCoroutine(Interact());
                }
                else if (gameObject.CompareTag("Vent") && InteractedWith)
                {
                    _player.gameObject.transform.position = telepoint.position;
                    ServerSend.PlayerPosition(_player);
                }
                else if (gameObject.CompareTag("Comp") && !InteractedWith)
                {
                    _player.stopMoving();
                    StartCoroutine(Interact());
                }
                else if (gameObject.CompareTag("Door") && !InteractedWith)
                {
                    Door.GetComponent<Transform>().Rotate(0, 0, -90, Space.Self);
                    Door.GetComponent<BoxCollider>().enabled = true;
                    InteractedWith = true;
                    ServerSend.InteractibleTouched(InteractibleID);
                }
                else if (gameObject.CompareTag("Locker") && !InteractedWith)
                {
                    _player.gameObject.transform.position = telepoint.position;
                    _player.stopMoving();
                    InteractedWith = true;
                    ServerSend.PlayerPosition(_player);
                    ServerSend.InteractibleTouched(InteractibleID);
                }
                else if (gameObject.CompareTag("Locker") && InteractedWith)
                {
                    _player.gameObject.transform.position = returnPoint.position;
                    _player.startMoving();
                    InteractedWith = false;
                    ServerSend.PlayerPosition(_player);
                    ServerSend.InteractibleUnTouched(InteractibleID);
                }
            }
        }
    }

    private IEnumerator Interact() {
        yield return new WaitForSeconds(8f);
        InteractedWith = true;
        ServerSend.InteractibleTouched(InteractibleID);
        _player.startMoving();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !InteractedWith && _player == null) {
            _player = other.GetComponent<Player>();  
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _player = null;
        }
    }
    public void setIw(bool input) {
        InteractedWith = input;
    }
    public int getInteractibleID() {
        return InteractibleID;
    }
}
