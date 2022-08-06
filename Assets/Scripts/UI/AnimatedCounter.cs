using DG.Tweening;
using TMPro;
using UnityEngine;

namespace UI
{
    public class AnimatedCounter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _textCounter;

        private Tween _counterAnimation;

        public void StartCount(uint secondCount)
        {
            uint secondsLeft = secondCount;
            _counterAnimation = DOTween.Sequence()
                .Append(_textCounter.transform.DOScale(Vector3.one, .75f)
                    .From(Vector3.zero)
                    .SetEase(Ease.OutBounce))
                .Append(_textCounter.transform.DOScale(Vector3.zero, .25f).SetEase(Ease.Linear))
                .SetLoops(-1)
                .OnStart(() =>
                {
                    _textCounter.text = secondsLeft.ToString();
                })
                .OnStepComplete(() =>
                {
                    --secondsLeft;
                    _textCounter.text = secondsLeft.ToString();
                })
                .OnUpdate(() =>
                {
                    if (secondsLeft == 0)
                        _counterAnimation.Kill();
                });
        }
    }
}