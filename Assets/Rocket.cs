using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {

    Rigidbody rigidBody;
    AudioSource myAudio;

	// Use this for initialization
	void Start ()
    {
        rigidBody = GetComponent<Rigidbody>();
        myAudio = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        Thurst();
        Rotate();
	}

    private void Thurst()
    {
        if (Input.GetKey(KeyCode.Space)) // Ao pressionar espaço
        {
            rigidBody.AddRelativeForce(Vector3.up);
            if (!myAudio.isPlaying) // Se áudio não está tocando, executar trilha
                myAudio.Play();
        }
        else
        {
            if (myAudio.isPlaying) // Se áudio está tocando, interromper trilha
                myAudio.Stop();
        }
    }

    private void Rotate()
    {
        rigidBody.freezeRotation = true;

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward); // Rotação no sentido anti-horário
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward); // Rotação no sentido horário (Atentar ao uso do sinal negativo).
        }

        rigidBody.freezeRotation = false;
    }
}
