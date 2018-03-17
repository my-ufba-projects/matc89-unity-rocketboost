using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour {

    [SerializeField] Vector3 movementVector = new Vector3(10f,10f,10f);
    [SerializeField] float period = 2f;

    float movementFactor;

    Vector3 startingPos;

	// Use this for initialization
	void Start () {
        startingPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {

        if (period <= Mathf.Epsilon)
            return; // Disallowing period being 0. Itsn't right to compare floats with ==, that's why we use mathf.epsilon (smallest float)

        float cycles = Time.time / period;

        const float tau = Mathf.PI * 2f; // around 6.28
        float rawSinWave = Mathf.Sin(tau * cycles);

        movementFactor = rawSinWave/2f + 0.5f;

        Vector3 offset = movementVector * movementFactor;
        transform.position = startingPos + offset;
	}
}
