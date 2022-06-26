using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.CodeBase.Hero
{
    public class PlayerAnimator : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private CharacterController _characterController;
        private const string _movingState = "Running";
        private const string _nameOfReapParametr = "Reaping";

        private void Update()
        {
            _animator.SetFloat(_movingState, _characterController.velocity.magnitude, 0.1f, Time.deltaTime);
        }

        public void Reap()
        {
            _animator.SetBool(_nameOfReapParametr,true);
        }

        public void StopReap()
        {
            _animator.SetBool(_nameOfReapParametr,false);
        }
    }
}