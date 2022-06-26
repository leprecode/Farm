using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.CodeBase.Hero
{
    public class PlayerMoveController : MonoBehaviour
    {
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private float _movementSpeed;
        Vector3 movementVector;
        Vector3 lastMovementVector;

        private void Update()
        {
            movementVector = Vector3.zero;

            movementVector.x = SimpleInput.GetAxis("Horizontal");
            movementVector.z = SimpleInput.GetAxis("Vertical");

            if (movementVector.x != 0 && movementVector.z != 0)
            {
                lastMovementVector = new Vector3(movementVector.x, 0, movementVector.z);
                transform.forward = movementVector;
            }
            else
            {
                transform.forward = lastMovementVector;
            }

            movementVector.y += Physics.gravity.y;
        }

        private void FixedUpdate()
        {
            _characterController.Move(_movementSpeed * movementVector * Time.fixedDeltaTime);
        }
    }
}