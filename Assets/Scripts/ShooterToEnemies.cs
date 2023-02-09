using System;
using UnityEngine;

public abstract class ShooterToEnemies : MonoBehaviour
{
    protected IMediator _mediator;
    protected bool canUse;

    public void Configure(IMediator mediator)
    {
        _mediator = mediator;
        canUse = true;
    }

    protected abstract bool TryGetTouchPosition();
    protected abstract void FixUpdate();

    void FixedUpdate()
    {
        if (!canUse || !TryGetTouchPosition()) return;
        FixUpdate();
    }
}