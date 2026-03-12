using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Transform previousRoom;
    [SerializeField] private Transform nextRoom;
    [SerializeField] private CameraController cam;
    private bool isUsed;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isUsed = true;

            if (collision.transform.position.x < transform.position.x)
                cam.MovetoNewRoom(nextRoom);
            else
                cam.MovetoNewRoom(previousRoom);
        }
        if (isUsed) return;
    }
}
