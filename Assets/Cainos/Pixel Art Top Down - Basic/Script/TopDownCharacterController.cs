using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using Unity.Collections;
using Unity.Mathematics;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;


namespace Cainos.PixelArtTopDown_Basic
{
    //public enum ElementType {Fire, Earth, Wind, Water};
    public class TopDownCharacterController : MonoBehaviour
    {
        public float speed;

        private int CurrentDirection => animator.GetInteger("Direction");

        private Animator animator;
        private SpriteRenderer spriteRenderer;
        public Rigidbody2D myRigidbody;
        public TrailRenderer trail;
        public ParticleSystem dashParticles;
        public GameObject spellPoint; // The point to cast spells from
        // References for spell prefabs
        public ParticleSystem fireballPrefab;
        public ParticleSystem waterballPrefab;
        public ParticleSystem earthballPrefab;
        public ParticleSystem windballPrefab;

        [SerializeField] private float dashDistance = 3f; // How far the dash will move the object
        [SerializeField] private float dashDuration = 0.1f; // How long the dash takes to complete
        [SerializeField] private float dashCooldown = 1f; // Time between dashes
        private ElementType elementMode;

        private bool canDash = true;
        private bool isDashing = false;
        private float dashTimer = 0f;
        private float cooldownTimer = 0f;
        private Vector3 dashStartPosition;
        private Vector3 dashTargetPosition;

        [SerializeField] private InventoryController inventory; // To pull data from inventory controller

        private void Start()
        {
            animator = GetComponent<Animator>();
            trail = GetComponent<TrailRenderer>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer == null)
            {
                Debug.LogError("No SpriteRenderer found on this GameObject!");
            }
            trail.enabled = false;
            dashParticles.Stop(true); // Ensure that particles aren't played on start

            gameObject.name = "Adventurer";
        }


