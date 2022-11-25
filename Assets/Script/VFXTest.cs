using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VFXTest : MonoBehaviour
{
    private VisualEffect visualEffect;

    private void Awake()
    {
    }
    void Start()
    {
        visualEffect = GetComponent<VisualEffect>();
    }

    void Update()
    {
        visualEffect.SetFloat("MaxSpeed", Time.time);
    }
}
