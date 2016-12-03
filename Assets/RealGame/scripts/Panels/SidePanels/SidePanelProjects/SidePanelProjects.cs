using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class SidePanelProjects : MonoBehaviour
{
	private List<SidePanelProjectsScript> sidePanelProjectsScripts;
	public Text sidePanelTitle;
	private ProjectManager projectManager;

	// Use this for initialization
	void Start ()
	{
		sidePanelTitle = this.transform.Find ("Title").Find ("Text").GetComponent<Text> ();
		sidePanelProjectsScripts = new List<SidePanelProjectsScript> ();
		sidePanelProjectsScripts.Add ((SidePanelProjectsScript)this.transform.Find ("ProjectOne").GetComponent ("SidePanelProjectsScript"));
		sidePanelProjectsScripts.Add ((SidePanelProjectsScript)this.transform.Find ("ProjectTwo").GetComponent ("SidePanelProjectsScript"));
		sidePanelProjectsScripts.Add ((SidePanelProjectsScript)this.transform.Find ("ProjectThree").GetComponent ("SidePanelProjectsScript"));
		SetBase ();
		projectManager = (ProjectManager)GameObject.Find ("Projects").GetComponent ("ProjectManager");
	}

	public void SetBase ()
	{
		foreach (SidePanelProjectsScript sidePanelProjectsScript in sidePanelProjectsScripts) {
			sidePanelProjectsScript.SetBase ();
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		List<ActiveProjectBaseData> activeProjects = projectManager.getActiveProjects (ProjectManager.SORT_NATURAL, 3);
		int totalActiveProjects = projectManager.ActiveProjects.Count;
		sidePanelTitle.text = "Projects (Displaying " + activeProjects.Count + "\\" + totalActiveProjects + ")";
		for (int i = 0; i < activeProjects.Count; i++) {
			sidePanelProjectsScripts [i].SidePanelProjectsData = GetSidePanelProjectData (activeProjects [i]);
		}
		for (int i = 2; i > activeProjects.Count - 1; i--) {
			sidePanelProjectsScripts [i].SidePanelProjectsData = null;
		}
	}

	private SidePanelProjectsData GetSidePanelProjectData (ActiveProjectBaseData activeProjectBaseData)
	{
		if (activeProjectBaseData.isBuildingType ()) {
			BuildingType buildingType = (BuildingType)activeProjectBaseData.ActiveProjectType;
			string titleName = buildingType.DisplayName;
			string requiresText = "Building";
			string progressText = TextConverter.Instance.getTime (activeProjectBaseData.TimeLeft.getValue ());
			ActiveProjectCost activeProjectCost = activeProjectBaseData.getSpecificCost (AdvancedResourceType.TIME);
			if (activeProjectCost == null) {
				Debug.LogError ("could not find type");
				return new SidePanelProjectsData ("Error!");
			}
			float currentPercent = (float)activeProjectCost.CurrentValue.DivideNumber (activeProjectCost.MaxValue).getValue () * 100;
			return new SidePanelProjectsData (titleName, requiresText, progressText, currentPercent);
		} else if (activeProjectBaseData.isTechnologyType ()) {
			TechnologyType technologyType = (TechnologyType)activeProjectBaseData.ActiveProjectType;
			string titleName = technologyType.DisplayName;
			string requiresText = "Technology";
			string progressText = TextConverter.Instance.getTime (activeProjectBaseData.TimeLeft.getValue ());
			ActiveProjectCost activeProjectCost = activeProjectBaseData.getSpecificCost (AdvancedResourceType.TIME);
			if (activeProjectCost == null) {
				Debug.LogError ("could not find type");
				return new SidePanelProjectsData ("Error!");
			}
			float currentPercent = (float)activeProjectCost.CurrentValue.DivideNumber (activeProjectCost.MaxValue).getValue () * 100;
			return new SidePanelProjectsData (titleName, requiresText, progressText, currentPercent);
		} else {
			Debug.LogError ("non created cost type");
			return new SidePanelProjectsData ("Error!");
		}
	}


}
