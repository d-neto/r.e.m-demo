using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbleToAim : MonoBehaviour
{
    [SerializeField] protected Transform center;

    public Transform Get() => this.center;
}
