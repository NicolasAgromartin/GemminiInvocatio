using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAudio : MonoBehaviour
{
    [SerializeField] private AudioSource voiceSource;

    [Header("Audio Clips")]
    [SerializeField] private List<AudioClip> voiceClips;
    [SerializeField] private List<AudioClip> deadClips;

    private readonly float minDelay = 10f;
    private readonly float maxDelay = 20f;




    public void PlayPatrollingSounds()
    {
        StartCoroutine(VoiceRoutine());
    }

    public void StopPatrollingSounds()
    {
        StopAllCoroutines();
    }
    public void PlayDeathSound()
    {
        StopAllCoroutines();
        PlayClip(deadClips, voiceSource);
    }









    private IEnumerator VoiceRoutine()
    {
        while (true)
        {
            float waitTime = Random.Range(minDelay, maxDelay);
            yield return new WaitForSeconds(waitTime);

            PlayClip(voiceClips, voiceSource);
        }
    }




    private int randomIndex;
    private void PlayClip(List<AudioClip> clips, AudioSource source)
    {
        //Debug.Log(clips.Count);

        if (clips.Count == 0) return;

        randomIndex = UnityEngine.Random.Range(0, clips.Count);

        source.PlayOneShot(clips[randomIndex]);
    }



}
