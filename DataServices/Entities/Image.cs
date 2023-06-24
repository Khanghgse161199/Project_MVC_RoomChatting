using System;
using System.Collections.Generic;

namespace DataServices.Entities;

public partial class Image
{
    public string Id { get; set; } = null!;

    public string ImgSetId { get; set; } = null!;

    public string ImgUrl { get; set; } = null!;

    public bool IsActive { get; set; }

    public virtual ImgSet ImgSet { get; set; } = null!;
}
