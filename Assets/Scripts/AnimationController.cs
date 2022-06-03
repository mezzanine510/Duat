using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
[SerializeField]
        float acceleration = 6.0f;

        [SerializeField]
        float strafeAcceleration = 3.0f;

        [SerializeField]
        float deceleration = 10.0f;

        private Animator animator;
        private float delta;
        private float inputX;
        private float inputZ;
        private bool movementPressed;
        private bool aimPressed;
        private float strafeVelocity;
        private float forwardVelocity;
        private bool movingForward;
        private bool movingBackward;
        private bool strafingLeft;
        private bool strafingRight;

        // Start is called before the first frame update
        void Awake()
        {
            animator = GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
            delta = Time.deltaTime;
        }

        public void HandleAnimation(Vector3 movement, Vector3 aim)
        {
            movementPressed = movement.x != 0 || movement.z != 0;
            aimPressed = aim.x != 0 || aim.z != 0;

            CalculateMotion(movement, aim);

            animator.SetFloat("velocityZ", forwardVelocity);
            animator.SetFloat("velocityX", strafeVelocity);
            animator.SetBool("isWalking", movementPressed);
        }

        private void CalculateMotion(Vector3 movement, Vector3 aim)
        {
            Vector3 localRight = Vector3.Cross(Vector3.up, aim).normalized;
            inputX = Vector3.Dot(movement.normalized, localRight);
            inputZ =    aimPressed ? 
                        Vector3.Dot(movement.normalized, aim.normalized) :
                        1;
            
            float playerDirectionX = Mathf.Sign(inputX);
            float playerDirectionZ = Mathf.Sign(inputZ);
            movingForward = aimPressed ? movementPressed && playerDirectionZ > 0 : movementPressed;
            movingBackward = movementPressed && playerDirectionZ < 0;
            strafingRight = movementPressed && aimPressed && playerDirectionX > 0;
            strafingLeft = movementPressed && aimPressed && playerDirectionX < 0;

            SetVelocity();
        }

        private void SetVelocity()
        {
            if (movingForward) MoveForward();
            if (movingBackward) MoveBackward();
            if (strafingLeft) StrafeLeft();
            if (strafingRight) StrafeRight();
            MovementCheck();
        }

        private void MoveForward()
        {
            if (forwardVelocity < inputZ)
            {
                forwardVelocity += acceleration * delta;
            }

            if (forwardVelocity > inputZ)
            {
                forwardVelocity = inputZ;
            }
        }


        private void MoveBackward()
        {
            if (forwardVelocity > -1)
            {
                forwardVelocity -= acceleration * delta;
            }

            if (forwardVelocity < inputZ)
            {
                forwardVelocity = inputZ;
            }
        }

        private void StrafeLeft()
        {
            if (strafeVelocity > inputX) { strafeVelocity -= strafeAcceleration * delta; }

            // Clamp
            if (strafeVelocity < inputX) { strafeVelocity = inputX; }
        }

        private void StrafeRight()
        {
            if (strafeVelocity < inputX) { strafeVelocity += strafeAcceleration * delta; }

            // Clamp
            if (strafeVelocity > inputX) { strafeVelocity = inputX; }
        }

        private void MovementCheck()
        {
            if (forwardVelocity == 0 && strafeVelocity == 0) { return; }

            if (!movementPressed)
            {
                StopMoving();
                return;
            }

            if (movementPressed && !aimPressed)
            {
                StopStrafing();
            }
        }

        private void StopMoving()
        {
            forwardVelocity = Decelerate(forwardVelocity);
            strafeVelocity = Decelerate(strafeVelocity);
            ClampMovement();
        }

        private void StopStrafing()
        {
            strafeVelocity = Decelerate(strafeVelocity);
            ClampMovement();
        }

        private float Decelerate(float velocity)
        {
            return Mathf.Lerp(velocity, 0, deceleration * delta);
        }

        private void ClampMovement()
        {
            if (forwardVelocity > -0.01f && forwardVelocity < 0.01f) { forwardVelocity = 0; }
            if (strafeVelocity > -0.01f && strafeVelocity < 0.01f) { strafeVelocity = 0; }
        }
}
