using Assets.CodeBase.Plant;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.CodeBase.Infrastructure.Bank
{
    public class BankInteractor : Interactor
    {
        private const int _priceForHay = 15;

        public delegate void OnCoinsChanged(int value, int HarvestCount = 0);
        public event OnCoinsChanged CoinsChanged;

        private BankRepository _bankRepository;
        public int coins => _bankRepository.coins;

        public BankInteractor(BankRepository bankRepository)
        {
            _bankRepository = bankRepository;
        }

        public override void Initialize()
        {
            _bankRepository.Initialize();
        }

        public void TakeHarvest(List<Hay> hays,float timeToDestroy)
        {
            Debug.Log("Taked one harvest");

            var Totalprice = hays.Count * _priceForHay;
            var HarvestCount = hays.Count;

            AddCoins(Totalprice, HarvestCount);

            DestroyAllHays(hays, timeToDestroy);
        }

        private static void DestroyAllHays(List<Hay> hays, float timeToDestroy)
        {
            for (int i = 0; i < hays.Count; i++)
            {
                GameObject.Destroy(hays[i].gameObject, timeToDestroy);
            }
        }

        private void AddCoins(int value, int HarvestCount)
        {
            Debug.Log("AddCoins");

            if (value > 0)
            {
                _bankRepository.coins += value;
                CoinsChanged?.Invoke(value, HarvestCount);
            }
        }
    }
}


