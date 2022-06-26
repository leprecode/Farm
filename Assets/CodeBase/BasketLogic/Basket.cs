using Assets.CodeBase.Plant;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.CodeBase.Hero
{
    public class Basket : MonoBehaviour
    {
        private const int _maxCapacityOfBasket = 40;
        private const float _overlapSphereRadius = 4f;
        private const string _tagOfHay = "Hay";
        private const float _offsetToLiftUpSocket = 1.0f;
        private Vector3 _startPositionOfSocketsParent = new Vector3(0f, -0.5f, 0f);

        [SerializeField] private Transform _playerTransform;
        [SerializeField] private Transform _firstSocketOfBasket;
        [SerializeField] private LayerMask _hayLayer;
        [SerializeField] private Transform _parentOfSocketsInBasket;
        [SerializeField] private Transform _parentOfHaysInBasket;

        private List<GameObject> _harvestHeap;
        private Vector3 _playerPosition => _playerTransform.position;

        public int MaxCapacityOfBasket => _maxCapacityOfBasket;
        public int CurrentBasketCapacity => _harvestHeap.Count;

        private Collider[] _overlapedColliders;
        private List<Hay> _hayToTake;

        private float timer = 0f;
        private float _timeToCheckHay = 1f;
        private bool _needToNotifyAboutFull = true;
        private const float _timeToClearHeap = 0.2f;

        public delegate void OnHarvestChanged(int value);
        public delegate void OnHarvestFull();
        public delegate void OnHarvestEmpty();

        public event OnHarvestChanged HarvestChanged;
        public event OnHarvestFull HarvestFull;
        public event OnHarvestEmpty HarvestEmpty;

        private void Start()
        {
            _harvestHeap = new List<GameObject>();
            _hayToTake = new List<Hay>();
            _overlapedColliders = new Collider[_maxCapacityOfBasket];
        }

        private void Update()
        {
            //Проверка вместимости 

                timer += Time.deltaTime;
                if (timer <= _timeToCheckHay)
                {
                    CheckNewHay();
                    timer = 0.0f;
                }
        }

        public List<GameObject> GetHarvestHeap()
        {
            Invoke("ResetHarvestHeap", _timeToClearHeap);
            return _harvestHeap;
        }

        private void ResetHarvestHeap()
        {
            _harvestHeap.Clear();
            _parentOfSocketsInBasket.localPosition = _startPositionOfSocketsParent;

            _needToNotifyAboutFull = true;
            HarvestEmpty?.Invoke();
        }


        private void CheckNewHay()
        {
            Array.Clear(_overlapedColliders, 0, _overlapedColliders.Length);
            _hayToTake.Clear();

            Physics.OverlapSphereNonAlloc(_playerPosition, _overlapSphereRadius, _overlapedColliders, _hayLayer, QueryTriggerInteraction.Collide);
            Debug.Log("CheckNewHay");

            for (int i = 0; i < _overlapedColliders.Length; i++)
            {
                if (_overlapedColliders[i] == null)
                    continue;

                if (_overlapedColliders[i].CompareTag(_tagOfHay))
                {
                    _hayToTake.Add(_overlapedColliders[i].GetComponent<Hay>());
                }
            }

            if (_hayToTake.Count > 0)
            {
                ApplyHarvest(_hayToTake);
            }
        }

        private void ApplyHarvest(List<Hay> harvest)
        {
            var harvestCount = harvest.Count;
            var countToTake = CheckCapacity(harvestCount);

            if (countToTake != 0)
            {
                TakeHarvest(harvest, countToTake);
            }
            else
            {
                StuckIsFull();
            }
        }

        private void TakeHarvest(List<Hay> harvest, int countToTake)
        {
/*            if (harvest.Count == 0)
                return;*/

            for (int i = 0; i < countToTake; i++)
            {
                var socketToPlace = LiftUpPlacesInBasket();
                _harvestHeap.Add(harvest[i].gameObject);

                harvest[i].transform.SetParent(_parentOfHaysInBasket);
                harvest[i].FallToPlayerBasket(_firstSocketOfBasket, socketToPlace);
            }

            HarvestChanged?.Invoke(_harvestHeap.Count);
        }

        private Transform LiftUpPlacesInBasket()
        {
  /*          var prevPos = _parentOfSocketsInBasket.transform.localPosition.y;
            var newPosition = (prevPos += _offsetToLiftUpSocket);
            Debug.Log($"LisftUP my lastPosition {prevPos} and my new {newPosition}");*/

            _parentOfSocketsInBasket.transform.localPosition += new Vector3(0, _offsetToLiftUpSocket, 0);
            return _parentOfSocketsInBasket.transform;
        }
    
        private void StuckIsFull()
        {
            if (_needToNotifyAboutFull)
            {
                HarvestFull?.Invoke();
                _needToNotifyAboutFull = false;
            }
        }

        private int CheckCapacity(int countOFHarvest)
        {
            if (_harvestHeap.Count == _maxCapacityOfBasket)
                return 0;

            var countToTake = 0;

            if (_harvestHeap.Count + countOFHarvest <= _maxCapacityOfBasket)
                countToTake = countOFHarvest;
            else
                countToTake = _maxCapacityOfBasket - _harvestHeap.Count;

            return countToTake;
        }
    }
}
