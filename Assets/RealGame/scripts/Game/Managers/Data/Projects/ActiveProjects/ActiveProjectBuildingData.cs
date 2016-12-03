using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActiveProjectBuildingData : ActiveProjectBaseData
{
	
	public ActiveProjectBuildingData (object activeProjectType, BiggerNumber timeLeft) : base (activeProjectType, modifyCosts (timeLeft), timeLeft, false)
	{
		
	}

	private static List<ActiveProjectCost> modifyCosts (BiggerNumber timeLeft)
	{
		List<ActiveProjectCost> costs = new List<ActiveProjectCost> ();
		costs.Add (new ActiveProjectCost (AdvancedResourceType.TIME, timeLeft, 0, 1));
		return costs;
	}

	
}
