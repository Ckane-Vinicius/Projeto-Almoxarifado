using ApiAlmoxarifado.Domain.Entities;

namespace ApiAlmoxarifado.Domain.Services_Interfaces
{
    public interface IApplicationServices : IServiceBase<Registros>
    {
        Task<IEnumerable<Registros>> GetByCnpjAsync(string cnpj);

        Task<IEnumerable<Registros>> GetTodosRegistros();
    }
}
