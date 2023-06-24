using System;
using System.Collections.Generic;

namespace DataServices.Entities;

public partial class Account
{
    public string Id { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public bool IsActive { get; set; }

    public virtual ICollection<RoomUserMapping> RoomUserMappings { get; set; } = new List<RoomUserMapping>();

    public virtual ICollection<Token> Tokens { get; set; } = new List<Token>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
