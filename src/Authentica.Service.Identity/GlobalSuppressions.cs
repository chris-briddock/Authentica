// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Mapper", "RMG012:Source member was not found for target member", Justification = "Source member does not need to be mapped here.", Scope = "member", Target = "~M:Application.Mappers.ClientApplicationMapper.ToResponse(Domain.Aggregates.Identity.ClientApplication)~Api.Responses.ReadApplicationResponse")]
[assembly: SuppressMessage("Mapper", "RMG020:Source member is not mapped to any target member", Justification = "Source member does not need to be mapped here.", Scope = "member", Target = "~M:Application.Mappers.ClientApplicationMapper.ToResponse(Domain.Aggregates.Identity.ClientApplication)~Api.Responses.ReadApplicationResponse")]
[assembly: SuppressMessage("Mapper", "RMG012:Source member was not found for target member", Justification = "Source member does not need to be mapped here.", Scope = "member", Target = "~M:Application.Mappers.GetAllUsersMapper.ToResponse(System.Collections.Generic.IList{Domain.Aggregates.Identity.User})~System.Collections.Generic.List{Api.Responses.GetUserResponse}")]
[assembly: SuppressMessage("Mapper", "RMG020:Source member is not mapped to any target member", Justification = "Source member does not need to be mapped here.", Scope = "member", Target = "~M:Application.Mappers.GetAllUsersMapper.ToResponse(System.Collections.Generic.IList{Domain.Aggregates.Identity.User})~System.Collections.Generic.List{Api.Responses.GetUserResponse}")]
[assembly: SuppressMessage("Mapper", "RMG020:Source member is not mapped to any target member", Justification = "Source member does not need to be mapped here.", Scope = "member", Target = "~M:Application.Mappers.ReadAllActivitiesMapper.ToResponse(System.Collections.Immutable.ImmutableList{Domain.Aggregates.Identity.Activity})~System.Collections.Immutable.ImmutableList{Api.Responses.ActivityResponse}")]
