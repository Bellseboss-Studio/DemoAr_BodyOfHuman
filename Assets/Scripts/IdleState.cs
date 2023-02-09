using System;
using Cysharp.Threading.Tasks;

public class IdleState : IEnemyState
{
    private readonly float _secondsToWait;

    public IdleState(float secondsToWait)
    {
        _secondsToWait = secondsToWait;
    }

    public async UniTask<StateResult> DoAction(object data)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(_secondsToWait));
        return new StateResult(EnemyStatesConfiguration.FindTargetState);
    }
}