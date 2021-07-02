/*************************************************
 * 项目名称：Unity实现启用摄像头扫描与生成二维码
 * 脚本创建人：魔卡
 * 脚本创建时间：2017.12.20
 * 脚本功能：二维码识别生成控制类
 * ***********************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using ZXing;
using ZXing.QrCode;

using UnityEngine.Rendering;

//二维码识别生成控制类
public class QRCodeManager : MonoBehaviour
{
    #region 扫描二维码
    //定义一个用于存储调用电脑或手机摄像头画面的RawImage
    //public RawImage m_cameraTexture;


    public Camera ArCamera;
    //摄像头实时显示的画面
    private Texture2D m_Texture;

    //申请一个读取二维码的变量
    private BarcodeReader m_barcodeRender = new BarcodeReader();

    //多久检索一次二维码
    private float m_delayTime = 0.2f;
    #endregion

    #region 生成二维码
    //用于显示生成的二维码RawImage
    public RawImage m_QRCode;

    //申请一个写二维码的变量
    private BarcodeWriter m_barcodeWriter;
    #endregion

    /// <summary>
    /// 是否识别二维码
    /// </summary>
    public bool IsQr = true;

    public UniWebView UniWebView;

    public RectTransform UniWebViewTransform;

    public Button BackButton;
    #region 扫描二维码
    void Start()
    {
        //调用摄像头并将画面显示在屏幕RawImage上
        //WebCamDevice[] tDevices = WebCamTexture.devices;    //获取所有摄像头
        //string tDeviceName = tDevices[0].name;  //获取第一个摄像头，用第一个摄像头的画面生成图片信息
        //m_webCameraTexture = new WebCamTexture(tDeviceName, 400, 300);//名字,宽,高
        //m_cameraTexture.texture = m_webCameraTexture;   //赋值图片信息
        //m_webCameraTexture.Play();  //开始实时显示

        //InvokeRepeating("CheckQRCode", 0, m_delayTime);

        StartCoroutine(WaitTime());
        //UniWebView.SetBackButtonEnabled(false);
        //UniWebView = UniWebViewTransform.gameObject.AddComponent<UniWebView>();
        //UniWebView.ReferenceRectTransform = UniWebViewTransform;
        BackButton.onClick.AddListener((() =>
        {
            Close();
        }));
    }


    private IEnumerator WaitTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(m_delayTime);
            //StartCoroutine(AsyncLoad());
            if (IsQr)
                StartCoroutine(ScreenShoot());
        }
    }
    IEnumerator AsyncLoad()
    {

        RenderTexture rt = new RenderTexture(Screen.width, Screen.height,0,RenderTextureFormat.ARGB32);

        ArCamera.targetTexture = rt;
        ArCamera.Render();



        RenderTexture.active = rt;
        //Graphics.xxx...
        var req = AsyncGPUReadback.Request(rt);
        yield return new WaitUntil(() => req.done);
        ArCamera.targetTexture = null;
       var tex = new Texture2D(Screen.width, Screen.height, TextureFormat.RGBA32, false);
        var colorArray = req.GetData<Color32>().ToArray();
        tex.SetPixels32(colorArray);
        tex.Apply();
        //m_cameraTexture.texture = tex;
    }

    private IEnumerator ScreenShoot()
    {


        //if (m_cameraTexture.texture != null)
        //{
        //    // Destroy(m_cameraTexture.texture);
        //}
        //Wait for graphics to render
        yield return new WaitForEndOfFrame();

        RenderTexture rt = new RenderTexture(Screen.width, Screen.height, 24);
        //m_Texture = new Texture2D(800, 800, TextureFormat.RGB24, false);
        m_Texture = new Texture2D(800, 800, TextureFormat.RGB24, false);


        ArCamera.targetTexture = rt;
        ArCamera.Render();
         
        

        RenderTexture.active = rt;
        m_Texture.ReadPixels(new Rect(200f, 600f, 800, 800), 0, 0);

        m_Texture.Apply();
        ArCamera.targetTexture = null;

        //Added to avoid errors
        RenderTexture.active = null;
        Destroy(rt);

        //Split the process up--ReadPixels() and the GetPixels() call inside of the encoder are both pretty heavy
        yield return 0;
        //m_cameraTexture.texture = m_Texture;
       // m_cameraTexture.SetNativeSize();
        //存储摄像头画面信息贴图转换的颜色数组
        Color32[] m_colorData = m_Texture.GetPixels32();
        Destroy(m_Texture);
        //将画面中的二维码信息检索出来
        var tResult = m_barcodeRender.Decode(m_colorData, m_Texture.width, m_Texture.height);

        if (tResult != null)
        {
            OpenUrl(tResult.Text);

            IsQr = false;
        }
    }
   
    #endregion

    private void OpenUrl(string url)
    {
        if (UniWebView == null)
        {
            UniWebView = UniWebViewTransform.gameObject.AddComponent<UniWebView>();
            UniWebView.ReferenceRectTransform = UniWebViewTransform;

          
        }
        UniWebView.Frame = new Rect(0, 0, Screen.width, Screen.height);

        // Load a URL.
        UniWebView.Load(url);

        // Show it.
        UniWebView.Show();
        BackButton.gameObject.SetActive(true);

    }
    public void Close()
    {
        if (UniWebView == null)
        {
            UniWebView = UniWebViewTransform.gameObject.AddComponent<UniWebView>();
            UniWebView.ReferenceRectTransform = UniWebViewTransform;
        }
        UniWebView.Hide(false,UniWebViewTransitionEdge.Bottom,0.4f,(() =>
        {
            UniWebView.CleanCache();
            UniWebView.Stop();
            Destroy(UniWebView);
        }));
       
        IsQr = true;
        BackButton.gameObject.SetActive(false);
    }

    public Texture2D TexQR;
    public void Test()
    {
        //存储摄像头画面信息贴图转换的颜色数组
        Color32[] m_colorData = TexQR.GetPixels32();

        //将画面中的二维码信息检索出来
        var tResult = m_barcodeRender.Decode(m_colorData, TexQR.width, TexQR.height);

        if (tResult != null)
        {
            OpenUrl(tResult.Text);
            IsQr = false;

        }
    }

#if UNITY_EDITOR_WIN
    private void OnGUI()
    {
        if (GUI.Button(new Rect(0f, 0f, 100f, 100f), "test"))
        {
            Test();
        }
    }
#endif

    #region 传递字符串生成二维码
    void Update()
    {
       
    }
    /// <summary>
    /// 显示绘制的二维码
    /// </summary>
    /// <param name="s_formatStr">扫码信息</param>
    /// <param name="s_width">码宽</param>
    /// <param name="s_height">码高</param>
    void ShowQRCode(string s_str, int s_width, int s_height)
    {
        //定义Texture2D并且填充
        Texture2D tTexture = new Texture2D(s_width, s_height);

        //绘制相对应的贴图纹理
        tTexture.SetPixels32(GeneQRCode(s_str, s_width, s_height));

        tTexture.Apply();

        //赋值贴图
        m_QRCode.texture = tTexture;
    }
    /// <summary>
    /// 返回对应颜色数组
    /// </summary>
    /// <param name="s_formatStr">扫码信息</param>
    /// <param name="s_width">码宽</param>
    /// <param name="s_height">码高</param>
    Color32[] GeneQRCode(string s_formatStr, int s_width, int s_height)
    {
        //设置中文编码格式，否则中文不支持
        QrCodeEncodingOptions tOptions = new QrCodeEncodingOptions();
        tOptions.CharacterSet = "UTF-8";
        //设置宽高
        tOptions.Width = s_width;
        tOptions.Height = s_height;
        //设置二维码距离边缘的空白距离
        tOptions.Margin = 3;

        //重置申请写二维码变量类       (参数为：码格式（二维码、条形码...）    编码格式（支持的编码格式）    )
        m_barcodeWriter = new BarcodeWriter { Format = BarcodeFormat.QR_CODE, Options = tOptions };

        //将咱们需要隐藏在码后面的信息赋值上
        return m_barcodeWriter.Write(s_formatStr);
    }
    #endregion

}