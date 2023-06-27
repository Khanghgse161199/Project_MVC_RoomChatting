using System;
using System.Collections.Generic;

namespace DataServices.Entities;

public partial class ChatRoom
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Creator { get; set; } = null!;

    public string Admin { get; set; } = null!;

    public bool IsActive { get; set; }

    public bool IsPrivate { get; set; }

    public string? Serect { get; set; }

    public DateTime LastedUpdate { get; set; }

    public string? LastMessage { get; set; }

    public string? LastSenderMessage { get; set; }

    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();

    public virtual ICollection<RoomUserMapping> RoomUserMappings { get; set; } = new List<RoomUserMapping>();
}
