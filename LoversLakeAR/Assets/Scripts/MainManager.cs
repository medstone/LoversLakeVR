using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour {

    bool _won = false;

    /** The size of the band playing the romantic music */
    private const int GOOD_BAND_SIZE = 3;

    /** The size of the band playing the metal music */
    private const int METAL_BAND_SIZE = 2;

    static private MainManager instance;
    static public MainManager Instance
    {
        get
        {
            return instance;
        }
    }

    Dictionary<VuforiaObjectTest.InstrumentType, int> instrumentCounts =
        new Dictionary<VuforiaObjectTest.InstrumentType, int>();

	// Use this for initialization
	void Start () {
        instance = this;
	}
	
	// Update is called once per frame
	void Update () {

        if (!_won) {
            // check if boat is in view
            if (instrumentCounts[VuforiaObjectTest.InstrumentType.BOAT] >= 1)
            {
                // check if the full metal band is playing
                if (instrumentCounts[VuforiaObjectTest.InstrumentType.GOOD] == 0 &&
                    instrumentCounts[VuforiaObjectTest.InstrumentType.METAL] >= METAL_BAND_SIZE &&
                    instrumentCounts[VuforiaObjectTest.InstrumentType.WEIRD] == 0
                    )
                {

                    // TODO change boat animation to fighting maybe?

                }

                // check if the current playing band is good
                else if (instrumentCounts[VuforiaObjectTest.InstrumentType.METAL] == 0 &&
                    instrumentCounts[VuforiaObjectTest.InstrumentType.WEIRD] == 0)
                {
                    if (instrumentCounts[VuforiaObjectTest.InstrumentType.GOOD] == GOOD_BAND_SIZE)
                    {
                        _won = true;

                        // TODO change boat animation to win scene

                        // TODO decouple band member models from the targets
                    }
                    else
                    {
                        /* TODO change boat animation depending on how many good 
                         * band members are currently playing */
                    }
                }

                // the band must be playing weird music
                else
                {
                    // TODO change boat animation to weird faces
                }
            }
        }

	}
}
