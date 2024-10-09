using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monter_die : MonoBehaviour
{
    public GameObject itemPrefab;  // Vật phẩm sẽ rơi ra
    public float dropChance = 0.5f;  // Xác suất rơi (0.5 = 50%)

    // Hàm xử lý khi quái chết
    public void Die()
    {
        DropItem();
        Destroy(gameObject);  // Xóa quái khỏi cảnh
    }

    // Hàm rơi vật phẩm
    void DropItem()
    {
        // Kiểm tra xem itemPrefab có được gán hay không
        if (itemPrefab == null)
        {
            Debug.LogWarning("itemPrefab chưa được gán!");
            return; // Ngăn không cho tiếp tục nếu itemPrefab chưa được gán
        }

        float randomValue = Random.Range(0f, 1f);  // Sinh số ngẫu nhiên từ 0 đến 1
        if (randomValue <= dropChance)
        {
            Instantiate(itemPrefab, transform.position, Quaternion.identity);  // Tạo vật phẩm tại vị trí của quái
        }
}

}
