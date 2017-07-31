using UnityEngine;

public class CameraScript : MonoBehaviour
{

    public float CameraSpeed = 1;
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(Vector2.up * CameraSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(Vector2.down * CameraSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.RightArrow))
	    {
            transform.Translate(Vector2.right * CameraSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(Vector2.left * CameraSpeed * Time.deltaTime);
        }
    }
}
