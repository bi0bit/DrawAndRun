using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "GamePlaySetting", menuName = "Assets/Gameplay/GameplaySetting", order = 0)]
    public class GamePlaySetting : ScriptableObject
    {
        [SerializeField] private float _levelTime;
        [SerializeField] private float _safeForRun;
        [SerializeField] private uint _startFromUnit;
        [SerializeField] private uint _secondForReady;
        [SerializeField] private float _speedReplaceUnit;



        public float LevelTime => _levelTime;

        public float SafeForRun => _safeForRun;

        public uint StartFromUnit => _startFromUnit;

        public uint SecondForReady => _secondForReady;
        
        public float SpeedReplaceUnit => this._speedReplaceUnit;
    }
}
