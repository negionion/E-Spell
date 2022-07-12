using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceLoad : MonoBehaviour {
	private static bool bundleLock = false;
	public static AudioClip load(string vocabulary)
	{
		if(bundleLock)
			return null;
		bundleLock = true;
		AssetBundle bundle = AssetBundle.LoadFromFile(Application.persistentDataPath + "/vocabulary_voice.assetbundle");
		//AssetBundle bundle = AssetBundle.LoadFromFile("D:\\Unity_project\\LAB_spell\\spell\\AssetBundles\\vocabulary_voice.assetbundle");
		if(!bundle.Contains(vocabulary.ToLower() + ".mp3"))
		{
			bundle.Unload(true);
			bundleLock = false;			
			return null;
		}
		AudioClip voice = (AudioClip)bundle.LoadAsset(vocabulary.ToLower() + ".mp3");
		bundle.Unload(false);
		bundleLock = false;
		return voice;
	}
}
