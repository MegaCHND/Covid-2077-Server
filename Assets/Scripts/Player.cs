using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int id;
    public string username;

    private float moveSpeed = 5f / Constants.TICKS_PER_SEC;
    private bool[] inputs;
    [SerializeField]
    private bool canMove = true;

    public void Initialize(int _id, string _username)
    {
        id = _id;
        username = _username;

        inputs = new bool[3];
    }

    /// <summary>Processes player input and moves the player.</summary>
    public void FixedUpdate()
    {
        Vector2 _inputDirection = Vector2.zero;
        if (inputs[0])
        {
            _inputDirection.y += 1;
        }
        if (inputs[1])
        {
            _inputDirection.y -= 1;
        }

        Move(_inputDirection);
    }

    /// <summary>Calculates the player's desired movement direction and moves him.</summary>
    /// <param name="_inputDirection"></param>
    private void Move(Vector2 _inputDirection)
    {
        /*Vector3 _moveDirection = transform.right * _inputDirection.x + transform.forward * _inputDirection.y;
        transform.position += _moveDirection * moveSpeed;*/
        if (canMove) {
            Vector3 forwardFace = transform.forward;
            forwardFace.y = 0;
            forwardFace = forwardFace.normalized;
            transform.position += (forwardFace * _inputDirection.y) * moveSpeed;
        }

        ServerSend.PlayerPosition(this);
        ServerSend.PlayerRotation(this);
    }

    /// <summary>Updates the player input with newly received input.</summary>
    /// <param name="_inputs">The new key inputs.</param>
    /// <param name="_rotation">The new rotation.</param>
    public void SetInput(bool[] _inputs, Quaternion _rotation)
    {
        inputs = _inputs;
        transform.rotation = _rotation;
    }

    public void stopMoving()
    {
        canMove = false;
    }
    public void startMoving() {
        canMove = true;
    }
}
