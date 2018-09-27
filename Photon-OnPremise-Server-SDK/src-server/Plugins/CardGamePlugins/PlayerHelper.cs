using CardGame;
using Photon.Hive.Plugin;
using System;
using System.Collections.Generic;

namespace CardGamePlugins {

    public class PlayerHelper {
        public static SerializablePlayer Serialize(Player pPlayer) {
            SerializablePlayer player = new SerializablePlayer {
                ActorNr = pPlayer.ActorNr,
                UserId = pPlayer.UserId,
                Health = pPlayer.Health,
            };

            return player;
        }
    }
}
