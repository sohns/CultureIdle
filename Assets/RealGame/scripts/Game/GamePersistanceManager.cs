using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class GamePersistanceManager
{
	private static string saveLocation = Application.persistentDataPath + "/savedGames.gd";
	//Managers
	private static ResourcesManager resourcesManager;
	private TechnologyManager technologyManager;
	private ProjectManager projectManager;
	private BuildingManager buildingManager;
	private InfoPanel infoPanel;
	private StartGame startGame;

	private static  string splitter = "---";

	public GamePersistanceManager ()
	{
		infoPanel = (InfoPanel)GameObject.FindWithTag ("InfoPanel").GetComponent ("InfoPanel");
		startGame = (StartGame)GameObject.FindWithTag ("StartGame").GetComponent ("StartGame");
		resourcesManager = (ResourcesManager)GameObject.Find ("Resources").GetComponent ("ResourcesManager");
		technologyManager = (TechnologyManager)GameObject.Find ("Technologies").GetComponent ("TechnologyManager");
		projectManager = (ProjectManager)GameObject.Find ("Projects").GetComponent ("ProjectManager");
		buildingManager = (BuildingManager)GameObject.Find ("Buildings").GetComponent ("BuildingManager");
	}


	public bool SaveGame ()
	{
		IncrementalManager.IsPaused = true;
		Debug.LogWarning ("start save:" + saveLocation);
		//System.Threading.Thread.Sleep (5000);
		GamePersistanceSave gamePersistanceSave = GenerateGamePersistanceSave ();
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (saveLocation);
		bf.Serialize (file, gamePersistanceSave);
		file.Close ();
		Debug.LogWarning ("saved:" + saveLocation);
		IncrementalManager.IsPaused = false;
		return true;
	}

	private  GamePersistanceSave GenerateGamePersistanceSave ()
	{
		//Technologies
		List<TechnologySave> technologySaves = GetTechnologySaves ();
		//Resources
		List<ResourceSave> resourceSaves = GetResourceSaves ();
		//Buildings
		List<BuildingSave> buildingSave = GetBuildingSave ();
		//ActiveProjectSave
		List<ActiveProjectSave> activeProjectSave = GetActiveProjectSave ();
		//SaveGameType
		StartGameSave startGameSave = GetStartGameSave ();
		return new GamePersistanceSave (technologySaves, resourceSaves, buildingSave, activeProjectSave, startGameSave);

	}

	StartGameSave GetStartGameSave ()
	{
		StartingPointType type = startGame.StartingPointType;
		string name = type.SaveName;
		return new StartGameSave (name);
	}

	List<ActiveProjectSave> GetActiveProjectSave ()
	{
		List<ActiveProjectSave> activeProjectSaves = new List<ActiveProjectSave> ();
		foreach (ActiveProjectBaseData data in projectManager.ActiveProjects) {
			double valueSmallCurrent = 0;
			double valueSmallTime = data.getSpecificCost (AdvancedResourceType.TIME).CurrentValue.getValue ();
			string name = "";
			if (data.isBuildingType ()) {
				BuildingType type = data.ActiveProjectType as BuildingType;
				name = type.GetType () + splitter + type.SaveName;
			} else if (data.isTechnologyType ()) {
				TechnologyType type = data.ActiveProjectType as TechnologyType;
				name = type.GetType () + splitter + type.SaveName;
			} else {
				Debug.LogError ("Loading unknowntype");
				continue;
			}
			activeProjectSaves.Add (new ActiveProjectSave (name, valueSmallCurrent, valueSmallTime));
		}
		return activeProjectSaves;
	}

	List<BuildingSave> GetBuildingSave ()
	{
		List<BuildingSave> buildingSaves = new List<BuildingSave> ();
		foreach (var item in buildingManager.Buildings) {
			BuildingType type = item.Key;
			BuildingData data = item.Value;
			double valueSmallCurrent = data.AmountOwned.getValue ();
			double valueSmallBuilt = data.AmountBuilt.getValue ();
			string name = type.GetType () + splitter + type.SaveName;
			buildingSaves.Add (new BuildingSave (name, valueSmallCurrent, valueSmallBuilt));
		}
		return buildingSaves;
	}

	private  List<TechnologySave> GetTechnologySaves ()
	{
		List<TechnologySave> technologySaves = new List<TechnologySave> ();
		foreach (var item in technologyManager.Technologies) {
			TechnologyType technologyType = item.Key;
			TechnologyData technologyData = item.Value;
			string name = technologyType.GetType () + splitter + technologyType.SaveName;
			bool isResearched = technologyData.IsResearched;
			bool isResearching = technologyData.IsResearching;	
			technologySaves.Add (new TechnologySave (name, 0, isResearched, isResearching));
		}
		return technologySaves;
	}

	private  List<ResourceSave> GetResourceSaves ()
	{
		//TODO:Make generic

		List<ResourceSave> resourceSaves = new List<ResourceSave> ();
		GetResourceSavesFromType (resourceSaves, resourcesManager.Resources, resourcesManager.TotalResources);
		GetResourceSavesFromType (resourceSaves, resourcesManager.AdvancedResources, resourcesManager.TotalAdvancedResources);
		GetResourceSavesFromType (resourceSaves, resourcesManager.CivilizationPoints, resourcesManager.TotalCivilizationPoints);
		return resourceSaves;
	}

	private  void GetResourceSavesFromType<A,B> (List<ResourceSave> resourceSaves, Dictionary<A,B> current, Dictionary<A,BiggerNumber> total)
	{
		foreach (A genericKey in current.Keys) {
			GenericResource genericResource = genericKey as GenericResource;
			string name = genericKey.GetType () + splitter + genericResource.SaveName;
			GenericResourceData genericResourceData = current [genericKey] as GenericResourceData;
			double valueSmallCurrent = genericResourceData.CurrentAmount.getValue ();
			double valueSmallTotal = total [genericKey].getValue ();
			resourceSaves.Add (new ResourceSave (name, valueSmallCurrent, valueSmallTotal));
		}
	}

	public  bool LoadGame ()
	{
		IncrementalManager.IsPaused = true;
		if (File.Exists (saveLocation)) {
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (saveLocation, FileMode.Open);
			GamePersistanceSave gamePersistanceSave = (GamePersistanceSave)bf.Deserialize (file);
			file.Close ();
			//Loading Time
			LoadFromPersistanceSave (gamePersistanceSave);
			Debug.LogWarning ("loaded:" + saveLocation);
			resetDisplay ();
			IncrementalManager.IsPaused = false;
		} else {
			Debug.LogWarning ("cant load:" + saveLocation);
			IncrementalManager.IsPaused = false;
			return false;
		}
		return true;
	}

	public bool ResetGame ()
	{
		IncrementalManager.IsPaused = true;
		resetData ();
		if (File.Exists (saveLocation)) {
			File.Delete (saveLocation);
			Debug.LogWarning ("Delete Save:" + saveLocation);
			IncrementalManager.IsPaused = false;
		} else {
			Debug.LogWarning ("No Save To Delete:" + saveLocation);
		}
		resetDisplay ();
		IncrementalManager.IsPaused = false;
		return true;
	}

	private void resetDisplay ()
	{
		infoPanel.resetPanels ();
	}

	private void resetData ()
	{
		//Buildings
		ResetBuilding ();
		//ActiveProjectSave
		ResetActiveProjectSave ();
		//Resources
		ResetResources ();
		//Technologies
		ResetTechnologies ();
	}

	void ResetTechnologies ()
	{
		technologyManager.reset ();
	}

	void ResetResources ()
	{
		resourcesManager.reset ();
	}

	void ResetActiveProjectSave ()
	{
		projectManager.reset ();
	}

	void ResetBuilding ()
	{
		buildingManager.reset ();
	}

	private  void LoadFromPersistanceSave (GamePersistanceSave gamePersistanceSave)
	{
		//Start Game
		StartGameSave startGameSave = gamePersistanceSave.StartGameSave;
		StartingPointType startingPointType = StartingPointType.getType (startGameSave.Name);
		if (startingPointType == null) {
			Debug.LogError ("corrupt save starting value:" + startGameSave.Name);
			return;
		}
		//Buildings
		LoadFromBuildingSaves (gamePersistanceSave.BuildingSaves);
		//ActiveProjectSave
		LoadFromActiveProjectSaves (gamePersistanceSave.ActiveProjectSaves);
		//Resources
		LoadFromResourceSaves (gamePersistanceSave.ResourceSaves);
		//Technologies
		LoadFromTechnologySaves (gamePersistanceSave.TechnologySaves);
	}

	void LoadFromBuildingSaves (List<BuildingSave> buildingSaves)
	{
		Dictionary<BuildingType,BuildingData> buildings = new Dictionary<BuildingType,BuildingData> ();
		foreach (BuildingSave save in buildingSaves) {
			string[] splitName = save.Name.Split (splitter.ToCharArray (), System.StringSplitOptions.RemoveEmptyEntries);
			string genericResourceType = splitName [0];
			string cleanName = splitName [1];
			BiggerNumber amount = save.ValueSmallCurrent;
			BiggerNumber amountBuilt = save.ValueSmallBuilt;
			BuildingType type = BuildingType.getType (cleanName);
			if (type != null) {
				BuildingData data = new BuildingData (type, amount, amountBuilt);
				buildings.Add (type, data);
			} else {
				Debug.LogError ("Loading unknowntype");
			}
		}
		buildingManager.Buildings = buildings;
	}

	void LoadFromActiveProjectSaves (List<ActiveProjectSave> activeProjectSaves)
	{
		List<ActiveProjectBaseData> activeProjects = new List<ActiveProjectBaseData> ();
		foreach (ActiveProjectSave save in activeProjectSaves) {
			string[] splitName = save.Name.Split (splitter.ToCharArray (), System.StringSplitOptions.RemoveEmptyEntries);
			string genericResourceType = splitName [0];
			string cleanName = splitName [1];
			ActiveProjectBaseData activeProjectBaseData = null;
			BiggerNumber timeLeft = save.ValueSmallTime;
			if (genericResourceType.Equals (typeof(BuildingType).Name)) {
				BuildingType type = BuildingType.getType (cleanName);
				if (type != null) {
					activeProjectBaseData = new ActiveProjectBuildingData (type, type.Time.BaseAmount);
					ActiveProjectCost activeProjectCost = activeProjectBaseData.getSpecificCost (AdvancedResourceType.TIME);
					activeProjectCost.CurrentValue = activeProjectCost.CurrentValue = timeLeft;
					activeProjectBaseData.TimeLeft = activeProjectCost.MaxValue.SubNumber (activeProjectCost.CurrentValue);
				} else {
					Debug.LogError ("Loading unknowntype");
				}
			} else if (genericResourceType.Equals (typeof(TechnologyType).Name)) {
				TechnologyType type = TechnologyType.getType (cleanName);
				if (type != null) {
					activeProjectBaseData = new ActiveProjectTechnologyData (type, type.Time.BaseAmount);
					ActiveProjectCost activeProjectCost = activeProjectBaseData.getSpecificCost (AdvancedResourceType.TIME);
					activeProjectCost.CurrentValue = activeProjectCost.CurrentValue = timeLeft;
					activeProjectBaseData.TimeLeft = activeProjectCost.MaxValue.SubNumber (activeProjectCost.CurrentValue);
				} else {
					Debug.LogError ("Loading unknowntype");
				}
			} else {
				Debug.LogError ("Loading unknowntype");
				continue;
			}
			activeProjects.Add (activeProjectBaseData);
		}
		projectManager.ActiveProjects = activeProjects;
	}


	void LoadFromTechnologySaves (List<TechnologySave> technologySaves)
	{
		Dictionary<TechnologyType,TechnologyData> technologies = new Dictionary<TechnologyType,TechnologyData> ();
		foreach (TechnologySave technologySave in technologySaves) {
			string[] splitName = technologySave.Name.Split (splitter.ToCharArray (), System.StringSplitOptions.RemoveEmptyEntries);
			string genericResourceType = splitName [0];
			string cleanName = splitName [1];
			TechnologyType type = TechnologyType.getType (cleanName);
			if (type != null) {
				TechnologyData data = new TechnologyData (type);
				data.IsResearched = technologySave.IsResearched;
				data.IsResearching = technologySave.IsResearching;
				technologies.Add (type, data);
			} else {
				Debug.LogError ("Loading unknowntype");
			}
		}
		technologyManager.Technologies = technologies;
	}

	private  void LoadFromResourceSaves (List<ResourceSave> resourceSaves)
	{
		//Resources
		Dictionary<ResourceType,ResourceData> resources = new Dictionary<ResourceType,ResourceData> ();
		Dictionary<ResourceType,BiggerNumber> totalResources = new Dictionary<ResourceType,BiggerNumber> ();
		//AdvancedResources
		Dictionary<AdvancedResourceType,AdvancedResourceData> advancedResources = new Dictionary<AdvancedResourceType,AdvancedResourceData> ();
		Dictionary<AdvancedResourceType,BiggerNumber> totalAdvancedResources = new Dictionary<AdvancedResourceType,BiggerNumber> ();
		//CivilizationPoints
		Dictionary<CivilizationPointType,CivilizationPointData> civilizationPoints = new Dictionary<CivilizationPointType,CivilizationPointData> ();
		Dictionary<CivilizationPointType,BiggerNumber> totalCivilizationPoints = new Dictionary<CivilizationPointType,BiggerNumber> ();
		foreach (ResourceSave resourceSave in resourceSaves) {
			string[] splitName = resourceSave.Name.Split (splitter.ToCharArray (), System.StringSplitOptions.RemoveEmptyEntries);
			string genericResourceType = splitName [0];
			string cleanName = splitName [1];
			//TODO:make generic
			if (genericResourceType.Equals (typeof(ResourceType).Name)) {
				ResourceType type = ResourceType.getType (cleanName);
				if (type != null) {
					resources.Add (type, new ResourceData (type, new BiggerNumber (resourceSave.ValueSmallCurrent)));
					totalResources.Add (type, new BiggerNumber (resourceSave.ValueSmallCurrent));
				} else {
					Debug.LogError ("Loading unknowntype");
				}
			} else if (genericResourceType.Equals (typeof(AdvancedResourceType).Name)) {
				AdvancedResourceType type = AdvancedResourceType.getType (cleanName);
				if (type != null) {
					advancedResources.Add (type, new AdvancedResourceData (type, new BiggerNumber (resourceSave.ValueSmallCurrent), 0));
					totalAdvancedResources.Add (type, new BiggerNumber (resourceSave.ValueSmallCurrent));
				} else {
					Debug.LogError ("Loading unknowntype");
				}
			} else if (genericResourceType.Equals (typeof(CivilizationPointType).Name)) {
				CivilizationPointType type = CivilizationPointType.getType (cleanName);
				if (type != null) {
					civilizationPoints.Add (type, new CivilizationPointData (type, new BiggerNumber (resourceSave.ValueSmallCurrent)));
					totalCivilizationPoints.Add (type, new BiggerNumber (resourceSave.ValueSmallCurrent));
				} else {
					Debug.LogError ("Loading unknowntype");
				}
			} else {
				Debug.LogError ("Loading unknowntype");
			}
		}
		resourcesManager.resetResources (resources, totalResources, advancedResources, totalAdvancedResources, civilizationPoints, totalCivilizationPoints);
	}

}
