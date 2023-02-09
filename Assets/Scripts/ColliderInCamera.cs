using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderInCamera : MonoBehaviour
{
    private IMediator _mediator;
    public delegate void OnCollision();
    public OnCollision OnCollisionEnterDelegate;

    public void Configurate(IMediator mediator)
    {
        _mediator = mediator;
    }
    private void OnCollisionEnter(Collision collision)
    {
        _mediator.Write("Colisiono con un objeto!");
        OnCollisionEnterDelegate?.Invoke();
        Destroy(collision.gameObject, 2);
    }
}
