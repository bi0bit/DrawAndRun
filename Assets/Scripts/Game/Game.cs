using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Game
{
    public class Game : MonoBehaviour
    {
        public static Game Instance { get; private set; }

        [SerializeField] private Transform _targetFinish;
        
        [SerializeField] private GamePlaySetting _gamePlaySetting;
        [SerializeField] private Counter _pointCounter;

        [SerializeField] private UnityEvent<uint> _eventRunPrepare;
        [SerializeField] private UnityEvent _eventRunStart;
        [SerializeField] private UnityEvent _eventFinishRun;
        [SerializeField] private UnityEvent _eventGameOver;

        [SerializeField] private UnityEvent<uint> _pointUpdate;
        
        public GamePlaySetting GamePlaySetting => _gamePlaySetting;
        public Counter PointCounter => _pointCounter;

        public Vector3 FinishPosition => _targetFinish.position;
        
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
                return;
            }
            Instance = this;
            _pointCounter.EventUpdateCount += InvokeUpdateCounter;
            _eventRunPrepare?.Invoke(_gamePlaySetting.SecondForReady);
            InvokeUpdateCounter(_pointCounter.Count);
            StartCoroutine(DelayRun());
        }

        private void InvokeUpdateCounter(uint updateCount)
        {
            _pointUpdate.Invoke(updateCount);
        }

        private IEnumerator DelayRun()
        {
            yield return new WaitForSeconds(_gamePlaySetting.SecondForReady);
            _eventRunStart?.Invoke();
        }

        public void FinishRun()
        {
            _eventFinishRun?.Invoke();
        }

        public void CheckGameOver(uint countUnits)
        {
            if(countUnits <= 0)
                _eventGameOver?.Invoke();
        }
        
    }
}