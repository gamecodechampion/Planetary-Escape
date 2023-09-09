using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading;

public class CollisionHandler : MonoBehaviour
{

    [SerializeField] AudioClip death;

    [SerializeField] AudioClip success;

    [SerializeField] AudioClip troll;

    [SerializeField] ParticleSystem successParticals;

    [SerializeField] ParticleSystem deathParticals;

    [SerializeField] float levelLoad = 2f;

    AudioSource audioSource;

    bool isTransitioning = false;
    bool collisionDisabled = false;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }


    void Update()
    { 
        /*
        if(Input.GetKeyDown("l"))
        {
            LoadNextLevel();
            Debug.Log("Next level loaded");
        }

        else if(Input.GetKeyDown("c"))
        {
            collisionDisabled = !collisionDisabled; // toggle collision detection
        }
        */
        // code used to cheat through levels for testing purposes
    }

    void OnCollisionEnter(Collision other) 
    {
        if(isTransitioning || collisionDisabled) { return; }

        switch(other.gameObject.tag)
        {
           case "Friendly":
                Debug.Log("This is a friendly object");
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            case "Troll":
                deathParticals.Play();
                audioSource.PlayOneShot(troll);
                Thread.Sleep(1000);
                Application.Quit();
                break;
            default:
                StartCrashSequence(); 
                break;
        }
    }
    void StartSuccessSequence()
    {
        isTransitioning = true;
        successParticals.Play();
        audioSource.Stop();
        audioSource.PlayOneShot(success);
        GetComponent<Movement>().enabled = false;
        Invoke("LoadNextLevel", levelLoad);
    }

    void StartCrashSequence()
    {
        isTransitioning = true;
        deathParticals.Play();
        audioSource.Stop();
        audioSource.PlayOneShot(death);
        GetComponent<Movement>().enabled = false;
        Invoke("ReloadLevel", levelLoad);
    }

    void ReloadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if(nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
        SceneManager.LoadScene(currentSceneIndex + 1);
    }
}