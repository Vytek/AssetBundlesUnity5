﻿using UnityEngine;
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
		isReady = false;
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
		ms_Instance.StartCoroutine(LoadAutoUnload(assetBundleName, assetName, callback));
	}

	public static void LoadAssetAsync(string assetBundleName, Action<UnityEngine.Object> callback)
	{
		ms_Instance.StartCoroutine(LoadAutoUnload(assetBundleName, string.Empty, callback));
	}

	protected static IEnumerator LoadAutoUnload(string assetBundleName, string assetName, Action<UnityEngine.Object> callback)
	{
		yield return ms_Instance.Load(assetBundleName, assetName, callback);

		yield return new WaitForSeconds(BUNDLE_HOLD_SECONDS);

		AssetBundleManager.UnloadAssetBundle(assetBundleName);
	}

	public static void LoadAssetBundleLoadAllAssetsAsync(string assetBundleName, Action<AssetBundle> callback)
	{
		ms_Instance.StartCoroutine(ms_Instance.LoadAssetBundleLoadAllAssets(assetBundleName, callback));
	}

	public static void LoadLevelAsync(string assetBundleName, string levelName, bool isAdditive, Action completeCallback)
	{
		ms_Instance.StartCoroutine(LoadLevelProxy(assetBundleName, levelName, isAdditive, completeCallback));
	}

	protected static IEnumerator LoadLevelProxy(string assetBundleName, string levelName, bool isAdditive, Action completeCallback)
	{
		yield return ms_Instance.LoadLevel(assetBundleName, levelName, isAdditive);

		if (completeCallback != null)
		{
			completeCallback();
		}
	}
}