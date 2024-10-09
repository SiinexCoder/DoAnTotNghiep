using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monter : MonoBehaviour
{
    public float speed = 2.5f;  // Tốc độ di chuyển của quái
    private Transform player;    // Đối tượng player mà quái sẽ đuổi theo

    [System.Serializable]
    public class DropItem
    {
        public GameObject itemPrefab;  // Vật phẩm sẽ rơi ra
        public float dropChance;       // Xác suất rơi vật phẩm (0.5 = 50%)
    }

    public DropItem[] dropItems;  // Danh sách các vật phẩm có thể rơi
    public float dropRadius = 1.0f;  // Bán kính ngẫu nhiên của vị trí rơi

    void Start()
    {
        // Tìm đối tượng có tag "Player" khi game bắt đầu
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        // Kiểm tra nếu tìm thấy đối tượng player
        if (playerObject != null)
        {
            player = playerObject.transform; // Lưu vị trí của đối tượng player
        }

        // Sau 5 giây kể từ khi đối tượng được kích hoạt, tự động hủy
        Destroy(gameObject, 5f);
    }

    void Update()
    {
        // Kiểm tra xem player có được tìm thấy không
        if (player != null)
        {
            // Tính toán hướng đến player và di chuyển quái vật về phía player
            Vector2 direction = (player.position - transform.position).normalized;
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);

            // Gọi hàm LookAtPlayer để đảm bảo quái vật luôn hướng về phía player
            LookAtPlayer();
        }
    }

    public void LookAtPlayer()
    {
        // Tính toán hướng đến player (chỉ quan tâm đến trục X)
        Vector2 direction = player.position - transform.position;

        // Xoay quái vật để hướng về phía player theo chiều ngang (chỉ xoay theo trục X)
        if (direction.x > 0 && transform.localScale.x < 0)
        {
            // Nếu player ở bên phải và quái đang lật, lật lại
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (direction.x < 0 && transform.localScale.x > 0)
        {
            // Nếu player ở bên trái và quái không lật, lật lại
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    // Hàm xử lý khi quái chết
    void OnDestroy()
    {
        DropItems();  // Gọi hàm rơi vật phẩm khi quái chết
    }

    // Hàm rơi vật phẩm
    void DropItems()
    {
        foreach (DropItem dropItem in dropItems)
        {
            // Kiểm tra xem itemPrefab có được gán hay không
            if (dropItem.itemPrefab == null)
            {
                Debug.LogWarning("Một itemPrefab chưa được gán!");
                continue; // Bỏ qua nếu itemPrefab chưa được gán
            }

            // Sinh số ngẫu nhiên từ 0 đến 1
            float randomValue = Random.Range(0f, 1f);

            // Nếu giá trị ngẫu nhiên nhỏ hơn hoặc bằng tỷ lệ rơi, tạo ra vật phẩm
            if (randomValue <= dropItem.dropChance)
            {
                // Tính toán vị trí ngẫu nhiên trong bán kính xung quanh quái
                Vector2 randomPosition = (Vector2)transform.position + Random.insideUnitCircle * dropRadius;

                // Tạo vật phẩm tại vị trí ngẫu nhiên
                Instantiate(dropItem.itemPrefab, randomPosition, Quaternion.identity);  
            }
        }
    }
}
