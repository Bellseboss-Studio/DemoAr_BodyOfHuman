using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInGame : MonoBehaviour
{
    public delegate void OnEnemyDeleted();
    public OnEnemyDeleted OnEnemyDestroy;
    public OnEnemyDeleted OnEnemyDestroyLegal;
    private bool isInUse;
    private float _velocity;

    public void Configurate(GameObject model3D, float velocity)
    {
        var instantiate = Instantiate(model3D, transform);
        instantiate.transform.rotation = transform.rotation;
        isInUse = true;
        _velocity = velocity;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
        OnEnemyDestroy?.Invoke();
    }

    private void Update()
    {
        if (!isInUse) return;
        transform.Translate(Vector3.forward * _velocity * Time.deltaTime);
    }

    public void DestroyLogic()
    {
        Destroy(gameObject);
        OnEnemyDestroyLegal?.Invoke();
    }
}
