using System;
using System.Collections.Generic;

namespace DataServices.Entities;

public partial class Message
{
    public string Id { get; set; } = null!;

    public string ChatRoomId { get; set; } = null!;

    public string Message1 { get; set; } = null!;

    public string? ImgSetId { get; set; }

    public DateTime CreatedDate { get; set; }

    public string Sender { get; set; } = null!;

    public bool IsMessageSystem { get; set; }

    public virtual ChatRoom ChatRoom { get; set; } = null!;

    public virtual ImgSet? ImgSet { get; set; }
}
