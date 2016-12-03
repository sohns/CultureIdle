using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ResourceType : GenericResource
{
	private readonly BiggerNumber baseMaxValue;

	private static Dictionary<string,ResourceType> typeMap = new Dictionary<string,ResourceType> ();
	private static object thisLock = new object ();

	//To add a new one, copy this and then add to map;
	private static int i;
	public static readonly ResourceType FOOD = new ResourceType (i++, "FOOD", "Food", "Food placeholder", 150);
	public static readonly ResourceType WOOD = new ResourceType (i++, "WOOD", "Wood", "Wood placeholder", 250);
	public static readonly ResourceType BONE = new ResourceType (i++, "BONE", "Bone", "Bone placeholder", 20);
	public static readonly ResourceType HIDE = new ResourceType (i++, "HIDE", "Hide", "Hide placeholder", 20);
	public static readonly ResourceType STONE = new ResourceType (i++, "STONE", "Stone", "Stone placeholder", 30);

	public ResourceType (int id, string saveName, string displayName, string description, BiggerNumber baseMaxValue) : base (id, saveName, displayName, description)
	{
		this.baseMaxValue = baseMaxValue;
		lock (thisLock) {
			typeMap.Add (saveName, this);
		}
	}

	public BiggerNumber BaseMaxValue {
		get {
			return this.baseMaxValue;
		}
	}

	public static new ResourceType getType (string saveName)
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

	public static int getResourceTypeMapCount ()
	{
		if (i != typeMap.Count) {
			Debug.LogError ("There is less items in the ResourceTypeMap then ResourceTypes. Please check your id's for uniqueness");
		}
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
