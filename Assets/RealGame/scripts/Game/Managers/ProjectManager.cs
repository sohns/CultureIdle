using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using System.Linq;

public class ProjectManager : MonoBehaviour, Incremental
{
	private BuildingManager buildingManager;
	private TechnologyManager technologyManager;
	//Projects
	private List<ActiveProjectBaseData> activeProjects = new List<ActiveProjectBaseData> ();
	//Sort Types
	public const int SORT_NATURAL = 1;


	void Start ()
	{
		buildingManager = (BuildingManager)GameObject.Find ("Buildings").GetComponent ("BuildingManager");
		technologyManager = (TechnologyManager)GameObject.Find ("Technologies").GetComponent ("TechnologyManager");
//		//TODO:Remove these
//		activeProjects.Add(new ActiveProjectBuildingData(BuildingType.FORAGING_HUT,new BiggerNumber(1000)));
//		activeProjects.Add(new ActiveProjectBuildingData(BuildingType.HUNTING_HUT,new BiggerNumber(250)));
//		activeProjects.Add(new ActiveProjectBuildingData(BuildingType.HUT,new BiggerNumber(10)));
//		activeProjects.Add(new ActiveProjectBuildingData(BuildingType.HUT,new BiggerNumber(5)));
	}

	public void reset ()
	{
		activeProjects = new List<ActiveProjectBaseData> ();
	}

	public List<ActiveProjectBaseData> ActiveProjects {
		get {
			return this.activeProjects;
		}
		set {
			activeProjects = value;
		}
	}

	public List<ActiveProjectBaseData> getActiveProjects (int sortType)
	{
		return getActiveProjects (sortType, 0);
	}

	public List<ActiveProjectBaseData> getActiveProjects (int sortType, int numberToReturn)
	{
		
		switch (sortType) {
		case SORT_NATURAL:
			activeProjects = activeProjects.OrderBy (a => a.IsPaused).ThenBy (a => a.TimeLeft).ToList ();
			break;
		default:
			Debug.LogError ("This is not a correct sort method:" + sortType);
			break;
		}

		if (numberToReturn == 0) {
			return activeProjects;
		} else {
			return activeProjects.Take (numberToReturn).ToList ();
		}
	}

	public void OnUpdate (float rate)
	{
		List<ActiveProjectBaseData> recentlyFinishedProjects = new List<ActiveProjectBaseData> ();
		foreach (ActiveProjectBaseData activeProjectBaseData in activeProjects) {
			if (activeProjectBaseData.isBuildingType ()) {
				if (CanAffordToUpdate (activeProjectBaseData, rate)) {
					UpdateActiveProjectBaseData (activeProjectBaseData, rate);
					if (activeProjectBaseData.TimeLeft.CompareTo (0) < 1) {
						recentlyFinishedProjects.Add (activeProjectBaseData);
					}
				} else {
					Debug.LogError ("non existed advancedResourceType");
				}
			} else if (activeProjectBaseData.isTechnologyType ()) {
				if (CanAffordToUpdate (activeProjectBaseData, rate)) {
					UpdateActiveProjectBaseData (activeProjectBaseData, rate);
					if (activeProjectBaseData.TimeLeft.CompareTo (0) < 1) {
						recentlyFinishedProjects.Add (activeProjectBaseData);
					}
				} else {
					Debug.LogError ("non existed advancedResourceType");
				}
			} else {
				Debug.LogError ("non existed activeProjectBaseData");
			}
		}
		foreach (ActiveProjectBaseData activeProjectBaseData in recentlyFinishedProjects) {
			if (activeProjectBaseData.isBuildingType ()) {
				activeProjects.RemoveAll (p => p.UniqueId == activeProjectBaseData.UniqueId);
				buildingManager.incrementBuilding ((BuildingType)activeProjectBaseData.ActiveProjectType);
			} else if (activeProjectBaseData.isTechnologyType ()) {
				activeProjects.RemoveAll (p => p.UniqueId == activeProjectBaseData.UniqueId);
				technologyManager.finishResearch ((TechnologyType)activeProjectBaseData.ActiveProjectType);
			} else {
				Debug.LogError ("non existed activeProjectBaseData");
			}
		}
	}

	public bool AddProject (BuildingType buildingType)
	{
		activeProjects.Add (new ActiveProjectBuildingData (buildingType, buildingType.Time.BaseAmount));
		return true;
	}

	public bool AddProject (TechnologyType technologyType)
	{
		activeProjects.Add (new ActiveProjectBuildingData (technologyType, technologyType.Time.BaseAmount));
		return true;
	}

	private bool UpdateActiveProjectBaseData (ActiveProjectBaseData activeProjectBaseData, float rate)
	{
		foreach (ActiveProjectCost activeProjectCost in activeProjectBaseData.Costs) {
			if (activeProjectCost.isAdvancedResourceType ()) {
				AdvancedResourceType advancedResourceType = (AdvancedResourceType)activeProjectCost.ActiveProjectCostType;
				if (advancedResourceType == AdvancedResourceType.TIME) {
					//Time is free. No need to touch resources
					//We use new bigger number incase something about this changes
					activeProjectCost.CurrentValue = activeProjectCost.CurrentValue.AddNumber ((new BiggerNumber (1)).MultNumber (rate));
					activeProjectBaseData.TimeLeft = activeProjectCost.MaxValue.SubNumber (activeProjectCost.CurrentValue);
				} else {
					Debug.LogError ("non existed advancedResourceType:" + advancedResourceType.DisplayName);
					return false;
				}
			} else {
				Debug.LogError ("non created cost type");
				return false;
			}
		}
		return true;
	}

	private bool CanAffordToUpdate (ActiveProjectBaseData activeProjectBaseData, float rate)
	{
		foreach (ActiveProjectCost activeProjectCost in activeProjectBaseData.Costs) {
			if (activeProjectCost.isAdvancedResourceType ()) {
				AdvancedResourceType advancedResourceType = (AdvancedResourceType)activeProjectCost.ActiveProjectCostType;
				if (advancedResourceType == AdvancedResourceType.TIME) {
					continue;
				} else {
					Debug.LogError ("non existed advancedResourceType:" + advancedResourceType.DisplayName);
					return false;
				}
			} else {
				Debug.LogError ("non created cost type");
				return false;
			}
		}
		return true;
	}
   
}
