using DummyClient;
using ServerCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
	ServerSession _session = new ServerSession();
	private IPAddress ipAddr;
	private Connector connector;
	private IPEndPoint endPoint;
	private Ping ping = null;
	public int ping_time { get; private set; }
	public bool isHost { get; private set; } = false;
	private static NetworkManager instance;

    public static NetworkManager Instance
	{
		get
		{
			if (!instance)
			{
				instance = FindObjectOfType(typeof(NetworkManager)) as NetworkManager;
			}
			return instance;
		}
	}

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		else if (instance != this)
		{
			Destroy(gameObject);
		}
		DontDestroyOnLoad(gameObject);
	}


    void Start()
    {
		// DNS (Domain Name System)
		string host = Dns.GetHostName();
		IPHostEntry ipHost = Dns.GetHostEntry(host);
		IPAddress ipAddr = ipHost.AddressList[0];
		endPoint = new IPEndPoint(ipAddr, 7777);

		connector = new Connector();

		Connect();
		ping_time = 10;
	}

	public void Send(ArraySegment<byte> sendBuff)
	{
		_session.Send(sendBuff);
	}

	public void Connect()
    {
		connector.Connect(endPoint,
			() => { return _session; },
			1);
	}

	public void ConnectRoom(int roomNum)
	{
		C_EnterRoom packet = new C_EnterRoom();
		//if(packet.roomNum != roomNum)
		packet.roomNum = roomNum;
		PlayerManager.Instance.LeaveAll();
		Send(packet.Write());
	}

	void Ping()
	{
		if (ping == null)
		{
			// ping time 측정 시작
			ping = new Ping(endPoint.Address.ToString());
		}
		else
		{
			// ping time 측정 완료
			if (true == ping.isDone)
			{
				ping_time = ping.time;
				ping = null;
			}
		}
	}

	public void SetHost()
    {
		isHost = true;
    }
	void Update()
    {
		List<IPacket> list = PacketQueue.Instance.PopAll();
		foreach (IPacket packet in list)
			PacketManager.Instance.HandlePacket(_session, packet);
		//Ping();
	}
}
