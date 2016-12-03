using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BottomPanelFour : MonoBehaviour
{

	private InfoPanel infoPanel;
	private StartGame startGame;
	private Button buttonOne, buttonTwo, buttonThree, title, buttonFour, buttonFive;

	// Use this for initialization
	void Start ()
	{
		infoPanel = (InfoPanel)GameObject.FindWithTag ("InfoPanel").GetComponent ("InfoPanel");
		startGame = (StartGame)GameObject.FindWithTag ("StartGame").GetComponent ("StartGame");
		buttonOne = this.transform.Find ("Button1").GetComponent<Button> ();
		buttonOne.onClick.AddListener (() => onButtonOneClick ());
		buttonTwo = this.transform.Find ("Button2").GetComponent<Button> ();
		buttonTwo.onClick.AddListener (() => onButtonTwoClick ());
		buttonThree = this.transform.Find ("Button3").GetComponent<Button> ();
		buttonThree.onClick.AddListener (() => onButtonThreeClick ());
		buttonFour = this.transform.Find ("Button4").GetComponent<Button> ();
		buttonFour.onClick.AddListener (() => onButtonFourClick ());
		buttonFive = this.transform.Find ("Button5").GetComponent<Button> ();
		buttonFive.onClick.AddListener (() => onButtonFiveClick ());
		title = this.transform.Find ("Title").GetComponent<Button> ();
		title.onClick.AddListener (() => onTitleClick ());
	}

	// Update is called once per frame
	void Update ()
	{

	}

	public void onButtonOneClick ()
	{
		//infoPanel.setPanel (InfoPanelEnum.KINGDOM_STATISTICS);
	}

	public void onButtonTwoClick ()
	{
		startGame.SetSaveGame ();
	}

	public void onButtonThreeClick ()
	{
		//infoPanel.setPanel (InfoPanelEnum.KINGDOM_CIVILIZATION_POINTS);
	}

	public void onButtonFourClick ()
	{
		startGame.SetLoadGame ();
	}

	public void onButtonFiveClick ()
	{
		startGame.SetResetGame ();
	}

	public void onTitleClick ()
	{
	}
}
