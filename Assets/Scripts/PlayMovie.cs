using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class PlayMovie : MonoBehaviour
{

    private VideoPlayer _videoPlayer;

    public GameObject ShowGameObject;

    private Rect _showRect;
    private void Awake()
    {
        _videoPlayer = this.GetComponent<VideoPlayer>();

        _showRect= new Rect(200f,600f,800f,800f);
    }
    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        if (this.gameObject.activeInHierarchy)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(this.transform.position);

            if (_showRect.Contains(screenPos))
            {
                if (!_videoPlayer.isPlaying)
                {
                    _videoPlayer.Play();
                }
            }


        }
    }

    private void OnDisable()
    {
        _videoPlayer.Stop();
        ShowGameObject.gameObject.SetActive(true);
    }

    private void OnEnable()
    {

        ShowGameObject.gameObject.SetActive(false);
        //_videoPlayer.Play();
    }
}
