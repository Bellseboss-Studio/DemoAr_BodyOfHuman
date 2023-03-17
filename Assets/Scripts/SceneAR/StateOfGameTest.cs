using System;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using Input = UnityEngine.Windows.Input;

namespace SceneAR
{
    public class StateOfGameTest : StateOfGame
    {
        [SerializeField] GameObject placedPrefab;
        private EnemyStatesConfiguration _newEnemyStatesConfiguration;
        private IMediadorAR _newAr;
        private bool _newBuclePrincipal;
        private bool _hasWait;
        private bool _canUse;
        private bool _newHasClick;
        private int _newVidaHeal;
        private bool _canRespawn, _isShowAnimations;
        private bool _inGame;
        private Vector2 _positionToClick;


        

        private void Update()
        {
            if (Keyboard.current.f1Key.wasPressedThisFrame)
            {
                Write($"instancia");
                if (escenarioInteractivo == null)
                {
                    escenarioInteractivo = Instantiate(placedPrefab);
                }
                OnInstantiateElement?.Invoke();
            }
            if (!_canUse) return;
            if (!_newHasClick && _newAr.Touch())
            {
                _newHasClick = true;
                _positionToClick = _newAr.GetMousePosition();
            }
        }

        private async void StartState(IEnemyState state, object data = null)
        {
            while (_newBuclePrincipal)
            {
                var resultData = await state.DoAction(data);
                var nextState = _newEnemyStatesConfiguration.GetState(resultData.NextStateId);
                state = nextState;
                data = resultData.ResultData;
            }
        }


        private void OnDestroy()
        {
            _newBuclePrincipal = false;
            _hasWait = true;
            //this.GetCancellationTokenOnDestroy();
        }

        private void OnDisable()
        {
            _newBuclePrincipal = false;
            _hasWait = true;
        }
    }
}