using System.ComponentModel.DataAnnotations;

namespace Vlast.Gamific.Model.Firm.DTO
{
    public enum Icons
    {
        [Display(Name = "Dinheiro")]
        fa_usd,

        [Display(Name = "Ligações")]
        fa_phone,

        [Display(Name = "Visitas")]
        fa_users,

        [Display(Name = "Ideias")]
        fa_lightbulb_o,

        [Display(Name = "Pedidos")]
        fa_bar_chart_o,

        [Display(Name = "Avião")]
        fa_plane,

        [Display(Name = "Gráfico pizza")]
        fa_pie_chart,

        [Display(Name = "Comentario")]
        fa_commenting,

        [Display(Name = "Envelope")]
        fa_envelope,

        [Display(Name = "Relógio")]
        fa_clock_o,

        //[Display(Name = "Aperto de mãos")]
        //fa_handshake_o,

        [Display(Name = "Map pin")]
        fa_map_pin,

        [Display(Name = "Troféu")]
        fa_trophy,

        [Display(Name = "grafico area")]
        fa_area_chart,

        [Display(Name = "Balança")]
        fa_balance_scale,

        [Display(Name = "inseto")]
        fa_bug,

        [Display(Name = "folha")]
        fa_leaf,

        [Display(Name = "caminhao")]
        fa_truck,

        [Display(Name = "grafico linha")]
        fa_line_chart,

        [Display(Name = "hospital")]
        fa_hospital_o,

        [Display(Name = "ambulancia")]
        fa_ambulance
    }

    public enum Params
    {
        GRAFICO_PRODUTOS,
        GRAFICO_HISTOGRAMO,
        GRAFICO_EVOLUCAO
    }

    public enum Target3D
    {
        TEAM,
        PLAYER
    }

    public enum Profiles
    {
        ADMINISTRADOR,
        JOGADOR,
        LIDER
    }

}