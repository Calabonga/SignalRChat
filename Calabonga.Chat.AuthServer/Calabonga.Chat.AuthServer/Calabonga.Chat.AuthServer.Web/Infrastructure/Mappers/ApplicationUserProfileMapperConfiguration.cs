using Calabonga.Chat.AuthServer.Data;
using Calabonga.Chat.AuthServer.Web.Infrastructure.Mappers.Base;
using Calabonga.Chat.AuthServer.Web.ViewModels.AccountViewModels;

namespace Calabonga.Chat.AuthServer.Web.Infrastructure.Mappers
{
    /// <summary>
    /// Mapper Configuration for entity Person
    /// </summary>
    public class ApplicationUserProfileMapperConfiguration : MapperConfigurationBase
    {
        /// <inheritdoc />
        public ApplicationUserProfileMapperConfiguration() => CreateMap<RegisterViewModel, ApplicationUserProfile>()
            .ForAllOtherMembers(x => x.Ignore());
    }
}