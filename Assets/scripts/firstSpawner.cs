using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class firstSpawner : MonoBehaviour
{
    private Transform _firstTarget;

    private void Start()
    {
        _firstTarget = GameObject.FindObjectOfType<FirstTarget>().transform;
    }

    public Transform GetTarget()
    {
      return _firstTarget; 
    }
}

