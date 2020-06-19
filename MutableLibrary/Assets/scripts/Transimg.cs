using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class Transimg : MonoBehaviour
{
    [SerializeField]
    XRReferenceImageLibrary xrlib;

    [SerializeField]
    GameObject prefabs;

    public Text txtt;

    string folderPath = "pics/";
    string fileExtension = ".jpg";
    List<string> Filename = new List<string>();

    ARTrackedImageManager arImgManager;
    public void Start()
    {
        Filename.Add("1");
        Filename.Add("2");
        Filename.Add("3");
        Filename.Add("4");

        if (xrlib != null && prefabs != null)
        {
            arImgManager = gameObject.GetComponent<ARTrackedImageManager>();
            arImgManager.referenceLibrary = arImgManager.CreateRuntimeLibrary(xrlib);
            txtt.text = "Clear RuntimeLibrary\n";
            arImgManager.maxNumberOfMovingImages = 1;
            arImgManager.trackedImagePrefab = prefabs;

            arImgManager.enabled = true;

            MakeTImg();

            arImgManager.trackedImagesChanged += TrackImgChanged;
        }
        else
        {
            txtt.text = "Can't Use\n";
        }
    }


    public void MakeTImg()
    {
        JobHandle handle = new JobHandle();

        if (arImgManager.referenceLibrary is MutableRuntimeReferenceImageLibrary mlib)
        {
            foreach (string idx in Filename)
            {
                var txt = Resources.Load(folderPath + idx + fileExtension, typeof(Texture2D)) as Texture2D;
                txtt.text += idx + fileExtension + "\n";
                handle = mlib.ScheduleAddImageJob(txt, idx, 0.1f);
            }
            if (!handle.IsCompleted)
            {

            }
        }
        else
            txtt.text += "Can't use Mutable\n";


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void TrackImgChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach(ARTrackedImage trackedImage in eventArgs.added)
        {
            txtt.text += trackedImage.referenceImage.name+"\n";
        }

        foreach(ARTrackedImage trackedImage in eventArgs.updated)
        {
            txtt.text = trackedImage.referenceImage.name + "\n";
        }

        foreach(ARTrackedImage trackedImage in eventArgs.removed)
        {

        }
    }
}
