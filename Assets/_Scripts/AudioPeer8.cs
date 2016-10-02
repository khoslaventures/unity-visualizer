using UnityEngine;
using System.Collections;

[RequireComponent (typeof (AudioSource))]
public class AudioPeer8 : MonoBehaviour {
	AudioSource _audioSource;
	public static float[] _samples = new float[512];
	public static float[] _freqBand = new float[8];

	// Speed at which the bars lerp to the next sampled value
	const float SPEED = 2.0f;


	// Use this for initialization
	void Start () {
		_audioSource = GetComponent<AudioSource> ();
	}

	// Update is called once per frame
	void Update () {
		getSpectrumAudioSource ();
		MakeFrequencyBands ();
	}

	// get all the sample data into our array of floats
	void getSpectrumAudioSource() {
		_audioSource.GetSpectrumData (_samples, 0, FFTWindow.BlackmanHarris);
	}

	void MakeFrequencyBands() {
		/* 8 band visualizer
		 * 22050 / 512 = 43 hz per sample
		 * 20 - 60 hz
		 * 60 - 250 hz
		 * 250 - 500 hz
		 * 500 - 2000 hz
		 * 2000 - 4000 hz
		 * 4000 - 6000 hz
		 * 6000 - 20000 hz
		 * 
		 * 0: 2 = 86 hz
		 * 1: 4 = 172 hz: 87-258hz
		 * 2: 8 = 344 hz: 259-602hz
		 * 3: 16 = 688 hz: 603-1290
		 * 4: 32 = 1376 hz: 1291-2666
		 * 5: 64 = 2752 hz: 2667-5418
		 * 6: 128 = 5504 hz: 5419-10922
		 * 7: 256 = 11008 hz: 10923-21930
		 * 510, so add 2 samples to last one
		 */ 
		int count = 0;
		for (int i = 0; i < 8; i++) {
			float average = 0;
			int sampleCount = (int)Mathf.Pow (2, i) * 2;

			if (i == 7) {
				sampleCount += 2;
			}
			for (int j = 0; j < sampleCount; j++) {
				average += _samples [count] * (count + 1);
				count++;
			}

			average /= count;

			_freqBand[i] = average * 10;
		}
	}
}
