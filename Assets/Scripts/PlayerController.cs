using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class PlayerController : MonoBehaviour
{
    [SerializeField] TMP_Text HpText;
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject rifleStart;
    [SerializeField] float range = 100f;
    [SerializeField] float shootDelay;
    [SerializeField] float tf;
    [SerializeField] GameObject grenade;
    [HideInInspector] public bool isshoot;
    [SerializeField] AudioSource fire;
    [SerializeField] ParticleSystem flash;
    [SerializeField] GameObject impact;
    enum BulletType { Ray, Obj }
    [SerializeField] BulletType type;
    [SerializeField] TMP_Dropdown Dropdown;
    Animator animator;
    public bool dead;
    private float impactForce = 100f;
    public int coins;
    [SerializeField] TMP_Text coin_text;
    [SerializeField] GameObject questTarget;
    [SerializeField] Dialog dialogue;
    [SerializeField]
    int capacity

    {
        get
        {
            return _capacity;
        }
        set
        {
            _capacity = value;
            ammoText.text = ammo + "--" + capacity + "/" + maxcapacity;
        }
    }

    int _capacity;
    [SerializeField] int maxcapacity;
    [SerializeField] int ammo;
    [SerializeField] TMP_Text ammoText;




    private int health;
    public void ChangeHealth(int count)
    {

        health = health + count;
        HpText.text = health.ToString();
        if (health <= 0)
        {
            dead = true;
            this.enabled = false;

        }
    }
    public void AddCoin(int count)
    {
        coins += count;
        coin_text.text = "money: "+coins.ToString();
    }
    public static PlayerController instance;
    private void Awake()
    {
        if (instance ==  null) 
        {
            instance = this;

        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        health = 100;
        animator = GetComponent<Animator>();
        StartCoroutine("shoot");
        capacity = maxcapacity;
        PlayerPrefs.SetInt("item getting", 1);
        PlayerPrefs.SetInt("Item getting2", 2);
        AddCoin(1000);

    }
    public void AddAmmo(int count)
    {
        ammo += count;
        ammoText.text = ammo + "--" + capacity + "/" + maxcapacity;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Airdrop")
        {
            AddAmmo(100);
        }
        if (collision.collider.CompareTag("Item"))
        {

            if (PlayerPrefs.GetInt("item getting") == 2)
            {
                dialogue.target.transform.position = questTarget.transform.position;
                Destroy(collision.gameObject);
                PlayerPrefs.SetInt("item getting", 3);
            }
        }
       //if (collision.collider.CompareTag("Item2"))
       // {
       //     if (PlayerPrefs.GetInt("Item getting2") == 4)
       //     {
       //         dialogue.target.transform.position = questTarget.transform.position;
       //         Destroy(collision.gameObject);
       //         PlayerPrefs.SetInt("Item getting2", 5);
       //     }
       // }
            
            if (collision.gameObject.tag == "Airdrop2")
            {

                ChangeHealth(100);
                HpText.text = health.ToString();
            }
        }
        public void SayHello()
        {
            animator.SetTrigger("Hi");
        }
        public void HelloGuys(string say)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, 50);
            foreach (var people in colliders)
            {
                if (people.tag == "people")
                {
                    people.GetComponent<Animator>().SetTrigger("Hi");
                    print(say);
                }
            }
        }
        void Update()
        {
            RaycastHit hit; //объект, в который попал луч
            Ray ray = new Ray(Camera.main.gameObject.transform.position, Camera.main.gameObject.transform.forward); // направление луча вниз
            Debug.DrawRay(Camera.main.gameObject.transform.position, Camera.main.gameObject.transform.forward * 15, Color.red);
            if (Physics.Raycast(ray, out hit, 15)) // если луч задел коллайдер на расстоянии deploymentHeight
            {
                if (hit.collider.tag == "NPC")
                {
                    hit.transform.GetComponent<AIController>().Panic();
                }

            }
            //if (Input.GetKeyDown(KeyCode.Mouse0))
            //{
            //    StartCoroutine("shoot");

            //}
            //if (Input.GetKeyUp(KeyCode.Mouse0))
            //{
            //    StopCoroutine("shoot");
            ////}
            //if (Input.GetKeyDown(KeyCode.G)) 
            //{

            //}
            Debug.DrawRay(rifleStart.transform.position, rifleStart.transform.forward * range);

            if (Input.GetKeyDown(KeyCode.Escape))
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
            Application.Quit();
#endif


            }
        }

        IEnumerator shoot()
        {
            while (true)
            {
                yield return new WaitForSeconds(shootDelay);

                if (isshoot)
                {
                    if (capacity > 0)
                    {
                        capacity--;
                        switch (type)
                        {
                            case BulletType.Ray:
                                RaycastHit hit;
                                if (Physics.Raycast(rifleStart.transform.position, rifleStart.transform.forward, out hit, range))
                                {
                                    if (hit.collider.gameObject.tag == "NPC")
                                    {
                                        hit.collider.gameObject.GetComponent<AIController2>().ChangeHealth(-20);

                                    }
                                    if (hit.collider.gameObject.tag == "Enemy")

                                    {
                                        hit.collider.gameObject.GetComponent<Enemy>().ChangeHealth(-20);

                                    }
                                    if (hit.collider.gameObject.tag == "Player")
                                    {
                                        hit.collider.gameObject.GetComponent<PlayerController>().ChangeHealth(-20);
                                    }
                                    if (hit.collider.gameObject.tag == "RedBarrel")
                                    {
                                        hit.collider.gameObject.GetComponent<RedBarrel>().Boom();
                                    }
                                    if (hit.collider.gameObject.CompareTag("enemy_ragdoll"))
                                    {

                                        hit.collider.gameObject.gameObject.GetComponentInParent<Enemy>().ChangeHealth(-20);

                                        hit.collider.gameObject.GetComponent<Rigidbody>().AddForce(transform.TransformDirection(Vector3.forward) * 100, ForceMode.Impulse);
                                    }
                                    if (hit.rigidbody != null)
                                    {
                                        hit.rigidbody.AddForce(-hit.normal * impactForce);
                                    }
                                    //выводим в консоль имя объекта, в который попали 
                                    Debug.Log(hit.collider.name);
                                    GameObject inst = Instantiate(impact, hit.point, Quaternion.LookRotation(hit.normal));
                                    //Уничтожаем эффект через 0,5сек
                                    Destroy(inst, 0.5f);
                                }

                                flash.Play();

                                break;
                            case BulletType.Obj:
                                GameObject buffer = Instantiate(bullet);
                                buffer.GetComponent<Bullet>().SetDirection(transform.forward);
                                buffer.transform.position = rifleStart.transform.position;
                                buffer.transform.rotation = transform.rotation;
                                Destroy(buffer, 5);
                                break;
                        }



                        fire.Play();

                    }
                }
            }
        }
        public void Changed(int value)
        {
            if (value == 0)
            {
                type = BulletType.Ray;
            }
            else
            {
                type = BulletType.Obj;
            }
        }
        public void startShoot()
        {
            isshoot = true;
        }
        public void stopShoot()
        {
            isshoot = false;
        }
        public void reload()
        {
            int need = maxcapacity - capacity;
            if (need <= ammo)
            {
                ammo -= need;
                capacity = maxcapacity;
            }
            else
            {
                capacity += ammo;
                ammo = 0;
            }
        }


        public void throwgrenade()
        {
            GameObject newGrenade = Instantiate(grenade, transform.position + new Vector3(0.25f, 0.5f, 0f), Quaternion.identity);
            newGrenade.GetComponent<Rigidbody>().AddForce(transform.forward * tf, ForceMode.Impulse);
        }
    }