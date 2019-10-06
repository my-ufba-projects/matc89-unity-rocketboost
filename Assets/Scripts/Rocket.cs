using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {

    [SerializeField] float mainThrust = 1000f; // SerializeField permite que seja editado no Inspector, mas não fora do script, enquanto public são os dois.
    [SerializeField] float rotThrust = 100f;
    [SerializeField] float levelLoadDelay = 2f;

    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip deathSound;
    [SerializeField] AudioClip winSound;

    [SerializeField] ParticleSystem mainEngineParticle;
    [SerializeField] ParticleSystem deathParticle;
    [SerializeField] ParticleSystem winParticle;

    bool collision = true; 

    Rigidbody rigidBody;
    AudioSource myAudio;
    BoxCollider myBoxCollider;

    enum State { Alive, Dying, Transcending };
    State state;

	// Use this for initialization
	void Start ()
    {
        rigidBody = GetComponent<Rigidbody>();
        myAudio = GetComponent<AudioSource>();
        myBoxCollider = GetComponent<BoxCollider>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        
        if (state == State.Alive)
        {
            ThrustWhenInput();
            RotateWhenInput();
        }

        if(Debug.isDebugBuild)
        {
            ToggleDebugKeys();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive)
            return;

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Finish":
                StartWinSequence();
                break;
            default:
                StartDeathSequence();
                break;
        }
    }

    private void StartWinSequence()
    {
        state = State.Transcending;
        runStateAudio(state);
        winParticle.Play();
        Invoke("LoadNextScene", levelLoadDelay);
    }

    private void runStateAudio(State state)
    {
        if (myAudio.isPlaying)
        {
            myAudio.Stop();
            if (state == State.Transcending)
                myAudio.PlayOneShot(winSound);
            else
                myAudio.PlayOneShot(deathSound);
        }
        else
        {
            if (state == State.Transcending)
                myAudio.PlayOneShot(winSound);
            else
                myAudio.PlayOneShot(deathSound);
        }
    }

    private void StartDeathSequence()
    {
        state = State.Dying;
        runStateAudio(state);
        mainEngineParticle.Stop();
        deathParticle.Play();
        Invoke("LoadFirstLevel", levelLoadDelay);
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(1);
    }

    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }

    private void ThrustWhenInput()
    {
        if (Input.GetKey(KeyCode.Space)) // Ao pressionar espaço
        {
            ApplyThrust();
        }
        else
        {
            if (myAudio.isPlaying) // Se áudio está tocando, interromper trilha
                myAudio.Stop();
            mainEngineParticle.Stop();
        }
    }

    private void ApplyThrust()
    {
        float thurstByFrame = mainThrust * Time.deltaTime; // Time.deltaTime é usado para padronizar a velocidade de acordo com o Frame de cada um

        rigidBody.AddRelativeForce(Vector3.up * thurstByFrame);

        if (!myAudio.isPlaying) // Se áudio não está tocando, executar trilha
            myAudio.PlayOneShot(mainEngine);

        mainEngineParticle.Play();
    }

    private void RotateWhenInput()
    {
        rigidBody.freezeRotation = true; // Desativando rotação através da física, deixando manual (usuário)

        float rotationByFrame = rotThrust * Time.deltaTime; // Time.deltaTime é usado para padronizar a velocidade de acordo com o Frame de cada um

        if (Input.GetKey(KeyCode.A))
            transform.Rotate(Vector3.forward * rotationByFrame); // Rotação no sentido anti-horário 
        else if (Input.GetKey(KeyCode.D))
            transform.Rotate(-Vector3.forward * rotationByFrame); // Rotação no sentido horário (Atentar ao uso do sinal negativo).
        

        rigidBody.freezeRotation = false; // Ativando rotação através da física, deixando automática (Engine)
    }

    private void ToggleDebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))
            LoadNextScene();

        if (Input.GetKeyDown(KeyCode.C))
        {
            if (collision)
            {
                collision = false;
                myBoxCollider.enabled = false;
            }
            else
            {
                collision = true;
                myBoxCollider.enabled = true;
            }
        }
    }

}
