﻿using AutoMapper;
using Calabonga.AspNetCore.Controllers;
using Calabonga.AspNetCore.Controllers.Records;
using Calabonga.Chat.AuthServer.Entities;
using Calabonga.Chat.AuthServer.Web.ViewModels.LogViewModels;
using Calabonga.Microservices.Core.QueryParams;
using Calabonga.OperationResults;
using Calabonga.UnitOfWork;
using System.Threading;
using System.Threading.Tasks;

namespace Calabonga.Chat.AuthServer.Web.Mediator.LogsReadonly
{
    /// <summary>
    /// Request for paged list of Logs
    /// </summary>
    public record LogGetPagedRequest(PagedListQueryParams QueryParams) : OperationResultRequestBase<IPagedList<LogViewModel>>;

    /// <summary>
    /// Request for paged list of Logs
    /// </summary>
    public class LogGetPagedRequestHandler : OperationResultRequestHandlerBase<LogGetPagedRequest, IPagedList<LogViewModel>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public LogGetPagedRequestHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public override async Task<OperationResult<IPagedList<LogViewModel>>> Handle(LogGetPagedRequest request,
            CancellationToken cancellationToken)
        {
            var operation = OperationResult.CreateResult<IPagedList<LogViewModel>>();

            var pagedList = await _unitOfWork.GetRepository<Log>()
                .GetPagedListAsync(
                    pageIndex: request.QueryParams.PageIndex,
                    pageSize: request.QueryParams.PageSize,
                    cancellationToken: cancellationToken);

            if (pagedList == null)
            {
                operation.Result = PagedList.Empty<LogViewModel>();
                operation.AddWarning("'Repository.GetPagedList' does not return the result for pagination.");
                return operation;
            }

            if (pagedList.PageIndex >= pagedList.TotalPages)
            {
                pagedList = await _unitOfWork.GetRepository<Log>()
                    .GetPagedListAsync(
                        pageIndex: 0,
                        pageSize: request.QueryParams.PageSize, cancellationToken: cancellationToken);
            }

            operation.Result = _mapper.Map<IPagedList<LogViewModel>>(pagedList);
            return operation;
        }
    }
}
