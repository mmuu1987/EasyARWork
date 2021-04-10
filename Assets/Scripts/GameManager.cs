using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using easyar;
using UnityEngine;
using VideoPlayer = UnityEngine.Video.VideoPlayer;

public class GameManager : MonoBehaviour
{

    public RectTransform MoveLine;

    public VideoPlayer VideoPlayer;

    public ImageTargetController Controller;

    public GameObject PlayBtn;

    public GameObject PauseBtn;

    public GameObject BgAlpah;

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
        VideoPlayer.gameObject.SetActive(true);
        BgAlpah.gameObject.SetActive(true);
        VideoPlayer.Play();
        PlayBtn.gameObject.SetActive(false);
        PauseBtn.gameObject.SetActive(true);
    }

    public void CloseVideo()
    {
        isShow = false;
        VideoPlayer.gameObject.SetActive(false);
        BgAlpah.gameObject.SetActive(false);
        PlayBtn.gameObject.SetActive(false);
        PauseBtn.gameObject.SetActive(true);
    }

    public void PauseVideo()
    {
        VideoPlayer.Pause();
        PlayBtn.gameObject.SetActive(true);
        PauseBtn.gameObject.SetActive(false);
    }

    
}
