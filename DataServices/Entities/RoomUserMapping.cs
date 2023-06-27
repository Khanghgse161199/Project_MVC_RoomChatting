using System;
using System.Collections.Generic;

namespace DataServices.Entities;

public partial class RoomUserMapping
{
    public string Id { get; set; } = null!;

    public string AccId { get; set; } = null!;

    public string ChatRoomId { get; set; } = null!;

    public bool IsActive { get; set; }

    public int CountNotify { get; set; }

    public virtual Account Acc { get; set; } = null!;

    public virtual ChatRoom ChatRoom { get; set; } = null!;
}
