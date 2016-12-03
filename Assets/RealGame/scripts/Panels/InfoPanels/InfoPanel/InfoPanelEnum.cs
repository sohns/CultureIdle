using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public sealed class InfoPanelEnum
{
	private readonly string name;
	private readonly int value;
	private readonly string subName;
	private readonly string title;
	//Misc Panel
	private static readonly int valueMiscPanel_One = 1000;
	private static readonly string nameMiscPanel_One = "Extra Info";


	//Panel Bottom Panel One:
	private static readonly int valueBottomPanelOne_One = 1;
	private static readonly string nameBottomPanelOne_One = "Statistics";

	private static readonly int valueBottomPanelOne_Two = 2;
	private static readonly string nameBottomPanelOne_Two = "Resources";

	private static readonly int valueBottomPanelOne_Three = 3;
	private static readonly string nameBottomPanelOne_Three = "Civilization Points";

	//Panel Bottom Panel Two:
	private static readonly int valueBottomPanelTwo_One = 11;
	private static readonly string nameBottomPanelTwo_One = "Buildings";

	private static readonly int valueBottomPanelTwo_Two = 12;
	private static readonly string nameBottomPanelTwo_Two = "Technologies";

	//Side Panel
	private static readonly int valueSidePanelOne_One = 100;
	private static readonly string nameSidePanelOne_One = "Challenges";

	public static readonly InfoPanelEnum KINGDOM_STATISTICS = new InfoPanelEnum (valueBottomPanelOne_One, nameBottomPanelOne_One);
	public static readonly InfoPanelEnum KINGDOM_RESOURCES = new InfoPanelEnum (valueBottomPanelOne_Two, nameBottomPanelOne_Two);
	public static readonly InfoPanelEnum KINGDOM_CIVILIZATION_POINTS = new InfoPanelEnum (valueBottomPanelOne_Three, nameBottomPanelOne_Three);

	public static readonly InfoPanelEnum PROJECT_BUILDINGS = new InfoPanelEnum (valueBottomPanelTwo_One, nameBottomPanelTwo_One);
	public static readonly InfoPanelEnum PROJECT_TECHNOLOGIES = new InfoPanelEnum (valueBottomPanelTwo_Two, nameBottomPanelTwo_Two);

	public static readonly InfoPanelEnum CHALLENGES = new InfoPanelEnum (valueSidePanelOne_One, nameSidePanelOne_One);
	public static readonly InfoPanelEnum EXTRA_INFO = new InfoPanelEnum (valueMiscPanel_One, nameMiscPanel_One);



	private InfoPanelEnum (int value, string name)
	{
		this.name = name;
		this.value = value;

	}

	public string Name {
		get {
			return this.name;
		}
	}

	public int Value {
		get {
			return this.value;
		}
	}

}
