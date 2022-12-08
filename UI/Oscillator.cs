using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillator : MonoBehaviour
{

    [SerializeField] Vector3 movementVector = new Vector3(10f, 10f, 10f);
    [Range(0, 10)][SerializeField] float periodX = 2f;
    [Range(0, 10)][SerializeField] float periodY = 2f;

    float movementFactor; // 0 for not moved, 1 for fully moved.
    Vector3 startingPos;

    // Use this for initialization
    void Start()
    {
        startingPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // todo protect against period is zero
        float cyclesX = Time.time / periodX; // grows continually from 0
        float cyclesY = Time.time / periodY; // grows continually from 0

        const float tau = Mathf.PI * 2f; // about 6.28
        float rawSinWaveX = Mathf.Sin(cyclesX * tau); // goes from -1 to +1
        float rawSinWaveY = Mathf.Sin(cyclesY * tau); // goes from -1 to +1

        float movementFactorX = rawSinWaveX / 2f + 0.5f;
        float movementFactorY = rawSinWaveY / 2f + 0.5f;
        Vector3 offset = new Vector3(movementFactorX * movementVector.x, movementFactorY * movementVector.y, 0);
        transform.position = startingPos + offset;
    }
}
