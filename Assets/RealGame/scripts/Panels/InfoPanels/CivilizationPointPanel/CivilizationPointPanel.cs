using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CivilizationPointPanel : MonoBehaviour, Panel
{

	//Things needed
	private GameObject exampleColumn;
	private ResourcesManager resourceManager;
	private Transform data, scrollContent;
	private InfoPanel infoPanel;
	private Button nameTitleButton, levelTitleButton, baseTitleButton, totalEarnedTitleButton, currentAmountTitleButton, maxAmountTitleButton;
	//Public and used for testing
	public bool isActiveTab;

	//Used for data
	private float offSetY;
	private int columnInfoHeight;
	private Font columnFont;
	private int sortType;
	private bool isNeedingUpdating = false;

	public void infomationHasChanged ()
	{
		isNeedingUpdating = true;
	}

	private string columnInfoName = "columnInfo";

	void Start ()
	{
		nameTitleButton = setUpSortButton ("Name", 1);
		levelTitleButton = setUpSortButton ("Level", 2);
		baseTitleButton = setUpSortButton ("Base", 3);
		totalEarnedTitleButton = setUpSortButton ("TotalEarned", 4);
		currentAmountTitleButton = setUpSortButton ("CurrentAmount", 5);
		maxAmountTitleButton = setUpSortButton ("MaxAmount", 6);

		resourceManager = (ResourcesManager)GameObject.Find ("Resources").GetComponent ("ResourcesManager");
		exampleColumn = (GameObject)Resources.Load ("CivilizationPointPanelColumn");
		columnFont = Resources.GetBuiltinResource<Font> ("Arial.ttf");
		offSetY = -120;
		columnInfoHeight = 60;
		sortType = 0;
		data = this.transform.Find ("Data");
		scrollContent = data.transform.Find ("ScrollRect").transform.Find ("ScrollContent");
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

	//TODO figure a way to not need this!!!
	private BiggerNumber getAmount (Dictionary<CivilizationPointType,BiggerNumber> mapToGetFrom, CivilizationPointType typeToGet)
	{
		if (mapToGetFrom.ContainsKey (typeToGet)) {
			return mapToGetFrom [typeToGet];
		}
		return 0;
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

	void setSubScriptInfo (GameObject newObject, CivilizationPointColumnData columnData, bool initialTime)
	{
		CivilizationPointColumnScript subscript = (CivilizationPointColumnScript)newObject.GetComponent ("CivilizationPointColumnScript");
		if (initialTime) {
			subscript.SetBase (columnData.Type, infoPanel);
		} else {
			subscript.SetShallowData (columnData.Type);
		}
		subscript.NameText.text = columnData.Name;
		subscript.NameText.font = columnFont;
		subscript.LevelText.text = columnData.Level.ToDisplayString (3, true);
		subscript.LevelText.font = columnFont;
		subscript.BaseText.text = columnData.BasePerSec.ToDisplayString (3, false);
		subscript.BaseText.font = columnFont;
		subscript.TotalEarnedText.text = columnData.TotalEarned.ToDisplayString (2, false);
		subscript.TotalEarnedText.font = columnFont;
		subscript.CurrentAmountText.text = columnData.CurrentAmount.ToDisplayString (3, false);
		subscript.CurrentAmountText.font = columnFont;
		subscript.MaxAmountText.text = columnData.MaxAmount.ToDisplayString (3, true);
		subscript.MaxAmountText.font = columnFont;
	}

	private Dictionary<CivilizationPointType,CivilizationPointColumnData> getColumnDataMap ()
	{
		Dictionary<CivilizationPointType,CivilizationPointData> civilizationPointsCurrent = resourceManager.CivilizationPoints;
		Dictionary<CivilizationPointType,BiggerNumber> totalCivilizationPoints = resourceManager.TotalCivilizationPoints;
		Dictionary<CivilizationPointType,BiggerNumber> currentRateCivilizationPoints = resourceManager.CurrentRateCivilizationPoints;
		Dictionary<CivilizationPointType,CivilizationPointColumnData> columnDatas = new Dictionary<CivilizationPointType,CivilizationPointColumnData> ();
		foreach (CivilizationPointType type in civilizationPointsCurrent.Keys) {
			CivilizationPointData data = civilizationPointsCurrent [type]; 
			string name = type.DisplayName;
			BiggerNumber level = data.Level;
			BiggerNumber basePerSec = getAmount (currentRateCivilizationPoints, type);
			BiggerNumber totalEarned = totalCivilizationPoints [type];
			BiggerNumber currentAmount = data.CurrentAmount;
			BiggerNumber maxAmount = data.MaxAmount;
			CivilizationPointColumnData columnData = new CivilizationPointColumnData (name, level, basePerSec, totalEarned, currentAmount, maxAmount, type);
			columnDatas.Add (type, columnData);
		}
		return columnDatas;
	}

	private void updateInfo ()
	{
		if (scrollContent != null) {
			Dictionary<CivilizationPointType,CivilizationPointColumnData> columnDataMap = getColumnDataMap ();
			foreach (Transform child in scrollContent) {
				GameObject childGameObject = child.gameObject;
				CivilizationPointColumnScript subscript = (CivilizationPointColumnScript)childGameObject.GetComponent ("CivilizationPointColumnScript");
				CivilizationPointType type = subscript.Type;
				setSubScriptInfo (childGameObject, columnDataMap [type], false);
			}
		}
	}

	private void createInfo ()
	{
		clearChildren ();
		Dictionary<CivilizationPointType,CivilizationPointColumnData> columnDataMap = getColumnDataMap ();
		List<CivilizationPointColumnData> columnDatas = new List<CivilizationPointColumnData> ();
		foreach (CivilizationPointColumnData columnData in columnDataMap.Values) {
			columnDatas.Add (columnData);
		}
		int i = 0;
		sortColumnDatas (columnDatas);
		foreach (CivilizationPointColumnData columnData in columnDatas) {
			GameObject newObject = Instantiate (exampleColumn);
			float posY = -(columnInfoHeight * i) + offSetY;
			setColumnInfoObject (newObject, posY, i, scrollContent);
			setSubScriptInfo (newObject, columnData, true);
			i++;
		}
	}


	private void sortColumnDatas (List<CivilizationPointColumnData> columnDatas)
	{
		int absolutSortType = Mathf.Abs (sortType);
		switch (absolutSortType) {
		case 0:
			//No sort
			break;
		case 1:
			columnDatas.Sort ((p, q) => p.Name.CompareTo (q.Name));
			break;
		case 2:
			columnDatas.Sort ((p, q) => p.Level.CompareTo (q.Level));
			break;
		case 3:
			columnDatas.Sort ((p, q) => p.BasePerSec.CompareTo (q.BasePerSec));
			break;
		case 4:
			columnDatas.Sort ((p, q) => p.TotalEarned.CompareTo (q.TotalEarned));
			break;
		case 5:
			columnDatas.Sort ((p, q) => p.CurrentAmount.CompareTo (q.CurrentAmount));
			break;
		case 6:
			columnDatas.Sort ((p, q) => p.MaxAmount.CompareTo (q.MaxAmount));
			break;
		default:
			break;
		}
		if (sortType < 0) {
			columnDatas.Reverse ();
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

	private void setSortType (int i)
	{
		isNeedingUpdating = true;
		if (i == Mathf.Abs (sortType)) {
			sortType = sortType * -1;
		} else {
			sortType = i;
		}
	}
}
