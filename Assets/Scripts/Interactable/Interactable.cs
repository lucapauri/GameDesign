using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    protected GameObject _Parent;

    public GameObject Parent
    {
        get => _Parent;
        protected set => _Parent = value;
    }

    protected virtual void Start()
    {
        _Parent = Parent;
    }

    public abstract void Interact(GameObject interactable);
}

