using ServerCore;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PacketManager
{
	#region Singleton
	static PacketManager _instance = new PacketManager();
	public static PacketManager Instance { get { return _instance; } }
	#endregion

	PacketManager()
	{
		Register();
	}

	Dictionary<ushort, Func<PacketSession, ArraySegment<byte>, IPacket>> _makeFunc = new Dictionary<ushort, Func<PacketSession, ArraySegment<byte>, IPacket>>();
	Dictionary<ushort, Action<PacketSession, IPacket>> _handler = new Dictionary<ushort, Action<PacketSession, IPacket>>();
		
	public void Register()
	{
		_makeFunc.Add((ushort)PacketID.S_BroadcastEnterGame, MakePacket<S_BroadcastEnterGame>);
		_handler.Add((ushort)PacketID.S_BroadcastEnterGame, PacketHandler.S_BroadcastEnterGameHandler);
		_makeFunc.Add((ushort)PacketID.S_BroadcastLeaveGame, MakePacket<S_BroadcastLeaveGame>);
		_handler.Add((ushort)PacketID.S_BroadcastLeaveGame, PacketHandler.S_BroadcastLeaveGameHandler);
		_makeFunc.Add((ushort)PacketID.S_PlayerList, MakePacket<S_PlayerList>);
		_handler.Add((ushort)PacketID.S_PlayerList, PacketHandler.S_PlayerListHandler);
		_makeFunc.Add((ushort)PacketID.S_BroadcastMove, MakePacket<S_BroadcastMove>);
		_handler.Add((ushort)PacketID.S_BroadcastMove, PacketHandler.S_BroadcastMoveHandler);
		_makeFunc.Add((ushort)PacketID.S_BroadcastTrainMove, MakePacket<S_BroadcastTrainMove>); 
		_handler.Add((ushort)PacketID.S_BroadcastTrainMove, PacketHandler.S_BroadcastTrainMoveHandler);
		_makeFunc.Add((ushort)PacketID.S_BroadcastShot, MakePacket<S_BroadcastShot>);
		_handler.Add((ushort)PacketID.S_BroadcastShot, PacketHandler.S_BroadcastShotHandler);
		_makeFunc.Add((ushort)PacketID.S_BroadcastMapSeed, MakePacket<S_BroadcastMapSeed>);
		_handler.Add((ushort)PacketID.S_BroadcastMapSeed, PacketHandler.S_BroadcastMapSeedHandler);
		_makeFunc.Add((ushort)PacketID.S_BroadcastEnterRoom, MakePacket<S_BroadcastEnterRoom>);
		_handler.Add((ushort)PacketID.S_BroadcastEnterRoom, PacketHandler.S_BroadcastEnterRoomHandler);

		/*
		 * _makeFunc.Add((ushort)PacketID.???, MakePacket<???>);
		 * _handler.Add((ushort)PacketID.???, PacketHandler.???);
		 */
	}

	public void OnRecvPacket(PacketSession session, ArraySegment<byte> buffer, Action<PacketSession, IPacket> onRecvCallback = null)
	{
		ushort count = 0;

		ushort size = BitConverter.ToUInt16(buffer.Array, buffer.Offset);
		count += 2;
		ushort id = BitConverter.ToUInt16(buffer.Array, buffer.Offset + count);
		count += 2;

		Func<PacketSession, ArraySegment<byte>, IPacket> func = null;
		if (_makeFunc.TryGetValue(id, out func))
		{
			IPacket packet = func.Invoke(session, buffer);
			if (onRecvCallback != null)
				onRecvCallback.Invoke(session, packet);
			else
				HandlePacket(session, packet);
		}
	}

	T MakePacket<T>(PacketSession session, ArraySegment<byte> buffer) where T : IPacket, new()
	{
		T pkt = new T();
		pkt.Read(buffer);
		return pkt;
	}

	public void HandlePacket(PacketSession session, IPacket packet)
	{
		Action<PacketSession, IPacket> action = null;
		if (_handler.TryGetValue(packet.Protocol, out action))
			action.Invoke(session, packet);
	}
}