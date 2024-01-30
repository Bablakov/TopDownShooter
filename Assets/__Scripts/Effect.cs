using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// ��� ����������� �������� ����� �� ������������
public class Effect : MonoBehaviour
{
    private float lifeTime;

    private void Start()
    {
        lifeTime = GetComponent<ParticleSystem>().main.startLifetimeMultiplier;
    }

    private void Update()
    {
        if (lifeTime <= 0)
            Destroy(gameObject);
        else
            lifeTime -= Time.deltaTime;
    }
}
