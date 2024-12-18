﻿// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

#pragma warning disable 1591

using System.Collections.Generic;

namespace IdentityServer4.EntityFramework.Entities
{
    public class ApiScope
    {
        public int Id { get; set; }
        public bool Enabled { get; set; } = true;
        public string? Name { get; set; }
        public string? DisplayName { get; set; }
        public string? Note { get; set; }
        public bool Required { get; set; }
        public bool Emphasize { get; set; }
        public bool ShowInDiscoveryDocument { get; set; } = true;
        public List<ApiScopeClaim> UserClaims { get; set; }=    new List<ApiScopeClaim>();
        public List<ApiScopeProperty> Properties { get; set; } = new List<ApiScopeProperty>();
    }
}