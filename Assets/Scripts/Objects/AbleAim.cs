using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbleAim : MonoBehaviour
{
    [SerializeField] protected Transform center;

    public Transform Get() => this.center;
}
