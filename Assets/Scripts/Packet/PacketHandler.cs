using DummyClient;
using ServerCore;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

class PacketHandler
{
	public static void S_BroadcastEnterGameHandler(PacketSession session, IPacket packet)
	{
		S_BroadcastEnterGame pkt = packet as S_BroadcastEnterGame;
		ServerSession serverSession = session as ServerSession;

		PlayerManager.Instance.EnterGame(pkt);
	}

	public static void S_BroadcastLeaveGameHandler(PacketSession session, IPacket packet)
	{
		S_BroadcastLeaveGame pkt = packet as S_BroadcastLeaveGame;
		ServerSession serverSession = session as ServerSession;

		PlayerManager.Instance.LeaveGame(pkt);
	}

	public static void S_PlayerListHandler(PacketSession session, IPacket packet)
	{
		S_PlayerList pkt = packet as S_PlayerList;
		ServerSession serverSession = session as ServerSession;

		PlayerManager.Instance.Add(pkt);
		TrainManager.Instance.Add();
	}

	public static void S_BroadcastMoveHandler(PacketSession session, IPacket packet)
	{
		S_BroadcastMove pkt = packet as S_BroadcastMove;
		ServerSession serverSession = session as ServerSession;

		PlayerManager.Instance.Move(pkt);
	}
	
	public static void S_BroadcastTrainMoveHandler(PacketSession session, IPacket packet)
	{
		S_BroadcastTrainMove pkt = packet as S_BroadcastTrainMove;
		ServerSession serverSession = session as ServerSession;

		TrainManager.Instance.Move(pkt);
	}

	public static void S_BroadcastShotHandler(PacketSession session, IPacket packet)
	{
		S_BroadcastShot pkt = packet as S_BroadcastShot;
		ServerSession serverSession = session as ServerSession;

		BulletManager.Instance.Add(pkt);
	}

	/*
	public static void Handler(PacketSession session, IPacket packet)
	{
		Class pkt = packet as Class;
		ServerSession serverSession = session as ServerSession;

		Manager.Instance.Function(pkt);
	}
	*/
}