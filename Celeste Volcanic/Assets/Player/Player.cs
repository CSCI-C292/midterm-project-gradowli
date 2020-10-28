using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    [SerializeField] float _moveSpeed = 3f;
    [SerializeField] float _jumpHeight = 2f;
    [SerializeField] float _wallJumpSpeed = 1f;
    [SerializeField] float _dashSpeed = 8f;
    float _verticalVelocity = 0f;
    float _horizontalVelocity = 0f;
    float _gravity = -6f;
    bool _grounded = false;
    bool _jumping = true;
    bool _side = false;
    bool _c = false;
    int _ceilingCount = 0;
    bool _playerDirectionRight = true;
    float _addedHorizontalVelocity = 0f;
    int _horizontalCount = 0;
    int _horizontalCountLimit = 20;
    int _horizontalMod = 3;
    bool _jumpedLeft = false;
    bool _jumpedRight = false;
    bool _x = false;
    bool _dashed = false;

    CharacterController controller;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_addedHorizontalVelocity == 0f) { //determines wheter added velocity is used from dash/wall jump or if normal velocity from input is used
            _horizontalVelocity = 1.5f * Input.GetAxis("Horizontal");
        }
        else {
            _horizontalVelocity = _addedHorizontalVelocity;
        }


        if (_horizontalVelocity + _addedHorizontalVelocity > 0) { //determines player's direction
            _playerDirectionRight = true;
        }
        else if (_horizontalVelocity + _addedHorizontalVelocity < 0) {
            _playerDirectionRight = false;
        }

        _grounded = controller.isGrounded; //determines if player is on ground
        if (_ceilingCount == 0 && (controller.collisionFlags & CollisionFlags.Above) != 0) { //stops upward velocity if player hits head
            _verticalVelocity = 0f;
            _ceilingCount = 20;
            
        }

        _verticalVelocity += _gravity * Time.deltaTime;

        _side = (controller.collisionFlags & CollisionFlags.Sides) != 0;

        if (_grounded) { //dash/jump cooldowns reset upon touching ground
            _verticalVelocity = 0f;
            _jumping = false;
            _jumpedLeft = false;
            _jumpedRight = false;
            _dashed = false;
        }
        else {
            if (_verticalVelocity < 0 && _horizontalVelocity != 0 && _side) {
                _verticalVelocity -= 0.75f * _gravity * Time.deltaTime;
            }
        }

        _c = Input.GetKeyDown("c");
        _x = Input.GetKeyDown("x");

        if (_c && _side && _jumping) { //c for jump/wall jump, x for dash
            _horizontalCount = 1;
            _horizontalMod = 3;
            _horizontalCountLimit = 20;
            if (_playerDirectionRight && ! _jumpedRight) {
                _addedHorizontalVelocity -= _wallJumpSpeed;
                _verticalVelocity = _jumpHeight;
                _jumpedRight = true;
                _jumpedLeft = false;
            }
            else if (! _playerDirectionRight && ! _jumpedLeft) {
                _addedHorizontalVelocity += _wallJumpSpeed;
                _verticalVelocity = _jumpHeight;
                _jumpedLeft = true;
                _jumpedRight = false;
            }
        }
        else if (! _jumping && _c) {
            _verticalVelocity = _jumpHeight;
            _jumping = true;
        }
        else if (_x && ! _dashed) {
            float temph = Input.GetAxis("Horizontal");
            float tempv = Input.GetAxis("Vertical");
            _horizontalCount = 1;
            _horizontalMod = 20;
            _horizontalCountLimit = 40;
            _dashed = true;
            if (temph < 0) {
                if (tempv < 0) {
                    _addedHorizontalVelocity -= _dashSpeed;
                    _verticalVelocity = -0.5f * _jumpHeight;
                }
                else if (tempv > 0) {
                    _addedHorizontalVelocity -= _dashSpeed;
                    _verticalVelocity = _jumpHeight;
                }
                else {
                    _addedHorizontalVelocity -= _dashSpeed;
                }
            }
            else if (temph > 0) {
                if (tempv < 0) {
                    _addedHorizontalVelocity += _dashSpeed;
                    _verticalVelocity = -0.5f * _jumpHeight;
                }
                else if (tempv > 0) {
                    _addedHorizontalVelocity += _dashSpeed;
                    _verticalVelocity = _jumpHeight;
                }
                else {
                    _addedHorizontalVelocity += _dashSpeed;
                }
            }
            else {
                if (tempv < 0) {
                    _verticalVelocity = -0.5f * _jumpHeight;
                }
                else if (tempv > 0) {
                    _verticalVelocity = _jumpHeight;
                }
                else {
                    _dashed = false;
                }
            }

        }
        
        if (_horizontalCount == _horizontalCountLimit) { //reset counter system for horizontal velocity
            _horizontalCount = 1;
            _addedHorizontalVelocity = 0f;
             _horizontalVelocity = 1.5f * Input.GetAxis("Horizontal");
        }
        else if (_horizontalCount % _horizontalMod == 0) {
            _addedHorizontalVelocity /= 2;
            _horizontalCount += 1;
        }
        else {
            _horizontalCount += 1;
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

    void OnControllerColliderHit(ControllerColliderHit collider) {
        if (collider.transform.CompareTag("Destroy")){
            GameEvents.InvokeResetPlayer();
            GameEvents.InvokeScoreIncreased(-20);
            Destroy(this.gameObject);
        }
        else if (collider.transform.CompareTag("Collect")) {
            Destroy(collider.gameObject);
            GameEvents.InvokeScoreIncreased(100);
        }
        else if (collider.transform.CompareTag("Win")) {
            GameEvents.InvokeResetPlayer();
            GameEvents.InvokeScoreIncreased(100);
            GameEvents.InvokeLevelIncreased();
            Destroy(this.gameObject);
        }
    }
}
