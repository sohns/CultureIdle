using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ResourcePanel : MonoBehaviour, Panel
{

	//Things needed
	private GameObject resourceColumn;
	private ResourcesManager resourceManager;
	private Transform resourceData, scrollContent;
	private Button nameTitleButton, baseTitleButton, totalEarnedTitleButton, currentAmountTitleButton, maxAmountTitleButton;

	//Public and used for testing
	public bool isActiveTab;

	//Used for data
	private float offSetY;
	private int columnInfoHeight;
	private Font columnFont;
	private int sortType;

	public void infomationHasChanged ()
	{
		Update ();
	}

	private string columnInfoName = "columnInfo";

	void Start ()
	{
		nameTitleButton = setUpSortButton ("Name", 1);
		baseTitleButton = setUpSortButton ("Base", 2);
		totalEarnedTitleButton = setUpSortButton ("TotalEarned", 3);
		currentAmountTitleButton = setUpSortButton ("CurrentAmount", 4);
		maxAmountTitleButton = setUpSortButton ("MaxAmount", 5);
		resourceManager = (ResourcesManager)GameObject.Find ("Resources").GetComponent ("ResourcesManager");
		resourceColumn = (GameObject)Resources.Load ("ResourcePanelColumn");
		columnFont = Resources.GetBuiltinResource<Font> ("Arial.ttf");
		offSetY = -120;
		columnInfoHeight = 60;
		sortType = 0;
		resourceData = this.transform.Find ("ResourceData");
		scrollContent = resourceData.transform.Find ("ScrollRect").transform.Find ("ScrollContent");
	}

	private Button setUpSortButton (string name, int i)
	{
		Button button = this.transform.Find ("ResourceData").Find ("ColumnsTitle").Find (name).gameObject.GetComponent<Button> ();
		button.onClick.AddListener (() => setSortType (i));
		return button;
	}

	public void setInitialInfo (InfoPanel infoPanel)
	{
		this.transform.localPosition = new Vector3 (0, 0, 0);
		this.setInactive ();
	}

	public void setActive ()
	{
		this.gameObject.SetActive (true);
		isActiveTab = true;
	}

	public void setInactive ()
	{
		clearChildren ();
		this.gameObject.SetActive (false);
		isActiveTab = false;

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

	//TODO:Do I need this??
	private BiggerNumber getAmount (Dictionary<ResourceType,BiggerNumber> mapToGetFrom, ResourceType typeToGet)
	{
		if (mapToGetFrom.ContainsKey (typeToGet)) {
			return mapToGetFrom [typeToGet];
		}
		return 0;
	}

	//TODO:Do I need this??
	private BiggerNumber getAmount (Dictionary<ResourceType,ResourceData> mapToGetFrom, ResourceType typeToGet)
	{
		if (mapToGetFrom.ContainsKey (typeToGet)) {
			return mapToGetFrom [typeToGet].CurrentAmount;
		}
		return 0;
	}

	// Update is called once per frame
	void Update ()
	{
		if (isActiveTab) {
			clearChildren ();
			//TODO: resources need to be named/done differently
			List<ResourceColumnData> resources = new List<ResourceColumnData> ();
			Dictionary<ResourceType,ResourceData> resourcesCurrent = resourceManager.Resources;
			Dictionary<ResourceType,BiggerNumber> totalResources = resourceManager.TotalResources;
			Dictionary<ResourceType,BiggerNumber> currentRateResources = resourceManager.CurrentRateResources;
			foreach (ResourceType resourceType in resourcesCurrent.Keys) {
				ResourceData resourceData = resourcesCurrent [resourceType]; 
				string resourceName = resourceType.DisplayName;
				BiggerNumber basePerSec = getAmount (currentRateResources, resourceType);
				BiggerNumber totalEarned = getAmount (totalResources, resourceType);
				BiggerNumber currentAmount = resourceData.CurrentAmount;
				BiggerNumber maxAmount = resourceData.MaxAmount;
				ResourceColumnData resource = new ResourceColumnData (resourceName, basePerSec, totalEarned, currentAmount, maxAmount);
				resources.Add (resource);
			}

			int absolutSortType = Mathf.Abs (sortType);
			switch (absolutSortType) {
			case 0:
                //No sort
				break;
			case 1:
				resources.Sort ((p, q) => p.ResourceName.CompareTo (q.ResourceName));
				break;
			case 2:
				resources.Sort ((p, q) => p.BasePerSec.CompareTo (q.BasePerSec));
				break;
			case 3:
				resources.Sort ((p, q) => p.TotalEarned.CompareTo (q.TotalEarned));
				break;
			case 4:
				resources.Sort ((p, q) => p.CurrentAmount.CompareTo (q.CurrentAmount));
				break;
			case 5:
				resources.Sort ((p, q) => p.MaxAmount.CompareTo (q.MaxAmount));
				break;
			default:
				break;
			}
			if (sortType < 0) {
				resources.Reverse ();
			}
        
			int i = 0;
			foreach (ResourceColumnData resource in resources) {
            
				GameObject newObject = Instantiate (resourceColumn);
				//TODO:get height from object
				float posY = -(columnInfoHeight * i) + offSetY;
				setColumnInfoObject (newObject, posY, i, scrollContent);
				ResourceColumnScript subscript = (ResourceColumnScript)newObject.GetComponent ("ResourceColumnScript");
				subscript.SetBase ();
				subscript.NameText.text = resource.ResourceName;
				subscript.BaseText.text = resource.BasePerSec.ToDisplayString (3, false);
				subscript.BaseText.font = columnFont;
				subscript.TotalEarnedText.text = resource.TotalEarned.ToDisplayString (2, false);
				subscript.TotalEarnedText.font = columnFont;
				subscript.CurrentAmountText.text = resource.CurrentAmount.ToDisplayString (3, false);
				subscript.CurrentAmountText.font = columnFont;
				subscript.MaxAmountText.text = resource.MaxAmount.ToDisplayString (3, true);
				subscript.MaxAmountText.font = columnFont;
				i++;
			}   
		}
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
