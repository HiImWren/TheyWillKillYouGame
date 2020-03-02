using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MoveLook : MonoBehaviour
{
    public float Speed;
    public float TurnSpeed;
    public Camera cam;
    private Rigidbody body;
    public static MoveLook instance;
    public GameObject Laser;

    public int hp;
    public int maxHp;
    public Image healthBar;
    public AudioClip laser;
    public ParticleSystem part;



    public void kill()
    {
        // uwu no kill yet
        SceneManager.LoadScene(0);
    }

    private void OnEnable()
    {
        hp = maxHp;
    }

    public void damage(int am)
    {
        part.Emit(20);
        hp -= am;
        if (hp <= 0)
        {
            kill();
        }
        
    }


    private void Awake()
    {
        instance = this;
        body = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Vector2 InputVec;
        InputVec = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        InputVec.Normalize();
        InputVec *= Speed;
        body.velocity = new Vector3(InputVec.x, body.velocity.y, InputVec.y);
        RaycastHit hit;
        if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hit, 50))
        {
            
            transform.rotation = Quaternion.LookRotation(Vector3.Scale(transform.position - hit.point, new Vector3(1, 0, 1)).normalized, Vector3.up);
        }
        if (Input.GetMouseButtonDown(0))
        {
            Laser.SetActive(true);
            if (Physics.Raycast(transform.position, -transform.forward, out hit))
            {
                EnemySpawner.instance.audio.PlayOneShot(laser,.4f);
                if (hit.collider.GetComponent<Enemy>() != null)
                {
                    hit.collider.GetComponent<Enemy>().damage(1);
                }
            }
        }
        else
        {
            Laser.SetActive(false);

        }
        healthBar.fillAmount = hp / (float)maxHp;
        if (transform.position.y < -15)
        {
            kill();
        }

        if(Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.K))
        {
            var x = GameObject.FindObjectsOfType<Enemy>();
            foreach (var item in x)
            {
                item.damage(1000);
            }
        }
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.L))
        {
            EnemySpawner.instance.advanceWave();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        RaycastHit hit;
        if(Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition),out hit,50))
        {

            Gizmos.DrawSphere(hit.point, 1);
        }

        if (Input.GetKey(KeyCode.Q))
        {
            Gizmos.DrawCube(transform.position-transform.forward*1,new Vector3(1,1,1));
        }
        if (Input.GetKey(KeyCode.E))
        {
            Gizmos.DrawRay(transform.position,-transform.forward);
        }
    }
}
