using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using NDream.AirConsole;
using Newtonsoft.Json.Linq;

public class AirMessages : MonoBehaviour
{

	public GameObject playerPrefab;
	public Material[] playersMats;

	public Dictionary<int, PlayerController> players = new Dictionary<int, PlayerController>();

	void Awake()
	{
		AirConsole.instance.onMessage += OnMessage;
		AirConsole.instance.onConnect += OnConnect;
		AirConsole.instance.onReady += OnReady;
		AirConsole.instance.onDisconnect += OnDisconnect;
	}

	void OnReady(string code)
	{
		//Since people might be coming to the game from the AirConsole store once the game is live, 
		//I have to check for already connected devices here and cannot rely only on the OnConnect event 
		List<int> connectedDevices = AirConsole.instance.GetControllerDeviceIds();
		foreach (int deviceID in connectedDevices)
		{
			AddNewPlayer(deviceID);
		}
	}

	void OnConnect(int device_id)
	{
		AddNewPlayer(device_id);


		if (AirConsole.instance.GetActivePlayerDeviceIds.Count == 0)
		{

			if (AirConsole.instance.GetControllerDeviceIds().Count >= 2)
			{
				//StartGame();
				GameManager.Instance.StartGame(0);
			}
			else
			{
				//uiText.text = "NEED MORE PLAYERS";
			}
		}
	}

	void OnDisconnect(int device_id)
	{
		RemovePlayer(device_id);
	}

	private void AddNewPlayer(int deviceID)
	{

		if (players.ContainsKey(deviceID) || players.Count >= 4)
		{
			return;
		}

		//Instantiate player prefab, store device id + player script in a dictionary
		Vector3 pos = transform.position;
		//pos.x = deviceID -2f;
		GameObject newPlayer = Instantiate(playerPrefab, pos, transform.rotation) as GameObject;
		players.Add(deviceID, newPlayer.GetComponent<PlayerController>());

		pos.x = (players.Count - 2f) * 2;
		newPlayer.transform.position = pos;

		players[deviceID].SetMaterial(playersMats[players.Count - 1]);
	}

	private void RemovePlayer(int deviceID)
	{
		if (!players.ContainsKey(deviceID))
		{
			return;
		}

		PlayerController player = players[deviceID];
		Destroy(player.gameObject);
		players.Remove(deviceID);
	}

	
	
	void OnMessage(int device_id, JToken data)
	{
		if (players.ContainsKey(device_id) && data["data"] != null)
		{
			//Debug.Log("message: " + data);

			string element = (string)data["element"];

			switch (element)
			{
				case "btn-interact":
					players[device_id].InteractInput((bool)data["data"]["pressed"]);
					break;
				case "main-arrows":
					players[device_id].MoveInput((string)data["data"]["key"], (bool)data["data"]["pressed"]);
					break;
			}

			//I forward the command to the relevant player script, assigned by device ID
			//players[device_id].ButtonInput(data["action"].ToString());
		}
	}
}
