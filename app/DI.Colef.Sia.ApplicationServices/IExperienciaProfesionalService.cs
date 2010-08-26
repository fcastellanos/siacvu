using DecisionesInteligentes.Colef.Sia.Core;

namespace DecisionesInteligentes.Colef.Sia.ApplicationServices
{
	public interface IExperienciaProfesionalService
    {
        ExperienciaProfesional GetExperienciaProfesionalById(int id);
        ExperienciaProfesional[] GetAllExperienciasProfesionales();
        ExperienciaProfesional[] GetActiveExperienciasProfesionales();
	    ExperienciaProfesional[] GetActiveExperienciasProfesionales(Usuario usuario);
        void SaveExperienciaProfesional(ExperienciaProfesional experienciaProfesional);
	    ExperienciaProfesional[] GetAllExperienciasProfesionales(Usuario usuario);
    }
}
