using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    public Direction direction;

    // ѕеречесление всех вариантов возможных спавн поинтов 
    public enum Direction
    {
        Top,
        Bottom,
        Left,
        Right,
        None
    }
    private RoomVariants variants;
    private int rand;
    private bool spawned = false;
    private static float waitSpawn = 1f;

    private void Start()
    {
        variants = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomVariants>();
        if (direction != Direction.None)
        {
            Destroy(gameObject, waitSpawn);
            Invoke("Spawn", 0.5f);
        }
    }

    // ќтвечает за спавн комнаты
    public void Spawn()
    {
        if (!spawned)
        {
            if (direction == Direction.Top)
            {
                rand = Random.Range(0, variants.topRooms.Length);
                Instantiate(variants.topRooms[rand], transform.position, variants.topRooms[rand].transform.rotation);
                spawned = true;
            }
            else if (direction == Direction.Bottom)
            {
                rand = Random.Range(0, variants.bottomRooms.Length);
                Instantiate(variants.bottomRooms[rand], transform.position, variants.bottomRooms[rand].transform.rotation);
                spawned = true;
            }
            else if (direction == Direction.Left)
            {
                rand = Random.Range(0, variants.leftRooms.Length);
                Instantiate(variants.leftRooms[rand], transform.position, variants.leftRooms[rand].transform.rotation);
                spawned = true;
            }
            else if (direction == Direction.Right)
            {
                rand = Random.Range(0, variants.rightRooms.Length);
                Instantiate(variants.rightRooms[rand], transform.position, variants.rightRooms[rand].transform.rotation);
                spawned = true;
            }
        }
    }

    // ѕровер€ем нет ли на наших точках спавна уже заспавленные комнаты
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("RoomPoint"))
        {
            if (direction == Direction.None)
            {
                Destroy(collision.gameObject);
            }
            else if (collision.GetComponent<RoomSpawner>().spawned)
            {
                Destroy(gameObject);
            }
            else if (!collision.GetComponent<RoomSpawner>().spawned && collision.GetComponent<RoomSpawner>().direction != Direction.None)
            {
                Destroy(collision.gameObject);
                Destroy(gameObject, waitSpawn);
                Invoke("Spawn", 0.5f);
            }
        } 
    }
}
