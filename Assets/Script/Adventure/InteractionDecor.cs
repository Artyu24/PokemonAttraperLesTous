using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionDecor : MonoBehaviour, IInteractable
{
    [SerializeField] private DialogueID[] dialogue;

    public void Interact()
    {
        GameManager.Instance.ActualPlayerState = PlayerState.Interaction;
        DialogueManager.Instance.InitDialogue(this, dialogue);
    }
}
