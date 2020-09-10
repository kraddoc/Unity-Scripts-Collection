﻿using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class RobustMusicPlayer : MonoBehaviour
{
    /* 
    This is a music player script wich allows you to easily play, 
    shuffle and repeat entire playlists 
    and tracks from playlists and control the music volume.

    Following functions are all public
    and meant to be called from other scripts
    or added as listeners to events as needed:
    
    Play() starts playing the current track.
    Play(int index) plays specific track in playlist.
    Play(AudioClip clip) plays specific track.
    PlayNext() plays next track in list, PlayLast() - plays previous.
    Restart() stops the current track and resets the playlist to the beggining.
    Shuffle() randomises playlist order when called.
    Stop() stops the current track, Pause() pauses it.
    */

    [Header("Setup")]
    [SerializeField] bool isRandomOrder = true;
    [SerializeField] bool isLooped = true;
    [SerializeField] bool isPlaying = false;
    AudioSource audioSource;
    [Header("Track List")]
    [SerializeField] List<AudioClip> playlist;
    AudioClip currentTrack;
    int currentTrackIndex = 0;

    private void Awake()
    {
        TryGetComponent<AudioSource>(out audioSource);
        audioSource.loop = false;
        audioSource.playOnAwake = false;
        audioSource.Stop();
        if (isRandomOrder)
            Shuffle();
    }

    /// <summary>
    /// Shuffles the playlist using Fisher–Yates shuffle.
    /// </summary>
    public void Shuffle()
    {
        // swaps each track in playlist with another random track.
        for (int i = 0; i < playlist.Count; i++)
        {
            int randomIndex = Random.Range(0, playlist.Count - 1);
            AudioClip cachedSong = playlist[randomIndex];
            playlist[randomIndex] = playlist[i];
            playlist[i] = cachedSong;
        }
    }
    public void Restart()
    {
        audioSource.Stop();
        isPlaying = false;
        currentTrackIndex = 0;
    }
    public void Stop()
    {
        audioSource.Stop();
        isPlaying = false;
    }
    public void Pause()
    {
        audioSource.Pause();
        isPlaying = false;
        throw new System.NotImplementedException("Pause functionality in the works");
    }
    
    public void Play()
    {
        audioSource.PlayOneShot(playlist[currentTrackIndex]);
        isPlaying = true;
    }
    public void Play(int index)
    {
        audioSource.Stop();
        isPlaying = true;
        currentTrackIndex = index;
        Play();
    }
    public void Play(AudioClip clip)
    {
        audioSource.Stop();
        isPlaying = true;
        audioSource.PlayOneShot(clip);
    }
    public void PlayNext()
    {
        audioSource.Stop();
        currentTrackIndex = currentTrackIndex == playlist.Count - 1 ? 0 : currentTrackIndex + 1;
        Play();
    }
    public void PlayPrevious()
    {
        audioSource.Stop();
        currentTrackIndex = currentTrackIndex == 0 ? playlist.Count - 1 : currentTrackIndex - 1;
        Play();
    }

    /// <summary>
    /// Main logic is contained here.
    /// </summary>
    private void Controller()
    {
        Mathf.Clamp(currentTrackIndex, 0, playlist.Count - 1);
        if (isPlaying && !audioSource.isPlaying)
        {
            currentTrackIndex++;
            if (currentTrackIndex >= playlist.Count)
            {
                if (isLooped)
                    currentTrackIndex = 0;
                else
                    isPlaying = false;
            }
            currentTrack = playlist[currentTrackIndex];
            audioSource.PlayOneShot(currentTrack);
        }
    }

    private void TestingControls()
    {
        // Play|Stop on SPACE, Shuffle on S, Restart on R, Pause on P, 
        //Right|Left for PlayNext and Previous, 
        //1 and 2 for playing first and second song in list.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isPlaying)
                Stop();
            else
                Play();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            Shuffle();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Restart();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            Pause();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            PlayNext();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            PlayPrevious();
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Play(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Play(2);
        }
    }

    private void Update()
    {
        Controller();
    }



}
