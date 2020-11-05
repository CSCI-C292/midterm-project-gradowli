using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    bool _breakBool = false;
    int _breakCount = 100;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_breakCount == 0) {
            Destroy(this.gameObject);
            GameEvents.InvokeInstantiateBreakable(transform.position.x, transform.position.y, transform.position.z);
        }
        else if (_breakBool) {
            --_breakCount;
        }
        
    }

    public void InitializeBreak() {
        _breakBool  = true;
    }
}
