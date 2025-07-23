using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	[Header("Audio Source")]
	[SerializeField] AudioSource musicSource;
	[SerializeField] AudioSource SFXSource;

	[Header("Audio Clip")]
	public AudioClip background, Attack, Die, Hit, Jump, 月牙天衝, takeDmg;

	public void PlaySFX(AudioClip clip) {
		if (SFXSource == null) {
			Debug.LogError("SFXSource is not assigned in AudioManager!");
			return;
		}
		if (clip == null) {
			Debug.LogWarning("Attempted to play a null AudioClip!");
			return;
		}
		SFXSource.PlayOneShot(clip);
	}

	// Start is called before the first frame update
	void Start()
	{
		musicSource.clip = background;
		musicSource.Play();
	}

	// Update is called once per frame
	void Update()
	{
		
	}

}
