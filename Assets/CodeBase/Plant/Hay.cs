using Assets.CodeBase.Hero;
using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.CodeBase.Plant
{
    public class Hay : MonoBehaviour
    {
        [SerializeField] private BoxCollider _boxCollider;

        private const float _durationOfFallingToGround = 0.5f;
        private const float _durationOfJumpInAir = 0.2f;
        private const float _durationOfJumpInBasket = 0.2f;

        private Transform _socketOnTheGround;
        private Vector3 _pointToFallOnGround = new Vector3();
        private Vector3 _offsetY = new Vector3(0, 2.5f, 0);
        private Vector3 _newSizeInBasket = new Vector3(3f, 1f, 2.4f);
        private bool _jumpdedInPlayerBasket = false;
        private Sequence _fallInPlayerBasketSequence;

        private float _fallingSpeed = 10.0f;
        private float _step;
        private Basket basket;
        private Transform _socketPositionToMove;
        private bool _readyToFall = false;

        //remove
        private float posY = 0;
        
        
        private void Update()
        {
            if (_readyToFall)
            {
                _step = _fallingSpeed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, _socketPositionToMove.position, _step);

                if (Vector3.Distance(transform.position, _socketPositionToMove.position) < 0.01f)
                {
                    transform.position = _socketPositionToMove.position;
                    transform.position = new Vector3(transform.position.x, posY, transform.position.z);
                    transform.rotation = _socketPositionToMove.rotation;
                    _readyToFall = false;

                }
            }
        }

        public void OnCreate(Transform socketOnGround)
        {
            this._socketOnTheGround = socketOnGround;
            _pointToFallOnGround = _socketOnTheGround.position;
            JumpInAir();
        }

        private void JumpInAir()
        {
            transform.DOMove(_pointToFallOnGround + _offsetY, _durationOfJumpInAir).OnComplete(FallOnGround);   
        }

        public void FallToPlayerBasket(Transform firstSocketToMove, Transform secondSocketToMove)
        {
            _jumpdedInPlayerBasket = true;

            _boxCollider.enabled = false;
            _socketPositionToMove = secondSocketToMove;
                        _fallInPlayerBasketSequence = DOTween.Sequence();
                        _fallInPlayerBasketSequence.Append(transform.DOMove(firstSocketToMove.position, _durationOfJumpInBasket));
                        _fallInPlayerBasketSequence.Insert(0, transform.DOScale(_newSizeInBasket, _durationOfJumpInBasket));

            _readyToFall = true;
            posY = secondSocketToMove.position.y;
        }

        private void FallOnGround()
        {
            if (_jumpdedInPlayerBasket != true)
                transform.DOMove(_pointToFallOnGround, _durationOfFallingToGround);
        }
    }
}


