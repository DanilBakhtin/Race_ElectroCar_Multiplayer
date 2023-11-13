using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class MyPlayerManager : MonoBehaviour
{
    private PhotonView _photonView;
    void Start()
    {
        _photonView = GetComponent<PhotonView>();

        if (_photonView.IsMine)
        {

        }
    }

    private void CreateController()
    {
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefubs", "PlayerManager"), Vector3.zero, Quaternion.identity);
    }
}
