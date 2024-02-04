using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������������ ���� ���������� ��������� ������ � ����
// ������������ ������ ��� ��������� �������
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


    private void Start()
    {
        StartCoroutine(RandomSpawner());
    }

    // ��������� ��������� ����� � ������ �� �����
    IEnumerator RandomSpawner()
    {
        yield return new WaitForSeconds(5f);
        int last = rooms.Count - 1;
        while (rooms[last].GetComponent<AddRoom>().door == null)
            last--;
        AddRoom lastRoom = rooms[rooms.Count - 1].GetComponent<AddRoom>();

        // �������� ������� �������
        Vector3 positionBlueGun = lastRoom.transform.position;
        // ��������� ������� �����, ������� �������������� ����� �� �������������� ������� � �������� �� 2,
        // ��� ��� ����� � ����� ������(����� ���������� � ��������������) ����� ����� ���������� ������ � ����� �����������,
        // �� � ������ ����� ���������� �������� ����� � �����
        positionBlueGun += new Vector3((lastRoom.door.transform.position.x - lastRoom.transform.position.x) * 2, (lastRoom.door.transform.position.y - lastRoom.transform.position.y) * 2);

        Instantiate(key, rooms[Random.Range(1, rooms.Count - 3)].transform.position, Quaternion.identity);
        Instantiate(gun, positionBlueGun, Quaternion.identity);

        lastRoom.door.SetActive(true);
    }
}
