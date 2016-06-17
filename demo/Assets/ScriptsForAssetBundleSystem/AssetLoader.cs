using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;

public class AssetLoader : BaseLoader
{
	private const float BUNDLE_HOLD_SECONDS = 5.0f;
	private static AssetLoader ms_Instance = null;

	private static bool isReady = false;

	public static bool IsReady
	{
		get
		{
			return isReady;
		}
	}

	void Awake()
	{
		ms_Instance = this;
	}

	void OnDestroy()
	{
		ms_Instance = null;
		isReady = false;
	}

	IEnumerator Start()
	{
		yield return StartCoroutine( Initialize() );
		isReady = true;
	}

	public static void LoadAssetAsync( string assetBundleName, string assetName, Action<UnityEngine.Object> callback )
	{
		AssetLoader.ms_Instance.StartCoroutine(LoadAutoUnload(assetBundleName, assetName, callback));
	}

	protected static IEnumerator LoadAutoUnload(string assetBundleName, string assetName, Action<UnityEngine.Object> callback)
	{
		yield return AssetLoader.ms_Instance.Load(assetBundleName, assetName, callback);

		yield return new WaitForSeconds(BUNDLE_HOLD_SECONDS);

		AssetBundleManager.UnloadAssetBundle(assetBundleName);
	}

	public static void LoadLevelAsync(string assetBundleName, string levelName, bool isAdditive, Action completeCallback)
	{
		AssetLoader.ms_Instance.StartCoroutine(LoadLevelProxy(assetBundleName, levelName, isAdditive, completeCallback));
	}

	protected static IEnumerator LoadLevelProxy(string assetBundleName, string levelName, bool isAdditive, Action completeCallback)
	{
		yield return AssetLoader.ms_Instance.LoadLevel(assetBundleName, levelName, isAdditive);

		if (completeCallback != null)
		{
			completeCallback();
		}
	}

	public static void UnloadScene(string sceneName)
	{
		SceneManager.UnloadScene(sceneName);
	}
}