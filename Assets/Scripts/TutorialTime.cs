using System;
using Cysharp.Threading.Tasks;

public class TutorialTime : IEnemyState
{
    private readonly IMediator _mediator;

    public TutorialTime(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async UniTask<StateResult> DoAction(object data)
    {
        _mediator.HideDebuggers();
        await UniTask.Delay(TimeSpan.FromMilliseconds(100));
        while (!_mediator.FinishedOfViewTutorial())
        {
            await UniTask.Delay(TimeSpan.FromMilliseconds(100));
        }
        _mediator.HideTutorial();
        await UniTask.Delay(TimeSpan.FromMilliseconds(100));
        return new StateResult(EnemyStatesConfiguration.ConfigurationOfGame);
    }
}