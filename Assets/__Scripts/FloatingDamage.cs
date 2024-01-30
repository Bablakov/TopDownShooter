using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ��� ����������� ���������� ����� ������
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

    // ��� ����������� ������� ����������� �����
/*    private void Update()
    {
        if (liveTime <= 0)
            Destroy(destroyGameObject);
        else 
            liveTime = -Time.deltaTime;
    }*/

    // ��� ����������� ����������� ����� � ��������
    public void OnAnimationOver()
    {
        Destroy(destroyGameObject);
    }
}