        private void Update()
        {
            HandleMovement(); // Consolidated movement code to a different method

            setElementMode(); // To monitor keypresses in case element mode is registered to change

            if(Input.GetMouseButtonDown(1))
            {
                CheckAndStartSpell();
            }

        }
        private void HandleMovement()
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
            //Debug.Log($"Current dir values: x={dir.x}, y={dir.y}");


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
        private void StartDash(Vector3 direction, float adjustedDashDistance=0)
        {
            isDashing = true;
            canDash = false;
            dashTimer = 0f;
            dashStartPosition = transform.position;
            // Add visual effect to dash
            dashParticles.Play();
            
            if (adjustedDashDistance == 0) // If distance was not adjusted due to collision, dash full distance
            {
                dashTargetPosition = transform.position + (direction * dashDistance);
            }
            else // Otherwise dash adjusted distance, stopping before object
            {
                dashTargetPosition = transform.position + (direction * adjustedDashDistance);
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
            // Determine the direction for the dash
            Vector3 dashDirection = Vector3.zero;
            var shape = dashParticles.shape;
            Vector3 defaultPosition = new Vector3(-0.00000047684f, 0.40905f, 0f);

            switch(CurrentDirection)
            {
                case 0:
                    dashDirection = Vector3.down;
                    getShapesRotation(-90, -90, 0);
                    shape.position = new Vector3(0, 1, 0); // Shift starting position of particles up
                    break;
                case 1:
                    dashDirection = Vector3.up;
                    getShapesRotation(90, 90, 0);
                    shape.position = defaultPosition;
                    break;
                case 2:
                    dashDirection = Vector3.right;
                    getShapesRotation(330, 330, 0);
                    shape.position = new Vector3(-0.5f, 0, 0); // Shift starting position of particles left
                    break;
                case 3:
                    dashDirection = Vector3.left;
                    getShapesRotation(210, 210, 0);
                    shape.position = new Vector3(0.5f, 0, 0); // Shift starting position of particles right
                    break;
                case 4:
                    dashDirection = (Vector3.up + Vector3.left).normalized;
                    getShapesRotation(45, 45, 0);
                    shape.position = defaultPosition; // Reset starting position of particles
                    break;
                case 5:
                    dashDirection = (Vector3.up + Vector3.right).normalized;
                    getShapesRotation(135, 135, 0);
                    shape.position = defaultPosition; // Reset starting position of particles  
                    break;
                case 6:
                    dashDirection = (Vector3.down + Vector3.left).normalized;
                    getShapesRotation(225, 225, 0);
                    shape.position = new Vector3(0.5f, 0.5f, 0); // Shift starting position of particles up and right 
                    break;
                case 7:
                    dashDirection = (Vector3.down + Vector3.right).normalized;
                    getShapesRotation(315, 315, 0);
                    shape.position = new Vector3(-0.5f, 0.5f, 0); // Shift starting position of particles up and left 
                    break;
            }
            //Debug.Log($"Casting ray from {transform.position} in direction {dashDirection} with distance {dashDistance}");
            //Debug.DrawRay(transform.position, dashDirection * dashDistance, Color.green, 0.5f);
            //Debug.DrawRay(transform.position, Vector3.up * 0.5f, Color.blue, 0.5f);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, dashDirection, dashDistance, ~LayerMask.GetMask("Adventurer")); // To ignore collision with player itself
            //Debug.Log($"Hit something: {hit.collider != null}, Layer mask: {~LayerMask.GetMask("Adventurer")}");

                
            if (hit.collider == null) // If there is no collision with an object in trajectory
            {
                StartDash(dashDirection);
            }
            else // If it will hit an object, stop right before it
            {
                float adjustedDashDistance = hit.distance - 0.05f;
                StartDash(dashDirection, adjustedDashDistance);
                Debug.Log($"Dash blocked by: {hit.collider.gameObject.name}");
            }
                
        }

        public void getShapesRotation(float x, float y, float z)
        {
            // To manipulate position of dash particles below
            var shapes_prop = dashParticles.shape;

            // Set the rotation for the particles based on degrees passed to method
            shapes_prop.rotation = new Vector3(x, y, z);
        }
        public void setSpellShapesRotation(float x, float y, float z)
        {
            // Create references to change the rotation of each element
            ParticleSystem.ShapeModule fireShape = fireballPrefab.shape;
            ParticleSystem.ShapeModule waterShape = waterballPrefab.shape;
            ParticleSystem.ShapeModule earthShape = earthballPrefab.shape;
            ParticleSystem.ShapeModule windShape = windballPrefab.shape;

            Vector3 newRotation = new Vector3(x, y, z);

            fireShape.rotation = newRotation;
            waterShape.rotation = newRotation;
            earthShape.rotation = newRotation;
            windShape.rotation = newRotation;
        }

        public void setElementMode()
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                elementMode = ElementType.Fire;
                spriteRenderer.color = Color.red;
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                elementMode = ElementType.Earth;
                spriteRenderer.color = Color.green;
            }
            else if (Input.GetKeyDown(KeyCode.X))
            {
                elementMode = ElementType.Wind;
                spriteRenderer.color = Color.gray;
            }
            else if (Input.GetKeyDown(KeyCode.C))
            {
                elementMode = ElementType.Water;
                spriteRenderer.color = Color.blue;
            }
        }
        public void CheckAndStartSpell()
        {
            Debug.Log($"CheckAndStartSpell called with CurrentDirection: {CurrentDirection}, raw input: {Input.GetAxis("Vertical")}");
            Vector3 defaultPosition = new Vector3(-0.00000047684f, 0.40905f, 0f);
            switch(CurrentDirection)
            {
                case 0:
                    setSpellShapesRotation(90, 90, 0); // Facing down
                    spellPoint.transform.localPosition = new Vector3(0, -1, 0); // Shift starting position of particles up
                    break;
                case 1:
                    setSpellShapesRotation(-90, -90, 0); // Facing up
                    spellPoint.transform.localPosition = new Vector3(0, 1, 0); // Shift starting position of particles down
                    break;
                case 2:
                    setSpellShapesRotation(0, 90, 0); // Facing right
                    spellPoint.transform.localPosition = new Vector3(0.5f, 0, 0); // Shift starting position of particles right
                    break;
                case 3:
                    setSpellShapesRotation(180, 90, 0); // Facing left
                    spellPoint.transform.localPosition = new Vector3(-0.5f, 0, 0); // Shift starting position of particles left
                    break;
                case 4:
                    setSpellShapesRotation(-45, -45, 0); // Facing up left
                    spellPoint.transform.localPosition = new Vector3(-0.5f, 0, 0); // Shift starting position of particles left
                    break;
                case 5:
                    setSpellShapesRotation(-135, -135, 0); // Facing up right
                    spellPoint.transform.localPosition = new Vector3(0.5f, 0, 0); // Reset starting position of particles  
                    break;
                case 6:
                    setSpellShapesRotation(135, 135, 0);
                    spellPoint.transform.localPosition = new Vector3(-0.5f, -0.5f, 0); // Shift starting position of particles down left
                    break;
                case 7:
                    setSpellShapesRotation(45, 45, 0);
                    spellPoint.transform.localPosition = new Vector3(0.5f, -0.5f, 0); // Shift starting position of particles down right
                    break;
            }

            StartSpell();
        }    
        public void StartSpell()
        {
            //First checks if element is available
            if (!inventory.HasElement(elementMode))
            {
                Debug.Log("Not enough elements to cast this spell!");
                return;
            }
            Debug.Log("Executing a spell!");
            switch(elementMode)
            {
                case ElementType.Fire:
                    ParticleSystem fireBallInstance = Instantiate(fireballPrefab, spellPoint.transform.position, Quaternion.identity);
                    inventory.UseElement(ElementType.Fire); // Consume the element
                    Debug.Log("Fireball explosion!");
                    fireBallInstance.Play();
                    break;
                case ElementType.Water:
                    ParticleSystem waterBallInstance = Instantiate(waterballPrefab, spellPoint.transform.position, Quaternion.identity);
                    inventory.UseElement(ElementType.Water); 
                    Debug.Log("Water explosion!");
                    waterBallInstance.Play();
                    break;
                case ElementType.Earth:
                    ParticleSystem earthBallInstance = Instantiate(earthballPrefab, spellPoint.transform.position, Quaternion.identity);
                    inventory.UseElement(ElementType.Earth);
                    Debug.Log("Earth explosion!");
                    earthBallInstance.Play();
                    break;
                case ElementType.Wind:
                    ParticleSystem windBallInstance = Instantiate(windballPrefab, spellPoint.transform.position, Quaternion.identity);
                    inventory.UseElement(ElementType.Wind);
                    Debug.Log("Wind explosion!");
                    windBallInstance.Play();
                    break;
                default:
                    Debug.LogWarning($"Unkown element type: {elementMode}");
                    break;
            }
        }
        
    }
}
