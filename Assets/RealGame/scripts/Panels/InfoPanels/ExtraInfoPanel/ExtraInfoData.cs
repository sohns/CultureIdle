using UnityEngine;
using System.Collections;

public class ExtraInfoData {
	private float x, y;
	private string text;
	private bool isVisable;
	public ExtraInfoData (float x, float y, string text, bool isVisable)
	{
		this.x = x;
		this.y = y;
		this.text = text;
		this.isVisable = isVisable;
	}
	public float X {
		get {
			return this.x;
		}
	}

	public float Y {
		get {
			return this.y;
		}
	}

	public string Text {
		get {
			return this.text;
		}
	}

	public bool IsVisable {
		get {
			return this.isVisable;
		}
	}
}
