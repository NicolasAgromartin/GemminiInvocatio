using System;
using System.Collections;
using UnityEngine;

public class DarkCrystal : Enemy
{
    public event Action<DarkCrystal> OnDestroy;

    
    
    private readonly float floatingSpeed = 2f;
    private readonly float rotationSpeed = 50f;
    private readonly float floatAmplitude = 0.25f;
    private readonly float intensity = 0.05f;
    private readonly float speed = 40f;





    private void Start()
    {
        StartCoroutine(RotateAndFloat());
    }
    new protected void RecieveDamage(int damage)
    {
        //Stats.CurrentHealth -= damage;

        base.RecieveDamage(damage);
        Debug.Log($"{gameObject} recieved {damage}, {Stats.CurrentHealth} health left");

        if(Stats.CurrentHealth <= 0)
        {
            StartCoroutine(RunDestroySequence());
        }
    }



    private IEnumerator RunDestroySequence()
    {
        OnDestroy?.Invoke(this);
        StartCoroutine(Vibrate());

        foreach (Dissolver dissolver in GetComponentsInChildren<Dissolver>())
        {
            yield return StartCoroutine(dissolver.Dissapear());
        }

        StopAllCoroutines();
        Destroy(gameObject);
    }
    



    private IEnumerator RotateAndFloat()
    {
        Vector3 startPos = transform.position;

        while (enabled)
        {
            float newY = startPos.y + Mathf.Sin(Time.time * floatingSpeed) * floatAmplitude;
            transform.position = new Vector3(startPos.x, newY, startPos.z);

            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);

            yield return null;
        }
    }
    private IEnumerator Vibrate()
    {
        Vector3 startPos = transform.localPosition;

        while (enabled)
        {
            float offsetX = Mathf.Sin(Time.time * speed) * intensity;
            float offsetZ = Mathf.Cos(Time.time * speed * 1.2f) * intensity;

            transform.localPosition = startPos + new Vector3(offsetX, 0, offsetZ);

            yield return null;
        }
        transform.localPosition = startPos;
    }
}


