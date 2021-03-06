﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    
    [SerializeField]
    Image[] PlayerHealthImages;
    [SerializeField]
    PlayerUI[] playersUItemp;
    [SerializeField]
    PlayerUI[] playersUI;
    [SerializeField]
    List<Image[]> PlayerMarks;
    PlayerController[] playersTemp;
    public PlayerController[] players;
    GameManager gm;
    [SerializeField]
    private Canvas GameOverCanvas;
    [SerializeField]
    Sprite[] PlayerIcons;
    [SerializeField]
    Image killedPlayer;

    void Start () {
        //creates list of image arrays that coorispond to the number of kills each player has
        PlayerMarks = new List<Image[]>();
        //creates a PlayerController array based on the number of players in the scene
        players = new PlayerController[FindObjectsOfType<PlayerController>().Length];
        //stores all the PlayerCharacter instances in scene in a temporaty array
        playersTemp = FindObjectsOfType<PlayerController>();
        //for loop arranges the PlayerCharacter instances in order based on their Identifier number
        for (int i = 0; i < playersTemp.Length; i++)
        {
            players[playersTemp[i].Identifier] = playersTemp[i];
        }
        //creates 
        PlayerHealthImages = new Image[4];
        playersUItemp = FindObjectsOfType<PlayerUI>();
        playersUI = new PlayerUI[4];
        

        for (int i = 0; i < playersUItemp.Length; i++)
        {
            int temp = playersUItemp[i].Identifier;
            playersUI[temp] = playersUItemp[i];
        }

        gm = GetComponent<GameManager>();
        for (int k = 0; k < playersUI.Length; k++)
        {
            PlayerHealthImages[k] = playersUI[k].transform.GetChild(0).GetComponent<Image>();

            if (playersUI[k].Identifier >= (4-gm.playersToDeactivate))
            {
                playersUI[k].gameObject.SetActive(false);
                playersUI[k].enabled = false;
            }
        }
        for (int j = 0; j < players.Length; j++)
        {    
                PlayerMarks.Add(playersUI[j].transform.GetChild(1).GetComponentsInChildren<Image>());       
        }
        for (int l = 0; l < players.Length; l++)
        {
            for (int p = 0; p < PlayerMarks[l].Length; p++)
            {
                PlayerMarks[l][p].gameObject.SetActive(false);
            }
        }
    }
	
	// Update is called once per frame
	void Update () {

        for (int i = 0; i < players.Length; i++)
        {
        if (players[i] != null)
            PlayerHealthImages[i].fillAmount = players[i].GetComponent<Health>().currentHealth;
        }

        for (int i = 0; i < players.Length; i++)
        {
            for (int j = 0; j < players[i].KillCount && players[i].KillCount <= gm.MaxKills; j++)
            {
                PlayerMarks[i][j].gameObject.SetActive(true);
            }
        }

    }

    public void DisplayEndRound(int playerWhoWon)
    {
        //Cursor.lockState = CursorLockMode.None;
       // Cursor.visible = true;
        
        GameOverCanvas.gameObject.SetActive(true);
        StartCoroutine(PauseBeforeContinue(5));
        GameObject EndPanel = GameOverCanvas.transform.GetChild(0).gameObject;
        for (int p = 0; p < players.Length; p++)
        {
            EndPanel.transform.GetChild(p).gameObject.SetActive(true);
        }

        for (int i = 0; i < players.Length; i++)
        {
            for (int j = 0; j < players[i].KillCount; j++)
            {
                switch (players[i].KillList[j])
                {
                    case PlayerType.Cowboy:
                        killedPlayer.sprite = PlayerIcons[0];
                        Instantiate<Image>(killedPlayer,EndPanel.transform.GetChild(i).transform);
                        break;
                    case PlayerType.Spirit:
                        killedPlayer.sprite = PlayerIcons[1];
                        Instantiate<Image>(killedPlayer, EndPanel.transform.GetChild(i).transform);
                        break;
                    case PlayerType.Indian:
                        killedPlayer.sprite = PlayerIcons[2];
                        Instantiate<Image>(killedPlayer, EndPanel.transform.GetChild(i).transform);
                        break;
                    case PlayerType.Ninja:
                        killedPlayer.sprite = PlayerIcons[3];
                        Instantiate<Image>(killedPlayer, EndPanel.transform.GetChild(i).transform);
                        break;
                    default:
                        break;
                }
                
            }
        }
        for (int i = 0; i < players.Length; i++)
        {
            players[i].enabled = false;
        }
    }

    private IEnumerator PauseBeforeContinue(float pause)
    {
        yield return new WaitForSeconds(pause);
        Button[] buttons = GameOverCanvas.GetComponentsInChildren<Button>();
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].enabled = true;
        }
    }

}
