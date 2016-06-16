using UnityEngine;
using System.Collections;

public class AssetLoaderDemo : MonoBehaviour
{
	IEnumerator Start()
	{
		yield return new WaitUntil(() => AssetLoader.IsReady);
		AssetLoader.LoadAssetAsync("cube.unity3d", "cube", (assetObject) =>
		{
			GameObject.Instantiate(assetObject);
		});
	}
}