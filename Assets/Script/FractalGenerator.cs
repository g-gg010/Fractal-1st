using UnityEngine;
using System.Collections.Generic;
using System.Text;


public struct TransformInfo {
    public Vector3 position;
    public Quaternion rotation;
    public float length;
}
public class FractalGenerator : MonoBehaviour
{
    public int EI;
    public int SN;
    public int TF;
    public int JP;
    public int depth;
    public float length = 10f;
    public float lengthscale = 0.8f;
    public float widthscale;
    public float Kakuritu;
    public float angleX;
    public float angleY;
    public float angleZ;
    public float basewidth = 1f;
    public Color mbticolor;
    public Color mbticolor1;
    public string ruleF1 = "F^[+F[/+F][-F]]^[+F[/+F][-F]]^[+F[/+F][-F]]";
    public string ruleF2 = "F^[[+F^F^F]^[+F^F^F]^[+F^F^F]]";
    public string ruleX;

    public LineRenderer branchPrefab;
    Stack<TransformInfo> transformStack;
    Vector3 currentPosition;
    Quaternion currentRotation;    
    public string path = "F";
    private GameObject treevase;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        treevase = new GameObject("treevase");
        treevase.transform.position = transform.position;
        //path = makeString("X");
        //path = makeChaos(path);
        //DrawLsystem(path);
        //Debug.Log(path);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string makeString(string baseS)
    {
        if(EI < 50)
        {
            ruleF1 = "F[+F]";
            ruleF2 = "F[-F]";
            ruleX = "F^[+X]^[+X]^[+X]";
        }
        else
        {
            ruleF1 = "F[+F][+F]";
            ruleF2 = "F";
            ruleX = "F[^X][!&X][/&X]";
        }

        if(JP < 50)
        {
            ruleX += "X";
            length = 15f;
            lengthscale = 0.6f;
            Debug.Log("aaa");
        }

        for (int i = 0; i < depth; i++)
        {
            // 新しい世代の文字列を貯めるためのバッファ
            StringBuilder nextGen = new StringBuilder();
            // 今の文字列を1文字ずつチェック
            foreach (char c in baseS)
            {
                if (c == 'F')
                {
                    //Fを見つけるたびに抽選！
                    float probability = Random.value;
                    //Debug.Log(probability);

                    if (probability < Kakuritu)
                    {
                        nextGen.Append(ruleF1);
                    }
                    else
                    {
                        nextGen.Append(ruleF2);
                    }
                }
                else if(c == 'X')
                {
                    nextGen.Append(ruleX);
                }
                else
                {
                    // F以外の記号はそのままコピー
                    nextGen.Append(c);
                }
            }

            // 1世代分をすべて書き換えたら、baseSを更新して次の深さへ
            baseS = nextGen.ToString();
        }
        return baseS;
    }

    public void DrawLsystem(string lSystemString)
    {
        int currentDepth = 1;
        transformStack = new Stack<TransformInfo>();
        currentPosition = transform.position;
        currentRotation = transform.rotation;

        //TFの処理--------------------------
        Kakuritu = (float)TF / 200f + 0.5f;
        float variationX = (float)TF / 10;
        float variationY = (float)TF / 3;
        float variationZ = (float)TF / 20;
        //TFの処理終わり---------------------
        foreach(char c in lSystemString){
            switch (c)
            {
                case 'F':
                //線を引いて進む
                Vector3 nextPosition = currentPosition + (currentRotation * Vector3.up * length);

                LineRenderer line = Instantiate(branchPrefab, treevase.transform);

                //線の色、太さを決定する。
                SetLineColorandWidth(line, currentDepth);
                //set startPos
                line.SetPosition(0, currentPosition);
                //set endPos
                line.SetPosition(1, nextPosition);
                currentPosition = nextPosition;

                break;

                case'[':
                //今の状態をスタックに保存
                TransformInfo info = new TransformInfo {
                    position = currentPosition,
                    rotation = currentRotation,
                    length = length
                };
                transformStack.Push(info);

                length *= lengthscale;
                currentDepth++;
                break;

                case ']':
                // スタックから前の状態を取り出して戻る（ポップ）
                TransformInfo savedInfo = transformStack.Pop();
                currentPosition = savedInfo.position;
                currentRotation = savedInfo.rotation;
                length = savedInfo.length;
                currentDepth--;
                break;

                case '+':
                float rand = Random.value;
                currentRotation *= Quaternion.Euler(angleX + (rand * 2f - 1f) * variationX, 0, 0);
                break;

                case '-':
                rand = Random.value;
                currentRotation *= Quaternion.Euler(-angleX + (rand * 2f - 1f) * variationX, 0, 0);
                break;

                case '&':
                rand = Random.value;
                currentRotation *= Quaternion.Euler(0, angleY + (rand * 2f - 1f) * variationY, 0);
                break;

                case '^':
                rand = Random.value;
                currentRotation *= Quaternion.Euler(0, -angleY + (rand * 2f - 1f) * variationY, 0);
                break;

                case '/':
                rand = Random.value;
                currentRotation *= Quaternion.Euler(0, 0, angleZ + (rand * 2f - 1f) * variationZ);
                break;

                case '!':
                rand = Random.value;
                currentRotation *= Quaternion.Euler(0, 0, -angleZ + (rand * 2f - 1f) * variationZ);
                break;
                
            }
        }
    }

    public string makeChaos(string s)
    {
        if(SN < 50) SN = SN / 3;
        StringBuilder nextGen = new StringBuilder();
        int count = 0; //最初に曲がるのを防ぐ。
        foreach(char c in s)
        {
            if(count == 0)
            {
                count++;
                nextGen.Append(c);
                continue;
            }

            if(c == 'F')
            {
                float probability = Random.value;
                if(probability < ((float)SN / 150f)/2)
                {
                    nextGen.Append("/F");
                }
                else if(probability < (float)SN / 150f)
                {
                    nextGen.Append("!F");
                }
                else
                {
                    nextGen.Append("F");
                }
            }
            else
            {
                nextGen.Append(c);
            }
            count++;
        }

        s = nextGen.ToString();

        //ひねるを大きく
        angleZ += SN/20f;
        return s;
    }

    void SetLineColorandWidth(LineRenderer line, int currentDepth)
    {
        //太さを決定する。
        float width = basewidth * Mathf.Pow(widthscale, currentDepth-1);
        if(currentDepth == 1) width *= 2.0f;
        line.startWidth = width;
        line.endWidth = width * widthscale;

        //色を決定する。
        line.startColor = mbticolor;
        line.endColor = mbticolor1;

        //影を落とし、他の枝の影を受ける。
        line.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        line.receiveShadows = true;
    }

    public void TreeReset()
    {
        if(treevase != null)
        {
            Destroy(treevase);
        }

        treevase = new GameObject("treease");
        treevase.transform.position = transform.position;
    }

    

}
