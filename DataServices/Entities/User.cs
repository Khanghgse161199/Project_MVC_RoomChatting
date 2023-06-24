using System;
using System.Collections.Generic;

namespace DataServices.Entities;

public partial class User
{
    public string Id { get; set; } = null!;

    public string Fullname { get; set; } = null!;

    public DateTime BirthDay { get; set; }

    public string ImgSetId { get; set; } = null!;

    public string AccId { get; set; } = null!;

    public virtual Account Acc { get; set; } = null!;

    public virtual ImgSet ImgSet { get; set; } = null!;
}
