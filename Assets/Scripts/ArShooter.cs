using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ArShooter : ShooterToEnemies
{
    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();
    protected override bool TryGetTouchPosition()
    {
        //_mediator.Write("Click en AR");
        return _mediator.HasClickInScream();//Input.touchCount > 0;
    }

    protected override void FixUpdate()
    {
        _mediator.Write("Touch");
        var _mousePos = _mediator.GetMousePositionInScream();
        var touchPosition = _mousePos;//Input.GetTouch(0).position;
        var ray = _mediator.GetSessionOrigin().ScreenPointToRay(touchPosition);
        var allCol = Physics.RaycastAll(ray);
        foreach (var hit in allCol)
        {
            _mediator.Write($"{hit.collider.gameObject.name} colisiono");
            _mediator.Write($"{touchPosition} touchPosition");
            if (hit.collider.gameObject.TryGetComponent<ObjetoClickeable>(out var e))
            {
                e.Click();
                break;
            }
        }
    }
}