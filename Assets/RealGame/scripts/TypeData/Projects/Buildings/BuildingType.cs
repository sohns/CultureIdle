using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildingType
{

	private readonly int id;
	private readonly string saveName;
	private readonly string displayName;
	private readonly string description;
	private CurrencyData[] costs;
	private readonly CurrencyData[] generates;
	private readonly CurrencyData time;

	//created
	private static Dictionary<string,BuildingType> typeMap = new Dictionary<string,BuildingType> ();
	private static object thisLock = new object ();


	//To add a new one, copy this and then add to map;
	private static int i = 0;
	public static readonly BuildingType HUT = new BuildingType (i++, "HUT", "Hut", "Placeholder for Hut",
		                                          new CurrencyData[] {
			new CurrencyData (ResourceType.FOOD, new BiggerNumber (10), new BiggerNumber (1.2)),
			new CurrencyData (ResourceType.WOOD, new BiggerNumber (20), new BiggerNumber (1.1))
		},
		                                          new CurrencyData[] {
			new CurrencyData (ResourceType.FOOD, 1),
			new CurrencyData (ResourceType.WOOD, 1),
			new CurrencyData (CivilizationPointType.POPULATION, 1)
		},
		                                          new CurrencyData (AdvancedResourceType.TIME, new BiggerNumber (10))
	                                          );

	public static readonly BuildingType HUNTING_HUT = new BuildingType (i++, "HUNTING_HUT", "Hunting Hut", "Placeholder for Hunting Hut",
		                                                  new CurrencyData[] {
			new CurrencyData (ResourceType.FOOD, new BiggerNumber (10), new BiggerNumber (1.2)),
			new CurrencyData (ResourceType.WOOD, new BiggerNumber (20), new BiggerNumber (1.1)),
			new CurrencyData (BuildingType.HUT, 1)
		},
		                                                  new CurrencyData[] {
			new CurrencyData (ResourceType.FOOD, new BiggerNumber (5)),
			new CurrencyData (ResourceType.BONE, 1),
			new CurrencyData (ResourceType.HIDE, 1),
			new CurrencyData (CivilizationPointType.SKIRMISH, 1),
			new CurrencyData (CivilizationPointType.POPULATION, -.5)
		},
		                                                  new CurrencyData (AdvancedResourceType.TIME, new BiggerNumber (10))
	                                                  );

	public static readonly BuildingType FORAGING_HUT = new BuildingType (i++, "FORAGING_HUT", "Foraging Hut", "Placeholder for Foraging Hut",
		                                                   new CurrencyData[] {
			new CurrencyData (ResourceType.FOOD, new BiggerNumber (10), new BiggerNumber (1.2)),
			new CurrencyData (ResourceType.WOOD, new BiggerNumber (20), new BiggerNumber (1.1)),
			new CurrencyData (BuildingType.HUT, 1)
		},
		                                                   new CurrencyData[] {
			new CurrencyData (ResourceType.FOOD, new BiggerNumber (10)),
		},
		                                                   new CurrencyData (AdvancedResourceType.TIME, new BiggerNumber (10))
	                                                   );

	public static readonly BuildingType CUTTERS_HUT = new BuildingType (i++, "CUTTERS_HUT", "Cutting Hut", "Placeholder for Cutting Hut",
		                                                  new CurrencyData[] {
			new CurrencyData (ResourceType.FOOD, new BiggerNumber (10), new BiggerNumber (1.2)),
			new CurrencyData (ResourceType.WOOD, new BiggerNumber (20), new BiggerNumber (1.1)),
			new CurrencyData (BuildingType.HUT, 1)
		},
		                                                  new CurrencyData[] {
			new CurrencyData (ResourceType.WOOD, new BiggerNumber (10))
		},
		                                                  new CurrencyData (AdvancedResourceType.TIME, new BiggerNumber (10))
	                                                  );

	public static new BuildingType getType (string saveName)
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


	public BuildingType (int id, string saveName, string displayName, string description, CurrencyData[] costs, CurrencyData[] generates, CurrencyData time)
	{
		this.id = id;
		this.saveName = saveName;
		this.displayName = displayName;
		this.description = description;
		this.costs = costs;
		this.generates = generates;
		this.time = time;
		lock (thisLock) {
			typeMap.Add (saveName, this);
		}
	}

	public CurrencyData Time {
		get {
			return this.time;
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

	public string Description {
		get {
			return this.description;
		}
	}

	public string SaveName {
		get {
			return this.saveName;
		}
	}

	public CurrencyData[] Costs {
		get {
			return this.costs;
		}
	}

	public CurrencyData[] Generates {
		get {
			return this.generates;
		}
	}

	public static int getTypeMapCount ()
	{
		if (i != typeMap.Count) {
			Debug.LogError ("There is less items in the TechnologyTypeMap then TechnologyType. Please check your id's for uniqueness");
		}
		return typeMap.Count;
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
		return id == other.id;
	}


	public override int GetHashCode ()
	{
		unchecked {
			return id.GetHashCode ();
		}
	}

	public string ExtraInfo {
		get {
			return "Name:\n\t" + displayName + "\nDescription:\n\t" + description + "\nCosts:\n" + CostsString + "\nProduces:\n" + GeneratesString + "\nTime to Build This Project:\n" + TimeString;
		}

	}

	public string CostsString {
		get {
			List<string> innerCostListResource = new List<string> ();
			List<string> innerCostListBuilding = new List<string> ();
			List<string> innerCostListOther = new List<string> ();
			foreach (CurrencyData currencyData in costs) {
				string innerCostText;
				if (currencyData.isResourceType ()) {
					ResourceType resourceTypeCost = (ResourceType)currencyData.CurrencyType;
					innerCostText = resourceTypeCost.DisplayName + ":" + currencyData.BaseAmount.ToDisplayString (2, false);
					innerCostListResource.Add (innerCostText);
				} else if (currencyData.isBuildingType ()) {
					BuildingType buildingTypeCost = (BuildingType)currencyData.CurrencyType;
					innerCostText = "Consumes " + currencyData.BaseAmount.ToDisplayString (0, true) + " " + buildingTypeCost.DisplayName;
					innerCostListBuilding.Add (innerCostText);
				} else {
					innerCostText = "Error";
					innerCostListOther.Add (innerCostText);
				}

			}
			return JoinStringLists (innerCostListResource, innerCostListBuilding, innerCostListOther);
		}
	}

	private string JoinStringLists (params List<string>[] listsToJoin)
	{
		if (listsToJoin.Length == 0) {
			return "";
		}
		List<string> finalList = new List<string> ();
		foreach (List<string> listToJoin in listsToJoin) {
			if (listToJoin != null && listToJoin.Count > 0) {
				finalList.Add (string.Join (",", listToJoin.ToArray ()));
			}
		}
		if (finalList.Count > 0) {
			return "\t" + string.Join ("\n\t", finalList.ToArray ());
		}
		return "";
	}

	public string GeneratesString {
		get {
			List<string> innerCostListResource = new List<string> ();
			List<string> innerCostListBuilding = new List<string> ();
			List<string> innerCostListCivilizationPointPositive = new List<string> ();
			List<string> innerCostListCivilizationPointNegative = new List<string> ();
			List<string> innerCostListOther = new List<string> ();
			foreach (CurrencyData currencyData in generates) {
				string innerCostText;
				if (currencyData.isResourceType ()) {
					ResourceType resourceTypeCost = (ResourceType)currencyData.CurrencyType;
					innerCostText = resourceTypeCost.DisplayName + ":" + currencyData.BaseAmount.ToDisplayString (2, false);
					innerCostListResource.Add (innerCostText);
				} else if (currencyData.isBuildingType ()) {
					BuildingType type = (BuildingType)currencyData.CurrencyType;
					innerCostText = "Consumes " + currencyData.BaseAmount.ToDisplayString (0, true) + " " + type.DisplayName;
					innerCostListBuilding.Add (innerCostText);
				} else if (currencyData.isCivilizationPointType ()) {
					CivilizationPointType type = (CivilizationPointType)currencyData.CurrencyType;
					if (currencyData.BaseAmount.CompareTo (0) > 0) {
						innerCostText = currencyData.BaseAmount.ToDisplayString (2, false) + " " + type.DisplayName;
						innerCostListCivilizationPointPositive.Add ("Creates " + innerCostText + " points");
					} else {
						innerCostText = currencyData.BaseAmount.AbsoluteValue.ToDisplayString (2, false) + " " + type.DisplayName;
						innerCostListCivilizationPointNegative.Add ("Consumes " + innerCostText + " points");
					}
				} else {
					innerCostText = "Error";
					innerCostListOther.Add (innerCostText);
				}

			}
			return JoinStringLists (innerCostListResource, innerCostListBuilding, innerCostListOther, innerCostListCivilizationPointPositive, innerCostListCivilizationPointNegative);
		}
	}

	public string TimeString {
		get {
			return "\t" + TextConverter.Instance.getTime (this.time.BaseAmount.getValue ());
		}
	}
}
