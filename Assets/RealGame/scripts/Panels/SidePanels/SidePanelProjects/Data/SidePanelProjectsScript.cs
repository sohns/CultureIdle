using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class SidePanelProjectsScript : MonoBehaviour {

	private UnityEngine.UI.Text progressText, titleText, requiresText, emptyText;
	private Slider slider;
	private IncrementalManager incrementer;
	private SidePanelProjectsData sidePanelProjectsData;
	private bool isUpdating;
	private bool isEmptyTextActive;

	void Start()
	{
		
	}

	public void SetBase(){
		progressText = this.transform.Find ("Progress").Find ("Slider").Find ("ProgressText").GetComponent<Text> ();
		titleText = this.transform.Find("Title").GetComponent<Text> ();
		requiresText = this.transform.Find("Requires").GetComponent<Text> ();
		slider = this.transform.Find ("Progress").Find ("Slider").GetComponent<Slider> ();
		incrementer = (IncrementalManager)GameObject.Find("Incrementer").GetComponent ("IncrementalManager");
		emptyText = this.transform.Find ("EmptyStatus").Find("Text").gameObject.GetComponent<Text>();
	}

	public SidePanelProjectsData SidePanelProjectsData {
		set {
			this.sidePanelProjectsData=value;
		}
	}

	void Update()
	{
		if (sidePanelProjectsData != null) {
			titleText.text = sidePanelProjectsData.TitleName;
			requiresText.text = sidePanelProjectsData.RequiresText;
			if (sidePanelProjectsData.CurrentPercent >= 100) {
				progressText.text = "Done";
				slider.value = 100;
				isUpdating = false;
				if (sidePanelProjectsData.HasEmptyState) {
					SetEmptyText (true,sidePanelProjectsData.EmptyStateText);	
				}
			} else {
				DisableEmptyText ();
				progressText.text = sidePanelProjectsData.ProgressText;
				slider.value = sidePanelProjectsData.CurrentPercent;
			}
		} else {
			SetEmptyText (true, "No Project In This Slot");
		}
	}

	private void DisableEmptyText(){
		SetEmptyText (false, "");
	}

	private void SetEmptyText(bool setIsEmptyTextActive,string emptyTextValue)
	{
		isEmptyTextActive = setIsEmptyTextActive;
		if (isEmptyTextActive)
		{
			float height = this.transform.GetComponent<RectTransform>().rect.height;
			float width = this.transform.GetComponent<RectTransform>().rect.width;
			emptyText.transform.parent.GetComponent<RectTransform>().sizeDelta = new Vector2(width + 5, height + 5);
			emptyText.text = emptyTextValue;
		}
		else
		{
			emptyText.transform.parent.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
		}
	}
}
