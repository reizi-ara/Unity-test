using UnityEngine;
using UnityEngine.UI;

public class UniLightsMain : MonoBehaviour
{

    [SerializeField]
    int minSize = 3;
    [SerializeField]
    int maxSize = 9;
    [SerializeField]
    int randomCount = 5;
    [SerializeField]
    GameObject lightPrefab;
    [SerializeField]
    Transform lightParent;
    [SerializeField]
    GridLayoutGroup grid;
    [SerializeField]
    Color onButtonColor;
    [SerializeField]
    Color onButtonHighlightedColor;
    [SerializeField]
    Color offButtonColor;
    [SerializeField]
    Color offButtonHighlightedColor;
    [SerializeField]
    Text clearText;

    bool[,] lightStatus;
    GameObject[,] lightObjects;

    //ランダムに問題を生成する処理
    public void CreateProblem()
    {
        clearText.enabled = false;

        foreach (GameObject light in lightObjects)
        {
            Button button = light.GetComponent<Button>();
            button.interactable = true;
        }

        lightStatus = new bool[lightStatus.GetLength(0), lightStatus.GetLength(1)];

        int roopCount = Random.Range(2, lightStatus.Length);

        randomCount = Mathf.Max(randomCount, 1);

        for (int i = 0; i < roopCount; i++)
        {
            int choosedRow = 0;
            int choosedCol = 0;

            //乱数に「コク」を加える
            for (int j = 0; j < randomCount; j++)
            {
                choosedRow += Random.Range(0, lightStatus.GetLength(0));
                choosedCol += Random.Range(0, lightStatus.GetLength(1));
            }

            choosedRow /= randomCount;
            choosedCol /= randomCount;

            SwitchLights(choosedRow, choosedCol, true);
        }

        SetLightColor();
    }

    //ライトを生成する処理
    public void CreateLights(int size)
    {
        size = Mathf.Clamp(size, minSize, maxSize);

        clearText.enabled = false;
        lightStatus = new bool[size, size];
        lightObjects = new GameObject[size, size];
        grid.constraintCount = size;

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                GameObject button = Instantiate(lightPrefab);
                button.transform.SetParent(lightParent);
                button.transform.localScale = transform.lossyScale;

                LightButton light = button.GetComponent<LightButton>();
                light.row = i;
                light.col = j;

                lightObjects[i, j] = button;
            }
        }

        CreateProblem();
    }

    public void ClearLights()
    {
        if (lightObjects == null || lightObjects.Length <= 0)
        {
            return;
        }

        foreach (GameObject buttonObj in lightObjects)
        {
            Destroy(buttonObj);
        }
    }

    //ライトを押したときの処理
    public void SwitchLights(int row, int col, bool createMode)
    {
        if (lightStatus == null || lightStatus.Length <= 0)
        {
            return;
        }

        row = Mathf.Clamp(row, 0, lightStatus.GetLength(0));
        col = Mathf.Clamp(col, 0, lightStatus.GetLength(1));

        //その要素自身
        lightStatus[row, col] = !lightStatus[row, col];

        //上
        if (row - 1 >= 0)
        {
            lightStatus[row - 1, col] = !lightStatus[row - 1, col];
        }

        //下
        if (row + 1 < lightStatus.GetLength(0))
        {
            lightStatus[row + 1, col] = !lightStatus[row + 1, col];
        }

        //左
        if (col - 1 >= 0)
        {
            lightStatus[row, col - 1] = !lightStatus[row, col - 1];
        }

        //右
        if (col + 1 < lightStatus.GetLength(1))
        {
            lightStatus[row, col + 1] = !lightStatus[row, col + 1];
        }

        if (!createMode)
        {
            SetLightColor();
            CheckClear();
        }
    }

    //クリア判定
    void CheckClear()
    {
        if (lightStatus == null || lightStatus.Length <= 0)
        {
            return;
        }

        foreach (bool status in lightStatus)
        {
            if (status == true)
            {
                return;
            }
        }

        foreach (GameObject light in lightObjects)
        {
            Button button = light.GetComponent<Button>();
            button.interactable = false;
        }

        clearText.enabled = true;
    }

    //ライトの色設定処理
    void SetLightColor()
    {
        if (lightStatus == null || lightStatus.Length <= 0 || lightObjects == null || lightObjects.Length <= 0)
        {
            return;
        }

        Button button;

        for (int i = 0; i < lightStatus.GetLength(0); i++)
        {
            for (int j = 0; j < lightStatus.GetLongLength(1); j++)
            {
                button = lightObjects[i, j].GetComponent<Button>();
                ColorBlock colorBlock = button.colors;

                if (lightStatus[i, j])
                {
                    colorBlock.normalColor = onButtonColor;
                    colorBlock.pressedColor = onButtonColor;
                   // colorBlock.selectedColor = onButtonColor;
                    colorBlock.highlightedColor = onButtonHighlightedColor;
                }
                else
                {
                    colorBlock.normalColor = offButtonColor;
                    colorBlock.pressedColor = offButtonColor;
                    //colorBlock.selectedColor = offButtonColor;
                    colorBlock.disabledColor = offButtonColor;
                    colorBlock.highlightedColor = offButtonHighlightedColor;
                }

                button.colors = colorBlock;
                button.interactable = true;
            }
        }
    }

}