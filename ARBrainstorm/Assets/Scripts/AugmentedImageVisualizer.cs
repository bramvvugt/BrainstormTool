using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using GoogleARCore;

public class AugmentedImageVisualizer : MonoBehaviour {

    [SerializeField] private VideoClip[] _videoClips;
    [SerializeField] private Texture[] _textures;
    [SerializeField] private Texture[] _textures2;
    public AugmentedImage Image;
    private VideoPlayer _videoPlayer;
    private MeshRenderer _meshRenderer;
    private int texNum;

	// Use this for initialization
	void Start () {
        _videoPlayer = GetComponent<VideoPlayer>();
        _videoPlayer.loopPointReached += OnStop;
        
    }
	
    private void OnStop(VideoPlayer source)
    {
        gameObject.SetActive(false);
    }


    public void Texture(int num)
    {
        Texture2D texture = _textures[num] as Texture2D;
        Material material = new Material(Shader.Find("Diffuse"));
        material.mainTexture = texture;
        transform.GetComponent<MeshRenderer>().material = material;
    }

    public void Texture2(int num)
    {
        Texture2D texture = _textures2[num] as Texture2D;
        Material material = new Material(Shader.Find("Diffuse"));
        material.mainTexture = texture;
        transform.GetComponent<MeshRenderer>().material = material;
    }

    // Update is called once per frame
    void Update () {

		if (Image == null || Image.TrackingState != TrackingState.Tracking)
        {
            return;
        }

        if (!_videoPlayer.isPlaying)
        {
            // _videoPlayer.clip = _videoClips[Image.DatabaseIndex];
            //_videoPlayer.Play();


           // Texture(0);
            
        }

        transform.localScale = new Vector3(Image.ExtentX, Image.ExtentZ, 1);


    
	}
}
