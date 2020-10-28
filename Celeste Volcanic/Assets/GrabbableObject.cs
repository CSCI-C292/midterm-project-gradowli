﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbableObject : MonoBehaviour
{
    int _count = 0;
    bool _directionUp = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_directionUp && _count % 10 == 0) {
            Vector3 temp = new Vector3(transform.position.x, transform.position.y + 0.03f, transform.position.z); 
            transform.position = temp;
        }
        else if (_count % 10 == 0) {
            Vector3 temp = new Vector3(transform.position.x, transform.position.y - 0.03f, transform.position.z); 
            transform.position = temp;
        }

        if (_count == 50) {
            _directionUp = ! _directionUp;
            _count = 0;
        }
        else {
            _count += 1;
        }
    }
}
