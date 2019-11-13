﻿using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CreateRoomPanel : MonoBehaviourPunCallbacks {

    #region Private Fields

    [SerializeField] private TMP_InputField roomNameInputField;
    [SerializeField] private TMP_Dropdown roomSizeDropdown;
    [SerializeField] private TMP_Dropdown gameModeDropdown;
    [SerializeField] private Button createRoomButton;
    [SerializeField] private Button quitRoomCreationButton;

    #endregion

    #region UnityCallbacks

    private void Awake () {
        roomSizeDropdown.AddOptions(new List<string>(ServerConstants.ROOM_SIZES));
        gameModeDropdown.AddOptions(new List<string>(ServerConstants.GAME_MODES));

        createRoomButton.onClick.AddListener (() => OnCreateRoomButtonClicked ());
        quitRoomCreationButton.onClick.AddListener (() => OnQuitRoomCreationButtonClicked ());
    }

    #endregion

    #region PhotonCallbacks

    public override void OnCreatedRoom () {
        OnlineLobbyManager.Instance.SwitchToRoomPanel ();
    }

    public override void OnCreateRoomFailed (short returnCode, string message) {
        string log = string.Format("Error creating room\nReturn code: {0}\nError message: {1}", returnCode, message);
        OnlineLogging.Instance.Write(log);
    }

    #endregion

    #region UI Callbacks

    private void OnCreateRoomButtonClicked () {
        string roomName = roomNameInputField.text;
        if (string.IsNullOrEmpty (roomName) || string.IsNullOrWhiteSpace (roomName)) {
            return;
        }

        int roomSizeDropdownSelectedIndex = roomSizeDropdown.value;
        string roomSizeString = roomSizeDropdown.options[roomSizeDropdownSelectedIndex].text;
        byte roomSize = byte.Parse (roomSizeString);

        int gameModeDropdownSelectedIndex = gameModeDropdown.value;
        string gameModeString = gameModeDropdown.options[gameModeDropdownSelectedIndex].text;

        RoomOptions roomOptions = new RoomOptions ();
        roomOptions.MaxPlayers = roomSize;
        roomOptions.IsVisible = false;
        roomOptions.CustomRoomProperties = new Hashtable ();
        roomOptions.CustomRoomProperties.Add (ServerConstants.GAME_MODE_ROOM_KEY, gameModeString);

        PhotonNetwork.CreateRoom (roomName, roomOptions);
    }

    private void OnQuitRoomCreationButtonClicked () {
        OnlineLobbyManager.Instance.SwitchToLobbyMainMenu ();
    }

    #endregion

}