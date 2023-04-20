using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Gun", menuName = "Guns")]
public class Guns : ScriptableObject
{
    public new string name;
    public float dmg;
    public int startingAmmo;
}
