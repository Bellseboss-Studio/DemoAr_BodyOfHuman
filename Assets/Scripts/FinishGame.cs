using System;
using Cysharp.Threading.Tasks;

public class FinishGame : IEnemyState
{
    private readonly IMediator _mediator;

    public FinishGame(IMediator mediator)
    {
        _mediator = mediator;
    }
    public async UniTask<StateResult> DoAction(object data)
    {
        _mediator.StopSpawnAndDestroidAll();
        _mediator.ShowTheGameOver();
        while (!_mediator.HasWait())
        {
            if (_mediator.RespawnScene())
            {
                _mediator.RestartAllObjectsToRespawn();
                return new StateResult(EnemyStatesConfiguration.ConfigurationOfGame);
            }
            await UniTask.Delay(TimeSpan.FromMilliseconds(100));
        }
        return new StateResult(EnemyStatesConfiguration.FinishGame);
    }
}