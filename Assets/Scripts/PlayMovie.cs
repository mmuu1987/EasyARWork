using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class PlayMovie : MonoBehaviour
{

    private VideoPlayer _videoPlayer;

    public GameObject ShowGameObject;
    private void Awake()
    {
        _videoPlayer = this.GetComponent<VideoPlayer>();
    }
    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDisable()
    {
        _videoPlayer.Stop();
        ShowGameObject.gameObject.SetActive(true);
    }

    private void OnEnable()
    {

        ShowGameObject.gameObject.SetActive(false);
        _videoPlayer.Play();
    }
}
