using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReferenceGroup<T> : MonoBehaviour where T : System.Enum
{
    public GameObject this[T item]
    {
        get
        {
            return null;
        }
    }
}
