using BuildingBlocks.Application.DTOs;
using BuildingBlocks.Application.Interfaces.Command;
using BuildingBlocks.Application.Interfaces.Query;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Tenant.Application.Commands.Artifact;
using Tenant.Application.Commands.ArtifactType;
using Tenant.Application.Commands.Customer;
using Tenant.Application.Commands.CustomerArtifact;
using Tenant.Application.Commands.CustomerUser;
using Tenant.Application.Commands.Organization;
using Tenant.Application.Commands.OrganizationPlan;
using Tenant.Application.Commands.OrganizationUser;
using Tenant.Application.Commands.Plan;
using Tenant.Application.Commands.Team;
using Tenant.Application.Commands.User;
using Tenant.Application.DTOs;
using Tenant.Application.Queries.Artifact;
using Tenant.Application.Queries.ArtifactType;
using Tenant.Application.Queries.Customer;
using Tenant.Application.Queries.CustomerArtifact;
using Tenant.Application.Queries.CustomerUser;
using Tenant.Application.Queries.Organization;
using Tenant.Application.Queries.OrganizationPlan;
using Tenant.Application.Queries.OrganizationUser;
using Tenant.Application.Queries.Plan;
using Tenant.Application.Queries.Team;
using Tenant.Application.Queries.User;
using Tenant.Application.Requests.Organizations;

