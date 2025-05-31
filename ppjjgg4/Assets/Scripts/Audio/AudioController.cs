using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * AudioController is a singleton class that manages audio playback in the game.
 * It handles background music and sound effects, ensuring only one instance exists.
 * To add new sound effects: add audioClip param and method associated with it.
 */
public class AudioController : MonoBehaviour
{
    [Header("Hyper parameters")]
    [Range(0f, 1f)]
    [SerializeField] private float volumeSFX = 0.5f;

    [Range(0f, 1f)]
    [SerializeField] private float volumeBackgroundMusic = 0.5f;


    [Header("Sound Effects")]
    [SerializeField] private AudioClip plantingClip;
    [SerializeField] private AudioClip buttonClickClip;
    [SerializeField] private AudioClip shovelClip;
    [SerializeField] private AudioClip trophySuccesClip;

    private AudioSource soundEffectsSource;
    private AudioSource backgroundMusicSource;

    private static AudioController instance;
    private string lastLoadedScene;
    private AudioClip currentBackgroundMusic;

    [Header("Background Music")]
    [SerializeField] private AudioClip musicMenu; // Musique de fond pour le menu (et le tutoriel)
    [SerializeField] private AudioClip musicGame; //  Musique de fond pour le jeu

    public static AudioController Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject soundEffectsHelperObject = new GameObject("SoundEffectsHelper");
                instance = soundEffectsHelperObject.AddComponent<AudioController>();
                DontDestroyOnLoad(soundEffectsHelperObject);
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        soundEffectsSource = gameObject.AddComponent<AudioSource>();
        backgroundMusicSource = gameObject.AddComponent<AudioSource>();

        backgroundMusicSource.loop = true;
        backgroundMusicSource.playOnAwake = true;
        backgroundMusicSource.volume = volumeBackgroundMusic;

        DontDestroyOnLoad(gameObject);

        AudioClip backgroundMusic = GetBackgroundMusicForScene(SceneManager.GetActiveScene().name);
        if (backgroundMusic != null)
        {
            backgroundMusicSource.clip = backgroundMusic;
            backgroundMusicSource.Play();
            currentBackgroundMusic = backgroundMusic; // Stockez la musique de fond actuelle
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
        lastLoadedScene = SceneManager.GetActiveScene().name;
    }

    public void ChangeVolume(float newVolume)
    {
        volumeBackgroundMusic = newVolume;
        if (backgroundMusicSource != null)
        {
            backgroundMusicSource.volume = volumeBackgroundMusic;
        }

        if (soundEffectsSource != null)
        {
            soundEffectsSource.volume = volumeSFX;
        }
    }

    public void StopSound()
    {
        // Arrêtez tous les bruitages en cours
        soundEffectsSource.Stop();
    }


    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Last loaded scene: " + lastLoadedScene);
        Debug.Log("New loaded scene: " + scene.name);

        if (backgroundMusicSource == null)
            return;

        if (scene.name.Equals(lastLoadedScene))
        {
            Debug.Log("Same scene as before");
            //StopBackgroundMusic();
            //PlayBackgroundMusic();
            return;
        }

        // Si la musique de fond actuelle est la même que celle de la nouvelle sc�ne, ne la changez pas
        if (GetBackgroundMusicForScene(scene.name).Equals(currentBackgroundMusic))
        {
            Debug.Log("Same music as before but with a different scene");
            return;
        }

        lastLoadedScene = scene.name;

        AudioClip backgroundMusic = GetBackgroundMusicForScene(scene.name);
        if (backgroundMusic != null)
        {
            backgroundMusicSource.clip = backgroundMusic;
            backgroundMusicSource.Play();
            currentBackgroundMusic = backgroundMusic; // Stockez la musique de fond actuelle
        }
    }

    public void PlayBackgroundMusic()
    {
        if (!backgroundMusicSource.isPlaying && currentBackgroundMusic != null)
        {
            backgroundMusicSource.clip = currentBackgroundMusic; // Reprenez la musique de fond actuelle
            backgroundMusicSource.Play();
        }
    }

    public void PlayPlantingSound()
    {
        MakeSound(plantingClip, volumeSFX);
    }

    public void PlayButtonClickSound()
    {
        MakeSound(buttonClickClip, 0.15f, 1.1f, 1.5f);
    }

    public void PlayShovelSound()
    {
        MakeSound(shovelClip, volumeSFX, 0.8f, 1.2f);
    }

    public void PlayTrophySuccessSound()
    {
        MakeSound(trophySuccesClip, volumeSFX);
    }

    public void PlaySFXInParallel(AudioClip audioClip, float volume = 0.5f, float pitch = 1f)
    {
        soundEffectsSource.pitch = pitch; // Reset pitch to default
        soundEffectsSource.PlayOneShot(audioClip, volume);
    }

    public void StopBackgroundMusic()
    {
        if (backgroundMusicSource.isPlaying)
        {
            backgroundMusicSource.Stop();
        }
    }

    // Rajoute des paramètres pour modifier un peu le son de manière aléatoire
    public void MakeSound(AudioClip originalClip, float volume = 0.5f, float pitchMin = 1f, float pitchMax = 1f, bool hasPriority = true)
    {
        if (soundEffectsSource.isPlaying && !hasPriority)
        {
            return;
        }
        soundEffectsSource.PlayOneShot(originalClip);
        soundEffectsSource.pitch = Random.Range(pitchMin, pitchMax);
        soundEffectsSource.volume = volume;
    }

    private AudioClip GetBackgroundMusicForScene(string sceneName)
    {
        return sceneName switch
        {
            "Game" => musicGame,
            "Tutorial" => musicMenu, // même musique que le menu
            "Menu" => musicMenu,
            _ => musicGame, // Aucune musique spécifique pour cette scène
        };
    }

}