using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float _moveSpeed = 3f;
    [SerializeField] float _jumpHeight = 2f;
    float _verticalVelocity = 0f;
    float _horizontalVelocity = 0f;
    float _gravity = -6f;
    bool _grounded = false;
    bool _jumping = true;
    int _ceilingCount = 0;

    CharacterController controller;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        _grounded = controller.isGrounded;
        _horizontalVelocity = 1.5f * Input.GetAxis("Horizontal");
        if (_ceilingCount == 0 && (controller.collisionFlags & CollisionFlags.Above) != 0) {
            _verticalVelocity = 0f;
            _ceilingCount = 20;
            
        }

        _verticalVelocity += _gravity * Time.deltaTime;
        if (_grounded) {
            _verticalVelocity = 0f;
            _jumping = false;
        }
        else {
            if (_verticalVelocity < 0 && _horizontalVelocity != 0 && (controller.collisionFlags & CollisionFlags.Sides) != 0) {
                _verticalVelocity -= 0.75f * _gravity * Time.deltaTime;
            }
        }

        if (! _jumping && Input.GetKeyDown("c")) {
            _verticalVelocity += _jumpHeight;
            _jumping = true;
        }
        
        Movement();
        if (_ceilingCount > 0) {
            _ceilingCount--;
        }       
    }

    void Movement() {
        Vector3 sidewaysMovementVector = transform.right * _horizontalVelocity;
        Vector3 upDownMovementVector = transform.up * _verticalVelocity;
        Vector3 movementVector = sidewaysMovementVector + upDownMovementVector;
        
        GetComponent<CharacterController>().Move(movementVector * _moveSpeed * Time.deltaTime);
    }

    void OnControllerColliderHit(ControllerColliderHit collider){
        if (collider.transform.CompareTag("Destroy")){
            GameEvents.InvokeResetPlayer();
            Destroy(this.gameObject);
        }
    }
}
