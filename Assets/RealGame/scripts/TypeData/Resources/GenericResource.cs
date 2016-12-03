using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GenericResource
{

	private readonly int id;
	private readonly string saveName;
	private readonly string displayName;
	private readonly string description;

	public GenericResource (int id, string saveName, string displayName, string description)
	{
		this.id = id;
		this.saveName = saveName;
		this.displayName = displayName;
		this.description = description;
	}

	public int Id {
		get {
			return this.id;
		}
	}

	public string SaveName {
		get {
			return this.saveName;
		}
	}

	public string DisplayName {
		get {
			return this.displayName;
		}
	}

	public string Description {
		get {
			return this.description;
		}
	}

}
