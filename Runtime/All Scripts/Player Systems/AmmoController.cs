using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoController : MonoBehaviour
{
    [SerializeField] private int bolts;
    [SerializeField] private int maxBolts;
    
    // Events
    public static Action<int> OnBoltCountChanged;

    public int Bolts
    {
        get
        {
            return bolts;
        }
        set
        {
            bolts = value;
            OnBoltCountChanged?.Invoke(bolts);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RemoveBolt()
    {
        if (Bolts > 0)
        {
            --Bolts;
        }
    }

    public void RefillAmmo()
    {
        Bolts = maxBolts;
    }
}
