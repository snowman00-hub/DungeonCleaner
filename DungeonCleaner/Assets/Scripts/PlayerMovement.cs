using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class PlayerMovement : MonoBehaviour
{
    private static readonly string Run = "Run";
    private static readonly string Idle = "Idle";

    public Transform player;
    public VirtualJoystick joystick;
    public float speed = 3f;
    public float pickUpRadius = 2f;

    private Animation anim;

    private void Awake()
    {
        anim = GetComponentInChildren<Animation>();
        anim.wrapMode = WrapMode.Loop;
    }

    private void Update()
    {
        UpdateMove();
        PickUpNearbyItems();
    }

    private void PickUpNearbyItems()
    {
        Collider[] nearby = Physics.OverlapSphere(transform.position, pickUpRadius, LayerMask.GetMask(LayerName.PickUp));
        foreach (var pickup in nearby)
        {
            pickup.gameObject.GetComponent<PickUp>().Acquire(transform);            
        }
    }

    private void UpdateMove()
    {
        Vector2 input = new Vector2(joystick.Input.x, joystick.Input.y);
        Vector3 move = new Vector3(input.x, 0, input.y);
        transform.position += move * speed * Time.deltaTime;

        if (move != Vector3.zero)
        {
            player.rotation = Quaternion.LookRotation(move);

            if (!anim.IsPlaying(Run))
            {
                anim.Play(Run);
            }
        }
        else
        {
            if (!anim.IsPlaying(Idle))
            {
                anim.CrossFade(Idle, 0.3f);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tag.Exp))
        {
            var pickup = other.GetComponent<PickUp>();
            StageInfoManager.Instance.AddExp(pickup.value);
            pickup.OnUsed?.Invoke();
        }
    }
}
