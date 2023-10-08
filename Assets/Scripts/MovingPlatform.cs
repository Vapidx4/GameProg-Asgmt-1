using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    
    public float distance = 2.5f;  
    public float speed = 20.0f; 
    private Vector3 _initPos;

    // Start is called before the first frame update
    void Start()
    {
        _initPos = transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = _initPos;
        pos.x += distance * Mathf.Sin (Time.time * speed);
        transform.position = pos;
    }
}
