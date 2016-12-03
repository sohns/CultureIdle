using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ExtraInfoPanel : MonoBehaviour, Panel {
	private Text displayText;
	private ExtraInfoData extraInfoData;
	public float xi, yi;

	public ExtraInfoData ExtraInfoData {
		set {
			this.extraInfoData=value;
			setExtraInfoDisplay();
		}
	}

	public void infomationHasChanged(){
	}

	void Start(){
	}

	void Update(){
		setExtraInfoDisplay();
	}

	void setExtraInfoDisplay(){
		if (extraInfoData == null) {
			return;
		}
		displayText.text = extraInfoData.Text;
		RectTransform rt = this.gameObject.GetComponent<RectTransform>();
		float widthAdjust=rt.rect.width/4;
		float heightAdjust=-rt.rect.height/4;
		this.transform.position = new Vector3 (extraInfoData.X+xi+(widthAdjust), extraInfoData.Y+yi+(heightAdjust), 0);
		if (extraInfoData.IsVisable) {
			setActive ();
		} else {
			setInactive ();
		}
	}

	public void setInitialInfo(InfoPanel infoPanel){
		displayText = this.transform.Find ("Text").gameObject.GetComponent<Text> ();
		this.transform.localPosition = new Vector3 (0, 0, 0);
		this.setInactive ();
		yi = -5;
	}

	public void setActive ()
	{
		this.gameObject.SetActive (true);
	}

	public void setInactive ()
	{
		clearText ();
		this.gameObject.SetActive (false);
	}

	private void clearText(){
		displayText.text = "";
	}

}
