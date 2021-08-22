using Calabonga.Chat.API.Entities;
using Calabonga.Chat.API.Web.Infrastructure.Mappers.Base;
using Calabonga.Chat.API.Web.ViewModels.LogViewModels;
using Calabonga.UnitOfWork;

namespace Calabonga.Chat.API.Web.Infrastructure.Mappers
{
    /// <summary>
    /// Mapper Configuration for entity Log
    /// </summary>
    public class LogMapperConfiguration : MapperConfigurationBase
    {
        /// <inheritdoc />
        public LogMapperConfiguration()
        {
            CreateMap<LogCreateViewModel, Log>()
                .ForMember(x => x.Id, o => o.Ignore());

            CreateMap<Log, LogViewModel>();

            CreateMap<IPagedList<Log>, IPagedList<LogViewModel>>()
                .ConvertUsing<PagedListConverter<Log, LogViewModel>>();
        }
    }
}
