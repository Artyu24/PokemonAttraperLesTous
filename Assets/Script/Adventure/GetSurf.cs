using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetSurf : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        PlayerMovement.Instance.CanWalkOnWater = true;
    }
}
