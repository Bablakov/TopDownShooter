using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Перечесление всех имеиющихся вариантов комнат в игре
// Используется дальше для генерации локации
public class RoomVariants : MonoBehaviour
{
    public GameObject[] topRooms;
    public GameObject[] bottomRooms;
    public GameObject[] leftRooms;
    public GameObject[] rightRooms;

    public GameObject closedRoom;
    public GameObject key;
    public GameObject gun;

    public List<GameObject> rooms;

    public float waitTime;
    private bool spawnedBoss;
    public GameObject Boss;

    /*private void Update()
    {
        if (waitTime <= 0 && !spawnedBoss)
        {
            for (int i = 0; i < rooms.Count; i++)
            {
                if (i == rooms.Count - 1)
                {
                    Instantiate(Boss, rooms[i].transform.position, Quaternion.identity);
                    spawnedBoss = true;
                }
            }
        }
        else
        {
            waitTime -= Time.deltaTime;
        }
    }*/

    /*[HideInInspector] public List<GameObject> rooms;

    private void Start()
    {
        StartCoroutine(RandomSpawner());
    }

    // Рандомная генерация ключа и оружия на карте
    IEnumerator RandomSpawner()
    {
        yield return new WaitForSeconds(5f);
        AddRoom lastRoom = rooms[rooms.Count - 1].GetComponent<AddRoom>();
        int rand = Random.Range(0, rooms.Count - 2);

        Instantiate(key, rooms[rand].transform.position, Quaternion.identity);
        Instantiate(gun, rooms[rooms.Count - 2].transform.position, Quaternion.identity);

        lastRoom.door.SetActive(true);
        foreach (var room in rooms)
        {
            room.GetComponent<AddRoom>().CheckEmptyWay();
        }

    }*/
}
