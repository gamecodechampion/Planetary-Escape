using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [SerializeField] float thrustSpeed = 100f;
    [SerializeField] float rotationSpeed = 100f;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] ParticleSystem rocketJet;
    [SerializeField] ParticleSystem rightBooster;
    [SerializeField] ParticleSystem leftBooster;
    Rigidbody rb;
    AudioSource audioSource;
    Controller control;

    void Awake()
    {
        control = new Controller();
        control.controls.RotateRight.performed += ctx => RotateRight();
        control.controls.RotateLeft.performed += ctx => RotateLeft();
        control.controls.Boost.performed += ctx => StartThrusting();
    }

    void OnEnable()
    {
        control.Enable();
    }
    
    void OnDisable()
    {
        control.Disable();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = rb.GetComponent<AudioSource>();
    }

    void Update()
    {
        ProcessThrust();   
        ProcessRotation();
    }

    public void ProcessThrust()
    {
        if(Input.GetKey(KeyCode.Space) || control.controls.Boost.ReadValue<float>() > 0f)
        {
            StartThrusting();
        }
        else
        {
            StopThrusting();
        }
    }

    public void StopThrusting()
    {
        audioSource.Stop();
        rocketJet.Stop();
    }

    void StartThrusting()
    {
        rb.AddRelativeForce(Vector3.up * thrustSpeed * Time.deltaTime);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngine);
        }
        if (!rocketJet.isPlaying)
        {
            rocketJet.Play();
        }
    }

    public void ProcessRotation()
    {
        if(Input.GetKey(KeyCode.A) || control.controls.RotateLeft.ReadValue<float>() > 0f)
        {
            RotateLeft();
        }

        else if(Input.GetKey(KeyCode.D) || control.controls.RotateRight.ReadValue<float>() > 0f)
        {
            RotateRight();
        }
        else
        {
            StopRotating();
        }
    }

    public void StopRotating()
    {
        leftBooster.Stop();
        rightBooster.Stop();
    }

    public void RotateRight()
    {
        ApplyRotation(-rotationSpeed);
        if (!rightBooster.isPlaying)
        {
            rightBooster.Play();
        }
    }

    public void RotateLeft()
    {
        ApplyRotation(rotationSpeed);
        if (!leftBooster.isPlaying)
        {
            leftBooster.Play();
        }
    }

    void ApplyRotation(float rotationThisFrame)
    {
        rb.freezeRotation = true; // freezing the rotation so we can manually rotate
        transform.Rotate(Vector3.forward * rotationThisFrame * Time.deltaTime);
        rb.freezeRotation = false;
    }
}