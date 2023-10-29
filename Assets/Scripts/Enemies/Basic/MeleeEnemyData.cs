using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

[CreateAssetMenu(fileName="newMeleeEnemyData", menuName="Data/Enemies Data/Melee")]
public class MeleeEnemyData : ScriptableObject 
{
    [Header("Stats.")]
    public float life;
    public float damage;
    public float endurance;

    [Header("Movement")]
    public float speed;
    public AnimatorController animator;
}
