using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SCP_106 : MonoBehaviour
{
    NavMeshAgent _navMeshagent;
    float PlayerDistance= 20, timer, ambianceTimer;
    public GameObject Player;
    bool isActive = false, playedHorror, usingAStar = true, isSpawn = false;
    Quaternion toAngle, realAngle;
    public float speed, spawntimer;
    AudioSource sfx;
    Vector3 Destination;
    int frameInterval=4;
    public AudioClip[] Horror, Sfx;
    public AudioClip music;
    public Animator anim;
    // Use this for initialization
    void Start()
    {
        Player = GameController.instance.player;
        _navMeshagent = this.GetComponent<NavMeshAgent>();
        sfx = GetComponent<AudioSource>();
        _navMeshagent.enabled = false;
    }

    void Update()
    {
        if (isActive)
        {
            if (Time.frameCount % frameInterval == 0)
                PlayerDistance = (Vector3.Distance(Player.transform.position, transform.position));

            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                isSpawn = true;
            }


            if (PlayerDistance > 15)
            {
                playedHorror = false;
            }
            if (PlayerDistance > 3.5f)
            {
                usingAStar = true;
            }
            else
                usingAStar = false;

            DoSFX();


            if (isSpawn)
            {
                HorrorPlay();
                if (usingAStar)
                {
                    SetDestination();
                    _navMeshagent.enabled = true;
                }
                else
                {
                    Vector3 Point = new Vector3(Player.transform.position.x, transform.position.y, Player.transform.position.z) - transform.position;
                    toAngle = Quaternion.LookRotation(Point);
                    realAngle = Quaternion.LookRotation(new Vector3(Player.transform.position.x, Player.transform.position.y - 0.4f, Player.transform.position.z) - transform.position);
                    _navMeshagent.enabled = false;

                    transform.position += (realAngle * (Vector3.forward * speed)) * Time.deltaTime;
                    transform.rotation = Quaternion.Lerp(transform.rotation, toAngle, 0.1f);

                }
            }
        }
    }


    void DoSFX()
    {

        ambianceTimer -= Time.deltaTime;
        if (ambianceTimer <= 0)
        {
            sfx.PlayOneShot(Sfx[Random.Range(1, Sfx.Length)]);
            ambianceTimer = 2 * Random.Range(1, 5);
        }
    }

    void HorrorPlay()
    {
        if (PlayerDistance < 16 && PlayerDistance > 4)
        {
                if (playedHorror == false)
                {
                    GameController.instance.PlayHorror(Horror[Random.Range(0, Horror.Length)]);
                    playedHorror = true;
                }
        }
    }

    public void UnSpawn()
    {
        _navMeshagent.enabled = false;
        transform.position = (new Vector3(0, -10, 0));
        isActive = false;
        isSpawn = false;
    }

    public void Spawn(Vector3 here)
    {
        anim.SetTrigger("spawn");
        transform.position = here;
        _navMeshagent.enabled = true;
        _navMeshagent.Warp(here);
        isActive = true;
        sfx.PlayOneShot(Sfx[0]);
        timer = spawntimer;
        GameController.instance.ChangeMusic(music);
    }


    private void SetDestination()
    {
      if (Time.frameCount % frameInterval == 0)
      {
            _navMeshagent.SetDestination(Player.transform.position);
      }

    }


}
