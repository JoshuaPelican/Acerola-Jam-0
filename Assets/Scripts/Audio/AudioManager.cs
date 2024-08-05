using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static void PlayEffect(AudioClip clip, Vector3 position, float distance, float volume, float pitch)
    {
        AudioSource source = Instantiate(new GameObject($"{clip.name} - Effect Source", typeof(AudioSource))).GetComponent<AudioSource>();
        source.clip = clip;
        source.volume = volume;
        source.pitch = pitch;

        source.transform.position = position;

        source.spatialBlend = 1;
        source.minDistance = distance / 2;
        source.maxDistance = distance;

        source.Play();
    }
}
