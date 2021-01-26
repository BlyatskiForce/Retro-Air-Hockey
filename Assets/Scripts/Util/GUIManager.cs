using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;


public class GUIManager : MonoBehaviourPunCallbacks
{


	public static GUIManager Instance = null;

	public GameObject PauseScreen;

	public GameObject GameOverScreen;

	public GameObject HeartsContainer;

	public Text PointsText;

	public Text LevelText;

	public Text CountdownText;

	public Image Fader;

	public void Awake()
	{
		Instance = this;
	}

	/// <param name="state">If set to <c>true</c> state.</param>
	public virtual void SetCountdownActive(bool state)
	{
		if (CountdownText == null) { return; }
		CountdownText.enabled = state;
	}


	/// <param name="value">the new countdown text.</param>
	public virtual void SetCountdownText(string newText)
	{
		if (CountdownText == null) { return; }
		CountdownText.text = newText;
	}

	public virtual void SetLevelName(string name)
	{
		if (LevelText == null)
			return;

		LevelText.text = name;
	}


	/// <param name="state">If set to <c>true</c> fades the fader in, otherwise out if <c>false</c>.</param>
	public virtual void FaderOn(bool state, float duration)
	{
		if (Fader == null)
		{
			return;
		}
		Fader.gameObject.SetActive(true);
		if (state)
			StartCoroutine(MMFade.FadeImage(Fader, duration, new Color(0, 0, 0, 1f)));
		else
			StartCoroutine(MMFade.FadeImage(Fader, duration, new Color(0, 0, 0, 0f)));
	}


	/// <param name="newColor">The color to fade to.</param>
	/// <param name="duration">Duration.</param>
	public virtual void FaderTo(Color newColor, float duration)
	{
		if (Fader == null)
		{
			return;
		}
		Fader.gameObject.SetActive(true);
		StartCoroutine(MMFade.FadeImage(Fader, duration, newColor));
	}

}
