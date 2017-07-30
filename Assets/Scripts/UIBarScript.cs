using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBarScript : MonoBehaviour
{

    public float MaximumValue;
    public RectTransform Bar;
    public Text BarText;

    private float _originalX;
    private float _originalY;
    private float _originalWidth;
    private float _originalHeight;

    public void SetBar(float currentValue)
    {
        float percentage = currentValue / MaximumValue;

        Bar.localScale = new Vector3(percentage, 1);
        //-(_originalWidth*(1/percentage)/2)
       
        Bar.localPosition = new Vector3(((_originalWidth / 2)  * percentage) - (_originalWidth / 2), 0);

        BarText.text = currentValue + "/" + MaximumValue;
    }

    public void SetBar(float currentValue, float maxValue)
    {
        MaximumValue = maxValue;
        SetBar(currentValue);
    }

    // Use this for initialization
    void Start ()
	{
	    _originalX = Bar.rect.x;
	    _originalY = Bar.rect.y;
	    _originalWidth = Bar.rect.width;
	    _originalHeight = Bar.rect.height;


	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
