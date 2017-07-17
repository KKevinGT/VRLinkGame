using UnityEngine;
using System.Collections;
using RockVR.Rift;
using System;

public class TouchManager : MonoBehaviour {

    private RIFT_EventCtrl eventCtrl;
    private RIFT_Interaction vrIteraction;
    private RIFT_Teleport teleport;
    private static CubeManager startLinkObject;
    private static CubeManager endLinkObject;
    // Use this for initialization
    void Awake () {
        vrIteraction = this.transform.GetComponent<RIFT_Interaction>();
        eventCtrl = this.GetComponent<RIFT_EventCtrl>();
        teleport = this.GetComponent<RIFT_Teleport>();
    }

    private void OnEnable()
    {
        if (eventCtrl!=null)
        {
            eventCtrl.eventDelegate.OnTouchPrimaryThumbstickUp += OnTouchPrimaryThumbstickUp;
            eventCtrl.eventDelegate.OnPressPrimaryThumbstick += OnPressPrimaryThumbstick;
            eventCtrl.eventDelegate.OnPressButtonPrimaryIndexTriggerDown += OnPressButtonPrimaryIndexTriggerDown;
        }
    }

    private void OnDisable()
    {
        if (eventCtrl!=null)
        {
            eventCtrl.eventDelegate.OnTouchPrimaryThumbstickUp -= OnTouchPrimaryThumbstickUp;
            eventCtrl.eventDelegate.OnPressPrimaryThumbstick -= OnPressPrimaryThumbstick;
            eventCtrl.eventDelegate.OnPressButtonPrimaryIndexTriggerDown -= OnPressButtonPrimaryIndexTriggerDown;
        }
    }

    private void OnPressButtonPrimaryIndexTriggerDown()
    {
        if (vrIteraction.selectedObject!=null)
        {
            if (vrIteraction.selectedObject.GetComponent<CubeManager>()!=null)
            {
                if (startLinkObject==null)
                {
                    startLinkObject = vrIteraction.selectedObject.GetComponent<CubeManager>();
                    startLinkObject.isChoose = true;
                }
                else if(startLinkObject != null&&endLinkObject==null)
                {
                    endLinkObject = vrIteraction.selectedObject.GetComponent<CubeManager>();
                    if (endLinkObject.isChoose)
                    {
                        startLinkObject.isChoose = false;
                        endLinkObject.isChoose = false;
                        startLinkObject = null;
                        endLinkObject = null;
                        return;
                    }
                    else
                    {
                        endLinkObject.isChoose = true;
                    }
                    if (startLinkObject != null && endLinkObject != null)
                    {
                        if (startLinkObject.id == endLinkObject.id)
                        {
                            if (startLinkObject.SetPathPoint(startLinkObject.transform.position, endLinkObject.transform.position))
                            {
                                Destroy(startLinkObject.gameObject, 1f);
                                Destroy(endLinkObject.gameObject, 1f);
                                startLinkObject = null;
                                endLinkObject = null;
                            }
                            else
                            {
                                startLinkObject.isChoose = false;
                                endLinkObject.isChoose = false;
                                startLinkObject = null;
                                endLinkObject = null;
                            }
                        }
                        else
                        {
                            startLinkObject.isChoose = false;
                            endLinkObject.isChoose = false;
                            startLinkObject = null;
                            endLinkObject = null;
                        }
                    }
                }
            }
        }
    }

    private void OnPressPrimaryThumbstick()
    {
        if (teleport != null)
        {
            teleport.SearchDropPoint();
        }
    }

    private void OnTouchPrimaryThumbstickUp()
    {
        if (teleport != null)
        {
            teleport.SureDropPoint();
        }
    }

    // Update is called once per frame
    void Update () {
       
	}
}
