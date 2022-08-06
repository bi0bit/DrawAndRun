using System;
using UnityEditor;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(CapsuleCollider), typeof(Rigidbody))]
    public class Unit : MonoBehaviour
    {
        private static readonly int IdMovement = Animator.StringToHash("Movement");
        private static readonly int IdDeath = Animator.StringToHash("Death");
        private static readonly int IdFinalDance = Animator.StringToHash("Finish");

        public event Action<Unit> EventDeath;

        [SerializeField] private Animator _animator;
        private Rigidbody _rigidbody;
        private SphereCollider _collider;
        private Transform _center;

        private Vector3 _moveVector = Vector3.zero;
        private Vector3 _offsetMoveVector = Vector3.zero;

        public Vector3 MoveVector
        {
            get => _moveVector;
            set => _moveVector = value;
        }

        public Vector3 MoveOffset
        {
            get => _offsetMoveVector;
            set => _offsetMoveVector = value;
        }

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _collider = GetComponent<SphereCollider>();
        }

        public void SetCenter(Transform centeredTransform)
        {
            _center = centeredTransform;
        }

        public void Walk()
        {
            _animator.SetFloat(IdMovement, 0.5f);
        }

        public void Run()
        {
            _animator.SetFloat(IdMovement, 1f);
        }

        public void Dance()
        {
            _animator.SetTrigger(IdFinalDance);
        }

        public void Kill()
        {
            _animator.SetTrigger(IdDeath);
            EventDeath?.Invoke(this);
        }

        private void FixedUpdate()
        {
            var displacementFromOffset =
                (_center.position + _offsetMoveVector - transform.position) *
                Game.Instance.GamePlaySetting.SpeedReplaceUnit + _moveVector;
            _rigidbody.MovePosition(transform.position + displacementFromOffset * Time.deltaTime);
        }

        private void OnDrawGizmos()
        {
            if (_center == null) return;
            Gizmos.DrawWireSphere(_center.position + _offsetMoveVector, .1f);
        }
    }
}