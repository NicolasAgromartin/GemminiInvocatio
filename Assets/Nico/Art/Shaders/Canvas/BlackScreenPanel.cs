using UnityEngine;

public class BlackScreenPanel : MonoBehaviour
{
    public Material material;


    void Update()
    {
        material.SetFloat("_UnscaledTime", Time.unscaledTime);
    }
}
