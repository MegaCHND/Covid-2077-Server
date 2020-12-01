using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public static Dictionary<int, Enemy> enemies = new Dictionary<int, Enemy>();
    private static int nextEnemyId = 1;

    public Transform[] wayPoints;
    public float[] waitTimes;
    public int speed;
    public int id;
    public EnemyState state;

    private int wayPointIndex;
    private bool goingForward;
    private int currentSpeed;
    private float dist;
    private float timer;

    void Start()
    {
        id = nextEnemyId;
        nextEnemyId++;
        enemies.Add(id, this);
        ServerSend.spawnEnemy(this);
        wayPointIndex = 0;
        currentSpeed = speed;
        transform.LookAt(wayPoints[wayPointIndex].position);
        state = EnemyState.patrol;
    }

    private void FixedUpdate()
    {
        switch (state) {
            case EnemyState.patrol:
                dist = Vector3.Distance(transform.position, wayPoints[wayPointIndex].position);
                if (dist < .5f)
                {
                    state = EnemyState.idle;
                    timer = waitTimes[wayPointIndex];
                    currentSpeed = 0;
                }
                patrol();
                break;
            case EnemyState.idle:
                if (timer > 0)
                {
                    timer -= Time.deltaTime;
                }
                else
                {
                    NextIndex();
                    currentSpeed = speed;
                    state = EnemyState.patrol;
                }
                break;
            case EnemyState.inspect:
                if (timer > 0)
                {
                    timer -= Time.deltaTime;
                }
                break;
        }
    }

    private void patrol() {
        transform.LookAt(wayPoints[wayPointIndex].position);
        transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime);
        ServerSend.EnemyPos(this);
    }

    void NextIndex() {
        if (goingForward)
        {
            wayPointIndex++;
            if (wayPointIndex >= wayPoints.Length)
            {
                wayPointIndex--;
                goingForward = false;
            }
        }

        if (!goingForward)
        {
            wayPointIndex--;
            if (wayPointIndex < 0)
            {
                wayPointIndex++;
                goingForward = true;
            }
        }
    }

    public enum EnemyState{
        idle,
        patrol,
        inspect
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Door")) {
            state = EnemyState.inspect;
            timer = 10.711f;
            other.gameObject.transform.Rotate(0, 0, 90, Space.Self);
        }
    }
}
