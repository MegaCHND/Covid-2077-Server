using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public static Dictionary<int, Interactable> Interactables = new Dictionary<int, Interactable>();
    private static int nextInteractibleID = 1;

    public int InteractibleID;
    public int type;
    public bool InteractedWith = false;
    public Transform telepoint;
    public Transform returnPoint;
    public GameObject Door;
    private float WaitTimer = .653f;
    private float timer = 0;
    [SerializeField]
    private Player _player;
    [SerializeField]
    private Player _playerinLocker;

    private void Start()
    {
        InteractedWith = false;
        InteractibleID = nextInteractibleID;
        nextInteractibleID++;
        Interactables.Add(InteractibleID, this);
    }

    private void Update()
    {
        timer -= Time.deltaTime;
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
                    _playerinLocker = _player;
                    timer = WaitTimer;
                    InteractedWith = true;
                    ServerSend.PlayerPosition(_player);
                    ServerSend.InteractibleTouched(InteractibleID);
                }
            }
        }
        else if (_playerinLocker != null) {
            if (_playerinLocker.getE()) {
                if (gameObject.CompareTag("Locker") && InteractedWith && timer < 0)
                {
                    _playerinLocker.gameObject.transform.position = returnPoint.position;
                    _playerinLocker.startMoving();
                    InteractedWith = false;
                    ServerSend.PlayerPosition(_playerinLocker);
                    ServerSend.InteractibleUnTouched(InteractibleID);
                    _playerinLocker = null;
                }
            }
        }
    }

    private IEnumerator Interact() {
        yield return new WaitForSeconds(8f);
        InteractedWith = true;
        ServerSend.InteractibleTouchedOnce(InteractibleID, type);
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
