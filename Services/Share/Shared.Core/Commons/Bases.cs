﻿using System.ComponentModel.DataAnnotations;

namespace Shared.Core.Commons;

public class ActiveRequest
{
    [Required]
    public string Id { get; set; }

    public string Reason { get; set; }
}
public class InactiveRequest
{
    [Required]
    public string Id { get; set; }

    public string Reason { get; set; }
}


public class DeleteRequest
{
    public string Id { get; set; }

    public string Reason { get; set; }
}

public class ObjectString
{
    public string Id { get; set; }

    public string Name { get; set; }
    public string Type { get; set; } = "0";
}
