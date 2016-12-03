using UnityEngine;
using System.Collections;

public interface Panel {
	void setActive ();
	void setInactive ();
	void setInitialInfo(InfoPanel infoPanel);
	void infomationHasChanged();
}
