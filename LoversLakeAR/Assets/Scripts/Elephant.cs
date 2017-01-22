using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elephant : MonoBehaviour {

    const float ELEPHANT_VOLUME = 1.0f;
    const float ELEPHANT_FADE_TIME = 2.0f;

	// Use this for initialization
	void Start () {
        gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		if (MainManager.Instance.Won)
        {
            StartCoroutine(fadeInSound());
        }
	}

    /** Function to fade the elephant sound in. */
    IEnumerator fadeInSound()
    {
        float delta = ELEPHANT_VOLUME / ELEPHANT_FADE_TIME;
        AudioSource aSource = GetComponent<AudioSource>();
        while (aSource.volume < ELEPHANT_VOLUME - delta * Time.deltaTime &&
            aSource.volume < ELEPHANT_VOLUME - VuforiaObjectTest.FADE_OUT_DELTA)
        {
            aSource.volume += delta * Time.deltaTime;
            yield return null;
        }
        aSource.volume = ELEPHANT_VOLUME;
    }
}
