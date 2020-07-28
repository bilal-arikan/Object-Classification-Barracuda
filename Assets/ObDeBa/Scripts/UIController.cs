using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController Instance;

    public Text ResultsText;
    public RawImage CameraScreen;
    public AspectRatioFitter CamFitter;
    public Button SearchButton;
    public Text SearchText;
    public LayoutGroup SearchResults;
    public float ShowHigherThan = 5f;

    private float cameraScale = 1f;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        CameraScreen.texture = GameController.Instance.WebCamCamera;
        SearchButton.onClick.AddListener(() =>
        {

        });
    }

    private void Update()
    {
        var backCamera = GameController.Instance.WebCamCamera;
        float ratio = (float)backCamera.width / (float)backCamera.height;
        CamFitter.aspectRatio = ratio;

        float scaleX = cameraScale;
        float scaleY = backCamera.videoVerticallyMirrored ? -cameraScale : cameraScale;
        CameraScreen.rectTransform.localScale = new Vector3(scaleX, scaleY, 1f);

        int orient = -backCamera.videoRotationAngle;
        CameraScreen.rectTransform.localEulerAngles = new Vector3(0, 0, orient);

        if (orient != 0)
        {
            this.cameraScale = (float)Screen.width / Screen.height;
        }
    }

    public void ShowResult(List<KeyValuePair<string, float>> results)
    {
        ResultsText.text = String.Empty;

        var highers = results.Where(p => p.Value > ShowHigherThan);
        if (highers.Any())
        {
            foreach (var result in highers)
            {
                ResultsText.text += String.Format("{0:0.000}%", result.Value) + "\t: " + result.Key + "\n";
            }
        }
    }
}
