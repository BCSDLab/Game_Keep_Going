using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    MyPlayer _myPlayer;
    Dictionary<int, Player> _players = new Dictionary<int, Player>();
    private PhysicsScene physicsScene;
    private float deltaTime;
    public NetworkManager NetworkManager;
    public static PlayerManager Instance { get; } = new PlayerManager();

    public void Add(S_PlayerList packet)
    {
        Object obj = Resources.Load("Prefabs/player_test");

        foreach (S_PlayerList.Player p in packet.players)
        {
            GameObject go = Object.Instantiate(obj) as GameObject;

            if (p.isSelf)
            {
                MyPlayer myPlayer = go.AddComponent<MyPlayer>();
                myPlayer.PlayerId = p.playerId;
                myPlayer.transform.position = new Vector3(p.posX, 1.6f, p.posZ);
                _myPlayer = myPlayer;
            }
            else
            {
                Player player = go.AddComponent<Player>();
                player.PlayerId = p.playerId;
                player.transform.position = new Vector3(p.posX, 1.6f, p.posZ);
                _players.Add(p.playerId, player);
            }
        }
    }
    private IEnumerator PacketDelay(float delay, System.Action action)
    {
        yield return new WaitForSeconds(delay);
        action();
    }

    public void Move(S_BroadcastMove packet)
    {
        if (_myPlayer.PlayerId == packet.playerId)
        {
            //_myPlayer.transform.position = new Vector3(packet.posX, 1.6f, packet.posZ);
        }
        else
        {
            Player player = null;
            if (_players.TryGetValue(packet.playerId, out player))
            {
                player.move(packet.dirH, packet.dirV, packet.rotateY);
                StartCoroutine(PacketDelay((float)NetworkManager.ping_time / 1000, () =>
                {
                    player.transform.rotation = Quaternion.Euler(0, packet.rotateY,0);
                    player.transform.position = new Vector3(packet.posX, 1.6f, packet.posZ);

                    deltaTime += (float)NetworkManager.ping_time / 1000;
                    while (deltaTime >= Time.fixedDeltaTime)
                    {
                        deltaTime -= Time.fixedDeltaTime;
                        physicsScene.Simulate(Time.fixedDeltaTime);
                    }
                }));
            }
        }
    }

    public void EnterGame(S_BroadcastEnterGame packet)
    {
        if (packet.playerId == _myPlayer.PlayerId)
            return;

        Object obj = Resources.Load("Prefabs/player_test");
        GameObject go = Object.Instantiate(obj) as GameObject;

		Player player = go.AddComponent<Player>();
		player.transform.position = new Vector3(packet.posX, 1.6f, packet.posZ);
		_players.Add(packet.playerId, player);
	}

    public void LeaveGame(S_BroadcastLeaveGame packet)
    {
        if (_myPlayer.PlayerId == packet.playerId)
        {
            GameObject.Destroy(_myPlayer.gameObject);
            _myPlayer = null;
        }
        else
        {
            Player player = null;
            if (_players.TryGetValue(packet.playerId, out player))
            {
                GameObject.Destroy(player.gameObject);
                _players.Remove(packet.playerId);
            }
        }
    }
}
