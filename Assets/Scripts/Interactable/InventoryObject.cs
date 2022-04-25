using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GameObject))]

public class InventoryObject : Interactable
{
    private GameObject gameObject;
    private Collider _collider;

    protected override void Start()
    {
        base.Start();
        gameObject = GetComponent<GameObject>();

    }

    

    public override void Interact(GameObject justin)
    {
        // implementa se justin deve fare quacosa con l'oggetto.
        Debug.Log("interact");
    }
}