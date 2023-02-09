using UnityEngine;

public class ClickShooter : ShooterToEnemies
{
    
    protected override bool TryGetTouchPosition()
    {
        return _mediator.HasClickInScream();
    }

    protected override void FixUpdate()
    {
        //logica del raycast
        RaycastHit[] hits;
        var posicion = transform.position;
        // Does the ray intersect any objects excluding the player layer
        var _mousePos = _mediator.GetMousePositionInScream();
        _mousePos.z = 2;
        _mediator.Write($"{_mousePos} mouse");
        hits = Physics.RaycastAll(_mediator.GetCamera().transform.position, (_mediator.GetCamera().ScreenToWorldPoint(_mousePos) - _mediator.GetCamera().transform.position), Mathf.Infinity);
        _mediator.Write($"{_mediator.GetCamera().ScreenToWorldPoint(_mousePos)} screanToWord");
        Debug.DrawRay(_mediator.GetCamera().transform.position, ((_mediator.GetCamera().ScreenToWorldPoint(_mousePos) - _mediator.GetCamera().transform.position)) * 100, Color.yellow);
        foreach (var hit in hits)
        {
            _mediator.Write($"{hit.collider.gameObject.name} colisiono");
            if (hit.collider.gameObject.TryGetComponent<ObjetoClickeable>(out var e))
            {
                e.Click();
                break;
            }
        }
    }
}