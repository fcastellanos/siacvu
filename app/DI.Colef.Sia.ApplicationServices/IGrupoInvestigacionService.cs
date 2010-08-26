using DecisionesInteligentes.Colef.Sia.Core;

namespace DecisionesInteligentes.Colef.Sia.ApplicationServices
{
	public interface IGrupoInvestigacionService
    {
        GrupoInvestigacion GetGrupoInvestigacionById(int id);
        GrupoInvestigacion[] GetAllGrupoInvestigacions();
        GrupoInvestigacion[] GetActiveGrupoInvestigacions();
	    GrupoInvestigacion[] GetActiveGrupoInvestigacions(Usuario usuario);
        void SaveGrupoInvestigacion(GrupoInvestigacion grupoInvestigacion);
	    GrupoInvestigacion[] GetAllGrupoInvestigacions(Usuario usuario);
    }
}
