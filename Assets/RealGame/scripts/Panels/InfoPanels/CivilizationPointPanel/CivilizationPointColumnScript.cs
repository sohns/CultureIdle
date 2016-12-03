using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class CivilizationPointColumnScript : MonoBehaviour
{

	private Text nameText, levelText, baseText, totalEarnedText, currentAmountText, maxAmountText;
	private InfoPanel infoPanel;
	private Button infoButton;
	private CivilizationPointType type;

	public void SetBase (CivilizationPointType type, InfoPanel infoPanel)
	{
		this.infoPanel = infoPanel;
		nameText = this.transform.Find ("Name").Find ("Text").gameObject.GetComponent<Text> ();
		infoButton = this.transform.Find ("Info").gameObject.GetComponent<Button> ();
		levelText = this.transform.Find ("Level").Find ("Text").gameObject.GetComponent<Text> ();
		baseText = this.transform.Find ("Base").Find ("Text").gameObject.GetComponent<Text> ();
		totalEarnedText = this.transform.Find ("TotalEarned").Find ("Text").gameObject.GetComponent<Text> ();
		currentAmountText = this.transform.Find ("CurrentAmount").Find ("Text").gameObject.GetComponent<Text> ();
		maxAmountText = this.transform.Find ("MaxAmount").Find ("Text").gameObject.GetComponent<Text> ();
		SetShallowData (type);
	}

	public void SetShallowData (CivilizationPointType type)
	{
		this.type = type;
		//TODO extra Info
		SummonExtraInfo summonExtraInfo = ((SummonExtraInfo)infoButton.gameObject.GetComponent<SummonExtraInfo> ());
		summonExtraInfo.setInitialInfo (infoPanel, type.ExtraInfo);
	}

	public CivilizationPointType Type {
		get {
			return this.type;
		}
	}

	public Text NameText {
		get {
			return this.nameText;
		}
	}

	public Text LevelText {
		get {
			return this.levelText;
		}
	}

	public Text BaseText {
		get {
			return this.baseText;
		}
	}

	public Text TotalEarnedText {
		get {
			return this.totalEarnedText;
		}
	}

	public Text CurrentAmountText {
		get {
			return this.currentAmountText;
		}
	}

	public Text MaxAmountText {
		get {
			return this.maxAmountText;
		}
	}
}
