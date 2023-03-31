using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleAudioController : MonoBehaviour
{

    private AudioSource audioSource;
    public AudioClip song1;
    public AudioClip song2;
    public AudioClip song3;
    public AudioClip audienceClipStart;
    public AudioClip audienceClipEnd;
    public AudioClip spawnClip;

    public GameObject player;
    public GameObject spawnRoom;

    public Animator fadeAnimator;

    private int secondsToWait = 0;
    private bool coroutineStarted = false;

    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.clip = spawnClip;
        SetMusicVolume(0.1f);
        audioSource.Play();
    }


    void Update()
    {
        if (SimpleSongButton.isTriggered && (SimpleSongButton.songNumber == 1 || SimpleSongButton.songNumber == 2 || SimpleSongButton.songNumber == 3) && !coroutineStarted)
            StartCoroutine(PlaySong());

        if (!BackToSpawnScript.isTriggered)
            coroutineStarted = false;

        if (BackToSpawnScript.isTriggered)
        {
            StopCoroutine(PlaySong());
            StopAllCoroutines();
            audioSource.Stop();
            audioSource.clip = spawnClip;
            SetMusicVolume(0.1f);
            audioSource.Play();
            coroutineStarted = true;
        }

        if (!staticTeleporter.playerIsAtSpawn)
            SetMusicVolume(SimpleVolumeSlider.volumeValue);
    }

    IEnumerator PlaySong()
    {
        int temp_song_number = SimpleSongButton.songNumber;
        SimpleSongButton.songNumber = 0;

        audioSource.Stop();
        SetMusicVolume(0.5f);

        audioSource.clip = audienceClipStart;
        audioSource.Play();

        yield return new WaitForSeconds(15);

        if (temp_song_number == 1)
        {
            audioSource.clip = song1;
            secondsToWait = 205; // 3 minutes 25 seconds
        }

        if (temp_song_number == 2)
        {
            audioSource.clip = song2;
            secondsToWait = 175; // 2 minutes 55 seconds
        }

        if (temp_song_number == 3)
        {
            audioSource.clip = song3;
            secondsToWait = 139; // 2 minutes 19 seconds
        }

        // extra safety check (maybe player already went back to spawn again and something did not work properly)
        if (!staticTeleporter.playerIsAtSpawn)
            audioSource.Play();

        yield return new WaitForSeconds(secondsToWait);

        audioSource.clip = audienceClipEnd;
        SimpleSongButton.songNumber = 0;
        // extra safety check (maybe player already went back to spawn again and something did not work properly)
        if (!staticTeleporter.playerIsAtSpawn)
            audioSource.Play();

        yield return new WaitForSeconds(15);

        audioSource.clip = null;

        fadeAnimator.SetTrigger("FadeOut");
        yield return new WaitForSeconds(1);
        staticTeleporter.SendPlayerToSpawn(player, spawnRoom);
        fadeAnimator.ResetTrigger("FadeOut");
        fadeAnimator.SetTrigger("FadeIn");
        yield return new WaitForSeconds(1);
        fadeAnimator.ResetTrigger("FadeIn");
    }



    // Extra methods

    public void SetMusicVolume(float volume)
    {
        audioSource.volume = volume;
    }

    public void PlayMusic(AudioClip musicClip)
    {
        audioSource.Play();
    }

    public void StopMusic()
    {
        audioSource.Stop();
    }
}
