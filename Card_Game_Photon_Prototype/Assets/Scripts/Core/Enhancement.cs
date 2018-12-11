using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Enhancement {
    public readonly int Id;
    public readonly int BoardIndex;
    public readonly int OwnerId;
    public readonly CardGameCore.BoardEnhancements BoardEnhancement;
    public readonly int Value;

    public Enhancement(int pId, int pBoardIndex, int pOwnerId, int pBoardEnhancement, int pValue) {
        Id = pId;
        BoardIndex = pBoardIndex;
        OwnerId = pOwnerId;
        BoardEnhancement = (CardGameCore.BoardEnhancements) pBoardEnhancement;
        Value = pValue;
    }

    public static Enhancement Deserialize(byte[] bytes) {
        Enhancement enhancement;
        using (var s = new MemoryStream(bytes)) {
            using (var br = new BinaryReader(s)) {
                int id = br.ReadInt32();
                int boardIndex = br.ReadInt32();
                int ownerId = br.ReadInt32();
                int boardEnhancement = br.ReadInt32();
                int value = br.ReadInt32();

                enhancement = new Enhancement(id, boardIndex, ownerId, boardEnhancement, value);
            }
        }
        return enhancement;
    }
}
