using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TechnologyType
{

	private readonly int id;
	private readonly string saveName;
	private readonly string displayName;
	private readonly string description;
	private CurrencyData[] costs;
	private readonly BasicCurrencyData[] unlocks;
	private readonly CurrencyData time;

	private static Dictionary<string,TechnologyType> typeMap = new Dictionary<string,TechnologyType> ();
	private static object thisLock = new object ();

	//To add a new one, copy this and then add to map;
	private static int i = 0;
	public static readonly TechnologyType GET_STONE = new TechnologyType (i++, "GET_STONE", "Stone Working", "Placeholder for Stone Working",
		                                                  new CurrencyData[] {
			new CurrencyData (ResourceType.FOOD, new BiggerNumber (10), new BiggerNumber (1.2)),
			new CurrencyData (ResourceType.WOOD, new BiggerNumber (20), new BiggerNumber (1.1))
		},
		                                                  new BasicCurrencyData[] {
			new BasicCurrencyData (ResourceType.STONE)
		},
		                                                  new CurrencyData (null, new BiggerNumber (3))
	                                                  );
	public static readonly TechnologyType GET_CUTTERS_HUT = new TechnologyType (i++, "GET_CUTTERS_HUT", "Advanced Woodwork", "Placeholder for Advanced Woodwork",
		                                                        new CurrencyData[] {
			new CurrencyData (ResourceType.FOOD, new BiggerNumber (10), new BiggerNumber (1.2)),
			new CurrencyData (ResourceType.WOOD, new BiggerNumber (20), new BiggerNumber (1.1))
		},
		                                                        new BasicCurrencyData[] {
			new BasicCurrencyData (BuildingType.CUTTERS_HUT)
		},
		                                                        new CurrencyData (null, new BiggerNumber (3))
	                                                        );
	public static readonly TechnologyType GET_RESEARCHES = new TechnologyType (i++, "GET_RESEARCHES", "Start Researching", "Placeholder for Start Researching",
		                                                       new CurrencyData[] {
			new CurrencyData (ResourceType.FOOD, new BiggerNumber (10), new BiggerNumber (1.2)),
			new CurrencyData (ResourceType.WOOD, new BiggerNumber (20), new BiggerNumber (1.1))
		},
		                                                       new BasicCurrencyData[] {
			new BasicCurrencyData (GET_CUTTERS_HUT), new BasicCurrencyData (GET_STONE)
		},
		                                                       new CurrencyData (null, 1)
	                                                       );

	public static new TechnologyType getType (string saveName)
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

	public TechnologyType (int id, string saveName, string displayName, string description, CurrencyData[] costs, BasicCurrencyData[] unlocks, CurrencyData time)
	{
		this.id = id;
		this.saveName = saveName;
		this.displayName = displayName;
		this.description = description;
		this.costs = costs;
		this.unlocks = unlocks;
		this.time = time;
		lock (thisLock) {
			typeMap.Add (saveName, this);
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

	public CurrencyData[] Costs {
		get {
			return this.costs;
		}
	}

	public BasicCurrencyData[] Unlocks {
		get {
			return this.unlocks;
		}
	}

	public string SaveName {
		get {
			return this.saveName;
		}
	}

	public CurrencyData Time {
		get {
			return this.time;
		}
	}


	public override bool Equals (object obj)
	{
		if (obj == null)
			return false;
		if (ReferenceEquals (this, obj))
			return true;
		if (obj.GetType () != typeof(TechnologyType))
			return false;
		TechnologyType other = (TechnologyType)obj;
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
			return "Name:\n\t" + displayName + "\nDescription:\n\t" + description + "\nCosts:\n" + CostsString + "\nUnlocks:\n" + UnlocksString + "\nTime to Research This Project:\n" + TimeString;
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
					innerCostText = "Requires " + currencyData.BaseAmount.ToDisplayString (0, true) + " " + buildingTypeCost.DisplayName;
					innerCostListBuilding.Add (innerCostText);
				} else {
					innerCostText = "Error";
					innerCostListOther.Add (innerCostText);
				}

			}
			return JoinThreeStringLists (innerCostListResource, innerCostListBuilding, innerCostListOther);
		}
	}

	private string JoinThreeStringLists (List<string> listOne, List<string> listTwo, List<string> listThree)
	{
		List<string> finalList = new List<string> ();
		if (listOne != null && listOne.Count > 0) {
			finalList.Add (string.Join (",", listOne.ToArray ()));
		}
		if (listTwo != null && listTwo.Count > 0) {
			finalList.Add (string.Join (",", listTwo.ToArray ()));
		}
		if (listThree != null && listThree.Count > 0) {
			finalList.Add (string.Join (",", listThree.ToArray ()));
		}
		if (finalList.Count > 0) {
			return "\t" + string.Join ("\n\t", finalList.ToArray ());
		}
		return "";
	}

	public string UnlocksString {
		get {
			List<string> innerCostListResource = new List<string> ();
			List<string> innerCostListBuilding = new List<string> ();
			List<string> innerCostListOther = new List<string> ();
			foreach (BasicCurrencyData basicCurrencyData in unlocks) {
				string innerCostText;
				if (basicCurrencyData.isResourceType ()) {
					ResourceType resourceTypeCost = (ResourceType)basicCurrencyData.CurrencyType;
					innerCostText = resourceTypeCost.DisplayName;
					innerCostListResource.Add (innerCostText);
				} else if (basicCurrencyData.isBuildingType ()) {
					BuildingType buildingTypeCost = (BuildingType)basicCurrencyData.CurrencyType;
					innerCostText = buildingTypeCost.DisplayName;
					innerCostListBuilding.Add (innerCostText);
				} else if (basicCurrencyData.isTechnologyType ()) {
					TechnologyType technologyTypeCost = (TechnologyType)basicCurrencyData.CurrencyType;
					innerCostText = technologyTypeCost.DisplayName;
					innerCostListBuilding.Add (innerCostText);
				} else {
					innerCostText = "Error";
					innerCostListOther.Add (innerCostText);
				}

			}
			return JoinThreeStringLists (innerCostListResource, innerCostListBuilding, innerCostListOther);
		}
	}

	public string TimeString {
		get {
			return "\t" + TextConverter.Instance.getTime (this.time.BaseAmount.getValue ());
		}
	}
}
