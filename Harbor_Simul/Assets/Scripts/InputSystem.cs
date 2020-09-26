﻿#define INPUT_TEST


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.XR;
using Valve.VR;


public class InputSystem : Singleton<InputSystem>
{
    public float angle { get; private set; }
    public float speed { get; private set; }
    public bool select { get; private set; }
    public bool back { get; private set; }
    public bool virtualRight { get; private set; }
    public bool virtualLeft { get; private set; }

    public float testValue;
    public Scrollbar bar;
    private const float threshold = 0.5f;

    protected override void Awake()
    {
        base.Awake();
        /*if (InputSystem.instance != null && InputSystem.instance != this)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);*/
        instance = this;
    }
    private void Update()
    {

        if (Input.GetKey(KeyCode.W))
        {
            speed += 1f * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            speed -= 1f * Time.deltaTime;
        }
        if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
        {
            speed = getControllerSpeedValue();
        }
        speed = Mathf.Clamp(speed, -1, 1);

        if (Input.GetKey(KeyCode.A))
        {
            angle -= 1f * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            angle += 1f * Time.deltaTime;
        }
        if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            angle = getControllerAngleValue();
        }
        angle = Mathf.Clamp(angle, -1, 1);

        if (Math.Abs(getControllerAngleValue()) > threshold)
        {
            if (getControllerAngleValue() > 0)
                virtualRight = true;
            else
                virtualLeft = true;
        }
        else
        {
            virtualLeft = false;
            virtualRight = false;
        }

#if INPUT_TEST
        virtualLeft = Input.GetKey(KeyCode.LeftArrow) == true ? true : virtualLeft;
        virtualRight = Input.GetKey(KeyCode.RightArrow) == true ? true : virtualRight;
        select = Input.GetKeyDown(KeyCode.R) == true ? true : SerialManager.instance.select;
        back = Input.GetKeyDown(KeyCode.F) == true ? true : SerialManager.instance.back;
        if (back)
            RecenterVR();
#else
        select = SerialManager.instance.select;
        back = SerialManager.instance.back;
#endif

        //Debug.Log("R : " + virtualRight + " L : " + virtualLeft);
    }

    private float getControllerSpeedValue()
    {
#if INPUT_TEST
        return SerialManager.instance.speed;
#else
        return SerialManager.instance.speed;
#endif
    }

    private float getControllerAngleValue()
    {
#if INPUT_TEST
        return SerialManager.instance.angle;
#else
        return SerialManager.instance.angle;
#endif
    }

    public void ValueTest()
    {
        if (bar == null) bar = GameObject.Find("AngleBar").GetComponent<Scrollbar>();
        testValue = bar.value * 2 - 1;
    }


    public static void RecenterVR()
    {
        if (XRSettings.loadedDeviceName == "Oculus")
        {
            print("Reset VRSettings.loadedDeviceName == Oculus");
            UnityEngine.XR.InputTracking.Recenter();
        }

        if (XRSettings.loadedDeviceName == "OpenVR")
        {
            print("Reset VRSettings.loadedDeviceName == OpenVR");
            Valve.VR.OpenVR.System.ResetSeatedZeroPose();

            Valve.VR.OpenVR.Compositor.SetTrackingSpace(Valve.VR.ETrackingUniverseOrigin.TrackingUniverseSeated);
        }
    }
}
