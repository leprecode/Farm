using Assets.CodeBase.Plant;
using System;
using System.Collections;
using UnityEngine;

namespace Assets.CodeBase.WeaponLogic
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] private float _damage;

        public float Damage { get => _damage; }
    }

}