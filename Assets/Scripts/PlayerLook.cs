using UnityEngine;
using UnityEngine.UIElements;
public class PlayerLook : MonoBehaviour
{
    float mouseSense = 0.5f;
    [SerializeField] GameObject guns;
    GameObject enemy;
    void Update()
    {
        foreach (var touch in Input.touches)
        {
            if (touch.position.x >= Screen.width / 2)
            {
                float rotateX = touch.deltaPosition.x * mouseSense;
                float rotateY = touch.deltaPosition.y * mouseSense;

                Vector3 rotPlayer = transform.rotation.eulerAngles;
                Vector3 rotGuns = guns.transform.rotation.eulerAngles;

                rotGuns.x -= rotateY;
                rotGuns.y += rotateX;
                rotPlayer.y += rotateX;

                transform.rotation = Quaternion.Euler(rotPlayer);
                guns.transform.rotation = Quaternion.Euler(rotGuns);
                if (enemy != null)
                {
                    enemy.transform.Translate(-rotateX / 3, rotateY / 3, 0);
                }
            }
        }
    }
    public void FindEnemy(GameObject other)
    {
        enemy = other;
    }
    public void ChangeMouseSensivity(float count)
    {
        mouseSense = count;
    }
}