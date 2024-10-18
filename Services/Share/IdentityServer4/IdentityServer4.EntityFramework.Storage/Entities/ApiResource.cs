// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

#pragma warning disable 1591

using System;
using System.Collections.Generic;

namespace IdentityServer4.EntityFramework.Entities
{
    public class ApiResource
    {
        public int Id { get; set; }
        public bool Enabled { get; set; } = true;
        public string? Name { get; set; }
        public string? DisplayName { get; set; }
        public string? Note { get; set; }
        public string? AllowedAccessTokenSigningAlgorithms { get; set; }
        public bool ShowInDiscoveryDocument { get; set; } = true;
        public List<ApiResourceSecret> Secrets { get; set; } = new List<ApiResourceSecret>();
        public List<ApiResourceScope> Scopes { get; set; } = new List<ApiResourceScope>();
        public List<ApiResourceClaim> UserClaims { get; set; } = new List<ApiResourceClaim>();
        public List<ApiResourceProperty> Properties { get; set; } = new List<ApiResourceProperty>();
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public DateTime? Updated { get; set; }
        public DateTime? LastAccessed { get; set; }
        public bool NonEditable { get; set; }
    }
}
