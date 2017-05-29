using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class PaletteSwap : MonoBehaviour {

	const string PALETTE_INDEX_KEY = "paletteIndex";
	
	public static int PaletteIndex {
		get {
			return PlayerPrefs.GetInt(PALETTE_INDEX_KEY, 0);
		}
		set {
			PlayerPrefs.SetInt(PALETTE_INDEX_KEY, value);
			PlayerPrefs.Save();
		}
	}
	
	public bool EnableSwap = false;
	public bool EnableUp = false;
	public bool EnableDown = true;

	Resolutioner _resolutioner;
	Texture2D[] _palettes;

	#if UNITY_EDITOR
	bool _didResetPalette = false;
	#endif

	// Use this for initialization
	void Start () {

#if UNITY_EDITOR
		if (!_didResetPalette) {
			ResetPalette();
			_didResetPalette = true;
		}
#endif
		
		AudioHandler.Load("palette_change");

		_resolutioner = GetComponent<Resolutioner> ();

		_palettes = Resources.LoadAll<Texture2D>("Ramps");

		UpdatePalette ();

		Messenger.AddListener<bool> ("EnablePalleteSwap", SetEnableSwap);
	
	}
	
	void SetEnableSwap(bool enable) {
		this.EnableSwap = enabled;
	}

	// Update is called once per frame
	void Update () {
		if (!EnableSwap)
			return;
		
		if (InputExtensions.Pressed.Up && EnableUp)
			ChangePaletteUp();
		
		if (InputExtensions.Pressed.Down && EnableDown)
			ChangePaletteDown();
	
	}
	
	public void ChangePaletteUp() {
		PaletteSwap.PaletteIndex++;
		if (PaletteSwap.PaletteIndex >= _palettes.Length)
			PaletteSwap.PaletteIndex = 0;
		AudioHandler.Play("palette_change");
		UpdatePalette();
	}
	
	public void ChangePaletteDown() {
		PaletteSwap.PaletteIndex--;
		if (PaletteSwap.PaletteIndex < 0)
			PaletteSwap.PaletteIndex = _palettes.Length - 1;
		AudioHandler.Play("palette_change");
		UpdatePalette();
	}

	private void UpdatePalette() {
		if (_resolutioner == null)
			return;
		_resolutioner.postprocessDither.SetPalette(_palettes [PaletteSwap.PaletteIndex]);
	}
	
#if UNITY_EDITOR

	void OnApplicationQuit() {
		ResetPalette ();
	}

	void ResetPalette() {
		PaletteSwap.PaletteIndex = 0;
		UpdatePalette ();
	}

#endif

}
