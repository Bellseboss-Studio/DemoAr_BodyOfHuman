using System;
using Cysharp.Threading.Tasks;
using UnityEngine.XR.ARFoundation;

public class ConfigurationOfGame : IEnemyState
{
    private readonly IMediator _mediator;

    public ConfigurationOfGame(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async UniTask<StateResult> DoAction(object data)
    {
        _mediator.Write("Revisar de nuevo si esta listo");
        await UniTask.Delay(TimeSpan.FromMilliseconds(100));
        _mediator.Write("Habilitando todo");
        //_mediator.StartSessionOfAR();
        _mediator.Write("Configuracion completada");
        return new StateResult(EnemyStatesConfiguration.WaitForClickInSpace);
    }
}