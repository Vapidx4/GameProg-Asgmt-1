using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrapComponent : MonoBehaviour
{
    public float amp;
    public float freq;
    private Vector3 _initPos;
    public bool moveTrap = false; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            GameManager.Instance.score = 0;
        }
    }

    private void Start()
    {
        _initPos = this.transform.position;
    }

    private void Update()
    {
        if (moveTrap)
        {
            MoveTrap();
        }
    }

    private void MoveTrap()
    {
        transform.position = new Vector3(_initPos.x, Mathf.Sin(Time.time * freq) * amp + _initPos.y, 0);
    }
}