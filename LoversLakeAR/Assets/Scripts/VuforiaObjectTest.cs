using UnityEngine;
using System.Collections;
using Vuforia;

public class VuforiaObjectTest : MonoBehaviour, ITrackableEventHandler {

    /** Types of instruments. */
    public enum InstrumentType { GOOD, WEIRD, METAL, BOAT };

    [SerializeField]
    private InstrumentType _type;
    public InstrumentType Type
    {
        get
        {
            return _type;
        }
    }

    /** Whether this object is currently being tracked. To be used in the manager class */
    /*private bool _found = false;
    public bool Found
    {
        get
        {
            return _found;
        }
    } */

    /** The fade out coroutine ends when volume falls below this threshold */
    public const float FADE_OUT_DELTA = 0.03f;

    /** Standard fade in and fade out time for clips when tags are found and lost */
    public const float STANDARD_FADE = 0.2f;

    /** The volume at which the instrument should be played.
     * Change from the editor. */
    [SerializeField]
    private float _instrumentVolume;

    /** Dictionary holding the fade in and fade out coroutine references for AudioSources */
    private System.Collections.Generic.Dictionary<AudioSource, IEnumerator> _fades =
        new System.Collections.Generic.Dictionary<AudioSource, IEnumerator>();

    TrackableBehaviour mTrackableBehaviour;

    // Use this for initialization
    void Start () {
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
        {
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnTrackableStateChanged(TrackableBehaviour.Status previous, TrackableBehaviour.Status next){
        if(next == TrackableBehaviour.Status.DETECTED || next == TrackableBehaviour.Status.TRACKED)
        {
            OnTrackingFound();
        }
        else
        {
            OnTrackingLost();
        }
    }

    public void OnTrackingFound()
    {
        MainManager.Instance.increaseInstrument(_type);

        Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);
        Collider[] colliderComponents = GetComponentsInChildren<Collider>(true);
        AudioSource[] audioComponents = GetComponentsInChildren<AudioSource>(true);

        // Enable rendering:
        foreach (Renderer component in rendererComponents)
        {
            component.enabled = true;
        }

        // Enable colliders:
        foreach (Collider component in colliderComponents)
        {
            component.enabled = true;
        }
        foreach (AudioSource component in audioComponents)
        {
            if (_fades[component] != null) StopCoroutine(_fades[component]);
            _fades[component] = fadeInSound(component, STANDARD_FADE);
            StartCoroutine(_fades[component]);
            //component.volume = 1;
        }
        Debug.Log("Found");
    }

    public void OnTrackingLost()
    {
        MainManager.Instance.decreaseInstrument(_type);

        Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);
        Collider[] colliderComponents = GetComponentsInChildren<Collider>(true);
        AudioSource[] audioComponents = GetComponentsInChildren<AudioSource>(true);

        // Enable rendering:
        foreach (Renderer component in rendererComponents)
        {
            component.enabled = false;
        }

        // Enable colliders:
        foreach (Collider component in colliderComponents)
        {
            component.enabled = false;
        }
        foreach (AudioSource component in audioComponents)
        {
            if (_fades[component] != null) StopCoroutine(_fades[component]);
            _fades[component] = fadeOutSound(component, STANDARD_FADE);
            StartCoroutine(_fades[component]);
            //component.volume = 0;
        }
        Debug.Log("Lost");
    }

    /** Function to fade sounds out */
    IEnumerator fadeOutSound(AudioSource aSource, float fadeTime)
    {
        float delta = aSource.volume / fadeTime;
        while (aSource.volume > delta * Time.deltaTime &&
            aSource.volume > FADE_OUT_DELTA)
        {
            aSource.volume -= delta * Time.deltaTime;
            yield return null;
        }
        aSource.volume = 0;

        // Remove coroutine from fades dictionary at the end
        _fades[aSource] = null;
    }

    /** Function to fade sounds in. Do not enforce starting volume to be 0,
     * in case the sound was fading out when this coroutine was called */
    IEnumerator fadeInSound(AudioSource aSource, float fadeTime)
    {
        float delta = _instrumentVolume / fadeTime;
        while (aSource.volume < _instrumentVolume - delta * Time.deltaTime &&
            aSource.volume < _instrumentVolume - FADE_OUT_DELTA)
        {
            aSource.volume += delta * Time.deltaTime;
            yield return null;
        }
        aSource.volume = _instrumentVolume;

        // Remove coroutine from fades dictionary at the end
        _fades[aSource] = null;
    }
}
