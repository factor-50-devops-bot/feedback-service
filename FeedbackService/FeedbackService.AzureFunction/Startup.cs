using AutoMapper;
using FeedbackService.Core.Interfaces.Repositories;
using FeedbackService.Handlers;
using FeedbackService.Repo;
using MediatR;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(FeedbackService.AzureFunction.Startup))]
namespace FeedbackService.AzureFunction
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddMediatR(typeof(FunctionAHandler).Assembly);
            builder.Services.AddTransient<IRepository, Repository>();
        }
    }
}