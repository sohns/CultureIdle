using UnityEngine;
using System.Collections;

public class SidePanelProjectsData {

	private string titleName, requiresText, progressText;
	private float currentPercent;
	private bool isPaused=false;
	private bool hasEmptyState=false;
	private string emptyStateText="Should be some text here-placeholder";

	public string TitleName {
		get {
			return this.titleName;
		}
	}

	public string RequiresText {
		get {
			return this.requiresText;
		}
	}

	public string ProgressText {
		get {
			return this.progressText;
		}
	}

	public float CurrentPercent {
		get {
			return this.currentPercent;
		}
	}

	public bool IsPaused {
		get {
			return this.isPaused;
		}
	}

	public bool HasEmptyState {
		get {
			return this.hasEmptyState;
		}
	}

	public string EmptyStateText {
		get {
			return this.emptyStateText;
		}
	}
	public SidePanelProjectsData (string titleName, string requiresText, string progressText, float currentPercent)
	{
		this.titleName = titleName;
		this.requiresText = requiresText;
		this.progressText = progressText;
		this.currentPercent = currentPercent;
	}
	public SidePanelProjectsData (string titleName, string requiresText, string progressText, float currentPercent, bool isPaused, bool hasEmptyState, string emptyStateText)
	{
		this.titleName = titleName;
		this.requiresText = requiresText;
		this.progressText = progressText;
		this.currentPercent = currentPercent;
		this.isPaused = isPaused;
		this.hasEmptyState = hasEmptyState;
		this.emptyStateText = emptyStateText;
	}

	public SidePanelProjectsData (string errorText)
	{
		this.titleName = "";
		this.requiresText = "";
		this.progressText = "";
		this.currentPercent = 0;
		this.isPaused = false;
		this.hasEmptyState = true;
		this.emptyStateText = errorText;
	}
	
	
	
	
}