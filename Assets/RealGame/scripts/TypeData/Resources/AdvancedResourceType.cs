using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AdvancedResourceType : GenericResource
{

	private static Dictionary<string,AdvancedResourceType> typeMap = new Dictionary<string,AdvancedResourceType> ();
	private static object thisLock = new object ();

	//To add a new one, copy this and then add to map;
	private static int i = 0;
	public static readonly AdvancedResourceType TIME = new AdvancedResourceType (i++, "TIME", "Time", "Time placeholder");

	public AdvancedResourceType (int id, string saveName, string displayName, string description) : base (id, saveName, displayName, description)
	{
		lock (thisLock) {
			typeMap.Add (saveName, this);
		}
	}

	public static new AdvancedResourceType getType (string saveName)
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
		if (obj.GetType () != typeof(AdvancedResourceType))
			return false;
		AdvancedResourceType other = (AdvancedResourceType)obj;
		return Id == other.Id;
	}


	public override int GetHashCode ()
	{
		unchecked {
			return Id.GetHashCode ();
		}
	}

}
