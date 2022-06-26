using TMPro;
using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

namespace Assets.CodeBase.Infrastructure.Bank
{
    public class BankView : MonoBehaviour
    {
        private const float DurationOfCoinsTextIncrease = 2f;
        private const float DurationOfCoinsFly = 1f;
        private const float StepOfIncreaseCoinsFlyDuration = 0.025f;
        private const int _maxCoins = 40;

        private float _currentDurationOfCoinsFly;
        [SerializeField] private TextMeshProUGUI _coinsText;
        [SerializeField] private  GameObject _coinPrefab;
        [SerializeField] private  Transform _barn;
        [SerializeField] private  Transform _coinsCounter;
        private Vector3 _barnPosition => _barn.position;
        private Vector3 _coinsCounterPosition => _coinsCounter.position;

        private BankInteractor _bankInteractor;
        private List<GameObject> _coinsOnCanvas = new List<GameObject>();

        private void Start()
        {
            _bankInteractor = GameEntryPoint.instance._bankInteractor;

            _bankInteractor.CoinsChanged += ShowMoney;

            SetMoneyOnStart(_bankInteractor.coins);
            PrepareCoins();

            ResetCoinsFlyDuration();
        }


        private void ShowMoney(int value, int HarvestCount)
        {
            int currentCoinsValue = _bankInteractor.coins;
            int previousCoinsValue = currentCoinsValue - value;

            _coinsText.DOCounter(previousCoinsValue, currentCoinsValue, DurationOfCoinsTextIncrease);

            Debug.Log($"Prev {previousCoinsValue} current {currentCoinsValue}");
            Debug.Log($"CurrentCoinsValue {currentCoinsValue} value {value}");


            CoinsFly(HarvestCount);
        }

        private void SetMoneyOnStart(int value)
        {
            _coinsText.SetText(value.ToString());
        }

        private void PrepareCoins()
        {
            GameObject coin;

            for (int i = 0; i < _maxCoins; i++)
            {
                coin = Instantiate(_coinPrefab);
                coin.transform.parent = transform;
                _coinsOnCanvas.Add(coin);    
                coin.SetActive(false);
            }
        }

        private void CoinsFly(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                GameObject coin = _coinsOnCanvas[i];


                coin.SetActive(true);
                coin.transform.position = _barnPosition;
                
                coin.transform.DOMove(_coinsCounterPosition, _currentDurationOfCoinsFly).SetEase(Ease.InOutFlash)
                .OnComplete(() =>
                {
                    coin.SetActive(false);
                });

                _currentDurationOfCoinsFly += StepOfIncreaseCoinsFlyDuration;
                
                Debug.Log("Queue count in cycle" + _coinsOnCanvas.Count);
            }

            ResetCoinsFlyDuration();
        }


        private void ResetCoinsFlyDuration()
        {
            _currentDurationOfCoinsFly = DurationOfCoinsFly;
        }
    }
}



