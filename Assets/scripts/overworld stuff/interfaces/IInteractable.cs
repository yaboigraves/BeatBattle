using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    bool Interact();
    void notify(InteractionEvent interactionEvent);
}

public enum InteractionEvent
{
    IN_RANGE,
    OUT_OF_RANGE,
    SELECTED,
    UNSELECTED
}