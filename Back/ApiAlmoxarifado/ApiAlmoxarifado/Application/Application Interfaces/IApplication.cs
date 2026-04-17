using ApiAlmoxarifado.Domain.Entities;
using ApiAlmoxarifado.DTO;

namespace ApiAlmoxarifado.Application.Application_Interfaces
{
    public interface IApplication
    {
        Task<string> ProcessRequestAsync(Request request);
        Task<IEnumerable<Registros>> GetRegistrosAsync();
    }
}
