﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerSend
{
    /// <summary>Sends a packet to a client via TCP.</summary>
    /// <param name="_toClient">The client to send the packet the packet to.</param>
    /// <param name="_packet">The packet to send to the client.</param>
    private static void SendTCPData(int _toClient, Packet _packet)
    {
        _packet.WriteLength();
        Server.clients[_toClient].tcp.SendData(_packet);
    }

    /// <summary>Sends a packet to a client via UDP.</summary>
    /// <param name="_toClient">The client to send the packet the packet to.</param>
    /// <param name="_packet">The packet to send to the client.</param>
    private static void SendUDPData(int _toClient, Packet _packet)
    {
        _packet.WriteLength();
        Server.clients[_toClient].udp.SendData(_packet);
    }

    /// <summary>Sends a packet to all clients via TCP.</summary>
    /// <param name="_packet">The packet to send.</param>
    private static void SendTCPDataToAll(Packet _packet)
    {
        _packet.WriteLength();
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            Server.clients[i].tcp.SendData(_packet);
        }
    }
    /// <summary>Sends a packet to all clients except one via TCP.</summary>
    /// <param name="_exceptClient">The client to NOT send the data to.</param>
    /// <param name="_packet">The packet to send.</param>
    private static void SendTCPDataToAll(int _exceptClient, Packet _packet)
    {
        _packet.WriteLength();
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            if (i != _exceptClient)
            {
                Server.clients[i].tcp.SendData(_packet);
            }
        }
    }

    /// <summary>Sends a packet to all clients via UDP.</summary>
    /// <param name="_packet">The packet to send.</param>
    private static void SendUDPDataToAll(Packet _packet)
    {
        _packet.WriteLength();
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            Server.clients[i].udp.SendData(_packet);
        }
    }
    /// <summary>Sends a packet to all clients except one via UDP.</summary>
    /// <param name="_exceptClient">The client to NOT send the data to.</param>
    /// <param name="_packet">The packet to send.</param>
    private static void SendUDPDataToAll(int _exceptClient, Packet _packet)
    {
        _packet.WriteLength();
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            if (i != _exceptClient)
            {
                Server.clients[i].udp.SendData(_packet);
            }
        }
    }

    #region Packets
    /// <summary>Sends a welcome message to the given client.</summary>
    /// <param name="_toClient">The client to send the packet to.</param>
    /// <param name="_msg">The message to send.</param>
    public static void Welcome(int _toClient, string _msg)
    {
        using (Packet _packet = new Packet((int)ServerPackets.welcome))
        {
            _packet.Write(_msg);
            _packet.Write(_toClient);

            SendTCPData(_toClient, _packet);
        }
    }

    /// <summary>Tells a client to spawn a player.</summary>
    /// <param name="_toClient">The client that should spawn the player.</param>
    /// <param name="_player">The player to spawn.</param>
    public static void SpawnPlayer(int _toClient, Player _player)
    {
        using (Packet _packet = new Packet((int)ServerPackets.spawnPlayer))
        {
            _packet.Write(_player.id);
            _packet.Write(_player.username);
            _packet.Write(_player.transform.position);
            _packet.Write(_player.transform.rotation);

            SendTCPData(_toClient, _packet);
        }
    }

    /// <summary>Sends a player's updated position to all clients.</summary>
    /// <param name="_player">The player whose position to update.</param>
    public static void PlayerPosition(Player _player)
    {
        using (Packet _packet = new Packet((int)ServerPackets.playerPosition))
        {
            _packet.Write(_player.id);
            _packet.Write(_player.transform.position);

            SendUDPDataToAll(_packet);
        }
    }

    /// <summary>Sends a player's updated rotation to all clients except to himself (to avoid overwriting the local player's rotation).</summary>
    /// <param name="_player">The player whose rotation to update.</param>
    public static void PlayerRotation(Player _player)
    {
        using (Packet _packet = new Packet((int)ServerPackets.playerRotation))
        {
            _packet.Write(_player.id);
            _packet.Write(_player.transform.rotation);

            SendUDPDataToAll(_player.id, _packet);
        }
    }

    public static void CreateInteractible(int _toClient, int _InteractableID, Vector3 _position, bool interactedWith) {
        using (Packet _packet = new Packet((int)ServerPackets.createInteractible)) {
            _packet.Write(_InteractableID);
            _packet.Write(_position);
            _packet.Write(interactedWith);

            SendTCPData(_toClient, _packet);
        }
    }

    public static void InteractibleTouched(int _interactibleID)
    {
        using (Packet _packet = new Packet((int)ServerPackets.InteractibleTouched))
        {
            _packet.Write(_interactibleID);
            SendTCPDataToAll(_packet);
        }
    }

    public static void InteractibleTouchedOnce(int _interactibleID, int _interactibleType)
    {
        Debug.Log("Sending Packet "+ _interactibleID +" "+ _interactibleType);
        using (Packet _packet = new Packet((int)ServerPackets.InteractibleTouchedOnce))
        {
            _packet.Write(_interactibleID);
            _packet.Write(_interactibleType);
            SendTCPDataToAll(_packet);
        }
    }

    public static void InteractibleUnTouched(int _interactibleID)
    {
        using (Packet _packet = new Packet((int)ServerPackets.InteractibleUnTouched))
        {
            _packet.Write(_interactibleID);
            SendTCPDataToAll(_packet);
        }
    }

    public static void spawnEnemy(Enemy _enemy) {
        using (Packet _packet = new Packet((int)ServerPackets.spawnEnemy))
        {
            SendTCPDataToAll(SpawnEnemy_Data(_enemy, _packet));
        }
    }

    public static void spawnEnemy(int _toClient, Enemy _enemy)
    {
        using (Packet _packet = new Packet((int)ServerPackets.spawnEnemy))
        {
            SendTCPData(_toClient, SpawnEnemy_Data(_enemy, _packet));
        }
    }

    public static Packet SpawnEnemy_Data(Enemy _enemy, Packet _packet) {
        _packet.Write(_enemy.id);
        _packet.Write(_enemy.transform.position);
        _packet.Write(_enemy.transform.rotation);
        return _packet;
    }

    public static void EnemyPos(Enemy _enemy) {
        using (Packet _packet = new Packet((int)ServerPackets.enemyPosition))
        {
            _packet.Write(_enemy.id);
            _packet.Write(_enemy.transform.position);
            _packet.Write(_enemy.transform.rotation);
            SendUDPDataToAll(_packet);
        }
    }

    public static void playerDC(int _playerID) {
        using (Packet _packet = new Packet((int)ServerPackets.playerDC)) {
            _packet.Write(_playerID);
            SendTCPDataToAll(_packet);
        }
    }
    #endregion
}
