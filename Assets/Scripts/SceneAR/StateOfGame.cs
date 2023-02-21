using System;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class StateOfGame : MonoBehaviour, IMediator
{
    [SerializeField] private TextMeshProUGUI debugging;
    [SerializeField] GameObject m_PlacedPrefab;
    [SerializeField] private ShooterToEnemies shooter;
    [SerializeField] private Camera camera;
    public Action OnInstantiateElement;
    private EnemyStatesConfiguration _enemyStatesConfiguration;
    private IMediadorAR _ar;
    private bool _buclePrincipal;
    private bool hasWait;
    private bool canUse;
    private bool _hasClick;
    private int _vidaHeal;
    private GameObject escenarioInteractivo;
    private bool canRespawn, isShowAnimations;
    private bool inGame;
    private Vector2 positionToClick;


    public void Configuracion(IMediadorAR ar)
    {
        _ar = ar;

        _ar.StartSession();

        
        _enemyStatesConfiguration = new EnemyStatesConfiguration();
        _enemyStatesConfiguration.AddInitialState(EnemyStatesConfiguration.ConfigurationOfGame, new ConfigurationOfGame(this));
        _enemyStatesConfiguration.AddState(EnemyStatesConfiguration.WaitForClickInSpace, new WaitForClick(this));
        _enemyStatesConfiguration.AddState(EnemyStatesConfiguration.Game, new Game(this));
        _enemyStatesConfiguration.AddState(EnemyStatesConfiguration.FinishGame, new FinishGame(this));
        _buclePrincipal = true;
        StartState(_enemyStatesConfiguration.GetInitialState());
        canUse = true;
        _vidaHeal = 1;
        Write($"Configurado");
    }
    
    private void Update()
    {
        if (!canUse) return;
        if (!_hasClick && _ar.Touch())
        {
            _hasClick = true;
            positionToClick = _ar.GetMousePosition();
        }
    }
    private async void StartState(IEnemyState state, object data = null)
    {
        while (_buclePrincipal)
        {
            var resultData = await state.DoAction(data);
            var nextState = _enemyStatesConfiguration.GetState(resultData.NextStateId);
            state = nextState;
            data = resultData.ResultData;
        }
    }
    
    
    private void OnDestroy()
    {
        _buclePrincipal = false;
        hasWait = true;
        //this.GetCancellationTokenOnDestroy();
    }

    private void OnDisable()
    {
        _buclePrincipal = false;
        hasWait = true;
    }

    public void Write(string text)
    {
        //Debug.Log(text);
        debugging.text += $"{text} \n";
    }

    public bool ShootRaycast(Action action)
    {
        try
        {
            Write($"instancia");
            if (escenarioInteractivo == null)
            {
                escenarioInteractivo = _ar.InstantiateObjectInRaycast(GetPositionInWord(), m_PlacedPrefab);
            }
            action?.Invoke();
            OnInstantiateElement?.Invoke();
            return true;
        }
        catch (Exception e)
        {
            Write($"Error - {e.Message}");
            return false;
        }
    }

    public bool HasWait()
    {
        return hasWait;
    }

    public void ConfigureShooter()
    {
        shooter.Configure(this);
    }

    public ARRaycastManager GetRayCastManager()
    {
        return _ar.GetRayCastManager();
    }

    public Camera GetSessionOrigin()
    {
        return _ar.GetSessionOrigin();
    }

    public void HideDebuggers()
    {
        _ar.HideDebuggers();
    }

    public Camera GetCamera()
    {
        return camera;
    }

    public Vector3 GetMousePositionInScream()
    {
        return _ar.GetMousePosition();
    }

    public async UniTaskVoid ShowTrackingAnimator()
    {
        if(isShowAnimations) return;
        await UniTask.Delay(TimeSpan.FromMilliseconds(500));
    }

    public async UniTaskVoid ShowLookAroundAnimator()
    {
        if(isShowAnimations) return;
        inGame = true;
        await UniTask.Delay(TimeSpan.FromMilliseconds(200));
        //await UniTask.Delay(TimeSpan.FromMilliseconds(5000));
        isShowAnimations = true;
    }
    public bool RespawnScene()
    {
        return canRespawn;
    }

    public void RestartAllObjectsToRespawn()
    {
        Write("Restart all");
        Destroy(escenarioInteractivo.gameObject);
        canRespawn = false;
    }

    public void StopAudioGeneral()
    {
    }

    public bool HasClickInScream()
    {
        var aux = _hasClick;
        _hasClick = false;
        return aux;
    }

    public Vector2 GetPositionInWord()
    {
        Write($"{_ar.GetMousePosition()} _ar.GetMousePosition()");
        return _ar.GetMousePosition();
    }

    public bool FinishGame()
    {
        return _vidaHeal <= 0;
    }

    public void Restart()
    {
        _buclePrincipal = false;
        canUse = false;
    }

    public GameObject GetObjectInstantiate()
    {
        return escenarioInteractivo;
    }
}