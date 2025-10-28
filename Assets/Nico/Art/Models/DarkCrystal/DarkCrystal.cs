using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class DarkCrystal : Fiend
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
    protected override  void RecieveDamage(int damage)
    {
        base.RecieveDamage(damage);

        if(Stats.CurrentHealth <= 0)
        {
            StartCoroutine(RunDestroySequence());
        }
    }



    private IEnumerator RunDestroySequence()
    {
        OnDestroy?.Invoke(this);
        StartCoroutine(Vibrate());
        GetComponent<SphereCollider>().enabled = false;
        tag = "Untagged";

        Dismark();
        yield return StartCoroutine(DissolveShards());


        StopAllCoroutines();
        Destroy(gameObject);
    }

    private IEnumerator DissolveShards()
    {
        Dissolver[] dissolvers = GetComponentsInChildren<Dissolver>();
        List<Coroutine> coroutines = new List<Coroutine>();

        // Iniciar todas las corrutinas al mismo tiempo
        foreach (Dissolver dissolver in dissolvers)
        {
            coroutines.Add(StartCoroutine(dissolver.Dissapear()));
        }

        // Esperar hasta que todas hayan terminado
        foreach (Coroutine c in coroutines)
        {
            yield return c;
        }
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


