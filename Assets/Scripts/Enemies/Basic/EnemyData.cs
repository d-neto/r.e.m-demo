using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="newMeleeEnemyData", menuName="Data/Enemies Data/Enemy")]
public class EnemyData : ScriptableObject 
{
    [Header("Stats.")]
    public float life;
    public float damage;
    public float endurance;

    [Header("Rewards")]
    public GameObject XPPrefab;
    public float XPAmount;

    [Header("Movement")]
    public float speed;
    public RuntimeAnimatorController animator;
}
