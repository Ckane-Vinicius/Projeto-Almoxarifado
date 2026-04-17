using ApiAlmoxarifado.Data.Context;
using ApiAlmoxarifado.Data.Repository_Interfaces;
using ApiAlmoxarifado.Domain.Entities;

namespace ApiAlmoxarifado.Data.Repository
{
    public class ApplicationRepository : RepositoryBase<Registros>, IApplicationRepository
    {
        public ApplicationRepository(ApiContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Registros>> GetByCnpjAsync(string cnpj)
        {
            return context.Registros.Where(r => r.Cnpj == cnpj);
        }

        public async Task<IEnumerable<Registros>> GetTodosRegistros()
        {
            return context.Registros;
        }
    }
}
