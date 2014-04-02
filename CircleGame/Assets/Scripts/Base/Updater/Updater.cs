using UnityEngine;
using System;
using System.Net;
using System.Linq;
using System.Xml.Linq;
using System.Threading;
using System.Collections;
using System.Collections.Generic;

public class Updater : MonoBehaviour {
    public static Launchie.Launchie updater { get; private set; }
    public static Action onFinishedDownloadingReleaseNotes;
    public static Action onFinishedDownloadingPatch;
    public static Action onFinishedExtractingPatch;
    public static Action<double> onProgress;
    public static bool patchInstalled;
    public static bool initialized;

    public static UpdateData CheckForUpdate() {
        return CheckForUpdate(SOS.version);
    }


    public static UpdateData CheckForUpdate(string version) {
        updater = new Launchie.Launchie(SOS.updateURL, version);
        updater.setOnError(OnError);
        initialized = true;
        bool updateFound = updater.Check() == 1 ? true : false;
        return new UpdateData(updater, updateFound);
    }


    //threaded
    public static void OnError(Exception e) {
        Debug.LogWarning(e.ToString());
    }

    public static void DownloadReleaseNotes() {
        DownloadReleaseNotes(null);
    }

    public static void DownloadReleaseNotes(Action callBack) {
        if(callBack != null) onFinishedDownloadingReleaseNotes = callBack;
        updater.setOnDone(FinishedDownloadingReleaseNotes);
        updater.DownloadReleaseNotes();
    }

    private static void FinishedDownloadingReleaseNotes() {
        if(onFinishedDownloadingReleaseNotes != null) onFinishedDownloadingReleaseNotes();
    }

    public static void Download() {
        Download(null);
    }

    public static void Download(Action callback) {
        if(callback != null) onFinishedDownloadingPatch = callback;
        updater.setOnDone(FinishedDownloadingPatch);
        updater.setOnProgress(OnProgress);
        updater.Download();
    }

    private static void FinishedDownloadingPatch() {
        if(onFinishedDownloadingPatch != null) onFinishedDownloadingPatch();
    }

    public static void OnProgress(Action<double> callBack) {
        if(callBack != null) onProgress = callBack;
    }

    private static void OnProgress(double progress) {
        if(onProgress != null) onProgress(progress);
    }

    public static void ExtractPatch() {
        ExtractPatch(null);
    }

    public static void ExtractPatch(Action callBack) {
        if(callBack != null) onFinishedExtractingPatch = callBack;
        updater.setOnDone(FinishedExtractingPatch);
        updater.setOnProgress(OnProgress);
        updater.Extract();
    }

    //threaded
    private static void FinishedExtractingPatch() {
        updater.Finish();
        if(onFinishedExtractingPatch != null) onFinishedExtractingPatch();
        patchInstalled = true;
    }
}

public class UpdateData {
    public Launchie.Launchie updater;
    public bool updateFound { get; set; }
    public string releaseNotes { get; set; }
    public string version { get; set; }

    public UpdateData(Launchie.Launchie updater, bool updateFound) {
        this.updater = updater;
        this.updateFound = updateFound;
    }
}