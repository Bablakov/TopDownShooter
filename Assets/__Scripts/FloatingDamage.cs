using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Для отображение полученого урона врагом
public class FloatingDamage : MonoBehaviour
{
    public GameObject destroyGameObject;
    [HideInInspector] public float damage;
    private TextMesh textMesh;
/*    private float liveTime = 3f;*/

    void Start()
    {
        textMesh = GetComponent<TextMesh>();
        textMesh.text = "-" + damage;
    }

    // Для уничтожение объекта вылетающего урона
/*    private void Update()
    {
        if (liveTime <= 0)
            Destroy(destroyGameObject);
        else 
            liveTime = -Time.deltaTime;
    }*/

    // Для уничтожение вылетающего урона в анимации
    public void OnAnimationOver()
    {
        Destroy(destroyGameObject);
    }
}
