using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject uiPanel;
    [SerializeField] private GameObject resetPanel;
    [SerializeField] private Slider EISlider;
    [SerializeField] private Slider SNSlider;
    [SerializeField] private Slider TFSlider;
    [SerializeField] private Slider JPSlider;

    [SerializeField] private TextMeshProUGUI EIText;

    [SerializeField] private TextMeshProUGUI SNText;

    [SerializeField] private TextMeshProUGUI TFText;

    [SerializeField] private TextMeshProUGUI JPText;
    [SerializeField] private MonoBehaviour camerascript;


    private int clickcount;
    private bool isUIopen = true;
    public FractalGenerator fractalGenerator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        clickcount = 0;
        isUIopen = true;
        uiPanel.SetActive(isUIopen);
        resetPanel.SetActive(!isUIopen);
        UpdateSliderValue(EIText, EISlider.value, ref fractalGenerator.EI);
        UpdateSliderValue(SNText, SNSlider.value, ref fractalGenerator.SN);
        UpdateSliderValue(TFText, TFSlider.value, ref fractalGenerator.TF);
        UpdateSliderValue(JPText, JPSlider.value, ref fractalGenerator.JP);
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            
        }
        UpdateSliderValue(EIText, EISlider.value, ref fractalGenerator.EI);
        UpdateSliderValue(SNText, SNSlider.value, ref fractalGenerator.SN);
        UpdateSliderValue(TFText, TFSlider.value, ref fractalGenerator.TF);
        UpdateSliderValue(JPText, JPSlider.value, ref fractalGenerator.JP);
    }
    public void UpdateSliderValue(TextMeshProUGUI text, float value,ref int mbti)
    {
        // 1. テキストに値を表示 (小数点以下1桁まで)
        if (text != null)
        {
            text.text = value.ToString();
        }    

        mbti = (int)value;
    }
    public void ClickButton()
    {
        if(clickcount >= 1) return;
        clickcount++;
        fractalGenerator.path = fractalGenerator.makeString("X");
        fractalGenerator.path = fractalGenerator.makeChaos(fractalGenerator.path, 1);
        fractalGenerator.DrawLsystem(fractalGenerator.path);
        //Debug.Log(fractalGenerator.path);
        //Debug.Log(fractalGenerator.EI);
        //Debug.Log(fractalGenerator.SN);
        //Debug.Log(fractalGenerator.TF);
        //Debug.Log(fractalGenerator.JP);
        OpenCloseSetting();
    }

    public void ClickresetButton()
    {
        OpenCloseSetting();
        fractalGenerator.TreeReset();
        clickcount--;
    }
    private void OpenCloseSetting()
    {
        isUIopen = !isUIopen;
        uiPanel.SetActive(isUIopen);
        resetPanel.SetActive(!isUIopen);

        if(camerascript != null)
        {
            camerascript.enabled = !isUIopen;
        }
    }
}
