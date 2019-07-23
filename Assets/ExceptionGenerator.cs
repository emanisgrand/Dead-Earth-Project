using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExceptionGenerator : MonoBehaviour
{
    private int zero = 0;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            float exceptionValue = 10 / zero;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            throw new MissingComponentException("Missing component");
        }
    }
}
