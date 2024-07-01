using UnityEngine;
using System;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance; // Instancia singleton

    [Header("Música de Fondo")]
    public AudioClip clipMusicaFondo;
    public AudioSource fuenteMusicaFondo;

    [Header("Efectos Sonoros")]
    public AudioClip[] clipEfectoSonoro;
    public AudioSource fuenteEfectoSonoro;

    private void Awake()
    {
        // Implementar Singleton para asegurarse de que solo hay una instancia de AudioManager
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        //PlayMusicaFondo();
    }

    // Reproducir música de fondo
    public void PlayMusicaFondo()
    {
        if (clipMusicaFondo != null)
        {
            fuenteMusicaFondo.clip = clipMusicaFondo;
            fuenteMusicaFondo.loop = true;
            fuenteMusicaFondo.Play();
        }
    }

    // Detener la música de fondo
    public void StopMusicaFondo()
    {
        if (fuenteMusicaFondo.isPlaying)
        {
            fuenteMusicaFondo.Stop();
        }
        
    }

    // Reproducir un efecto de sonido por índice
    public void PlayEfectoSonoro(int index)
    {
        if (index >= 0 && index < clipEfectoSonoro.Length)
        {
            fuenteEfectoSonoro.clip = clipEfectoSonoro[index];
            fuenteEfectoSonoro.Play();
        }
    }

    // Detener un efecto de sonido por índice
    public void StopEfectoSonoro()
    {
        
        if (fuenteEfectoSonoro.isPlaying)
        {
            fuenteEfectoSonoro.Stop();
        }
    }

}
