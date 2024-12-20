using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    public Transform targetTransform; // Hedefin Transform bileşeni (karakter vs.)
    public float rotationSpeed = 5f;  // Rotasyon hızını ayarlamak için bir değişken

    void Update()
    {
        if (targetTransform != null)
        {
            // Kameranın hedefe bakması için yön vektörü oluştur
            Vector3 direction = targetTransform.position - transform.position;


            if (direction != Vector3.zero)
            {
                // Hedef yönüne doğru rotayı hesapla
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                // Rotasyonu yumuşatarak uygulamak için Slerp kullan
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            }
        }
    }
}

