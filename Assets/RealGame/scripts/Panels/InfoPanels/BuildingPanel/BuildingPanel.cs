using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class BuildingPanel : MonoBehaviour, Panel
{

	//Things needed
	private GameObject buildingColumn;
	private BuildingManager buildingManager;
	private Transform buildingData, scrollContent;
	private InfoPanel infoPanel;


	//Public and used for testing
	public bool isActiveTab;

	//Used for data
	private float offSetY;
	private int columnInfoHeight;
	private Font columnFont;
	private int sortType;
	private bool isNeedingUpdating = false;

	private string columnInfoName = "columnInfo";


	public void infomationHasChanged ()
	{
		isNeedingUpdating = true;
	}

	void Start ()
	{

		buildingManager = (BuildingManager)GameObject.Find ("Buildings").GetComponent ("BuildingManager");
		buildingColumn = (GameObject)Resources.Load ("BuildingPanelColumn");
		columnFont = Resources.GetBuiltinResource<Font> ("Arial.ttf");
		offSetY = -120;
		columnInfoHeight = 60;
		sortType = 0;
		buildingData = this.transform.Find ("BuildingData");
		scrollContent = buildingData.transform.Find ("ScrollRect").transform.Find ("ScrollContent");
	}

	private Button setUpSortButton (string name, int i)
	{
		Button button = this.transform.Find ("Data").Find ("ColumnsTitle").Find (name).gameObject.GetComponent<Button> ();
		button.onClick.AddListener (() => setSortType (i));
		return button;
	}

	public void setInitialInfo (InfoPanel infoPanel)
	{
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

	void setSubScriptInfo (GameObject newObject, BuildingColumnData buildingColumnData, bool initialTime)
	{
		BuildingColumnScript subscript = (BuildingColumnScript)newObject.GetComponent ("BuildingColumnScript");
		if (initialTime) {
			subscript.SetBase (buildingColumnData.BuildingType, infoPanel);
		} else {
			subscript.SetShallowData (buildingColumnData.BuildingType);
		}
		subscript.NameText.text = buildingColumnData.Name;
		subscript.NameText.font = columnFont;
		subscript.AmountText.text = buildingColumnData.Amount.ToDisplayString (0, true);
		subscript.AmountText.font = columnFont;
		subscript.CostText.text = buildingColumnData.CostText;
		subscript.AmountText.font = columnFont;
		if (buildingColumnData.CanBuy) {
			subscript.BuyButton.interactable = true;
			Text buttonText = (Text)subscript.BuyButton.transform.Find ("Text").gameObject.GetComponent<Text> ();
			buttonText.text = "Start Building";
		} else {
			subscript.BuyButton.interactable = false;
			Text buttonText = (Text)subscript.BuyButton.transform.Find ("Text").gameObject.GetComponent<Text> ();
			buttonText.text = "Cannot Afford";
		}
	}

	private Dictionary<BuildingType,BuildingColumnData> getBuildingColumnData ()
	{
		Dictionary<BuildingType,BuildingData> buildings = buildingManager.Buildings;
		Dictionary<BuildingType,BuildingColumnData> buildingColumnDatas = new Dictionary<BuildingType,BuildingColumnData> ();
		foreach (BuildingType buildingType in buildings.Keys) {
			BuildingData buildingData = buildings [buildingType];
			string name = buildingType.DisplayName;
			BiggerNumber amount = buildingData.AmountOwned;
			string innerCost = buildingType.CostsString;
			bool canBuy = buildingManager.canAffordBuilding (buildingType);
			BuildingColumnData buildingColumnData = new BuildingColumnData (name, amount, innerCost, canBuy, buildingType);
			buildingColumnDatas.Add (buildingType, buildingColumnData);
		}
		return buildingColumnDatas;
	}

	private void updateInfo ()
	{
		if (scrollContent != null) {
			Dictionary<BuildingType,BuildingColumnData> buildingColumnDataMap = getBuildingColumnData ();
			foreach (Transform child in scrollContent) {
				GameObject childGameObject = child.gameObject;
				BuildingColumnScript subscript = (BuildingColumnScript)childGameObject.GetComponent ("BuildingColumnScript");
				BuildingType buildingType = subscript.BuildingType;
				setSubScriptInfo (childGameObject, buildingColumnDataMap [buildingType], false);
			}
		}
	}

	private void createInfo ()
	{
		clearChildren ();
		Dictionary<BuildingType,BuildingColumnData> buildingColumnDataMap = getBuildingColumnData ();
		List<BuildingColumnData> buildingColumnDatas = new List<BuildingColumnData> ();
		foreach (BuildingColumnData buildingColumnData in buildingColumnDataMap.Values) {
			buildingColumnDatas.Add (buildingColumnData);
		}
		int i = 0;
		foreach (BuildingColumnData buildingColumnData in buildingColumnDatas) {
			GameObject newObject = Instantiate (buildingColumn);
			float posY = -(columnInfoHeight * i) + offSetY;
			setColumnInfoObject (newObject, posY, i, scrollContent);
			setSubScriptInfo (newObject, buildingColumnData, true);
			i++;
		}
	}

	public void resetInfo ()
	{
		isNeedingUpdating = true;
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
