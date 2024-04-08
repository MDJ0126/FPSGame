using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(CharacterData), menuName = "ScriptableObjects/" + nameof(CharacterData))]
public class CharacterData : ScriptableObject
{
    public int id;
    public float maxHp;
    public float moveSpeed;
}
