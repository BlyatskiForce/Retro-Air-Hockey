using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
//using MoreMountains.Tools;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class LoadingSceneManager : MonoBehaviourPunCallbacks
{
	[Header("Binding")]
	public static string LoadingScreenSceneName = "LoadingScreen";

	[Header("GameObjects")]
	public Text LoadingText;
	public CanvasGroup LoadingProgressBar;
	public CanvasGroup LoadingAnimation;
	public CanvasGroup LoadingCompleteAnimation;

	[Header("Time")]
	public float StartFadeDuration = 0.2f;
	public float ProgressBarSpeed = 2f;
	public float ExitFadeDuration = 0.2f;
	public float LoadCompleteDelay = 0.5f;

	protected AsyncOperation _asyncOperation;
	protected static string _sceneToLoad = "";
	protected float _fadeDuration = 0.5f;
	protected float _fillTarget = 0f;
	protected string _loadingTextValue;

	/// <param name="sceneToLoad">Level name.</param>
	public static void PhotonLoadScene(string sceneToLoad)
	{
		_sceneToLoad = sceneToLoad;
		Application.backgroundLoadingPriority = ThreadPriority.High;
		if (LoadingScreenSceneName != null && PhotonNetwork.IsMasterClient)
		{
			PhotonNetwork.LoadLevel(LoadingScreenSceneName);
			
		}
	}

	protected virtual void Start()
	{
		_loadingTextValue = LoadingText.text;
		if (_sceneToLoad != "")
		{

			StartCoroutine(LoadAsynchronously(2f));
		}
	}


	protected virtual void Update()
	{
		LoadingProgressBar.GetComponent<Image>().fillAmount = MMMaths.Approach(LoadingProgressBar.GetComponent<Image>().fillAmount, _fillTarget, Time.deltaTime * ProgressBarSpeed);
	}


	protected virtual IEnumerator LoadAsynchronously(float delay = 0.0f)
	{

		if (delay != 0)
			yield return new WaitForSeconds(delay);




		LoadingSetup();

		PhotonNetwork.LoadLevel(_sceneToLoad);

		while (PhotonNetwork.LevelLoadingProgress < 0.9)
		{

			_asyncOperation.allowSceneActivation = false;
		}

		//big mic mac, ne marche pas , pas grave faudra la mettre pour alentir le tout

		// we start loading the scene
		//_asyncOperation = PhotonNetwork.LevelLoadingProgress(_sceneToLoad,LoadSceneMode.Single );


		// while the scene loads, we assign its progress to a target that we'll use to fill the progress bar smoothly
		while (_asyncOperation.progress < 0.9f)
		{
			_fillTarget = _asyncOperation.progress;
			yield return null;
		}
		// when the load is close to the end (it'll never reach it), we set it to 100%
		_fillTarget = 1f;

		// we wait for the bar to be visually filled to continue
		while (LoadingProgressBar.GetComponent<Image>().fillAmount != _fillTarget)
		{
			yield return null;
		}

		// the load is now complete, we replace the bar with the complete animation
		LoadingComplete();
		yield return new WaitForSeconds(LoadCompleteDelay);

		// we fade to black
		GUIManager.Instance.FaderOn(true, ExitFadeDuration);
		yield return new WaitForSeconds(ExitFadeDuration);

		// we switch to the new scene
		_asyncOperation.allowSceneActivation = true;
	}

	protected virtual void LoadingSetup()
	{
		GUIManager.Instance.Fader.gameObject.SetActive(true);
		GUIManager.Instance.Fader.GetComponent<Image>().color = new Color(0, 0, 0, 1f);
		GUIManager.Instance.FaderOn(false, ExitFadeDuration);

		LoadingCompleteAnimation.alpha = 0;
		LoadingProgressBar.GetComponent<Image>().fillAmount = 0f;
		LoadingText.text = _loadingTextValue;

	}


	protected virtual void LoadingComplete()
	{
		LoadingCompleteAnimation.gameObject.SetActive(true);
		StartCoroutine(MMFade.FadeCanvasGroup(LoadingProgressBar, 0.1f, 0f));
		StartCoroutine(MMFade.FadeCanvasGroup(LoadingAnimation, 0.1f, 0f));
		StartCoroutine(MMFade.FadeCanvasGroup(LoadingCompleteAnimation, 0.1f, 1f));

	}
}

