using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;



public class Fiend : Unit
{
    [Header("Fiend ScriptableObject")]
    [SerializeField] protected FiendSO data;

    protected NavMeshAgent agent;
    protected List<Material> materials = new();
    protected Color materialBaseColor = Color.blanchedAlmond;


    protected virtual void Awake()
    {
        Stats = new(data.stats);
        
        agent = GetComponent<NavMeshAgent>();
        SetAgentData();
        InstantiateModel();
    }




    #region Model
    private void InstantiateModel()
    {
        if (data == null || data.modelPrefab == null)
        {
            Debug.LogError($"[{name}] No hay modelPrefab asignado en el FiendSO");
            return;
        }

        bool hasModel = transform.Cast<Transform>().Any(child => child.CompareTag("FiendModel"));
        if (materials.Count > 0) hasModel = true;

        if (!hasModel)
            Instantiate(data.modelPrefab, transform);

        GetModelMaterials();
    }

    protected void GetModelMaterials()
    {
        GameObject model = null;

        foreach (Transform child in transform)
        {
            if (child.CompareTag("FiendModel"))
            {
                model = child.gameObject;
                break;
            }
        }
        if(model != null)
        {
            List<MeshRenderer> meshes = model.GetComponentsInChildren<MeshRenderer>().ToList();

            foreach (MeshRenderer mesh in meshes)
            {
                materials.AddRange(mesh.materials);
            }
        }
        foreach(Material material in materials)
        {
            material.SetColor("_BaseColor", materialBaseColor);
        }
    }
    #endregion



    #region NavMesh Agent
    private void SetAgentData()
    {
        agent.radius = data.radius;
        agent.speed = data.speed;
        agent.stoppingDistance = data.stoppingDistance;
        agent.avoidancePriority = data.priority;
    }
    #endregion



    #region Damage
    public override void RecieveDamage(int damage)
    {
        base.RecieveDamage(damage);

        StartCoroutine(Damaged());

        if (Stats.CurrentHealth <= 0)
        {
            foreach (Material material in materials) material.SetColor("_BaseColor", Color.black);
        }
    }
    private IEnumerator Damaged()
    {
        foreach(Material material in materials) material.SetColor("_BaseColor", Color.red);

        yield return new WaitForSeconds(.5f);

        foreach (Material material in materials) material.SetColor("_BaseColor", materialBaseColor);
    }
    #endregion

}

