using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.CodeBase.Plant
{
    public class Grass : MonoBehaviour
    {
        [SerializeField] private Renderer _renderer;
        [SerializeField] private GameObject _sliceVFX;
        [SerializeField] private Hay _hayPrefab;
        [SerializeField] private Transform _socketHayPoint;
        [SerializeField] private Transform _socketDroundPoint;
        [SerializeField] private List<Collider> colliders = new List<Collider>();

        private const float _timeToGrowUp = 10f;

        private const float _lowGrass = 0.0f;
        private const float _middleGrass = 2.0f;
        private const float _highGrass = 4.0f;
        public bool _readyToReap {get;private set;}

        private float _elapsedTime = 0.0f;

        private float _maxDurability = 100.0f;
        private float _durability;

        public void ApplyDamage(float Damage, Vector3 positionOfEffect)
        {
            if (Damage > 0 && _readyToReap) 
            {
                EnableEffect(positionOfEffect);
                SpawnHay();
                Debug.Log("Taked damage");
                _durability -= Damage;

                if (_durability < 0)
                {
                    _durability = 0;
                }
                else if (_durability == 0)
                {
                    _readyToReap = false;
                    DisableOrEnableColliders(false);
                }

                Slice();
                Debug.Log("I takedDamage");
            }
        }

        private void DisableOrEnableColliders(bool value)
        {
            for (int i = 0; i < colliders.Count; i++)
            {
                colliders[i].enabled = value;
            }
        }


        private void SpawnHay()
        {
            GameObject newHay = Instantiate(_hayPrefab.gameObject,_socketHayPoint.position,Quaternion.identity);
            newHay.GetComponent<Hay>().OnCreate(_socketDroundPoint);
        }

        private void EnableEffect(Vector3 positionOfEffect)
        {
            DisableEffect();
            _sliceVFX.transform.position = positionOfEffect;
            _sliceVFX.SetActive(true);
            Debug.Log("VFX");
            Debug.Log(_sliceVFX.active);
        }

        private void DisableEffect()
        {
            Debug.Log("DisableVFX");
            _sliceVFX.SetActive(false);
        }

        private void Slice()
        {
            float height = 0f;

            if (_durability == 0)
                height = _lowGrass;
            else if (_durability <= 50)
                height = _middleGrass;
            else
                height = _highGrass;

            SetGrow(height);
        }

        private void SetGrow(float height)
        {
            _renderer.material.SetFloat("_BladeHeight", height);
        }

        private void Start()
        {
            _readyToReap = true;
            SetStartCondition();
        }

        private void SetStartCondition()
        {
            SetGrow(_highGrass);
            _durability = _maxDurability;
        }

        private void Update()
        {
            if (_readyToReap == false)
                GrowByTime();
        }

        private void GrowByTime()
        {
            _elapsedTime += Time.deltaTime;
            float toCompleteGrow = _elapsedTime / _timeToGrowUp;

            float grow = Mathf.Lerp(_lowGrass, _highGrass, toCompleteGrow);
            _renderer.material.SetFloat("_BladeHeight", grow);
            CheckReadyToReap(grow);
        }

        private void CheckReadyToReap(float height)
        {
            if (height >= _highGrass)
            {
                _durability = _maxDurability;
                _readyToReap = true;
                _elapsedTime = 0;
                DisableOrEnableColliders(true);
            }
        }
    }
}


