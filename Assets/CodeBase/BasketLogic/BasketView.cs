using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

namespace Assets.CodeBase.Hero
{
    public class BasketView : MonoBehaviour
    {
        [SerializeField] private Slider _capacitySlider;
        [SerializeField] private TextMeshProUGUI _capacityText;
        [SerializeField] private TextMeshProUGUI _fullText;
        [SerializeField] private Basket _basket;

        private Sequence _fullTextAnimation;

        private int _maxCapacityOfBasket;
        private const float _startFontSizeFullText = 15.0f;
        private const float _targetFontSizeFullText = 150.0f;

        private void Start()
        {
            _maxCapacityOfBasket = _basket.MaxCapacityOfBasket;

            Initial();

            _basket.HarvestChanged += OnValueChanged;
            _basket.HarvestFull += FullPopUp;
            _basket.HarvestEmpty += OnEmptyHarvest;
        }

        private void Initial()
        {
            _fullText.fontSize = _startFontSizeFullText;
            _fullText.gameObject.SetActive(false);
            _capacitySlider.maxValue = _maxCapacityOfBasket;
            _capacityText.SetText(0 + "/" + _maxCapacityOfBasket);
        }

        private void OnValueChanged(int currentValue)
        {
            _capacitySlider.value = currentValue;
            _capacityText.SetText(currentValue + "/" + _maxCapacityOfBasket);
        }

        private void FullPopUp()
        {
            Debug.Log("PopUPFULL");

            _fullText.gameObject.SetActive(true);
            
            _fullTextAnimation = DOTween.Sequence();
            _fullTextAnimation.Append(_fullText.DOFontSize(_targetFontSizeFullText, 1.0f));
            _fullTextAnimation.Append(_fullText.DOFontSize(_startFontSizeFullText, 0.5f).OnComplete(ResetFullText));
            //_fullTextAnimation.AppendCallback(ResetFullText);
        }

        private void ResetFullText()
        {
            _fullText.gameObject.SetActive(false);
        }

        private void OnEmptyHarvest()
        {
            _capacityText.SetText(0 + "/" + _maxCapacityOfBasket);
            _capacitySlider.DOValue(0,1.0f);
        }
    }
}
