using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Slider EISlider;
    [SerializeField] private Slider SNSlider;
    [SerializeField] private Slider TFSlider;
    [SerializeField] private Slider JPSlider;

    [SerializeField] private TextMeshProUGUI EIText;

    [SerializeField] private TextMeshProUGUI SNText;

    [SerializeField] private TextMeshProUGUI TFText;

    [SerializeField] private TextMeshProUGUI JPText;

    private int clickcount;
    public FractalGenerator fractalGenerator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        clickcount = 0;
        UpdateSliderValue(EIText, EISlider.value, fractalGenerator.EI);
        UpdateSliderValue(SNText, SNSlider.value, fractalGenerator.SN);
        UpdateSliderValue(TFText, TFSlider.value, fractalGenerator.TF);
        UpdateSliderValue(JPText, JPSlider.value, fractalGenerator.JP);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSliderValue(EIText, EISlider.value, fractalGenerator.EI);
        UpdateSliderValue(SNText, SNSlider.value, fractalGenerator.SN);
        UpdateSliderValue(TFText, TFSlider.value, fractalGenerator.TF);
        UpdateSliderValue(JPText, JPSlider.value, fractalGenerator.JP);
    }
    public void UpdateSliderValue(TextMeshProUGUI text, float value, int mbti)
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
        fractalGenerator.path = fractalGenerator.makeChaos(fractalGenerator.path);
        fractalGenerator.DrawLsystem(fractalGenerator.path);
    }
}
