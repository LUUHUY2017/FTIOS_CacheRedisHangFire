﻿// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


#pragma warning disable 1591

using System;
using System.ComponentModel.DataAnnotations;

namespace IdentityServer4.EntityFramework.Entities
{
    public class PersistedGrant
    {
        [MaxLength(150)]
        public string? Key { get; set; }
        public string? Type { get; set; }
        public string? SubjectId { get; set; }
        public string? SessionId { get; set; }
        public string? ClientId { get; set; }
        public string? Note { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime? Expiration { get; set; }
        public DateTime? ConsumedTime { get; set; }
        public string? Data { get; set; }
    }
}