using FeedbackService.Core.Domains.Entities;
using FeedbackService.Core.Interfaces.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FeedbackService.Handlers
{
    public class FunctionAHandler : IRequestHandler<FunctionARequest, FunctionAResponse>
    {
        private readonly IRepository _repository;

        public FunctionAHandler(IRepository repository)
        {
            _repository = repository;
        }

        public Task<FunctionAResponse> Handle(FunctionARequest request, CancellationToken cancellationToken)
        {
            var response = new FunctionAResponse()
            {
                Status = "Active"
            };
            return Task.FromResult(response);
        }
    }
}
