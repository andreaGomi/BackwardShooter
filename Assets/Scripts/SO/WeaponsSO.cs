using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "ScriptableObjects/Weapons")]
public class WeaponsSO : ScriptableObject
{
	public float fireRate;
	public float damage;
}
