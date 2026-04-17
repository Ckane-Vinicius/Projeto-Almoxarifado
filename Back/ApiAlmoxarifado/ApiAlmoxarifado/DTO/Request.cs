using Newtonsoft.Json;

namespace ApiAlmoxarifado.DTO
{
    public class Request
    {
        [JsonProperty("empresa")]
        public required string Empresa { get; set; }
        [JsonProperty("cnpjempresa")]
        public required string CnpjEmpresa { get; set; }
        [JsonProperty("cnpj")]
        public required string Cnpj { get; set; }

        [JsonProperty("razaosocial")]
        public required string Razaosocial { get; set; }

        [JsonProperty("datapedido")]
        public required DateTime DataPedido { get; set; }

        [JsonProperty("ddl")]
        public List<int> Ddl { get; set; }

        [JsonProperty("valor")]
        public required string Valor { get; set; }

    }
}
