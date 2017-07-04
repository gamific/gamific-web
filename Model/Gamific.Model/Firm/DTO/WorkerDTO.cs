using System.Runtime.Serialization;
using Vlast.Gamific.Account;
using Vlast.Gamific.Model.Firm.Domain;

namespace Vlast.Gamific.Model.Firm.DTO
{
    /// <summary>
    /// Classe com informações para edição de um funcionario
    /// </summary>
    public class WorkerDTO
    {
        public string Name { get; set; }

        public int IdWorker { get; set; }

        public int WorkerTypeId { get; set; }

        public int IdUser { get; set; }

        public int LogoId { get; set; }

        public string ExternalId { get; set; }

        public string Cpf { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Role { get; set; }

        // id da associação entre funcionario e equipe
        public int IdAssociation { get; set; }

        public string WorkerTypeName { get; set; }

        public Profiles ProfileName { get; set; }

        public int TotalPoints { get; set; }

        public int TotalXp { get; set; }

        public string FirmName { get; set; }

        public string ExternalFirmId { get; set; }

        public override bool Equals(System.Object obj)
        {
            if (obj == null)
            {
                return false;
            }

            WorkerDTO p = obj as WorkerDTO;
            if ((System.Object)p == null)
            {
                return false;
            }

            return p.IdWorker == this.IdWorker;
        }

    }
}
