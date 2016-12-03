using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;


public class SummonExtraInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
	private InfoPanel infoPanel;
	private string extraInfoText;

	public void setInitialInfo (InfoPanel infoPanel, string extraInfoText)
	{
		this.infoPanel = infoPanel;
		this.extraInfoText = extraInfoText;
	}
	

	public void OnPointerEnter(PointerEventData eventData)
	{
		RectTransform rt = this.gameObject.GetComponent<RectTransform>();
		float widthAdjust=-rt.rect.width/4;
		float heightAdjust=-rt.rect.height/4;
		ExtraInfoData extraInfoData = new ExtraInfoData (this.transform.position.x+widthAdjust, this.transform.position.y+heightAdjust, extraInfoText, true);
		infoPanel.setExtraInfo (extraInfoData);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		infoPanel.hideExtraInfo ();
	}
		
}
