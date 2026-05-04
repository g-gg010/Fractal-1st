using UnityEngine;
using System.Collections.Generic;


public struct TransformInfo {
    public Vector3 position;
    public Quaternion rotation;
    public float length;
}
public class FractalGenerator : MonoBehaviour
{
    public int depth;
    public float angle;

    public float firstlength = 10f;
    public float scale = 0.7f;
    public LineRenderer branchPrefab;
    Stack<TransformInfo> transformStack;
    Vector3 currentPosition;
    Quaternion currentRotation;    
    string path = "F";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        path = makeString("F", "F/[&^^F]");
        DrawLsystem(path, firstlength, scale);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string makeString(string baseS, string ruleF)
    {
        for(int i = 0; i < depth; i++)
        {
            baseS = baseS.Replace("F", ruleF);
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
                currentRotation *= Quaternion.Euler(angle, 0, 0);
                break;

                case '-':
                currentRotation *= Quaternion.Euler(-angle, 0, 0);
                break;

                case '&':
                currentRotation *= Quaternion.Euler(0, angle, 0);
                break;

                case '^':
                currentRotation *= Quaternion.Euler(0, -angle, 0);
                break;

                case '/':
                currentRotation *= Quaternion.Euler(0, 0, angle);
                break;

                case '!':
                currentRotation *= Quaternion.Euler(0, 0, -angle);
                break;
                
            }
        }
    }
}
