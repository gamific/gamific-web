namespace Vlast.Gamific.Model.Firm.DTO
{
    /// <summary>
    /// Classe que representa o carro do usuario
    /// </summary>
    public class WorkerCarDTO
    {
        public string Name { get; set; }

        public int IdTarget { get; set; }

        public bool isFirstPerson { get; set; }

        public int Points { get; set; }

        public string CarColor1 { get; set; }

        public string CarColor2 { get; set; }

        public string HelmetColor { get; set; }

        public int LogoId { get; set; }

        public string AvatarPath { get; set; }

    }
}
