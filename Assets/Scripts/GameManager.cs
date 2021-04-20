using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using easyar;
using RenderHeads.Media.AVProVideo;
using UnityEngine;
using VideoPlayer = UnityEngine.Video.VideoPlayer;

public class GameManager : MonoBehaviour
{

    public RectTransform MoveLine;

    public MediaPlayer MediaPlayer;

    public ImageTargetController Controller;

    public GameObject PlayBtn;

    public GameObject PauseBtn;

    public GameObject BgAlpah;

    public GameObject BG;

    private bool isShow = false;
    // Start is called before the first frame update
    void Start()
    {
        MoveLine.DOAnchorPos3DY(-420f, 3f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);



        

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
        BgAlpah.gameObject.SetActive(true);
       
        MediaPlayer.Play();
        PlayBtn.gameObject.SetActive(false);
        PauseBtn.gameObject.SetActive(true);
        BG.GetComponent<RectTransform>().DOAnchorPosY(-1600, 1f);
        BgAlpah.GetComponent<RectTransform>().DOAnchorPosY(0f, 1f);


    }

    public void CloseVideo()
    {
        isShow = false;
        MediaPlayer.Stop();
        MediaPlayer.Rewind(true);
        PlayBtn.gameObject.SetActive(false);
        PauseBtn.gameObject.SetActive(true);
        BG.GetComponent<RectTransform>().DOAnchorPosY(0f,1f);
        BgAlpah.GetComponent<RectTransform>().DOAnchorPosY(1600f, 1f);
    }

    public void PauseVideo()
    {
        MediaPlayer.Pause();
        PlayBtn.gameObject.SetActive(true);
        PauseBtn.gameObject.SetActive(false);
    }

    
}
