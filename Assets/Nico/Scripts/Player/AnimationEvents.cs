using System;
using System.Collections.Generic;
using UnityEngine;






public class AnimationEvents : MonoBehaviour
{
    #region Events
    public event Action OnDeathAnimationEnd;
    public event Action OnHurtAnimationEnd;
    public event Action OnAttackAnimationEnd;
    public event Action OnAttackAnimationStarts;
    public event Action OnResurrectAnimationEnd;
    public event Action OnComboEnabled;
    public event Action OnFootstep;
    #endregion


    private GameObject parent;
    private SphereCollider attackCollider;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource footstepSource;
    [SerializeField] private AudioSource wooshSource;
    [SerializeField] private AudioSource voiceSource;

    [Header("Audio SFX")]
    [SerializeField] private List<AudioClip> footstepSounds = new();
    [SerializeField] private List<AudioClip> attackWoosh;
    [SerializeField] private List<AudioClip> hurtSounds = new();
    [SerializeField] private List<AudioClip> deathSounds = new();





    #region Life Cycle
    private void Awake()
    {
        attackCollider = GetComponentInChildren<SphereCollider>();
        parent = transform.parent.gameObject;
    }
    #endregion



    #region Death
    public void DeathAnimationStarts()
    {
        PlayClip(deathSounds, voiceSource);
    }
    public void DeathAnimationEnd()
    {
        OnDeathAnimationEnd?.Invoke();
    }
    #endregion



    #region Hurt
    public void HurtAnimationEnd()
    {
        OnHurtAnimationEnd?.Invoke();
    }
    public void HurtAnimationStart()
    {
        PlayClip(hurtSounds, voiceSource);
    }
    #endregion


    #region Attack
    public void EnableAttackCollider()
    {
        attackCollider.enabled = true;
    }
    public void DisableAttackCollider()
    {
        attackCollider.enabled = false;
        if (parent.CompareTag("Player")) OnComboEnabled?.Invoke();
    }
    public void AttackAnimationStarts()
    {
        OnAttackAnimationStarts?.Invoke();
        
        if (parent.CompareTag("Player"))
        {
            PlayClip(attackWoosh, wooshSource);
        }
    }
    public void AttackAnimationEnd()
    {
        OnAttackAnimationEnd?.Invoke();
    }
    public void Woosh()
    {
        PlayClip(attackWoosh, wooshSource);
    }
    #endregion


    #region Resurrection
    public void ResurrectAnimationStarts()
    {

    }
    public void ResurrectAnimationEnd()
    {
        OnResurrectAnimationEnd?.Invoke();
    }
    #endregion



    public void Footsteps()
    {
        PlayClip(footstepSounds, footstepSource);
        OnFootstep?.Invoke();
    }




    private int randomIndex;
    private void PlayClip(List<AudioClip> clips, AudioSource source)
    {
        if (clips.Count == 0) return;

        randomIndex = UnityEngine.Random.Range(0, clips.Count);

        if(clips.Count < 3)
        {
            source.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
        }
        else
        {
            source.pitch = 1f;
        }

        source.PlayOneShot(clips[randomIndex]);
    }

}
