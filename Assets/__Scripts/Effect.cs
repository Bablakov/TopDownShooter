using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    private float lifeTime;

    private void Start()
    {
        lifeTime = GetComponent<ParticleSystem>().startLifetime;
    }

    private void Update()
    {
        if (lifeTime <= 0)
            Destroy(gameObject);
        else
            lifeTime -= Time.deltaTime;
    }
}
