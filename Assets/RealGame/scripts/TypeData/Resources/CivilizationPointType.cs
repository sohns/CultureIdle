using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CivilizationPointType : GenericResource
{
	private readonly BiggerNumber baseMaxValue;
	private readonly BiggerNumber baseGrowth;

	private static Dictionary<string,CivilizationPointType> typeMap = new Dictionary<string,CivilizationPointType> ();
	private static object thisLock = new object ();

	//To add a new one, copy this and then add to map;
	private static int i;
	public static readonly CivilizationPointType FOOD = new CivilizationPointType (i++, "FOOD", "Food", "Food placeholder", new BiggerNumber (10), new BiggerNumber (1.1));
	public static readonly CivilizationPointType WOOD = new CivilizationPointType (i++, "WOOD", "Wood", "Wood placeholder", new BiggerNumber (10), new BiggerNumber (1.2));
	public static readonly CivilizationPointType BONE = new CivilizationPointType (i++, "BONE", "Bone", "Bone placeholder", new BiggerNumber (10), new BiggerNumber (1.3));
	public static readonly CivilizationPointType HIDE = new CivilizationPointType (i++, "HIDE", "Hide", "Hide placeholder", new BiggerNumber (10), new BiggerNumber (1.4));
	public static readonly CivilizationPointType STONE = new CivilizationPointType (i++, "STONE", "Stone", "Stone placeholder", new BiggerNumber (10), new BiggerNumber (1.5));
	public static readonly CivilizationPointType POPULATION = new CivilizationPointType (i++, "POPULATION", "Population", "Population placeholder", new BiggerNumber (10), new BiggerNumber (1.6));
	public static readonly CivilizationPointType SKIRMISH = new CivilizationPointType (i++, "SKIRMISH", "Skirmish", "Skirmish placeholder", new BiggerNumber (10), new BiggerNumber (1.7));

	public CivilizationPointType (int id, string saveName, string displayName, string description, BiggerNumber baseMaxValue, BiggerNumber baseGrowth) : base (id, saveName, displayName, description)
	{
		this.baseMaxValue = baseMaxValue;
		this.baseGrowth = baseGrowth;
		lock (thisLock) {
			typeMap.Add (saveName, this);
		}
	}
	

	//TODO make this more interesting
	public string ExtraInfo {
		get {
			return this.Description;
		}
	}

	public BiggerNumber BaseMaxValue {
		get {
			return this.baseMaxValue;
		}
	}

	public BiggerNumber BaseGrowth {
		get {
			return this.baseGrowth;
		}
	}

	public static new CivilizationPointType getType (string saveName)
	{
		if (i != typeMap.Count) {
			Debug.LogError ("There is less items in the ResourceTypeMap then ResourceTypes. Please check your id's for uniqueness");
		}
		if (!typeMap.ContainsKey (saveName)) {
			Debug.LogError (saveName);
			return null;
		}
		return typeMap [saveName];
	}

	public static int getTypeMapCount ()
	{
		return typeMap.Count;
	}

	public override bool Equals (object obj)
	{
		if (obj == null)
			return false;
		if (ReferenceEquals (this, obj))
			return true;
		if (obj.GetType () != typeof(ResourceType))
			return false;
		ResourceType other = (ResourceType)obj;
		return Id == other.Id;
	}


	public override int GetHashCode ()
	{
		unchecked {
			return Id.GetHashCode ();
		}
	}
}
