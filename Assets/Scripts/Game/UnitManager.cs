using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Dreamteck.Splines;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Game
{
    public class UnitManager : MonoBehaviour
    {
        [SerializeField] private Rigidbody _centerMove;
        [SerializeField] private UnityEvent<uint> _unitsCountsUpdate;

        private Vector3 _runVelocity;
        private List<Unit> _units;
        private uint _countUnits;

        private Vector3[] _offsetFromTarget;

        private uint CountUnits
        {
            get => _countUnits;
            set
            {
                _countUnits = value;
                _unitsCountsUpdate.Invoke(_countUnits);
            }
        }
        private Unit FirstInactive => _units.FirstOrDefault(unit => !unit.gameObject.activeSelf);
        private Unit FirstActive => _units.FirstOrDefault(unit => unit.gameObject.activeSelf);
        private List<Unit> AllActive => _units.FindAll(unit => unit.gameObject.activeSelf);

        private Coroutine proccessChangeWalkToRun;

        public Rigidbody CenterMove => _centerMove;
        public Vector3 RunVelocity => _runVelocity;
        
        private void Awake()
        {
            InitUnits();
        }

        private void FixedUpdate()
        {
            _centerMove.MovePosition(_centerMove.position + _runVelocity * Time.deltaTime);
        }

        private void InitUnits()
        {
            _units = GetComponentsInChildren<Unit>().ToList();
            _units.ForEach(unit =>
            {
                SubscribeOnEventsUnit(unit);
                unit.SetCenter(_centerMove.transform);
                unit.gameObject.SetActive(false);
            });
            CountUnits = Game.Instance.GamePlaySetting.StartFromUnit;
            for (var i = 0; i < Game.Instance.GamePlaySetting.StartFromUnit; i++)
            {
                _units[i].gameObject.SetActive(true);
            }

            SetRectangleOffset();
            RelocateUnits();
        }

        private void SubscribeOnEventsUnit(Unit unit)
        {
            unit.EventDeath += RemoveUnit;
        }

        private IEnumerator UpdateWalkToRun()
        {
            yield return new WaitForSeconds(Game.Instance.GamePlaySetting.SafeForRun);
            UpdateToRun();
        }

        private void UpdateToWalk()
        {
            AllActive.ForEach(unit =>
            {
                unit.MoveVector = _runVelocity;
                unit.Walk();
            });
        }

        private void UpdateToRun()
        {
            AllActive.ForEach(unit => { unit.Run(); });
        }

        private void UpdateToFinish()
        {
            AllActive.ForEach(unit =>
            {
                _runVelocity = Vector3.zero;
                unit.MoveVector = Vector3.zero;
                unit.Dance();
            });
        }

        private void RelocateUnits()
        {
            if (_countUnits > 0)
            {
                var units = AllActive;
                var pointsPerUnit = (float) _offsetFromTarget.Length / _countUnits;
                var iUnit = 0;
                for (var i = 0f; i < _offsetFromTarget.Length && iUnit < _countUnits; i += pointsPerUnit)
                {
                    units[iUnit++].MoveOffset = _offsetFromTarget[Mathf.FloorToInt(i)];
                }
            }
        }

        private void SetRectangleOffset()
        {
            int row = (int) (CountUnits / 5), col = 5;
            float colSpace = .4f, rowSpace = .4f;
            var offsets = new Vector3[row * col];
            var startPoint = _centerMove.position.normalized + Vector3.forward * ((rowSpace / 2 * row)) +
                             Vector3.left * ((colSpace / 2 * col));
            for (var i = 0; i < row; ++i)
            {
                for (var j = 0; j < col; ++j)
                {
                    offsets[i * col + j] = startPoint + Vector3.back * ((i + 1) * rowSpace) +
                                           Vector3.right * ((j + 1) * colSpace);
                }
            }

            _offsetFromTarget = offsets;
        }


        public void CurveUpdate(Vector3[] offsetPoints)
        {
            _offsetFromTarget = offsetPoints;
            RelocateUnits();
        }

        public void StartRun()
        {
            var distance = Vector3.Distance(Game.Instance.FinishPosition, _centerMove.position);
            _runVelocity = Vector3.forward * (distance / Game.Instance.GamePlaySetting.LevelTime);
            UpdateToWalk();
            proccessChangeWalkToRun = StartCoroutine(UpdateWalkToRun());
        }

        public void EndRun()
        {
            RelocateUnits();
            SetRectangleOffset();
            UpdateToFinish();
            StopCoroutine(proccessChangeWalkToRun);
        }

        public void RemoveUnit(Unit unit)
        {
            --CountUnits;
            unit.gameObject.SetActive(false);
            UpdateToWalk();
            StopCoroutine(proccessChangeWalkToRun);
            proccessChangeWalkToRun = StartCoroutine(UpdateWalkToRun());
        }

        public void AddUnit()
        {
            var unit = FirstInactive;
            if (unit == null) return;
            unit.gameObject.SetActive(true);
            unit.transform.position = FirstActive.transform.position;
            unit.MoveVector = _runVelocity;
            ++CountUnits;
            RelocateUnits();
        }

        private void OnDrawGizmos()
        {
            if (_offsetFromTarget?.Length > 0 && _centerMove != null)
            {
                var pastPoint = _offsetFromTarget[0];
                foreach (var point in _offsetFromTarget.Skip(1))
                {
                    Gizmos.DrawLine(_centerMove.position + pastPoint, _centerMove.position + point);
                    pastPoint = point;
                }
            }
        }
    }
}