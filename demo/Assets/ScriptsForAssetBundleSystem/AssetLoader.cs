using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class AssetLoader : BaseLoader
{
	private static AssetLoader msInstance = null;

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
        msInstance = this;
    }

    void OnDestroy()
    {
        msInstance = null;
        isReady = false;
    }

	IEnumerator Start()
	{
		yield return StartCoroutine( Initialize() );
		isReady = true;
	}

	public static void LoadRequest( string assetBundleName, string assetName, Action<UnityEngine.Object> callback )
	{
        AssetLoader.msInstance.StartCoroutine(AssetLoader.msInstance.Load(assetBundleName, assetName, (assetObject) =>
		{
			if (callback != null)
			{
				callback( assetObject );
			}

			AssetBundleManager.UnloadAssetBundle( assetBundleName );
		} ) );
	}
}