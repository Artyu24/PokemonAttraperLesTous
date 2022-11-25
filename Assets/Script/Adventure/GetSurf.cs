using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetSurf : MonoBehaviour, IInteractable
{
    [SerializeField] private DialogueID[] dialogue;

    public void Interact()
    {
        PlayerMovement.Instance.CanWalkOnWater = true;
        DialogueManager.Instance.InitDialogue(this, dialogue);
    }
}
