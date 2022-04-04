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
	private Connector connector;
	private IPEndPoint endPoint;
	private Ping ping;
	public int ping_time { get; private set; }

	public void Send(ArraySegment<byte> sendBuff)
	{
		_session.Send(sendBuff);
	}

    void Start()
    {
		// DNS (Domain Name System)
		string host = Dns.GetHostName();
		IPHostEntry ipHost = Dns.GetHostEntry(host);
		IPAddress ipAddr = ipHost.AddressList[0];
		endPoint = new IPEndPoint(ipAddr, 7777);

		connector = new Connector();

		connector.Connect(endPoint,
			() => { return _session; },
			1);
	}
	void Ping()
	{
		if (null == ping)
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
	void Update()
    {
		List<IPacket> list = PacketQueue.Instance.PopAll();
		foreach (IPacket packet in list)
			PacketManager.Instance.HandlePacket(_session, packet);
		Ping();
		Debug.Log(ping_time);
	}
}
