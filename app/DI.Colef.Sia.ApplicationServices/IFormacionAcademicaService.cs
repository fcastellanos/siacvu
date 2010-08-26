using DecisionesInteligentes.Colef.Sia.Core;

namespace DecisionesInteligentes.Colef.Sia.ApplicationServices
{
	public interface IFormacionAcademicaService
    {
        FormacionAcademica GetFormacionAcademicaById(int id);
        FormacionAcademica[] GetAllFormacionAcademicas();
        FormacionAcademica[] GetActiveFormacionAcademicas();
	    FormacionAcademica[] GetActiveFormacionAcademicas(Usuario usuario);
        void SaveFormacionAcademica(FormacionAcademica formacionAcademica);
	    FormacionAcademica[] GetAllFormacionAcademicas(Usuario usuario);
    }
}
