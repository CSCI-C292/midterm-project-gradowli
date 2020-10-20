using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ResetHandler : MonoBehaviour
{
    [SerializeField] GameObject _playerPrefab;
    float _xSpawn = -3.4f;
    float _ySpawn = -3.3f;
    void Awake() {
        GameEvents.ResetPlayer += OnResetPlayer;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnResetPlayer(object sender, EventArgs args) {
        Instantiate(_playerPrefab, new Vector3(_xSpawn, _ySpawn, -1.6f), Quaternion.identity);
    }
}
