using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SCP_106 : MonoBehaviour
{
    NavMeshAgent _navMeshagent;
    float PlayerDistance= 20;
    public GameObject Player;
    bool isActive = false, playedHorror;
    AudioSource sfx;
    Vector3 Destination;
    int frameInterval=5;
    public AudioClip[] Horror;

    // Use this for initialization
    void Awake()
    {
        mainCamera = Camera.main;
        Player = GameController.instance.player;
        Destination = transform.position;


        _navMeshagent = this.GetComponent<NavMeshAgent>();
        sfx = GetComponent<AudioSource>();
    }

    void Start()
    {
        _navMeshagent.Warp(transform.position);
        sfx.Pause();
    }

    void Update()
    {
        if (Time.frameCount % frameInterval == 0)
            PlayerDistance = (Vector3.Distance(Player.transform.position, transform.position));

        if (PlayerDistance > 15)
        {
            playedHorror = false;
        }

        if (isActive)
        {
                    if (Time.frameCount % frameInterval == 0)
                        SetDestination();

                    if (PlayerDistance < 20f)
                        _navMeshagent.speed = 25;
                    else
                        _navMeshagent.speed = 10;
                    _navMeshagent.isStopped = false;
                    sfx.UnPause();
                    HorrorNear();

                }
                else
                {
                    _navMeshagent.speed = 0;
                    _navMeshagent.isStopped = true;
                    sfx.Pause();
                }
            }
            else
            {
                _navMeshagent.speed = 0;
                _navMeshagent.isStopped = true;
                sfx.Pause();
                HorrorFar();
                
                
            }
        }
        else
            sfx.Pause();
    }


    void HorrorFar()
    {
        if (PlayerDistance < 16 && PlayerDistance > 4 && CheckPlayer())
        {
                playedNear = false;
                if (playedHorror == false)
                {
                    GameController.instance.PlayHorror(farHorror[Random.Range(0, farHorror.Length)]);
                    playedHorror = true;
                }
        }
    }

    private void SetDestination()
    {
      if (Time.frameCount % frameInterval == 0
      {
            Destination = Player.transform.position;
            _navMeshagent.SetDestination(Destination);
      }

    }

    
    }

    public void WarpMe(bool beActive, Vector3 warppoint)
    {
        _navMeshagent.Warp(warppoint);
        canAttack = beActive;
    }


}
