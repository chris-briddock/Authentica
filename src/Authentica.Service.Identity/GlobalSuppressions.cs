// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Critical Code Smell", "S3218:Inner class members should not shadow outer class \"static\" or type members", Justification = "<Pending>", Scope = "member", Target = "~F:Api.Constants.Routes.Admin.Roles.Create")]
[assembly: SuppressMessage("Minor Code Smell", "S1075:URIs should not be hardcoded", Justification = "<Pending>", Scope = "member", Target = "~M:Persistence.Seed.Seed.SeedClientApplicationAsync(Microsoft.AspNetCore.Builder.WebApplication,System.String,System.Boolean,System.String,System.String,System.Nullable{System.DateTime})~System.Threading.Tasks.Task")]
[assembly: SuppressMessage("Minor Code Smell", "S6667:Logging in a catch clause should pass the caught exception as a parameter.", Justification = "<Pending>", Scope = "member", Target = "~M:Api.Middlware.ExceptionMiddleware.InvokeAsync(Microsoft.AspNetCore.Http.HttpContext)~System.Threading.Tasks.Task")]
