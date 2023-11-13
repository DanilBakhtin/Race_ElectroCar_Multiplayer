using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using WebSocketSharp;
using Photon.Realtime;
using Photon.Pun.Demo.Cockpit;

public class Launcher : MonoBehaviourPunCallbacks
{
    public static Launcher instance;

    [SerializeField] private InputField _inputNameRoom;
    [SerializeField] private Text _errorText;
    [SerializeField] private Text _roomNameText;
    [SerializeField] private Transform _roomList;
    [SerializeField] private GameObject _roomBtnPrefub;
    [SerializeField] private Transform _playerList;
    [SerializeField] private GameObject _playerNamePrefub;
    [SerializeField] private GameObject _startGameButton;

    [SerializeField] private InputField _inputNickName;
    [SerializeField] private Text _textNickName;
    [SerializeField] private GameObject _currentNNPanel;
    [SerializeField] private GameObject _changeNNPanel;

    private void Start()
    {   
        instance = this;
        Debug.Log("Присоединяемся к Мастер серверу");
        PhotonNetwork.ConnectUsingSettings();

        MenuManager.instance.OpenMenu("LoadingMenu");
        //GetComponent<MenuManager>().OpenMenu("LoadingMenu");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Присоединились к Мастер серверу");
        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Присоединились к Лобби");
        MenuManager.instance.OpenMenu("TitleMenu");

        if (PlayerPrefs.HasKey("NickName"))
        {
            PhotonNetwork.NickName = PlayerPrefs.GetString("NickName");
        }
        else PhotonNetwork.NickName = "Player " + Random.Range(0, 1000).ToString("0000");

        _textNickName.text = PhotonNetwork.NickName;
    }

    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(_inputNameRoom.text))
            return;

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;

        PhotonNetwork.CreateRoom(_inputNameRoom.text, roomOptions);

        MenuManager.instance.OpenMenu("LoadingMenu");
    }

    public override void OnJoinedRoom()
    {
        _roomNameText.text = PhotonNetwork.CurrentRoom.Name;
        MenuManager.instance.OpenMenu("RoomMenu");

        Player[] players = PhotonNetwork.PlayerList;

        for (int i = 0; i < _playerList.childCount; i++)
        {
            Destroy(_playerList.GetChild(i).gameObject);
        }

        for (int i = 0; i < players.Length; i++)
        {
            Instantiate(_playerNamePrefub, _playerList).GetComponent<PlayerListItem>().SetUp(players[i]);
        }

        _startGameButton.SetActive(PhotonNetwork.IsMasterClient);

    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        _startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        MenuManager.instance.OpenMenu("ErrorMenu");
        _errorText.text = "Error: " + message;
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        MenuManager.instance.OpenMenu("LoadingMenu");
    }

    public override void OnLeftRoom()
    {
        MenuManager.instance.OpenMenu("TitleMenu");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        for (int i = 0; i < _roomList.childCount;i++)
            Destroy(_roomList.GetChild(i).gameObject);

        for (int i = 0; i < roomList.Count; i++)
        {
            if (roomList[i].RemovedFromList) continue;
            Instantiate(_roomBtnPrefub, _roomList).GetComponent<RoomListButton>().SetUp(roomList[i]);
        }
    }

    public void JoinRoom(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);
        MenuManager.instance.OpenMenu("LoadingMenu");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Instantiate(_playerNamePrefub, _playerList).GetComponent<PlayerListItem>().SetUp(newPlayer);
    }

    public void StartGame()
    {   
        //if (PhotonNetwork.PlayerList.Length > 1)
            PhotonNetwork.LoadLevel(1);
    }

    public void saveNickName()
    {
        if (!string.IsNullOrEmpty(_inputNickName.text))
        {
            Debug.Log("Меняй");
            PhotonNetwork.NickName = _inputNickName.text;
            PlayerPrefs.SetString("NickName", PhotonNetwork.NickName);
            _textNickName.text = PhotonNetwork.NickName;
            setChangeNNPanel(false);
            setCurrentNNPanel(true);
        }
    }

    public void setChangeNNPanel(bool input)
    {
        _changeNNPanel.SetActive(input);
    }
    public void setCurrentNNPanel(bool input)
    {
        _currentNNPanel.SetActive(input);
    }
}
