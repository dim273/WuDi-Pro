using TMPro;
using UnityEngine;

public class UI_Tooltip : MonoBehaviour
{
    [SerializeField] private float xLimit = 960;
    [SerializeField] private float yLimit = 540;

    [SerializeField] private float xOffset = 100;
    [SerializeField] private float yOffset = 100;

    //设置toolTip位置函数
    public virtual void AdjustPosition()
    {
        Vector2 mousePos = Input.mousePosition;
        float newXoffset = 0;
        float newYoffset = 0;

        if (mousePos.x > xLimit)
            newXoffset = -xOffset;
        else
            newXoffset = xOffset;

        if (mousePos.y > yLimit)
            newYoffset = -yOffset;
        else
            newYoffset = yOffset;

        transform.position = new Vector2(newXoffset + mousePos.x, newYoffset + mousePos.y);
    }

    //调节文字大小
    public void AdjustFontSize(TextMeshProUGUI _text)
    {
        if(_text.text.Length > 12)
        {
            _text.fontSize = _text.fontSize * .8f;
        }
    }
}
