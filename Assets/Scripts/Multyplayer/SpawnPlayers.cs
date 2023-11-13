using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class SpawnPlayers : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Transform[] posSpawn;
    
    void Start()
    {
        Player[] pl = PhotonNetwork.PlayerList;
        

        for (int i = 0; i < pl.Length; i++)
        {   
            if (pl[i].NickName.Equals(PhotonNetwork.NickName))
                PhotonNetwork.Instantiate(player.name, posSpawn[i].position, Quaternion.identity);
        }
        
    }

}
