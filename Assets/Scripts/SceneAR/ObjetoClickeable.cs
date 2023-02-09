using UnityEngine;
using UnityEngine.Events;

public class ObjetoClickeable : MonoBehaviour
{
    public UnityEvent UnityAction;

    public void Click()
    {
        UnityAction?.Invoke();
    }
}
