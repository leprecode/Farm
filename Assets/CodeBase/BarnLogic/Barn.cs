using Assets.CodeBase.Hero;
using Assets.CodeBase.Infrastructure;
using Assets.CodeBase.Infrastructure.Bank;
using Assets.CodeBase.Plant;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.CodeBase.BarnLogic
{
    public class Barn : MonoBehaviour
    {
        private const float _durationOfHarvestTransition = 1f;
        private const float _stepForIncreaseDuration = 0.05f;

        private Vector3 _positionOfSocketBundle => _socketBundle.position;
        private float _currentDurationOfHarvestTransition;
        private BankInteractor _bankInteractor;

        [SerializeField] private Transform _socketBundle;

        private void Start()
        {
            _bankInteractor = GameEntryPoint.instance._bankInteractor;
            _currentDurationOfHarvestTransition = _durationOfHarvestTransition;
        }


        public void TakeHay(List<GameObject> harvestHeap)
        {
            if (harvestHeap.Count == 0 || harvestHeap[0] == null)
                return;

            List<Hay> hays = new List<Hay>();

            for (int i = 0; i < harvestHeap.Count; i++)
            {
                var harvest = harvestHeap[i].gameObject;
                SendToStuck(harvest);
                hays.Add(harvest.GetComponent<Hay>());
            }

            var maxDurationOfHayTransition = (harvestHeap.Count * _stepForIncreaseDuration)+ _durationOfHarvestTransition;
            _bankInteractor.TakeHarvest(hays, maxDurationOfHayTransition);

            ResetCurrentDuration();
        }

        private void ResetCurrentDuration()
        {
            _currentDurationOfHarvestTransition = _durationOfHarvestTransition;
        }

        private void SendToStuck(GameObject harvest)
        {
            harvest.transform.parent = null;

            harvest.transform.DOMove(_positionOfSocketBundle, _currentDurationOfHarvestTransition)
                .SetEase(Ease.InOutBack);
            _currentDurationOfHarvestTransition += _stepForIncreaseDuration;
        }
    }
}