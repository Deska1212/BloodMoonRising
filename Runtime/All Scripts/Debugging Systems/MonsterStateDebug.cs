using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MonsterStateDebug : MonoBehaviour
{
    [SerializeField] private Monster monster;
    [SerializeField] private MonsterBaseState state;

    [SerializeField] private TextMeshProUGUI stateText;
    
    // Start is called before the first frame update
    void Start()
    {
        monster = GetComponentInParent<Monster>();
        stateText = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        state = monster.GetMonsterState();
        stateText.text = state.GetStateName();
    }
}
