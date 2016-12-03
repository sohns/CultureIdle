using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class GamePersistanceSave
{
	List<TechnologySave> technologySaves;
	List<ResourceSave> resourceSaves;
	List<BuildingSave> buildingSaves;
	List<ActiveProjectSave> activeProjectSaves;
	StartGameSave startGameSave;

	public GamePersistanceSave (List<TechnologySave> technologySaves, List<ResourceSave> resourceSaves, List<BuildingSave> buildingSaves, List<ActiveProjectSave> activeProjectSaves, StartGameSave startGameSave)
	{
		this.technologySaves = technologySaves;
		this.resourceSaves = resourceSaves;
		this.buildingSaves = buildingSaves;
		this.activeProjectSaves = activeProjectSaves;
		this.startGameSave = startGameSave;
	}

	public StartGameSave StartGameSave {
		get {
			return this.startGameSave;
		}
	}

	public List<TechnologySave> TechnologySaves {
		get {
			return this.technologySaves;
		}
	}

	public List<ResourceSave> ResourceSaves {
		get {
			return this.resourceSaves;
		}
	}

	public List<BuildingSave> BuildingSaves {
		get {
			return this.buildingSaves;
		}
	}

	public List<ActiveProjectSave> ActiveProjectSaves {
		get {
			return this.activeProjectSaves;
		}
	}
}
