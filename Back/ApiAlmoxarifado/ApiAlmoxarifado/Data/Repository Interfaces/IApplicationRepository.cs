using ApiAlmoxarifado.Domain.Entities;

namespace ApiAlmoxarifado.Data.Repository_Interfaces
{
    public interface IApplicationRepository : IRepositoryBase<Registros>
    {
        Task<IEnumerable<Registros>> GetByCnpjAsync(string cnpj);

        Task<IEnumerable<Registros>> GetTodosRegistros();
    }
}
