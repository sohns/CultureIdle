using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class TechnologyManager : MonoBehaviour,Incremental
{
	private Dictionary<TechnologyType,TechnologyData> technologies = new Dictionary<TechnologyType,TechnologyData> ();

	private ResourcesManager resourcesManager;
	private ProjectManager projectManager;
	private BuildingManager buildingManager;
	private InfoPanel infoPanel;

	private Queue<TechnologyType> technologiesToStart = new Queue<TechnologyType> ();
	private Queue<TechnologyType> technologiesToUnlock = new Queue<TechnologyType> ();



	public Dictionary<TechnologyType, TechnologyData> Technologies {
		get {
			return this.technologies;
		}
		set {
			technologies = value;
		}
	}
	// Use this for initialization
	void Start ()
	{
		infoPanel = (InfoPanel)GameObject.FindWithTag ("InfoPanel").GetComponent ("InfoPanel");
		resourcesManager = (ResourcesManager)GameObject.Find ("Resources").GetComponent ("ResourcesManager");
		projectManager = (ProjectManager)GameObject.Find ("Projects").GetComponent ("ProjectManager");
		buildingManager = (BuildingManager)GameObject.Find ("Buildings").GetComponent ("BuildingManager");
	}

	public void reset ()
	{
		technologies = new Dictionary<TechnologyType,TechnologyData> ();
	}

	//Update resources from projects/challenges
	public void OnUpdate (float rate)
	{
		if (technologiesToStart.Count > 0) {
			TechnologyType technologyType = technologiesToStart.Dequeue ();
			TechnologyData technologyData = technologies [technologyType];
			if (canAffordTechnology (technologyType) && technologyData.IsAvailableToStartResearching) {
				if (!technologies.ContainsKey (technologyType)) {
					technologies.Add (technologyType, new TechnologyData (technologyType));
				}
				startTechnologyProduction (technologyType);
			} else {
			}
		}
		if (technologiesToUnlock.Count > 0) {
			TechnologyType technologyType = technologiesToUnlock.Dequeue ();
			if (technologies.ContainsKey (technologyType)) {
				Debug.LogWarning ("We should not be unlocking a tech we have" + technologyType.Id + " should not equal " + technologies [technologyType].TechnologyType.Id);
				Debug.LogWarning ("We should not be unlocking a tech we have" + technologyType.DisplayName + " should not equal " + technologies [technologyType].TechnologyType.DisplayName);
			} else {
				technologies.Add (technologyType, new TechnologyData (technologyType));
				infoPanel.resetPanel (InfoPanelEnum.PROJECT_TECHNOLOGIES);
			}

		}
	}

	//TODO fix this
	private void startTechnologyProduction (TechnologyType technologyType)
	{
		if (projectManager.AddProject (technologyType)) {
			TechnologyData technologyData = technologies [technologyType];
			Dictionary<ResourceType,ResourceData> resources = resourcesManager.Resources;
			foreach (CurrencyData currencyData in technologyData.Cost) {
				if (currencyData.isBuildingType ()) {
					//Buildings are not consumed			
				} else if (currencyData.isResourceType ()) {
					ResourceType resourceTypeCost = (ResourceType)currencyData.CurrencyType;
					if (!resources.ContainsKey (resourceTypeCost)) {
						Debug.LogError ("missingKey in buybuilding:" + resourceTypeCost.DisplayName);
					}
					resources [resourceTypeCost].CurrentAmount = resources [resourceTypeCost].CurrentAmount.SubNumber (currencyData.BaseAmount);
				} else {
					Debug.LogError ("Type does not exist in startTechnology cost:" + technologyType.DisplayName);
				}
			}
			technologies [technologyType].IsResearching = true;
		}
	}

	public void finishResearch (TechnologyType technologyType)
	{
		if (!technologies.ContainsKey (technologyType)) {
			Debug.LogError ("missingKey in finishResearch:" + technologyType.DisplayName);
		}
		foreach (BasicCurrencyData currencyData in technologyType.Unlocks) {
			if (currencyData.isBuildingType ()) {
				buildingManager.unlockBuilding ((BuildingType)currencyData.CurrencyType);		
			} else if (currencyData.isResourceType ()) {
				resourcesManager.addNewResource ((ResourceType)currencyData.CurrencyType);
			} else if (currencyData.isTechnologyType ()) {
				unlockTechnology ((TechnologyType)currencyData.CurrencyType);
			} else {
				Debug.LogError ("Type does not exist in startTechnology cost:" + technologyType.DisplayName);
			}
		}
		technologies [technologyType].IsResearched = true;
		technologies [technologyType].IsResearching = false;
	}

	public void initializeTechnologies (List<TechnologyData> technologyDatas)
	{
		if (technologies.Count > 0) {
			Debug.LogError ("This should only happen at start up");
		} else {
			foreach (TechnologyData technologyData in technologyDatas) {
				technologies.Add (technologyData.TechnologyType, technologyData);
			}
		}
	}

	public bool canAffordTechnology (TechnologyType technologyType)
	{
		if (technologies.ContainsKey (technologyType)) {
			TechnologyData technologyData = technologies [technologyType];
			Dictionary<ResourceType,ResourceData> resources = resourcesManager.Resources;
			Dictionary<BuildingType, BuildingData> buildings = buildingManager.Buildings;
			foreach (CurrencyData currencyData in technologyData.Cost) {
				BiggerNumber amountNeeded = currencyData.BaseAmount;
				BiggerNumber amountHave;
				if (currencyData.isBuildingType ()) {
					BuildingType buildingTypeCost = (BuildingType)currencyData.CurrencyType;
					if (!buildings.ContainsKey (buildingTypeCost)) {
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
		Debug.LogError ("Testing a can afford technology for technology we dont have:" + technologyType.DisplayName);
		return false;
	}

	public void addTechnologyToQueue (TechnologyType technologyType)
	{
		technologiesToStart.Enqueue (technologyType);
	}

	public void unlockTechnology (TechnologyType technologyType)
	{
		technologiesToUnlock.Enqueue (technologyType);
	}
}
