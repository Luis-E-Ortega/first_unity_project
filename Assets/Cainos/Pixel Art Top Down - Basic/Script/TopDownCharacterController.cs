using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Cainos.PixelArtTopDown_Basic
{
    public class TopDownCharacterController : MonoBehaviour
    {
        public float speed;

        private int CurrentDirection => animator.GetInteger("Direction");

        private Animator animator;

        [SerializeField] private float dashDistance = 3f; // How far the dash will move the object
        [SerializeField] private float dashDuration = 0.1f; // How long the dash takes to complete
        [SerializeField] private float dashCooldown = 1f; // Time between dashes
        public Rigidbody2D myRigidbody;
        private bool canDash = true;
        private bool isDashing = false;
        private float dashTimer = 0f;
        private float cooldownTimer = 0f;
        private Vector3 dashStartPosition;
        private Vector3 dashTargetPosition;

        private void Start()
        {
            animator = GetComponent<Animator>();
            gameObject.name = "Adventurer";
        }


        private void Update()
        {
            Vector2 dir = Vector2.zero;
            if (Input.GetKey(KeyCode.A))
            {
                dir.x = -1;
                //animator.SetInteger("Direction", 3);
            }
            if (Input.GetKey(KeyCode.D))
            {
                dir.x = 1;
                //animator.SetInteger("Direction", 2);
            }

            if (Input.GetKey(KeyCode.W))
            {
                dir.y = 1;
                //animator.SetInteger("Direction", 1);
            }
            if (Input.GetKey(KeyCode.S))
            {
                dir.y = -1;
                //animator.SetInteger("Direction", 0);
            }

            // Section for setting directions important to put before normalizing to capture exact values
            switch((dir.x, dir.y))
            {
                case (-1, 0):
                    animator.SetInteger("Direction", 3); // Left
                    Debug.Log($"Switch case hit: (-1, 0), Setting Direction to 3");
                    break;
                case (-1, 1):
                    animator.SetInteger("Direction", 4); // Up-left
                    Debug.Log($"Switch case hit: (-1, 0), Setting Direction to 4");
                    break;
                case (1, 0):
                    animator.SetInteger("Direction", 2); // Right
                    Debug.Log($"Switch case hit: (-1, 0), Setting Direction to 2");
                    break;
                case (1, 1):
                    animator.SetInteger("Direction", 5); // Up-right
                    Debug.Log($"Switch case hit: (-1, 0), Setting Direction to 5");
                    break;
                case (0, 1):
                    animator.SetInteger("Direction", 1); // Up
                    Debug.Log($"Switch case hit: (-1, 0), Setting Direction to 1");
                    break;
                case (0, -1):
                    animator.SetInteger("Direction", 0); // Down
                    Debug.Log($"Switch case hit: (-1, 0), Setting Direction to 0");
                    break;
                case (-1, -1):
                    animator.SetInteger("Direction", 6); // Down-left
                    Debug.Log($"Switch case hit: (-1, 0), Setting Direction to 6");
                    break;
                case (1, -1):
                    animator.SetInteger("Direction", 7); // Down-right
                    Debug.Log($"Switch case hit: (-1, 0), Setting Direction to 7");
                    break;
            }
            dir.Normalize(); // Normalize after direction is set, for smooth movement 
            Debug.Log($"Current dir values: x={dir.x}, y={dir.y}");


            animator.SetBool("IsMoving", dir.magnitude > 0);

            GetComponent<Rigidbody2D>().linearVelocity = speed * dir;

            Debug.DrawRay(transform.position, dir * dashDistance, Color.red, 0.1f);

            // Check for dash input when we're able to dash
            if (Input.GetKeyDown(KeyCode.Space) && canDash && !isDashing)
            {
                CheckAndStartDash();
            }

            if (isDashing)
            {
                dashTimer += Time.deltaTime;
                float progress = dashTimer / dashDuration;

                if (progress >= 1f)
                {
                    FinishDash();
                }
                else
                {
                    // Lerp the position for smooth movement
                    transform.position = Vector3.Lerp(dashStartPosition, dashTargetPosition, progress);
                }
            }
            if (!canDash)
            {
                cooldownTimer += Time.deltaTime;
                if (cooldownTimer >= dashCooldown)
                {
                    canDash = true;
                    cooldownTimer = 0f;
                }
            }
        }
        private void StartDash(Vector3 dashDirection, float adjustedDashDistance=0)
        {
            isDashing = true;
            canDash = false;
            dashTimer = 0f;
            dashStartPosition = transform.position;
            Debug.Log($"Attempting to dash with direction: {CurrentDirection}");

            if (adjustedDashDistance == 0)
            {
                dashTargetPosition = transform.position + (dashDirection * dashDistance);
            }
            else
            {
                dashTargetPosition = transform.position + (dashDirection * adjustedDashDistance);
            }

        }    
        private void FinishDash()
        {
            isDashing = false;
            transform.position = dashTargetPosition;
            cooldownTimer = 0f;
        }

        private void CheckAndStartDash()
        {
            Debug.Log($"CheckAndStartDash called with CurrentDirection: {CurrentDirection}, raw input: {Input.GetAxis("Vertical")}");

            Vector3 dashDirection = Vector3.zero;
            switch(CurrentDirection)
            {
                case 0:
                    dashDirection = Vector3.down;
                    break;
                case 1:
                    dashDirection = Vector3.up;
                    break;
                case 2:
                    dashDirection = Vector3.right;
                    break;
                case 3:
                    dashDirection = Vector3.left;
                    break;
                case 4:
                    dashDirection = (Vector3.up + Vector3.left).normalized;
                    break;
                case 5:
                    dashDirection = (Vector3.up + Vector3.right).normalized;
                    break;
                case 6:
                    dashDirection = (Vector3.down + Vector3.left).normalized;
                    break;
                case 7:
                    dashDirection = (Vector3.down + Vector3.right).normalized;
                    break;
            }
            Debug.Log($"Casting ray from {transform.position} in direction {dashDirection} with distance {dashDistance}");
            Debug.DrawRay(transform.position, dashDirection * dashDistance, Color.green, 0.5f);
            Debug.DrawRay(transform.position, Vector3.up * 0.5f, Color.blue, 0.5f);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, dashDirection, dashDistance, ~LayerMask.GetMask("Adventurer"));
            Debug.Log($"Hit something: {hit.collider != null}, Layer mask: {~LayerMask.GetMask("Adventurer")}");

                
            if (hit.collider == null)
            {
                StartDash(dashDirection);
            }
            else
            {
                float adjustedDashDistance = hit.distance - 0.05f;
                StartDash(dashDirection, adjustedDashDistance);
                Debug.Log($"Dash blocked by: {hit.collider.gameObject.name}");
            }
                
        }
        
    }
}
