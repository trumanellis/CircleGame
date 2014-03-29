using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Reflection;

public class SplashScreen : MonoBehaviour {
    private Color transparent = new Color(1f, 1f, 1f, 0f);
    private float finishedTime = 0f;
    private bool startTracking = false;
    private object operationData;

    //public static SplashScreen instance { get; private set; }
    public Transform[] children { get; private set; }
    public delegate void SplashDelegate(SplashScreen splash);
    public delegate void OperationDelegate(object data);
    public SplashDelegate onSplashStart;
    public SplashDelegate onSplashFinished;
    public SplashDelegate onSplashFadeInStarted;
    public SplashDelegate onSplashFadeOutStarted;
    public SplashDelegate onSplashFadeInFinished;
    public SplashDelegate onSplashFadeOutFinished;
    public OperationDelegate onStartOperationStarted;
    public OperationDelegate onStartOperationFinished;
    public bool disabled;
    public bool allowSkipping;
    public KeyCode skip = KeyCode.Escape;
    public ConcurrentOperation startingOperation;
    public bool fadeIn = true;
    public iTween.EaseType fadeInEase;
    public bool fadeOut = true;
    public iTween.EaseType fadeOutEase;
    public bool fadeBackground;
    public string sceneToLoad;
    public float fadeInTime = 2f;
    public float fadeOutTime = 2f;
    public float idleTime = 2f;

    //private void Awake() { instance = this; }

    private void Start() {
        if(!startingOperation.disabled && startingOperation.className != string.Empty)
            StartOperation();

        if(disabled) {
            SplashDone();
            return;
        }

        children = GetComponentsInChildren<Transform>();
        if(fadeIn) FadeIn();
        else {
            finishedTime = Time.time + idleTime;
            startTracking = true;
        }

        if(onSplashStart != null)
            onSplashStart(this);
    }

    private void StartOperation() {
        Type type = Type.GetType(startingOperation.className);
        if(type != null) {
            MethodInfo method = null;
            if(string.IsNullOrEmpty(startingOperation.argument))
                method = type.GetMethod(startingOperation.method, new Type[]{ /*void*/ });
            else
                method = type.GetMethod(startingOperation.method, new Type[] { typeof(string) });
            if(method != null) {
                ParameterInfo[] parms = method.GetParameters();
                object instance = null;
                if(onStartOperationStarted != null)
                    onStartOperationStarted(this);
                if(!method.IsStatic) {
                    if(typeof(MonoBehaviour).IsAssignableFrom(type)) Debug.LogWarning("It is highly recommended to call static methods if the type is a MonoBehaiour." +
                        " Calling the constructor of this type is unsupported in unity.");
                    instance = Activator.CreateInstance(type, null);
                }
                if(parms.Length == 0) operationData = method.Invoke(instance, null);
                else operationData = method.Invoke(instance, new object[1] { startingOperation.argument });
            } else throw new Exception("No method by that name found. Method name: " + startingOperation.method);
        } else throw new Exception("No class by that name found. class name: " + startingOperation.className);
        OperationFinished();
    }

    void Update() {
        if(allowSkipping && Input.GetKeyUp(skip))
            SplashDone();

        if(startTracking && Time.time >= finishedTime) {
            startTracking = false;

            if(fadeOut) FadeOut();
            else SplashDone();
        }
    }

    private void FadeIn() {
        if(onSplashFadeInStarted != null)
            onSplashFadeInStarted(this);
        for(int i = 0; i < children.Length; i++) {
            if(!fadeBackground && children[i].name.Equals("Background")) continue;
            if(i != children.Length - 1) iTween.ColorFrom(children[i].gameObject, iTween.Hash(
                "color", transparent,
                "time", fadeInTime,
                "easetype", fadeInEase,
                "includechildren", false));
            else iTween.ColorFrom(children[i].gameObject, iTween.Hash(
                "color", transparent,
                "time", fadeInTime,
                "easetype", fadeInEase,
                "includechildren", false,
                "oncomplete", "FadeInDone",
                "oncompletetarget", gameObject));
        }
    }

    public void FadeInDone() {
        if(onSplashFadeInFinished != null)
            onSplashFadeInFinished(this);
        startTracking = true;
        finishedTime = Time.time + idleTime;
    }

    private void FadeOut() {
        if(onSplashFadeOutStarted != null)
            onSplashFadeOutStarted(this);
        for(int i = 0; i < children.Length; i++) {
            if(!fadeBackground && children[i].name.Equals("Background")) continue;
            if(i != children.Length - 1) iTween.ColorTo(children[i].gameObject, iTween.Hash(
                "color", transparent,
                "time", fadeOutTime,
                "easetype", fadeOutEase,
                "includechildren", false));
            else iTween.ColorTo(children[i].gameObject, iTween.Hash(
                "color", transparent,
                "time", fadeOutTime,
                "easetype", fadeOutEase,
                "includechildren", false,
                "oncomplete", "FadeOutDone",
                "oncompletetarget", gameObject));
        }
    }

    public void FadeOutDone() {
        if(onSplashFadeOutFinished != null)
            onSplashFadeOutFinished(this);
        SplashDone();
    }

    private void SplashDone() {
        enabled = false;
        if(onSplashFinished != null)
            onSplashFinished(this);
        if(!string.IsNullOrEmpty(sceneToLoad)) Application.LoadLevel(sceneToLoad);
    }

    private void OperationFinished() {
        if(onStartOperationFinished != null)
            onStartOperationFinished(operationData);
    }

    [System.Serializable]
    public sealed class ConcurrentOperation {
        public bool disabled;
        public string className;
        public string method;
        public string argument;
    }
}