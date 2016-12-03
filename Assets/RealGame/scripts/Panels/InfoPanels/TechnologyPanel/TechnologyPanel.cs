using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TechnologyPanel : MonoBehaviour, Panel {

	//Things needed
	private GameObject technologyColumn;
	private TechnologyManager technologyManager;
	private Transform technologyData, scrollContent;
	private bool isNeedingUpdating = false;
	private InfoPanel infoPanel;

	//Public and used for testing
	public bool isActiveTab;


	//Used for data
	private float offSetY;
	private int columnInfoHeight;
	private Font columnFont;
	private int sortType;

	private string columnInfoName = "columnInfo";

	public void infomationHasChanged(){
		isNeedingUpdating = true;
	}

	void Start ()
	{
		technologyManager = (TechnologyManager)GameObject.Find("Technologies").GetComponent ("TechnologyManager");
		technologyColumn = (GameObject) Resources.Load ("TechnologyPanelColumn");
		columnFont = Resources.GetBuiltinResource<Font> ("Arial.ttf");
		offSetY = -120;
		columnInfoHeight = 60;
		sortType = 0;
		technologyData = this.transform.Find ("TechnologyData");
		scrollContent = technologyData.transform.Find ("ScrollRect").transform.Find ("ScrollContent");
	}
	public void setInitialInfo(InfoPanel infoPanel){
		this.infoPanel = infoPanel;
		this.transform.localPosition = new Vector3 (0, 0, 0);
		this.setInactive ();
	}

	public void setActive ()
	{
		this.gameObject.SetActive (true);
		isActiveTab = true;
		isNeedingUpdating = true;
	}

	public void setInactive ()
	{
		clearChildren ();
		this.gameObject.SetActive (false);
		isActiveTab = false;
		isNeedingUpdating = false;
	}

	private void setColumnInfoObject (GameObject columnInfoObject, float posY, int number, Transform scrollContent)
	{
		columnInfoObject.name = columnInfoName + number;
		columnInfoObject.transform.SetParent (scrollContent);
		//Set Scale
		columnInfoObject.transform.localScale = new Vector3 (1, 1, 1);
		//Set Position
		RectTransform columnInfoObjectRectTransform = columnInfoObject.GetComponent<RectTransform> ();
		float height = columnInfoObjectRectTransform.rect.height;
		columnInfoObjectRectTransform.offsetMin = new Vector2 (0, posY);
		float maxY = columnInfoObjectRectTransform.offsetMin.y + height;
		columnInfoObjectRectTransform.offsetMax = new Vector2 (0, maxY);

	}

	// Update is called once per frame
	void Update ()
	{
		if (isActiveTab) {
			if (isNeedingUpdating) {
				createInfo ();
				isNeedingUpdating = false;
			} else {
				updateInfo ();
			}
		}
	}

	void setSubScriptInfo (GameObject newObject,TechnologyColumnData technologyColumnData,bool initialTime)
	{
		TechnologyColumnScript subscript = (TechnologyColumnScript)newObject.GetComponent ("TechnologyColumnScript");
		if (initialTime) {
			subscript.SetBase (technologyColumnData.TechnologyType, infoPanel);
		} else {
			subscript.SetShallowData (technologyColumnData.TechnologyType);
		}
		subscript.NameText.text = technologyColumnData.Name;
		subscript.NameText.font = columnFont;
		subscript.CostText.text = technologyColumnData.CostText;
		if (technologyColumnData.IsResearched) {
			subscript.StatusButton.interactable = false;
			Text buttonText = (Text)subscript.StatusButton.transform.Find ("Text").gameObject.GetComponent<Text> ();
			buttonText.text = "Researched";
		}
		else if (technologyColumnData.IsResearching) {
			subscript.StatusButton.interactable = false;
			Text buttonText = (Text)subscript.StatusButton.transform.Find ("Text").gameObject.GetComponent<Text> ();
			buttonText.text = "Researching";
		}
		else if (technologyColumnData.CanBuy) {
			subscript.StatusButton.interactable = true;
			Text buttonText = (Text)subscript.StatusButton.transform.Find ("Text").gameObject.GetComponent<Text> ();
			buttonText.text = "Start Researching";
		}
		else {
			subscript.StatusButton.interactable = false;
			Text buttonText = (Text)subscript.StatusButton.transform.Find ("Text").gameObject.GetComponent<Text> ();
			buttonText.text = "Cannot Afford";
		}
	}
	private Dictionary<TechnologyType,TechnologyColumnData> getTechnologyColumnData(){
		Dictionary<TechnologyType,TechnologyData> technologies = technologyManager.Technologies;
		Dictionary<TechnologyType,TechnologyColumnData> technologyColumnDatas = new Dictionary<TechnologyType,TechnologyColumnData> ();
		foreach (TechnologyType technologyType in technologies.Keys) {
			TechnologyData technologyData = technologies [technologyType];
			string name=technologyType.DisplayName;
			string costText=technologyType.CostsString;
			bool canBuy=technologyManager.canAffordTechnology(technologyType);
			bool isResearching = technologyData.IsResearching;
			bool isResearched = technologyData.IsResearched;
			TechnologyColumnData technologyColumnData = new TechnologyColumnData (name, costText, canBuy, isResearching, isResearched, technologyType);
			technologyColumnDatas.Add (technologyType, technologyColumnData);
		}
		return technologyColumnDatas;
	}

	private void updateInfo(){
		if (scrollContent != null) {
			Dictionary<TechnologyType,TechnologyColumnData> technologyColumnDataMap=getTechnologyColumnData();
			foreach (Transform child in scrollContent) {
				GameObject childGameObject = child.gameObject;
				TechnologyColumnScript subscript = (TechnologyColumnScript)childGameObject.GetComponent ("TechnologyColumnScript");
				TechnologyType technologyType = subscript.TechnologyType;
				setSubScriptInfo (childGameObject, technologyColumnDataMap [technologyType],false);
			}
		}
	}

	private void createInfo(){
		clearChildren ();
		Dictionary<TechnologyType,TechnologyColumnData> technologyColumnDataMap = getTechnologyColumnData ();
		List<TechnologyColumnData> technologyColumnDatas = new List<TechnologyColumnData> ();
		foreach (TechnologyColumnData technologyColumnData in technologyColumnDataMap.Values) {
			technologyColumnDatas.Add (technologyColumnData);
		}
		int i = 0;
		foreach (TechnologyColumnData technologyColumnData in technologyColumnDatas) {
			GameObject newObject = Instantiate (technologyColumn);
			float posY = -(columnInfoHeight * i) + offSetY;
			setColumnInfoObject (newObject, posY, i, scrollContent);
			setSubScriptInfo (newObject,technologyColumnData,true);
			i++;
		}
	}

	public void resetInfo(){
		isNeedingUpdating=true;
	}
	private void clearChildren ()
	{
		var children = new List<GameObject> ();
		if (scrollContent != null) {
			foreach (Transform child in scrollContent) {
				children.Add (child.gameObject);
			}
			children.ForEach (child => Destroy (child));
		}
	}

	public void setSortType (int i)
	{
		if (i == Mathf.Abs (sortType)) {
			sortType = sortType * -1;
		} else {
			sortType = i;
		}
	}
}
