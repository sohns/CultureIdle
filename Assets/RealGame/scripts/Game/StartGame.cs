using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StartGame : MonoBehaviour
{

	private IncrementalManager incrementerManager;
	private BuildingManager buildingManager;
	private ResourcesManager resourcesManager;
	//	private ChallengeManager challengeManager;
	private ProjectManager projectManager;
	private TechnologyManager technologyManager;
	private GamePersistanceManager gamePersistanceManager;
	//variables
	private StartingPointType startingPointType;

	// Use this for initialization
	void Start ()
	{
		//TODO make cooler starts
		startingPointType = StartingPointType.SLOW_TESTING;
		gamePersistanceManager = new GamePersistanceManager ();
		incrementerManager = (IncrementalManager)GameObject.Find ("Incrementer").GetComponent ("IncrementalManager");
		buildingManager = (BuildingManager)GameObject.Find ("Buildings").GetComponent ("BuildingManager");
		resourcesManager = (ResourcesManager)GameObject.Find ("Resources").GetComponent ("ResourcesManager");
		//	challengeManager = (ChallengeManager)GameObject.Find ("Challenges").GetComponent ("ChallengeManager");
		projectManager = (ProjectManager)GameObject.Find ("Projects").GetComponent ("ProjectManager");
		technologyManager = (TechnologyManager)GameObject.Find ("Technologies").GetComponent ("TechnologyManager");

		//Connect Managers as required:
		incrementerManager.ToIncrement.Add (resourcesManager);
		incrementerManager.ToIncrement.Add (buildingManager);
		incrementerManager.ToIncrement.Add (projectManager);
		incrementerManager.ToIncrement.Add (technologyManager);

//		//Add base resources:
//		resourcesManager.addNewResource (ResourceType.FOOD, 0);
//		resourcesManager.addNewResource (ResourceType.WOOD, 0);
//		resourcesManager.addNewResource (ResourceType.BONE, 5);
//		resourcesManager.addNewResource (ResourceType.HIDE, 7);
//
//		resourcesManager.addNewResource (CivilizationPointType.SKIRMISH, 5);
//		resourcesManager.addNewResource (CivilizationPointType.POPULATION, 5);
//
//		//Add base buildings:
//		List<BuildingData> baseBuildings = new List<BuildingData> ();
//		baseBuildings.Add (new BuildingData (BuildingType.HUT, new BiggerNumber (5)));
//		baseBuildings.Add (new BuildingData (BuildingType.FORAGING_HUT, 0));
//		baseBuildings.Add (new BuildingData (BuildingType.HUNTING_HUT, 11));
//		buildingManager.initializeBuildings (baseBuildings);
//
//		//Add base technologies;
//		List<TechnologyData> baseTechnologies = new List<TechnologyData> ();
//		baseTechnologies.Add (new TechnologyData (TechnologyType.GET_RESEARCHES));
//		technologyManager.initializeTechnologies (baseTechnologies);
		LoadGame ();
	}

	public StartingPointType StartingPointType {
		get {
			return this.startingPointType;
		}
	}

	private void LoadGame ()
	{
		//Load from base here...
		if (!gamePersistanceManager.LoadGame ()) {
			foreach (SimpleCurrencyData data in startingPointType.BaseResources) {
				resourcesManager.addNewResource (data.CurrencyType as ResourceType, data.BaseAmount);
			}
			List<BuildingData> baseBuildings = new List<BuildingData> ();
			foreach (SimpleCurrencyData data in startingPointType.BaseBuildings) {
				baseBuildings.Add (new BuildingData (data.CurrencyType as BuildingType, data.BaseAmount));
			}
			buildingManager.initializeBuildings (baseBuildings);
			List<TechnologyData> baseTechnologies = new List<TechnologyData> ();
			foreach (SimpleCurrencyData data in startingPointType.BaseTechnologies) {
				baseTechnologies.Add (new TechnologyData (data.CurrencyType as TechnologyType));
			}
			technologyManager.initializeTechnologies (baseTechnologies);
			foreach (SimpleCurrencyData data in startingPointType.BaseCivilizationPoints) {
				resourcesManager.addNewResource (data.CurrencyType as CivilizationPointType, data.BaseAmount);
			}
		}
		Debug.LogWarning ("gameStarted");
	}

	public void SetSaveGame ()
	{
		gamePersistanceManager.SaveGame ();
	}

	public void SetLoadGame ()
	{
		gamePersistanceManager.LoadGame ();
	}

	public void SetResetGame ()
	{
		gamePersistanceManager.ResetGame ();
		LoadGame ();
	}
}
