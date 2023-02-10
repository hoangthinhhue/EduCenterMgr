// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Options;
global using System.Security.Claims;
global using Microsoft.AspNetCore.Components.Authorization;
global using Microsoft.AspNetCore.Identity;
global using Microsoft.Extensions.Logging;
global using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore;
global using CleanArchitecture.Blazor.Domain;
global using CleanArchitecture.Blazor.Domain.Entities;
global using CleanArchitecture.Blazor.Infrastructure.Persistence.Extensions;

global using CleanArchitecture.Blazor.Domain.Identity;
global using CleanArchitecture.Blazor.Infrastructure.Middlewares;
global using CleanArchitecture.Blazor.Infrastructure.Persistence;

//Reference core
global using CleanArchitecture.Core.Common;
global using CleanArchitecture.Core.Entities;
global using CleanArchitecture.Core.Entities.Log.Audit;
global using CleanArchitecture.Core.Enums;
global using CleanArchitecture.Core.Models;
global using CleanArchitecture.Core.Interfaces;
global using CleanArchitecture.Core.Exceptions;
