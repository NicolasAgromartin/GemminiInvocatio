using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAudio : MonoBehaviour
{
    [SerializeField] private AudioSource voiceSource;

    [Header("Audio Clips")]
    [SerializeField] private List<AudioClip> voiceClips;

    private readonly float minDelay = 1f;
    private readonly float maxDelay = 5f;




    public void PlayPatrollingSounds()
    {
        StartCoroutine(VoiceRoutine());
    }

    public void StopPatrollingSounds()
    {
        StopAllCoroutines();
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
