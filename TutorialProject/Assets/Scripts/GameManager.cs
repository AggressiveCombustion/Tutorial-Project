using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public float speed = 1.0f;

    List<Timer> timers = new List<Timer>(); // current list of timers
    List<Timer> timersToAdd = new List<Timer>(); // timers we'll add after the loop completes

    // Awake is called before any call to Start
    void Awake()
    {
        if (instance == null)
            instance = this;

        else if (instance != this)
            Destroy(gameObject);
        
    }

    public void ResetSpeed()
    {
        speed = 1;
    }

    // Update is called once per frame
    void Update()
    {
        foreach(Timer t in timers)
        {
            t.Update();
        }

        foreach(Timer t in timersToAdd)
        {
            timers.Add(t);
        }

        timersToAdd.Clear();
    }

    public void AddTimer(GameObject g, float duration, TimerEvent e, bool ignoreSpeed)
    {
        Timer t = new Timer();
        t.duration = duration;
        t.action = e;
        t.ignoreSpeed = ignoreSpeed;
        t.obj = g;

        timersToAdd.Add(t);
    }

    public void AddTimer(GameObject g, float duration, TimerEvent e)
    {
        AddTimer(g, duration, e, false);
    }
}

public delegate void TimerEvent();

public class Timer
{
    public float duration = 0;
    public float elapsed = 0;
    public TimerEvent action;
    public bool ignoreSpeed;
    public bool done = false;
    public GameObject obj;

    public void Update()
    {
        elapsed += Time.deltaTime * (ignoreSpeed ? 1.0f : GameManager.instance.speed);

        if(elapsed >= duration && !done)
        {
            // make sure object is alive before trying to call its function
            if(obj != null)
                action();
            done = true;
        }
    }
}
