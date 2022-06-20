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
		EnemyManager.Instance.EnemyMove();
	}
	public static void S_BroadcastMapSeedHandler(PacketSession session, IPacket packet)
	{
		S_BroadcastMapSeed pkt = packet as S_BroadcastMapSeed;
		ServerSession serverSession = session as ServerSession;

		MapManager.instance.seed = pkt.mapSeed;
	}
	public static void S_BroadcastEnterRoomHandler(PacketSession session, IPacket packet)
	{
		S_BroadcastEnterRoom pkt = packet as S_BroadcastEnterRoom;
		ServerSession serverSession = session as ServerSession;

		PlayerManager.Instance.LeaveRoom(pkt);
	}
	public static void S_BroadcastResourceHandler(PacketSession session, IPacket packet)
	{
		S_BroadcastResource pkt = packet as S_BroadcastResource;
		ServerSession serverSession = session as ServerSession;

		MapManager.instance.BlockBreak(pkt);
	}
	public static void S_BroadcastHealthHandler(PacketSession session, IPacket packet)
	{
		S_BroadcastHealth pkt = packet as S_BroadcastHealth;
		ServerSession serverSession = session as ServerSession;

		PlayerManager.Instance.ChangeHp(pkt);
	}
	public static void S_BroadcastEnemyMoveHandler(PacketSession session, IPacket packet)
	{
		S_BroadcastEnemyMove pkt = packet as S_BroadcastEnemyMove;
		ServerSession serverSession = session as ServerSession;

		EnemyManager.Instance.MoveEnemy(pkt);
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