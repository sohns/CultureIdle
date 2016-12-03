using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ChallengePanel : MonoBehaviour, Panel {

	// Use this for initialization
	public GameObject challengeColumn;
	//public ChallengeManager challengeManager;
	public Transform challengeData, scrollContent;
	public float offSetY;
	private int columnInfoHeight;
	private Font columnFont;
	private int sortType;
	private bool isActiveTab;

	private string columnInfoName = "columnInfo";

	void Start ()
	{
//		challengeManager = (ChallengeManager)GameObject.FindWithTag("Challenges").GetComponent ("ChallengeManager");
		columnFont = Resources.GetBuiltinResource<Font> ("Arial.ttf");
		offSetY = -120;
		columnInfoHeight = 60;
		sortType = 0;
		challengeData = this.transform.Find ("ChallengesData");
		scrollContent = challengeData.transform.Find ("ScrollRect").transform.Find ("ScrollContent");
	}
	public void setInitialInfo(InfoPanel infoPanel){
		this.transform.localPosition = new Vector3 (0, 0, 0);
		this.setInactive ();
	}
		
	public void setActive ()
	{
		this.gameObject.SetActive (true);
		isActiveTab = true;


	}

	public void infomationHasChanged(){

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

	// Update is called once per frame
	void Update ()
	{
		if (isActiveTab) {
//			clearChildren ();
//			//List<ChallengeData> activeChallengeDatas = challengeManager.ActiveChallenges;
//			int i = 0;
//			//foreach (ChallengeData challengeData in activeChallengeDatas) {
//
//		//		GameObject newObject = Instantiate (challengeColumn);
//		//		//TODO:get height from object
//				float posY = -(columnInfoHeight * i) + offSetY;
//				setColumnInfoObject (newObject, posY, i, scrollContent);
//				ResourceColumnScript subscript = (ResourceColumnScript)newObject.GetComponent ("ResourceColumnScript");
//				subscript.SetBase ();
//				subscript.NameText.text = challengeData.TitleName;
//				subscript.BaseText.text = challengeData.IncrementValue.ToDisplayString (3, false);
//				subscript.BaseText.font = columnFont;
//				subscript.TotalEarnedText.text = "";
//				subscript.TotalEarnedText.font = columnFont;
//				subscript.CurrentAmountText.text = challengeData.MaxValue.ToDisplayString (3, false);
//				subscript.CurrentAmountText.font = columnFont;
//				subscript.MaxAmountText.text = challengeData.CurrentValue.ToDisplayString (3, true);
//				subscript.MaxAmountText.font = columnFont;
//				i++;
			//}           
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
