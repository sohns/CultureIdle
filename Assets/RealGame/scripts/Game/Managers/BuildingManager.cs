using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class BuildingManager : MonoBehaviour, Incremental
{

	private Dictionary<BuildingType,BuildingData> buildings = new Dictionary<BuildingType,BuildingData> ();

	private InfoPanel infoPanel;
	private ResourcesManager resourcesManager;
	private ProjectManager projectManager;
	private Queue<BuildingType> buildingsToBuild = new Queue<BuildingType> ();
	private Queue<BuildingType> buildingsToUnlock = new Queue<BuildingType> ();

	// Use this for initialization
	void Start ()
	{
		resourcesManager = (ResourcesManager)GameObject.Find ("Resources").GetComponent ("ResourcesManager");
		projectManager = (ProjectManager)GameObject.Find ("Projects").GetComponent ("ProjectManager");
		infoPanel = (InfoPanel)GameObject.FindWithTag ("InfoPanel").GetComponent ("InfoPanel");
	}

	public void reset ()
	{
		buildings = new Dictionary<BuildingType,BuildingData> ();
	}

	//Update resources from projects/challenges
	public void OnUpdate (float rate)
	{
		if (buildingsToBuild.Count > 0) {
			BuildingType buildingType = buildingsToBuild.Dequeue ();
			if (canAffordBuilding (buildingType)) {
				if (!buildings.ContainsKey (buildingType)) {
					buildings.Add (buildingType, new BuildingData (buildingType));
				}
				startBuildingProduction (buildingType);
			} else {
			}
		}
		if (buildingsToUnlock.Count > 0) {
			BuildingType buildingType = buildingsToUnlock.Dequeue ();
			if (buildings.ContainsKey (buildingType)) {
				Debug.LogWarning ("We should not be unlocking a tech we have" + buildingType.Id + " should not equal " + buildings [buildingType].BuildingType.Id);
				Debug.LogWarning ("We should not be unlocking a tech we have" + buildingType.DisplayName + " should not equal " + buildings [buildingType].BuildingType.DisplayName);
			} else {
				BuildingData buildingData = new BuildingData (buildingType);
				buildings.Add (buildingType, buildingData);
				infoPanel.resetPanel (InfoPanelEnum.PROJECT_BUILDINGS);
			}
		}
	}

	private void startBuildingProduction (BuildingType buildingType)
	{
		if (projectManager.AddProject (buildingType)) {
			BuildingData buildingData = buildings [buildingType];
			Dictionary<ResourceType,ResourceData> resources = resourcesManager.Resources;
			foreach (CurrencyData currencyData in buildingData.CurrentCost) {
				if (currencyData.isBuildingType ()) {
					BuildingType buildingTypeCost = (BuildingType)currencyData.CurrencyType;
					if (!buildings.ContainsKey (buildingTypeCost)) {
						Debug.LogError ("missingKey in buybuilding:" + buildingTypeCost.DisplayName);
					}
					buildings [buildingTypeCost].AmountOwned = buildings [buildingTypeCost].AmountOwned.SubNumber (currencyData.BaseAmount);
				} else if (currencyData.isResourceType ()) {
					ResourceType resourceTypeCost = (ResourceType)currencyData.CurrencyType;
					if (!resources.ContainsKey (resourceTypeCost)) {
						Debug.LogError ("missingKey in buybuilding:" + resourceTypeCost.DisplayName);
					}
					resources [resourceTypeCost].CurrentAmount = resources [resourceTypeCost].CurrentAmount.SubNumber (currencyData.BaseAmount);
				} else {
					Debug.LogError ("Type does not exist in buybuilding cost:" + buildingType.DisplayName);
				}
				buildingData.increaseCosts ();
				buildingData.increaseAmountBuilt ();
			}
		}
	}

	public void incrementBuilding (BuildingType buildingType)
	{
		if (!buildings.ContainsKey (buildingType)) {
			Debug.LogError ("missingKey in buybuilding:" + buildingType.DisplayName);
		}
		BuildingData buildingData = buildings [buildingType];
		buildingData.increaseAmountOwned ();
	}

	public Dictionary<BuildingType, BuildingData> Buildings {
		get {
			return this.buildings;
		}
		set {
			buildings = value;
		}
	}

	public void initializeBuildings (List<BuildingData> buildingDatas)
	{
		if (buildings.Count > 0) {
			Debug.LogError ("This should only happen at start up");
		} else {
			foreach (BuildingData buildingData in buildingDatas) {
				buildings.Add (buildingData.BuildingType, buildingData);
			}
		}
	}

	public void unlockBuilding (BuildingType buildingType)
	{
		buildingsToUnlock.Enqueue (buildingType);
	}

	public bool canAffordBuilding (BuildingType buildingType)
	{
		if (buildings.ContainsKey (buildingType)) {
			BuildingData buildingData = buildings [buildingType];
			Dictionary<ResourceType,ResourceData> resources = resourcesManager.Resources;
			foreach (CurrencyData currencyData in buildingData.CurrentCost) {
				BiggerNumber amountNeeded = currencyData.BaseAmount;
				BiggerNumber amountHave;
				if (currencyData.isBuildingType ()) {
					BuildingType buildingTypeCost = (BuildingType)currencyData.CurrencyType;
					if (!buildings.ContainsKey (buildingTypeCost)) {
						//Debug.LogError ("missingKey:" + buildingTypeCost.DisplayName);
						return false;
					}
					amountHave = buildings [buildingTypeCost].AmountOwned;
				} else if (currencyData.isResourceType ()) {
					ResourceType resourceTypeCost = (ResourceType)currencyData.CurrencyType;
					if (!resources.ContainsKey (resourceTypeCost)) {
						//Debug.LogError ("missingKey:" + resourceTypeCost.DisplayName);
						return false;
					}
					amountHave = resources [resourceTypeCost].CurrentAmount;
				} else {
					//Debug.LogError ("Type does not exist in cost:" + buildingType.DisplayName);
					return false;
				}
					
				if (amountHave.CompareTo (amountNeeded) < 0) {
					//Debug.LogError (buildingType.DisplayName+" Have:" + amountHave.ToDisplayString(0,false) + " Need" + amountNeeded.ToDisplayString(0,false));
					return false;
				}
			}
			return true;
		}
		Debug.LogError ("Testing a can afford building for building we dont have:" + buildingType.DisplayName);
		return false;
	}

	public void addBuildingToQueue (BuildingType buildingType)
	{
		buildingsToBuild.Enqueue (buildingType);
	}
}