namespace Tenant.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<CreateOrganizationRequest>();

        services.AddMediatR(options =>
        {
            options.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
        });

        services
            .AddQueries()
            .AddCommands();

        return services;
    }

    private static IServiceCollection AddQueries(this IServiceCollection services)
    {
        services.AddScoped<IQueryHandler<GetCustomerByPageQuery, PageDto<CustomerDto>>, GetCustomerByPageQueryHandler>();
        services.AddScoped<IQueryHandler<GetCustomerByIdQuery, CustomerDto>, GetCustomerByIdQueryHandler>();

        services.AddScoped<IQueryHandler<GetOrganizationByPageQuery, PageDto<OrganizationDto>>, GetOrganizationByPageQueryHandler>();
        services.AddScoped<IQueryHandler<GetOrganizationByIdQuery, OrganizationDto>, GetOrganizationByIdQueryHandler>();

        services.AddScoped<IQueryHandler<GetPlanByPageQuery, PageDto<PlanDto>>, GetPlanByPageQueryHandler>();
        services.AddScoped<IQueryHandler<GetPlanByIdQuery, PlanDto>, GetPlanByIdQueryHandler>();

        services.AddScoped<IQueryHandler<GetOrganizationPlanByPageQuery, PageDto<OrganizationPlanDto>>, GetOrganizationPlanByPageQueryHandler>();
        services.AddScoped<IQueryHandler<GetOrganizationPlanByIdQuery, OrganizationPlanDto>, GetOrganizationPlanByIdQueryHandler>();

        services.AddScoped<IQueryHandler<GetUserByPageQuery, PageDto<UserDto>>, GetUserByPageQueryHandler>();
        services.AddScoped<IQueryHandler<GetUserByIdQuery, UserDto>, GetUserByIdQueryHandler>();

        services.AddScoped<IQueryHandler<GetCustomerUserByPageQuery, PageDto<CustomerUserDto>>, GetCustomerUserByPageQueryHandler>();
        services.AddScoped<IQueryHandler<GetCustomerUserByIdQuery, CustomerUserDto>, GetCustomerUserByIdQueryHandler>();

        services.AddScoped<IQueryHandler<GetOrganizationUserByPageQuery, PageDto<OrganizationUserDto>>, GetOrganizationUserByPageQueryHandler>();
        services.AddScoped<IQueryHandler<GetOrganizationUserByIdQuery, OrganizationUserDto>, GetOrganizationUserByIdQueryHandler>();

        services.AddScoped<IQueryHandler<GetArtifactTypeByPageQuery, PageDto<ArtifactTypeDto>>, GetArtifactTypeByPageQueryHandler>();
        services.AddScoped<IQueryHandler<GetArtifactTypeByIdQuery, ArtifactTypeDto>, GetArtifactTypeByIdQueryHandler>();

        services.AddScoped<IQueryHandler<GetArtifactByPageQuery, PageDto<ArtifactDto>>, GetArtifactByPageQueryHandler>();
        services.AddScoped<IQueryHandler<GetArtifactByIdQuery, ArtifactDto>, GetArtifactByIdQueryHandler>();

        services.AddScoped<IQueryHandler<GetCustomerArtifactByPageQuery, PageDto<CustomerArtifactDto>>, GetCustomerArtifactByPageQueryHandler>();
        services.AddScoped<IQueryHandler<GetCustomerArtifactByIdQuery, CustomerArtifactDto>, GetCustomerArtifactByIdQueryHandler>();

        services.AddScoped<IQueryHandler<GetTeamByPageQuery, PageDto<TeamDto>>, GetTeamByPageQueryHandler>();
        services.AddScoped<IQueryHandler<GetTeamByIdQuery, TeamDto>, GetTeamByIdQueryHandler>();

        return services;
    }

    private static IServiceCollection AddCommands(this IServiceCollection services)
    {
        services.AddScoped<ICommandHandler<CreateCustomerCommand, CustomerDto>, CreateCustomerCommandHandler>();
        services.AddScoped<ICommandHandler<UpdateCustomerCommand, CustomerDto>, UpdateCustomerCommandHandler>();
        services.AddScoped<ICommandHandler<DeleteCustomerCommand, CustomerDto>, DeleteCustomerCommandHandler>();

        services.AddScoped<ICommandHandler<CreateOrganizationCommand, OrganizationDto>, CreateOrganizationCommandHandler>();
        services.AddScoped<ICommandHandler<UpdateOrganizationCommand, OrganizationDto>, UpdateOrganizationCommandHandler>();
        services.AddScoped<ICommandHandler<DeleteOrganizationCommand, OrganizationDto>, DeleteOrganizationCommandHandler>();

        services.AddScoped<ICommandHandler<CreatePlanCommand, PlanDto>, CreatePlanCommandHandler>();
        services.AddScoped<ICommandHandler<UpdatePlanCommand, PlanDto>, UpdatePlanCommandHandler>();
        services.AddScoped<ICommandHandler<DeletePlanCommand, PlanDto>, DeletePlanCommandHandler>();

        services.AddScoped<ICommandHandler<CreateOrganizationPlanCommand, OrganizationPlanDto>, CreateOrganizationPlanCommandHandler>();
        services.AddScoped<ICommandHandler<UpdateOrganizationPlanCommand, OrganizationPlanDto>, UpdateOrganizationPlanCommandHandler>();
        services.AddScoped<ICommandHandler<DeleteOrganizationPlanCommand, OrganizationPlanDto>, DeleteOrganizationPlanCommandHandler>();

        services.AddScoped<ICommandHandler<CreateUserCommand, UserDto>, CreateUserCommandHandler>();
        services.AddScoped<ICommandHandler<UpdateUserCommand, UserDto>, UpdateUserCommandHandler>();
        services.AddScoped<ICommandHandler<DeleteUserCommand, UserDto>, DeleteUserCommandHandler>();

        services.AddScoped<ICommandHandler<CreateCustomerUserCommand, CustomerUserDto>, CreateCustomerUserCommandHandler>();
        services.AddScoped<ICommandHandler<UpdateCustomerUserCommand, CustomerUserDto>, UpdateCustomerUserCommandHandler>();
        services.AddScoped<ICommandHandler<DeleteCustomerUserCommand, CustomerUserDto>, DeleteCustomerUserCommandHandler>();

        services.AddScoped<ICommandHandler<CreateOrganizationUserCommand, OrganizationUserDto>, CreateOrganizationUserCommandHandler>();
        services.AddScoped<ICommandHandler<UpdateOrganizationUserCommand, OrganizationUserDto>, UpdateOrganizationUserCommandHandler>();
        services.AddScoped<ICommandHandler<DeleteOrganizationUserCommand, OrganizationUserDto>, DeleteOrganizationUserCommandHandler>();

        services.AddScoped<ICommandHandler<CreateArtifactTypeCommand, ArtifactTypeDto>, CreateArtifactTypeCommandHandler>();
        services.AddScoped<ICommandHandler<UpdateArtifactTypeCommand, ArtifactTypeDto>, UpdateArtifactTypeCommandHandler>();
        services.AddScoped<ICommandHandler<DeleteArtifactTypeCommand, ArtifactTypeDto>, DeleteArtifactTypeCommandHandler>();

        services.AddScoped<ICommandHandler<CreateArtifactCommand, ArtifactDto>, CreateArtifactCommandHandler>();
        services.AddScoped<ICommandHandler<UpdateArtifactCommand, ArtifactDto>, UpdateArtifactCommandHandler>();
        services.AddScoped<ICommandHandler<DeleteArtifactCommand, ArtifactDto>, DeleteArtifactCommandHandler>();

        services.AddScoped<ICommandHandler<CreateCustomerArtifactCommand, CustomerArtifactDto>, CreateCustomerArtifactCommandHandler>();
        services.AddScoped<ICommandHandler<UpdateCustomerArtifactCommand, CustomerArtifactDto>, UpdateCustomerArtifactCommandHandler>();
        services.AddScoped<ICommandHandler<DeleteCustomerArtifactCommand, CustomerArtifactDto>, DeleteCustomerArtifactCommandHandler>();

        services.AddScoped<ICommandHandler<CreateTeamCommand, TeamDto>, CreateTeamCommandHandler>();
        services.AddScoped<ICommandHandler<UpdateTeamCommand, TeamDto>, UpdateTeamCommandHandler>();
        services.AddScoped<ICommandHandler<DeleteTeamCommand, TeamDto>, DeleteTeamCommandHandler>();

        return services;
    }
}

