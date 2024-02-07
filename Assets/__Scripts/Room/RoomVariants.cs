using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ѕеречесление всех имеиющихс€ вариантов комнат в игре
// »спользуетс€ дальше дл€ генерации локации
public class RoomVariants : MonoBehaviour
{
    #region Initialization of the field
    [Header("Rooms")]
    public GameObject[] topRooms;
    public GameObject[] bottomRooms;
    public GameObject[] leftRooms;
    public GameObject[] rightRooms;
    public GameObject closedRoom;
    public List<GameObject> rooms;

    [Header("Bonus")]
    public GameObject key;
    public GameObject gun;

    [Header("Spawn")]
    public float waitTime = 5f;
    public GameObject Boss;
    public Slider sliderBoss;

    private AddRoom lastRoom;
    #endregion

    private void Start()
    {
        StartCoroutine(RandomSpawner());
    }

    // –андомна€ генераци€ ключа и оружи€ на карте
    IEnumerator RandomSpawner()
    {
        yield return new WaitForSeconds(5f);
        int last = rooms.Count - 1;
        while (rooms[last].GetComponent<AddRoom>().door == null)
            last--;
        lastRoom = rooms[last].GetComponent<AddRoom>();

        // ѕолучаем позицию комнаты
        Vector3 positionBlueGun = lastRoom.transform.position;
        // ¬ычесл€ем позицию двери, вычита€ местоположени€ двери от местоположени€ комнаты и умножаем на 2,
        // так как дверь в таком случае(после вычислени€ еЄ местоположени€) будет иметь координаты только в одном направлении,
        // то и оружие бедут спавнитьс€ напротив двери с босом
        positionBlueGun += new Vector3((lastRoom.door.transform.position.x - lastRoom.transform.position.x) * 2, (lastRoom.door.transform.position.y - lastRoom.transform.position.y) * 2);

        lastRoom.BossLevel = Boss;
        lastRoom.BossSlider = sliderBoss;
        lastRoom.RoomBoss = true; 
        
        // Create key and blueGun on level
        Instantiate(key, rooms[Random.Range(1, rooms.Count - 3)].transform.position, Quaternion.identity);
        Instantiate(gun, positionBlueGun, Quaternion.identity);

        lastRoom.door.SetActive(true);
    }
}
