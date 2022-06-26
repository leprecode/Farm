using Assets.CodeBase.Plant;
using Assets.CodeBase.WeaponLogic;
using UnityEngine;

namespace Assets.CodeBase.Hero
{
    public class Reaping : MonoBehaviour
    {
        private const string _tagOfHarvest = "Harvest";
        private const float _maxDistanceOfRay = 2.5f;
        private const float _sphereRadius = 0.2f;
        private const float _maxDistanceOfSphere = 2.5f;
        [SerializeField] private LayerMask _targetLayer;
        [SerializeField] private PlayerAnimator _playerAnimator;
        [SerializeField] private Transform _raycastSocket;

        private Vector3 _raycastStartPosition => _raycastSocket.position;
        private Vector3 _raycastStartDirection => _raycastSocket.forward;

        private bool _readyToDamage = true;

        private GameObject _weapon;
        private float _damage;

        public void DoDamage()
        {
            RaycastHit hit;

            if (Physics.SphereCast(_raycastStartPosition, _sphereRadius, _raycastStartDirection, out hit, _maxDistanceOfSphere,_targetLayer,QueryTriggerInteraction.Collide))
            {
                if (hit.transform.CompareTag(_tagOfHarvest))
                {
                    var harvest = hit.transform.GetComponent<Grass>();
                    var contactPoint = hit.point;

                    harvest.ApplyDamage(_damage, contactPoint);
                }
            }
        }

        private void Start()
        {
            var weaponComponent = GetComponentInChildren<Weapon>();
            _weapon = weaponComponent.gameObject;
            _damage = weaponComponent.Damage;
        }

        private void FixedUpdate()
        {
            CheckHarvest();
        }

        private void CheckHarvest()
        {
            RaycastHit hit;

            if (Physics.Raycast(_raycastStartPosition, _raycastStartDirection, out hit, _maxDistanceOfRay, _targetLayer, QueryTriggerInteraction.Collide))
            {
                if (hit.transform.CompareTag(_tagOfHarvest))
                {
                    if (hit.transform.GetComponent<Grass>()._readyToReap)
                    {
                        Reap();
                    }
                    else
                    {
                        StopReap();
                    }
                }
                else
                {
                    StopReap();
                }
            }
            else
            {
                StopReap();
            }
        }

        private void StopReap()
        {
            _weapon.SetActive(false);
            _playerAnimator.StopReap();
        }

        private void Reap()
        {
            _weapon.SetActive(true);
            _playerAnimator.Reap();
        }
    }
}