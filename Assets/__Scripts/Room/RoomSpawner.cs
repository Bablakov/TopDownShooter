using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Rendering;

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

    public float waitTime = 4f;

    private void Start()
    {
        Destroy(gameObject, waitTime);
        variants = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomVariants>();
        Invoke("Spawn", 0.1f);
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
            }
            else if (direction == Direction.Bottom)
            {
                rand = Random.Range(0, variants.bottomRooms.Length);
                Instantiate(variants.bottomRooms[rand], transform.position, variants.bottomRooms[rand].transform.rotation);
            }
            else if (direction == Direction.Left)
            {
                rand = Random.Range(0, variants.leftRooms.Length);
                Instantiate(variants.leftRooms[rand], transform.position, variants.leftRooms[rand].transform.rotation);
            }
            else if (direction == Direction.Right)
            {
                rand = Random.Range(0, variants.rightRooms.Length);
                Instantiate(variants.rightRooms[rand], transform.position, variants.rightRooms[rand].transform.rotation);
            }
            spawned = true;
        }
    }

    

    // ѕровер€ем нет ли на наших точках спавна уже заспавленные комнаты
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("SpawnPoint"))
        {
            if (!collision.GetComponent<RoomSpawner>().spawned && !spawned)
            {
                Instantiate(variants.closedRoom, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
            spawned = true;
        }
    }
}
