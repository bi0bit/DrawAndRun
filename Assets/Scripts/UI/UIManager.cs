using System;
using System.Numerics;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private Transform _viewCristalsCount;
        [SerializeField] private TextMeshProUGUI _textCristalsCount;
        [SerializeField] private TextMeshProUGUI _textUnitsCount;
        [SerializeField] private RectTransform _drawPanel;
        [SerializeField] private RectTransform _infoPanel;
        [SerializeField] private RectTransform _counter;
        
        public void HideUI()
        {
            _drawPanel.gameObject.SetActive(false);
            _infoPanel.gameObject.SetActive(false);
            _counter.gameObject.SetActive(true);
        }

        public void HideUIAnimation()
        {
            _drawPanel.DOMove(_drawPanel.transform.position + Vector3.down * 3.8f, .7f)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    _drawPanel.gameObject.SetActive(true);

                });
            _infoPanel.DOMove(_infoPanel.transform.position + Vector3.up * 2, .7f)
                .SetEase(Ease.Linear).OnComplete(() =>
                {
                    _infoPanel.gameObject.SetActive(false);
                });
        }

        public void ShowUI()
        {
            _drawPanel.gameObject.SetActive(true);
            _infoPanel.gameObject.SetActive(true);
            _counter.gameObject.SetActive(false);
            _drawPanel.DOMove(_drawPanel.transform.position, .5f)
                .From(_drawPanel.transform.position + Vector3.down * 3.8f)
                .SetEase(Ease.Linear);
            _infoPanel.DOMove(_infoPanel.transform.position, .5f).From(_infoPanel.transform.position + Vector3.up * 2)
                .SetEase(Ease.Linear);
        }

        public void UpdateUnitsCount(uint newValue)
        {
            _textUnitsCount.text = newValue.ToString();
        }

        public void UpdateCristalsCount(uint newValue)
        {
            _textCristalsCount.text = newValue.ToString();

            _viewCristalsCount.DORewind();
            _viewCristalsCount.transform.transform.DOShakeScale(.35f, .5f);
        }
    }
}