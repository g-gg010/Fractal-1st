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
    public float firstlength = 10f;
    public float scale = 0.5f;
    public float Kakuritu;
    public float angleX;
    public float angleY;
    public float angleZ;
    public string ruleF1 = "F^[+F[/+F][-F]]^[+F[/+F][-F]]^[+F[/+F][-F]]";
    public string ruleF2 = "F^[[+F^F^F]^[+F^F^F]^[+F^F^F]]";
    public string ruleX;

    public LineRenderer branchPrefab;
    Stack<TransformInfo> transformStack;
    Vector3 currentPosition;
    Quaternion currentRotation;    
    string path = "F";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        path = makeString("X");
        DrawLsystem(path, firstlength, scale);

        Debug.Log(path);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string makeString(string baseS)
    {
        if(JP < 50)
        {
            ruleX += "F";
            firstlength = 15f;
            scale = 0.5f;
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

    public void DrawLsystem(string lSystemString, float length, float scale)
    {
        transformStack = new Stack<TransformInfo>();
        currentPosition = transform.position;
        currentRotation = transform.rotation;

        foreach(char c in lSystemString){
            switch (c)
            {
                case 'F':
                //線を引いて進
                // direction は「どれくらい進むか」のベクトル
                Vector3 nextPosition = currentPosition + (currentRotation * Vector3.up * length);

                LineRenderer line = Instantiate(branchPrefab);
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

                length *= scale;
                break;

                case ']':
                // スタックから前の状態を取り出して戻る（ポップ）
                TransformInfo savedInfo = transformStack.Pop();
                currentPosition = savedInfo.position;
                currentRotation = savedInfo.rotation;
                length = savedInfo.length;
                break;

                case '+':
                currentRotation *= Quaternion.Euler(angleX, 0, 0);
                break;

                case '-':
                currentRotation *= Quaternion.Euler(-angleX, 0, 0);
                break;

                case '&':
                currentRotation *= Quaternion.Euler(0, angleY, 0);
                break;

                case '^':
                currentRotation *= Quaternion.Euler(0, -angleY, 0);
                break;

                case '/':
                currentRotation *= Quaternion.Euler(0, 0, angleZ);
                break;

                case '!':
                currentRotation *= Quaternion.Euler(0, 0, -angleZ);
                break;
                
            }
        }
    }

    public string makeChaos(string s)
    {
        StringBuilder nextGen = new StringBuilder();
        foreach(char c in s)
        {
            if(c == 'F')
            {
                float probability = Random.value;
                if(probability < (float)SN / 100f)
                {
                    nextGen.Append("/F");
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
        }

        s = nextGen.ToString();
        return s;
    }


}
