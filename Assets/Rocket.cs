﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {

    [SerializeField] float mainThrust = 1000f; // SerializeField permite que seja editado no Inspector, mas não fora do script, enquanto public são os dois.
    [SerializeField] float rotThrust = 100f;
    
    Rigidbody rigidBody;
    AudioSource myAudio;

    enum State { Alive, Dying, Transcending }
    State state;

	// Use this for initialization
	void Start ()
    {
        rigidBody = GetComponent<Rigidbody>();
        myAudio = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (state == State.Alive)
        {
            Thurst();
            Rotate();
        }
	}

    void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive)
            return;

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                print("OK");
                break;
            case "Finish":
                state = State.Transcending;
                Invoke("LoadNextScene", 1f);
                break;
            default:
                state = State.Dying;
                if (myAudio.isPlaying)
                    myAudio.Stop();
                Invoke("LoadFirstLevel", 1f);
                break;
        }
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(1);
    }

    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }

    private void Thurst()
    {
        if (Input.GetKey(KeyCode.Space)) // Ao pressionar espaço
        {
            float thurstByFrame = mainThrust * Time.deltaTime; // Time.deltaTime é usado para padronizar a velocidade de acordo com o Frame de cada um

            rigidBody.AddRelativeForce(Vector3.up * thurstByFrame);

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
        rigidBody.freezeRotation = true; // Desativando rotação através da física, deixando manual (usuário)

        float rotationByFrame = rotThrust * Time.deltaTime; // Time.deltaTime é usado para padronizar a velocidade de acordo com o Frame de cada um

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationByFrame); // Rotação no sentido anti-horário
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationByFrame); // Rotação no sentido horário (Atentar ao uso do sinal negativo).
        }

        rigidBody.freezeRotation = false; // Ativando rotação através da física, deixando automática (Engine)
    }
}
