using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;  // Hareket hızı
    private CharacterController characterController;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        // Yatay ve dikey girişleri al
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Kameranın yönüne göre hareket yönünü ayarla
        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;

        // Kameranın yukarı eksenindeki bileşeni kaldır
        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        // Hareket yönü
        Vector3 moveDirection = (forward * vertical + right * horizontal).normalized;

        // Karakteri hareket ettir (sadece pozisyon değiştir)
        characterController.Move(moveDirection * moveSpeed * Time.deltaTime);
    }
}
