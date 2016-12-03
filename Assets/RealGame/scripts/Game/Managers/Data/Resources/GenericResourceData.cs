using UnityEngine;
using System.Collections;

public class GenericResourceData
{

	//TODO refactor more in here
	private BiggerNumber currentAmount;

	public GenericResourceData (BiggerNumber currentAmount)
	{
		this.currentAmount = currentAmount;
	}

	public virtual BiggerNumber CurrentAmount {
		get {
			return this.currentAmount;
		}
		set {
			this.currentAmount = value;
		}
	}
}
