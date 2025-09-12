using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class PlayerMovement : MonoBehaviour
{
    public Transform player;
    public VirtualJoystick joystick;
    public float speed = 3f;

    private Animation anim;

    private void Awake()
    {
        anim = GetComponentInChildren<Animation>();
        anim.wrapMode = WrapMode.Loop;
    }

    private void Update()
    {
        Vector2 input = new Vector2(joystick.Input.x, joystick.Input.y); 
        Vector3 move = new Vector3(input.x, 0, input.y); 
        transform.position += move * speed * Time.deltaTime;

        if (move != Vector3.zero)
        {
            player.rotation = Quaternion.LookRotation(move);

            if (!anim.IsPlaying("Run"))
            {
                anim.Play("Run");
            }
        }
        else
        {
            if (!anim.IsPlaying("Idle"))
            {
                anim.CrossFade("Idle", 0.3f);
            }
        }
    }
}
