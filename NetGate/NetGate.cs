using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using NativeWebSocket;


public class NetGate : MonoBehaviour
{
	public static NetGate Instance { get; private set; }

	public Action OnConnected { get; set; }
	public Action<string> OnError { get; set; }
	public Action OnClose { get; set; }
	public Action<byte[]> OnData { get; set; }

	private WebSocket _websocket;

	public void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(this);
		}
		else
		{
			Instance = this;
			DontDestroyOnLoad(this.gameObject);
		}
	}

	public void Update()
	{
#if !UNITY_WEBGL || UNITY_EDITOR
		_websocket?.DispatchMessageQueue();
#endif
	}

	public async void Connect(string endpoint, string roomId)
	{
		_websocket = new WebSocket($"ws://{endpoint}/{roomId}");

		_websocket.OnOpen += () =>
		{
			Debug.Log("[NetGate] Connection open!");
			OnConnected?.Invoke();
		};

		_websocket.OnError += (e) =>
		{
			Debug.Log("[NetGate] Error! " + e);
			OnError?.Invoke(e);
		};

		_websocket.OnClose += (e) =>
		{
			Debug.Log("[NetGate] Connection closed! " + e);
			OnClose?.Invoke();
		};

		_websocket.OnMessage += (bytes) =>
		{
			Debug.Log("[NetGate] OnMessage! " + bytes);
			OnData?.Invoke(bytes);
		};

		await _websocket.Connect();
	}

	public async void Send(byte[] data)
	{
		if (_websocket.State == WebSocketState.Open)
		{
			await _websocket.Send(data);
		}
	}

	public async void Close()
	{
		if (_websocket == null)
			return;

		if (_websocket.State == WebSocketState.Open)
			await _websocket.Close();
	}

	private void OnApplicationQuit()
	{
		Close();
	}
}
