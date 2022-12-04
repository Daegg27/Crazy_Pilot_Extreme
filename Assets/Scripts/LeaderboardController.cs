using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LootLocker.Requests; 

public static class LeaderboardController
{

    public static int scoreToUpload;
    public static int leaderboardID = 2599;



    public static void SubmitScore()
    {
        string playerID = PlayerPrefs.GetString("PlayerID") + GetAndIncrementScoreCharacters();
        string metadata = PlayerPrefs.GetString("PlayerName", "#Guest" + PlayerPrefs.GetString("PlayerID"));

        if (PlayerPrefs.GetString("PlayerName").Length < 1)
        {
            metadata = "#Guest" + PlayerPrefs.GetString("PlayerID");
        }

        LootLockerSDKManager.SubmitScore(playerID, scoreToUpload, leaderboardID.ToString(), metadata, (response) =>
        {
            if (response.statusCode == 200)
            {
                Debug.Log("Successful");
            }
            else
            {
                Debug.Log("Failed" + response.Error);
            }
        });
    }

    static string GetAndIncrementScoreCharacters()
    {
        string incrementalScoreString = PlayerPrefs.GetString(nameof(incrementalScoreString), "a");
        char incrementalCharacter = PlayerPrefs.GetString(nameof(incrementalCharacter), "a")[0];

        if (incrementalScoreString[incrementalScoreString.Length - 1] == 'z')
        {
           incrementalScoreString += incrementalCharacter;
        }
        else
        {
            incrementalScoreString = incrementalScoreString.Substring(0, incrementalScoreString.Length - 1) + incrementalCharacter.ToString();
        }

        if ((int)incrementalCharacter < 122)
        {
            incrementalCharacter++;
        }
        else
        {
            incrementalCharacter = 'a';
        }

        PlayerPrefs.SetString(nameof(incrementalCharacter), incrementalCharacter.ToString());
        PlayerPrefs.SetString(nameof(incrementalScoreString), incrementalScoreString.ToString());

        return incrementalScoreString;
    }

    
    
}
