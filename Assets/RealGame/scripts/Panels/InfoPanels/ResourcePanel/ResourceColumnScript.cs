using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class ResourceColumnScript : MonoBehaviour {
	private Text nameText, baseText, totalEarnedText, currentAmountText, maxAmountText;

	public void SetBase(){
		nameText = this.transform.Find ("Name").Find ("Text").gameObject.GetComponent<Text> ();
		baseText = this.transform.Find ("Base").Find ("Text").gameObject.GetComponent<Text> ();
		totalEarnedText = this.transform.Find ("TotalEarned").Find ("Text").gameObject.GetComponent<Text> ();
		currentAmountText = this.transform.Find ("CurrentAmount").Find ("Text").gameObject.GetComponent<Text> ();
		maxAmountText = this.transform.Find ("MaxAmount").Find ("Text").gameObject.GetComponent<Text> ();
	}
	public Text NameText {
		get {
			return this.nameText;
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
