using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using NaughtyAttributes;
using UnityEngine;

public class ElectrifiableItem : NetworkBehaviour, IElectrifiable
{
    [SerializeField] private bool _causeAnotherInteraction;
    
    [ShowIf(nameof(_causeAnotherInteraction))] [field :SerializeField]
    public InteractItem ItemToInteract;

    public virtual void InteractElectricity()
    {
        ItemToInteract.Interact();
    }
}