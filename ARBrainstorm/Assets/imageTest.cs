using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class imageTest : MonoBehaviour {
    [SerializeField] private Texture[] _textures;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Texture2D texture = _textures[0] as Texture2D;
       Material material = new Material(Shader.Find("Diffuse"));
        material.mainTexture = texture;
        transform.GetComponent<MeshRenderer>().material = material;
    }
}
