using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InfoPanel : MonoBehaviour
{

	private ChallengePanel challengePanel;
	private ResourcePanel resourcePanel;
	private CivilizationPointPanel civilizationPointPanel;
	private KingdomStatisticPanel kingdomStatisticPanel;
	private BuildingPanel buildingPanel;
	private InfoPanelEnum currentInfoPanel;
	private ExtraInfoPanel extraInfoPanel;
	private TechnologyPanel technologyPanel;
	private Dictionary<InfoPanelEnum,Panel> panels = new Dictionary<InfoPanelEnum,Panel> ();

	// Use this for initialization
	void Start ()
	{
		extraInfoPanel = (ExtraInfoPanel)GameObject.Find ("ExtraInfoPanel").GetComponent ("ExtraInfoPanel");
		setInitial (extraInfoPanel, InfoPanelEnum.EXTRA_INFO);
		challengePanel = (ChallengePanel)GameObject.Find ("ChallengesPanel").GetComponent ("ChallengePanel");
		setInitial (challengePanel, InfoPanelEnum.CHALLENGES);

		kingdomStatisticPanel = (KingdomStatisticPanel)GameObject.Find ("KingdomStatisticPanel").GetComponent ("KingdomStatisticPanel");
		setInitial (kingdomStatisticPanel, InfoPanelEnum.KINGDOM_STATISTICS);
		resourcePanel = (ResourcePanel)GameObject.Find ("ResourcePanel").GetComponent ("ResourcePanel");
		setInitial (resourcePanel, InfoPanelEnum.KINGDOM_RESOURCES);
		civilizationPointPanel = (CivilizationPointPanel)GameObject.Find ("CivilizationPointPanel").GetComponent ("CivilizationPointPanel");
		setInitial (civilizationPointPanel, InfoPanelEnum.KINGDOM_CIVILIZATION_POINTS);

		buildingPanel = (BuildingPanel)GameObject.Find ("BuildingPanel").GetComponent ("BuildingPanel");
		setInitial (buildingPanel, InfoPanelEnum.PROJECT_BUILDINGS);
		technologyPanel = (TechnologyPanel)GameObject.Find ("TechnologyPanel").GetComponent ("TechnologyPanel");
		setInitial (technologyPanel, InfoPanelEnum.PROJECT_TECHNOLOGIES);

	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public void resetPanels ()
	{
		foreach (Panel panel in panels.Values) {
			panel.infomationHasChanged ();
		}
	}

	public void resetPanel (InfoPanelEnum infoPanelEnum)
	{
		panels [infoPanelEnum].infomationHasChanged ();
	}

	public void setExtraInfo (ExtraInfoData extraInfoData)
	{
		extraInfoPanel.ExtraInfoData = extraInfoData;
	}

	public void hideExtraInfo ()
	{
		extraInfoPanel.setInactive ();
	}

	private void setInitial (Panel panelToSet, InfoPanelEnum infoPanelEnum)
	{
		panels.Add (infoPanelEnum, panelToSet);
		panelToSet.setInitialInfo (this);
	}

	private void hideAll ()
	{
		foreach (Panel panel in panels.Values) {
			panel.setInactive ();
		}
	}

	public void setPanel (InfoPanelEnum newInfoPanel)
	{
		if (!panels.ContainsKey (newInfoPanel)) {
			Debug.LogError ("You tried to load a info panel that is not in InfoPanel.cs Name:" + newInfoPanel.Name);
			return;
		}
		if (newInfoPanel != currentInfoPanel) {
			hideAll ();
			Panel panel = panels [newInfoPanel];
			panel.setActive ();
			currentInfoPanel = newInfoPanel;
		} else {
		}
	}

}
