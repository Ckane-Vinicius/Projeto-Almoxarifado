namespace ApiAlmoxarifado.Domain.Entities
{
    public class Registros : Entity
    {
        public required string CnpjEmpresa { get; set; }
        public required string Empresa { get; set; }
        public required string Cnpj { get; set; }
        public required string RazaoSocial { get; set; }
        public required string Valor { get; set; }
        public int DDL { get; set; }
        public DateTime Data_pagamento { get; set; }
        public DateTime Data_entrada { get; set; }
    }
}
