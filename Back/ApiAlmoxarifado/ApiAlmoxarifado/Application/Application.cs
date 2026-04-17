using System.Text.RegularExpressions;
using ApiAlmoxarifado.Application.Application_Interfaces;
using ApiAlmoxarifado.Domain.Entities;
using ApiAlmoxarifado.Domain.Services_Interfaces;
using ApiAlmoxarifado.DTO;

namespace ApiAlmoxarifado.Application
{
    public class Application : IApplication
    {
        private readonly IApplicationServices _applicationServices;

        public Application(IApplicationServices applicationServices)
        {
            _applicationServices = applicationServices;
        }

        public async Task<string> ProcessRequestAsync(Request request)
        {

            var cnpjcorreto = Regex.Replace(request.Cnpj, "[^0-9]", "");

            var RegistroExistente = await _applicationServices.GetByCnpjAsync(cnpjcorreto);

            foreach (var ddl in request.Ddl)
            {
                var data_pagamento = request.DataPedido.AddDays(ddl);

                foreach (var registro in RegistroExistente)
                {
                    if (registro.Data_pagamento.Date == data_pagamento.Date && registro.CnpjEmpresa == request.CnpjEmpresa)
                    {
                        return $"Já existe uma DDL com essa data de pagamento DDL: {ddl}";
                    }
                }

                var cnpjEmpresaCorreto = Regex.Replace(request.CnpjEmpresa, "[^0-9]", "");

                var Registro = new Registros
                {
                    Empresa = request.Empresa,
                    CnpjEmpresa = cnpjEmpresaCorreto,
                    Cnpj = cnpjcorreto,
                    RazaoSocial = request.Razaosocial,
                    DDL = ddl,
                    Valor = request.Valor,
                    Data_pagamento = data_pagamento,
                    Data_entrada = DateTime.Now,
                    Data_pedido = data_pagamento
                };

                await _applicationServices.AddAsync(Registro);
            }

            return $"DDls Cadastrada: {request.Razaosocial}";
        }

        public async Task<IEnumerable<Registros>> GetRegistrosAsync()
        {
            var RegistroExistente = await _applicationServices.GetTodosRegistros();

            return RegistroExistente;
        }
    }
}
