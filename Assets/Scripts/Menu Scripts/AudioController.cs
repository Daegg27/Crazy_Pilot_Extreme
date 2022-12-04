using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioController : MonoBehaviour
{

    public static AudioController instance;

    //Public Variables
    public AudioSource menuMusic;
    public bool musicOn;
    public bool noiseIsMuted;


    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
        menuMusic.Play();
        musicOn = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("My Game"))
        {
            menuMusic.Stop();
            musicOn = false;
        } else if (menuMusic.isPlaying == false && SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Menu") && musicOn == false)
        {
            menuMusic.Play();
            musicOn = true;
            
        }

    }

}
