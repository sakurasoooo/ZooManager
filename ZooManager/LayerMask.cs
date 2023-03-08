using System;
namespace ZooManager
{
    public enum LayerMask
    {
        Ground = 1 << 0,
        Cat = 1 << 1,
        Mouse = 1 << 2,
        Raptor = 1 << 3,
        Chick = 1 << 4,
        Skull = 1 << 5,
        Alien = 1 << 6
    }
}

