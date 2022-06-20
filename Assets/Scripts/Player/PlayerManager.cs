using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    MyPlayer _myPlayer;
    Dictionary<int, Player> _players = new Dictionary<int, Player>();
    GameObject[] playerList;
    public static PlayerManager Instance { get; } = new PlayerManager();

    public void Add(S_PlayerList packet)
    {
        Object obj = Resources.Load("Prefabs/player_test");
        Debug.Log(packet.players.Count);
        if (packet.players.Count == 1)
            NetworkManager.Instance.SetHost();
        foreach (S_PlayerList.Player p in packet.players)
        {
            GameObject go = Instantiate(obj) as GameObject;
            if (p.isSelf)
            {
                MyPlayer myPlayer = go.AddComponent<MyPlayer>();
                myPlayer.PlayerId = p.playerId;
                myPlayer.transform.position = new Vector3(p.posX, 1.6f, p.posZ);
                _myPlayer = myPlayer;
            }
            else
            {
                /*if (_players.ContainsKey(_myPlayer.PlayerId))
                {
                    Destroy(go);
                    return;
                }
                */
                OtherPlayer player = go.AddComponent<OtherPlayer>();
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
                Vector3 targetPos = new Vector3(packet.posX, 1.6f, packet.posZ);
                player.gameObject.GetComponent<OtherPlayer>().SetTargetPos(targetPos);
                player.transform.rotation = Quaternion.Euler(0, packet.rotateY, 0);
                player.transform.position = new Vector3(packet.posX, 1.6f, packet.posZ);
            }
        }
    }

    public void EnterGame(S_BroadcastEnterGame packet)
    {
        if (packet.playerId == _myPlayer.PlayerId)
            return;
        
        Object obj = Resources.Load("Prefabs/player_test");
        GameObject go = Object.Instantiate(obj) as GameObject;
        /*if (_players.ContainsKey(_myPlayer.PlayerId))
        {
            Destroy(go);
            return;
        }
        */
        Player player = go.AddComponent<OtherPlayer>();
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
    public void LeaveRoom(S_BroadcastEnterRoom packet)
    {
        LeaveAll();
        if (_myPlayer.PlayerId == packet.playerId)
        {
            Debug.Log("EnterRoom Delete");
            GameObject.Destroy(_myPlayer.gameObject);
            _myPlayer = null;
        }
        else
        {
            Player player1 = null;
            if (_players.TryGetValue(packet.playerId, out player1))
            {
                GameObject.Destroy(player1.gameObject);
                _players.Remove(packet.playerId);
            }
        }
    }
    public void LeaveAll()
    {
        foreach (KeyValuePair<int, Player> player in _players)
        {
            Destroy(player.Value.gameObject);
            _players.Remove(player.Key);
        }
        _players.Clear();
    }

    public void ReloadPlayerList()
    {
        playerList = GameObject.FindGameObjectsWithTag("Player");
    }
    public void ChangeHp(S_BroadcastHealth packet)
    {
        if (_myPlayer.PlayerId == packet.playerId)
            return;
         foreach (GameObject player in playerList)
        {
            if (player.GetComponent<OtherPlayer>().PlayerId == packet.playerId)
                player.GetComponent<PlayerStat>().TakeDamage(packet.health);
        }
    }
}
