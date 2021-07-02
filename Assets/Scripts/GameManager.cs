using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using easyar;
using RenderHeads.Media.AVProVideo;
using UnityEngine;
using VideoPlayer = UnityEngine.Video.VideoPlayer;

public class GameManager : MonoBehaviour
{

   

    public MediaPlayer MediaPlayer;

    public ImageTargetController Controller;
   

    private bool isShow = false;
    // Start is called before the first frame update
    void Start()
    {
       



        

#if UNITY_ANDROID && !UNITY_EDITOR
        //VideoPlayer.url = "jar:file://" + Application.streamingAssetsPath + "/demo.mp4";
#endif

#if UNITY_IPHONE
        Debug.Log("这里苹果设备");
#endif

#if UNITY_EDITOR && UNITY_EDITOR_WIN
        // VideoPlayer.url = "file://" + Application.streamingAssetsPath + "/demo.mp4";
#endif

    }

    // Update is called once per frame
    void Update()
    {
        if (!isShow)
        {
            if (Controller.gameObject.activeInHierarchy)
            {
                isShow = true;
                PlayVideo();
            }
        }
    }

   public void PlayVideo()
    {
        MediaPlayer.gameObject.SetActive(true);
    
        MediaPlayer.Play();
     
      
      


    }

    public void CloseVideo()
    {
        isShow = false;
        MediaPlayer.Stop();
        MediaPlayer.Rewind(true);
       
     
       
    }

    public void PauseVideo()
    {
        MediaPlayer.Pause();
     
    }

    
}
