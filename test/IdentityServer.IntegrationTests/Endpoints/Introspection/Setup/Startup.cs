﻿// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using IdentityServer4.Core.Services.InMemory;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace IdentityServer4.Tests.Endpoints.Introspection
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            var cert = new X509Certificate2(Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "idsrvtest.pfx"), "idsrv3test");

            var builder = services.AddIdentityServer(options =>
            {
                options.SigningCertificate = cert;
                options.IssuerUri = "https://idsrv4";
                options.Endpoints.EnableAuthorizeEndpoint = false;
            });

            builder.AddInMemoryClients(Clients.Get());
            builder.AddInMemoryScopes(Scopes.Get());
            builder.AddInMemoryUsers(new List<InMemoryUser>());
        }

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            app.UseIdentityServer();
        }
    }
}