using System;
using Cysharp.Threading.Tasks;
using ServiceLocatorPath;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class StateOfGame : MonoBehaviour, IMediator
{
    [SerializeField] private TextMeshProUGUI debugging;
    [SerializeField] GameObject m_PlacedPrefab;
    [SerializeField] private ShooterToEnemies shooter;
    [SerializeField] private Camera camera;
    [SerializeField] private Animator anim_tracking, anim_make3D, anim_look_around;
    [SerializeField] private Button butonRespawn;
    private EnemyStatesConfiguration _enemyStatesConfiguration;
    private IMediadorAR _ar;
    private bool _buclePrincipal;
    private bool hasWait;
    private bool canUse;
    private Vector2 positionToClick;
    private bool _hasClick;
    private int _vidaHeal;
    private EscenarioInteractivo escenarioInteractivo;
    private bool canRespawn, isShowAnimations;


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
        Write($"Configurado");
        
        ServiceLocator.Instance.GetService<ILoadScream>().Open(() => { });
        
        anim_tracking.gameObject.SetActive(false);
        anim_make3D.gameObject.SetActive(false);
        anim_look_around.gameObject.SetActive(false);
        butonRespawn.gameObject.SetActive(false);
    }

    public async void LoadScene(int sceneIndex)
    {
        ServiceLocator.Instance.GetService<ILoadScream>().Close(() =>
        {
            _ar.ResetSession();
            Destroy(escenarioInteractivo.gameObject);
            SceneManager.LoadScene(sceneIndex, LoadSceneMode.Single);
        }).Forget();
    }
    public void Respawn()
    {
        canRespawn = true;
        //Debug.Log("respawn = true");
    }
    
    private void ColocarVida(int _vida)
    {
        _vidaHeal = _vida;
        RedibujarVida();
    }

    private void RedibujarVida()
    {
        //vida.text = $"{_vidaHeal}";
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
                escenarioInteractivo = _ar.InstantiateObjectInRaycast(GetPositionInWord(), m_PlacedPrefab).GetComponent<EscenarioInteractivo>();
                escenarioInteractivo.Configuracion(camera, this, _ar.GetPlayer());
            }
            action?.Invoke();
            return true;
        }
        catch (Exception e)
        {
            Write($"Error - {e.Message}");
            return false;
        }
    }

    public void ConfiguraEnemigoSpawner()
    {
        //spawnerEnemy.Configuration(this);
    }

    public void RestarVida(int vidaToRestar)
    {
        _vidaHeal -= vidaToRestar;
        RedibujarVida();
    }

    public bool HasWait()
    {
        return hasWait;
    }

    public void StopSpawnAndDestroidAll()
    {
        //spawnerEnemy.StopSpawn();
        //spawnerEnemy.CleanEnemies();
    }

    public void ShowTheGameOver()
    {
        //panelToGameOver.SetActive(true);        
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
        anim_tracking.gameObject.SetActive(true);
        await UniTask.Delay(TimeSpan.FromMilliseconds(5000));
        anim_tracking?.gameObject.SetActive(false);
        if(inGame) return;
        anim_make3D?.gameObject.SetActive(true);
    }

    private bool inGame;
    public async UniTaskVoid ShowLookAroundAnimator()
    {
        if(isShowAnimations) return;
        inGame = true;
        anim_tracking.gameObject.SetActive(false);
        anim_make3D.gameObject.SetActive(false);
        await UniTask.Delay(TimeSpan.FromMilliseconds(200));
        anim_look_around.gameObject.SetActive(true);
        await UniTask.Delay(TimeSpan.FromMilliseconds(5000));
        anim_look_around?.gameObject.SetActive(false);
        isShowAnimations = true;
    }

    public void ShowButtonRespawn()
    {
        butonRespawn.gameObject.SetActive(true);
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
        anim_look_around.gameObject.SetActive(false);
    }

    public void StopAudioGeneral()
    {
        escenarioInteractivo.StopAudioGeneral();
    }

    public void StartSessionOfAR()
    {
        //_ar.StartSession();
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
}