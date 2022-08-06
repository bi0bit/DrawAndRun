using System;
using DG.Tweening;
using UnityEngine;

namespace UI
{
    public class RotateObject : MonoBehaviour
    {
        [SerializeField] private Vector3 rotateDirection;
        private void Awake()
        {
            var tween = transform.DORotate(rotateDirection.normalized * 360, 7f, RotateMode.LocalAxisAdd)
                .SetEase(Ease.Linear)
                .SetLoops(-1, LoopType.Restart);
        }

        private void OnDestroy()
        {
            DOTween.Kill(transform);
        }
    }
}