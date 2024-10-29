using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    public Button button1;
    public Button button2;
    public Button button3;
    public Button button4;

    // 用于存储按钮2、3、4的状态
    private bool[] buttonStates;

    private void Start()
    {
        button1.onClick.AddListener(OnButton1Click);
        // 初始化按钮状态数组
        buttonStates = new bool[]
        {
            button2.interactable,
            button3.interactable,
            button4.interactable
        };
    }

    private void OnButton1Click()
    {
        for (int i = 0; i < 3; i++)
        {
            Button button;
            switch (i)
            {
                case 0:
                    button = button2;
                    break;
                case 1:
                    button = button3;
                    break;
                case 2:
                    button = button4;
                    break;
                default:
                    button = null;
                    break;
            }

            // 切换按钮的可交互性
            button.interactable = !button.interactable;
            // 根据可交互性切换按钮的可见性
            button.gameObject.SetActive(button.interactable);
            // 更新按钮状态数组
            buttonStates[i] = button.interactable;
        }
    }
}