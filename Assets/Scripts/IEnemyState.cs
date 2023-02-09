using Cysharp.Threading.Tasks;

public interface IEnemyState
{
    UniTask<StateResult> DoAction(object data);
}