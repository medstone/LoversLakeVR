using UnityEngine;
using System.Collections;
using Vuforia;

public class VuforiaObjectTest : MonoBehaviour, ITrackableEventHandler {

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
            component.volume = 1;
        }
        Debug.Log("Found");
    }

    public void OnTrackingLost()
    {
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
            component.volume = 0;
        }
        Debug.Log("Lost");
    }
}
