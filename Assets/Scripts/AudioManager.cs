using System;
using UI.Window.Entities;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    [SerializeField] private Slider volumeEntity;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        DontDestroyOnLoad(transform.gameObject);
        
        try
        {
            audioSource.volume = volumeEntity.Value;
        }
        catch (NullReferenceException)
        {
            Debug.LogWarning("Value of volume entity is not available. Leaving volume level at default");
        }
        audioSource.Play();
    }

    private void OnEnable()
    {
        volumeEntity.OnValueChanged.AddListener(AdjustVolume);
    }

    private void OnDisable()
    {
        volumeEntity.OnValueChanged.RemoveListener(AdjustVolume);
    }

    private void AdjustVolume(float newVolume)
    {
        audioSource.volume = newVolume;
    }
}