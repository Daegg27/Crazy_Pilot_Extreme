using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellScript : MonoBehaviour
{
    [SerializeField] private Text rankText;
    [SerializeField] private Text nameText;
    [SerializeField] private Text scoreText;
    public string playerRank;
    public string playerName;
    public string playerScore;


    private void Awake()
    {
        rankText.text = playerRank;
        nameText.text = playerName;
        scoreText.text = playerScore;
    }
}
