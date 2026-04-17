using ApiAlmoxarifado.Data.Repository_Interfaces;
using ApiAlmoxarifado.Domain.Entities;
using ApiAlmoxarifado.Domain.Services_Interfaces;

namespace ApiAlmoxarifado.Domain.Services
{
    public class ApplicationServices(IApplicationRepository applicationRepository) : ServiceBase<Registros>(applicationRepository), IApplicationServices
    {
        public async Task<IEnumerable<Registros>> GetByCnpjAsync(string cnpj)
        {
            return await applicationRepository.GetByCnpjAsync(cnpj);
        }

        public async Task<IEnumerable<Registros>> GetTodosRegistros()
        {
            return await applicationRepository.GetTodosRegistros();
        }
    }
}
