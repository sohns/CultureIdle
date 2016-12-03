using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StartingPointType
{
	private readonly int id;
	private readonly string saveName;
	private readonly string displayName;
	private readonly string description;
	private List<SimpleCurrencyData> baseResources = new List<SimpleCurrencyData> ();
	private List<SimpleCurrencyData> baseBuildings = new List<SimpleCurrencyData> ();
	private List<SimpleCurrencyData> baseTechnologies = new List<SimpleCurrencyData> ();
	private List<SimpleCurrencyData> baseCivilizationPoints = new List<SimpleCurrencyData> ();
	//created
	private static Dictionary<string,StartingPointType> typeMap = new Dictionary<string,StartingPointType> ();
	private static object thisLock = new object ();

	//To add a new one, copy this and then add to map;
	private static int i = 0;
	public static readonly StartingPointType ORIGINAL_TESTING = new StartingPointType (i++, "ORIGINAL_TESTING", "ORIGINAL TESTING", "Placeholder for ORIGINAL_TESTING",
		                                                            new SimpleCurrencyData[] {
			new SimpleCurrencyData (ResourceType.FOOD, new BiggerNumber (0)),
			new SimpleCurrencyData (ResourceType.WOOD, new BiggerNumber (0)),
			new SimpleCurrencyData (ResourceType.BONE, new BiggerNumber (5)),
			new SimpleCurrencyData (ResourceType.HIDE, new BiggerNumber (7)),
			new SimpleCurrencyData (CivilizationPointType.SKIRMISH, new BiggerNumber (6)),
			new SimpleCurrencyData (CivilizationPointType.POPULATION, new BiggerNumber (8)),
			new SimpleCurrencyData (BuildingType.HUT, new BiggerNumber (5)),
			new SimpleCurrencyData (BuildingType.FORAGING_HUT, new BiggerNumber (0)),
			new SimpleCurrencyData (BuildingType.HUNTING_HUT, new BiggerNumber (11)),
			new SimpleCurrencyData (TechnologyType.GET_RESEARCHES)
		}
	                                                            );

	public static readonly StartingPointType SLOW_TESTING = new StartingPointType (i++, "SLOW_TESTING", "SLOW TESTING", "Placeholder for SLOW_TESTING",
		                                                        new SimpleCurrencyData[] {
			new SimpleCurrencyData (ResourceType.FOOD, new BiggerNumber (100)),
			new SimpleCurrencyData (ResourceType.WOOD, new BiggerNumber (150)),
			new SimpleCurrencyData (ResourceType.BONE, new BiggerNumber (5)),
			new SimpleCurrencyData (ResourceType.HIDE, new BiggerNumber (7)),
			new SimpleCurrencyData (CivilizationPointType.SKIRMISH, new BiggerNumber (6)),
			new SimpleCurrencyData (CivilizationPointType.POPULATION, new BiggerNumber (8)),
			new SimpleCurrencyData (BuildingType.HUT, new BiggerNumber (0)),
			new SimpleCurrencyData (BuildingType.FORAGING_HUT, new BiggerNumber (0)),
			new SimpleCurrencyData (BuildingType.HUNTING_HUT, new BiggerNumber (0)),
			new SimpleCurrencyData (TechnologyType.GET_RESEARCHES)
		}
	                                                        );

	public static new StartingPointType getType (string saveName)
	{
		if (i != typeMap.Count) {
			Debug.LogError ("There is less items in the TechnologyTypeMap then TechnologyType. Please check your id's for uniqueness");
		}
		if (!typeMap.ContainsKey (saveName)) {
			Debug.LogError (saveName);
			return null;
		}
		return typeMap [saveName];
	}

	public StartingPointType (int id, string saveName, string displayName, string description, SimpleCurrencyData[] baseThings)
	{
		this.id = id;
		this.displayName = displayName;
		this.saveName = saveName;
		this.description = description;
		addBaseThing (baseThings);
		lock (thisLock) {
			typeMap.Add (saveName, this);
		}
	}

	void addBaseThing (SimpleCurrencyData[] baseThings)
	{
		foreach (SimpleCurrencyData simpleCurrencyData in baseThings) {
			if (simpleCurrencyData.isResourceType ()) {
				baseResources.Add (simpleCurrencyData);
			} else if (simpleCurrencyData.isBuildingType ()) {
				baseBuildings.Add (simpleCurrencyData);
			} else if (simpleCurrencyData.isAdvancedResourceType ()) {
				Debug.LogError ("unhandled type");
			} else if (simpleCurrencyData.isTechnologyType ()) {
				baseTechnologies.Add (simpleCurrencyData);
			} else if (simpleCurrencyData.isCivilizationPointType ()) {
				baseCivilizationPoints.Add (simpleCurrencyData);
			} else {
				Debug.LogError ("unhandled type");
			}
		}
	}

	public int Id {
		get {
			return this.id;
		}
	}

	public string DisplayName {
		get {
			return this.displayName;
		}
	}

	public string SaveName {
		get {
			return this.saveName;
		}
	}

	public string Description {
		get {
			return this.description;
		}
	}

	public List<SimpleCurrencyData> BaseResources {
		get {
			return this.baseResources;
		}
	}

	public List<SimpleCurrencyData> BaseBuildings {
		get {
			return this.baseBuildings;
		}
	}

	public List<SimpleCurrencyData> BaseTechnologies {
		get {
			return this.baseTechnologies;
		}
	}

	public List<SimpleCurrencyData> BaseCivilizationPoints {
		get {
			return this.baseCivilizationPoints;
		}
	}

	public override bool Equals (object obj)
	{
		if (obj == null)
			return false;
		if (ReferenceEquals (this, obj))
			return true;
		if (obj.GetType () != typeof(BuildingType))
			return false;
		BuildingType other = (BuildingType)obj;
		return id == other.Id;
	}


	public override int GetHashCode ()
	{
		unchecked {
			return id.GetHashCode ();
		}
	}

}
