using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;



public class Weapon : MonoBehaviour
{
    public int Damage { get; private set;}
    public bool IsEnemy => isEnemy;
    [SerializeField] bool isEnemy = false;
    [SerializeField] private LayerMask damageableLayer;

    private GameObject parent;
    private GameObject impacted;


    [Header("Impacted SFX")]
    [SerializeField] private AudioSource impactSource;
    [SerializeField] private List<AudioClip> impactSounds;


    private void Awake()
    {
        parent = gameObject.transform.root.gameObject;
    }
    private void OnTriggerEnter(Collider other)
    {
        impacted = other.gameObject;

        if (parent.CompareTag("Player") || parent.CompareTag("PlayerMinion"))
        {
            if (!impacted.CompareTag("Enemy")) return;

            PlayImpactSFX();
            if (parent.CompareTag("Player"))
                StartCoroutine(HitStop());
        }
        else if (parent.CompareTag("Enemy") && (impacted.CompareTag("Player") || impacted.CompareTag("PlayerMinion")))
        {
            PlayImpactSFX();
        }
    }
    //if (((1 << other.gameObject.layer) & damageableLayer.value) == 0) return; // si el other esta dentro de la layer damageable

    //impacted = other.transform.parent.gameObject;


    //if (!isEnemy)
    //{

    //    if (impacted.transform.parent.CompareTag("Enemy"))
    //    {
    //        Debug.Log("Attacked an enemy");
    //        impacted.transform.parent.GetComponent<Unit>().RecieveDamage(Damage);
    //    }
    //}
    //else
    //{
    //    if (impacted.CompareTag("Player") || impacted.CompareTag("PlayerMinion"))
    //    {
    //        impacted.GetComponent<Unit>().RecieveDamage(Damage);
    //    }
    //}





    private void PlayImpactSFX()
    {
        if (impactSounds.Count > 0)
        {
            var clip = impactSounds[Random.Range(0, impactSounds.Count)];
            impactSource.PlayOneShot(clip);
        }
    }


    private readonly float hitStopRate = .15f;
    private readonly float hitStopTime = .2f;
    private IEnumerator HitStop()
    {
        Time.timeScale = hitStopRate;
        yield return new WaitForSecondsRealtime(hitStopTime);
        Time.timeScale = 1f;
    }



    public void SetIsEnemy(bool isEnemy) => this.isEnemy = isEnemy;
    public void SetWeaponDamage(int damage) => Damage = damage;
}
