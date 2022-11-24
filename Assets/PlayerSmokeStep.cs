using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerSmokeStep : MonoBehaviour
{
    private bool isCurrentlyWalking;
    private float lastWalkingStart;
    private VisualEffect visualEffect;

    public void UpdateWalkingState(bool isWalking)
    {
        if (isWalking)
        {
            lastWalkingStart = Time.time;
        }
        else
        {
            visualEffect.SetFloat("Walking Time", 0);
        }
        isCurrentlyWalking = isWalking;
    }

    private void Awake()
    {
        visualEffect = GetComponent<VisualEffect>();
    }
    private void Update()
    {
        if (isCurrentlyWalking) visualEffect.SetFloat("Walking Time", Time.time - lastWalkingStart);
    }
}
