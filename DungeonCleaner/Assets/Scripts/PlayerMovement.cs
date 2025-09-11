using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class PlayerMovement : MonoBehaviour
{
    public VirtualJoystick joystick;
    public float speed = 3f;

    private void Update()
    {
        Vector2 input = new Vector2(joystick.Input.x, joystick.Input.y); 
        Vector3 move = new Vector3(input.x, 0, input.y); 
        transform.position += move * speed * Time.deltaTime;
    }
}
