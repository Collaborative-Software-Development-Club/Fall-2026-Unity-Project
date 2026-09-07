using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;

    public static AudioManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogWarning("AudioManager instance is null");
            }
            return _instance;
        }
    }

    [Header("AudioClips")] 
    [SerializeField] private AudioClip[] sfxClips;
    [SerializeField] private AudioClip[] musicClips;
    private AudioClip _currentMusicClip;

    private Queue<AudioClip> _musicQueue = new();
    private float _pausedTimeStamp;
    private bool _queuePaused;
    
    [Header("AudioSources")]
    [SerializeField] private AudioSource sfxAudioSource;
    [SerializeField] private AudioSource musicAudioSource;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return; 
        } 
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }


    private void Start()
    {
        StartCoroutine(PlaySongQueue());
    }

    public void PlaySfxClip(int value)
    {
        if (value < 0 || value >= sfxClips.Length)
        {
            sfxAudioSource.Pause();
            return;
        }
        
        sfxAudioSource.clip = sfxClips[value];
        sfxAudioSource.Play();
    }

    public void PlaySfxClip(AudioClip clip)
    {
        if (clip == null) return;
        
        sfxAudioSource.clip = clip;
        sfxAudioSource.Play();
    }

    public void PlayMusicClipInterruptQueue(int value)
    {
        if (value < 0 || value >= musicClips.Length) return;

        StopAllCoroutines();
        StartCoroutine(InterruptMusic(musicClips[value]));
    }
    public void AddMusicClipToQueue(AudioClip clip)
    {
        _musicQueue.Enqueue(clip);
    }
    
    private IEnumerator InterruptMusic(AudioClip interruptClip)
    {
        _queuePaused = true;

        musicAudioSource.Stop();
        musicAudioSource.clip = interruptClip;
        musicAudioSource.Play();

        yield return new WaitUntil(() => !musicAudioSource.isPlaying);

        _queuePaused = false;

        StartCoroutine(PlaySongQueue());
    }
    private IEnumerator PlaySongQueue()
    {
        while (true)
        {
            if (_queuePaused)
            {
                yield return null;
                continue;
            }

            if (_musicQueue.Count == 0)
            {
                AudioClip[] shuffledClips = (AudioClip[])musicClips.Clone();
                Shuffle(shuffledClips);

                if (shuffledClips.Length > 1 && shuffledClips[0] == _currentMusicClip)
                {
                    int randomIndex = UnityEngine.Random.Range(1, shuffledClips.Length);
                    (shuffledClips[0], shuffledClips[randomIndex]) = (shuffledClips[randomIndex], shuffledClips[0]);
                }

                foreach (AudioClip clip in shuffledClips)
                    _musicQueue.Enqueue(clip);
            }

            _currentMusicClip = _musicQueue.Dequeue();
            musicAudioSource.clip = _currentMusicClip;
            musicAudioSource.Play();

            float startTime = Time.time;
            while (Time.time - startTime < musicAudioSource.clip.length)
            {
                yield return null;
            }
        }
    }
    
    private void Shuffle<T>(T[] array)
    {
        for (int i = array.Length - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);
            (array[i], array[j]) = (array[j], array[i]);
        }
    }
}