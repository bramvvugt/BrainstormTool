using GoogleARCore;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AugmentedImageController : MonoBehaviour {

    [SerializeField] private AugmentedImageVisualizer _augmentedImageVisualizer;
    private readonly Dictionary<int, AugmentedImageVisualizer> _visualizers =
        new Dictionary<int, AugmentedImageVisualizer>();

    private readonly List<AugmentedImage> _images = new List<AugmentedImage>();

    Anchor myAnchor;
    float right;

    public GameObject uiScan;
    public GameObject uiReset;

    //List<AugmentedImageVisualizer> images = new List<AugmentedImageVisualizer>();


    private void Update()
    {
        if (Session.Status != SessionStatus.Tracking)
        {
            
            return;
        }

        Session.GetTrackables(_images, TrackableQueryFilter.Updated);
        VisualizeTrackables();



        for (var i = 0; i < Input.touchCount; ++i)
        {
            if (Input.GetTouch(i).phase == TouchPhase.Began)
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider != null)
                    {
                       hit.collider.gameObject.tag = "select";
                       hit.collider.transform.position = new Vector3(0, 0f, 0.4f);

                       // GameObject[] images = GameObject.FindGameObjectsWithTag("image");
                       // foreach (GameObject foto in images)
                       // {
                        //    GameObject.Destroy(foto);
                         
                       // }

                        for (int j = 0; j < 3; j++)
                       {
                            //  Destroy(hit.collider.gameObject);
                                                      
                            Vector3 pos = new Vector3(-1f + right, 1f, 0.4f);
                            Pose pose = new Pose(pos, transform.rotation);
                            Anchor anchor = Session.CreateAnchor(pose, null);
                            var visualizer = Instantiate(_augmentedImageVisualizer, anchor.transform);
                           // images.Add(visualizer);
                            visualizer.Texture2(j);
                            right += 1f;
                        }
                    }
                }
            }
        }
    }

    public void Start()
    {
        right = 0;

        Button btn = uiReset.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        Scene scene = SceneManager.GetActiveScene(); SceneManager.LoadScene(scene.name);
       
    }

    private void VisualizeTrackables()
    {
       foreach(var image in _images)
        {
            var visualizer = GetVisualizer(image);
          
            if (image.TrackingState == TrackingState.Tracking && visualizer == null)
            {
                AddVisualizer(image);
            }
            else if (image.TrackingState == TrackingState.Paused && visualizer != null)
            {
                RemoveVisualizer(image, visualizer);
            }

        }
    }

    private void RemoveVisualizer(AugmentedImage image, AugmentedImageVisualizer visualizer)
    {
        _visualizers.Remove(image.DatabaseIndex);
        Destroy(visualizer.gameObject);
    }

    private void AddVisualizer(AugmentedImage image)
    {
        
        // video en image positie
        for(int i = 0; i < 2; i++)
        {
            
            Vector3 test = new Vector3(0.2f + right, 0.1f, 0.5f);
            transform.eulerAngles = new Vector3(-90, 0, 0);

            Pose pose = new Pose(test, transform.rotation);
            Anchor anchor = Session.CreateAnchor(pose, null);
            //var anchor = image.CreateAnchor(image.CenterPose);
            var visualizer = Instantiate(_augmentedImageVisualizer, anchor.transform);
            visualizer.Image = image;
            visualizer.Texture(i);
            visualizer.tag = "image";
            //images.Add(visualizer);
            
            
            //_augmentedImageVisualizer.transform.GetComponent<MeshRenderer>();
            _visualizers.Add(image.DatabaseIndex, visualizer);
            right += 0.2f;
            uiScan.SetActive(false);
       }
    }

    private AugmentedImageVisualizer GetVisualizer(AugmentedImage image)
    {
        AugmentedImageVisualizer visualizer;
        _visualizers.TryGetValue(image.DatabaseIndex, out visualizer);
        return visualizer;

    }
}
