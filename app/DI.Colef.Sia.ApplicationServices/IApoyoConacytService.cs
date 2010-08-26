using DecisionesInteligentes.Colef.Sia.Core;

namespace DecisionesInteligentes.Colef.Sia.ApplicationServices
{
	public interface IApoyoConacytService
    {
        ApoyoConacyt GetApoyoConacytById(int id);
        ApoyoConacyt[] GetAllApoyosConacyt();
        ApoyoConacyt[] GetActiveApoyosConacyt();
        ApoyoConacyt[] GetActiveApoyosConacyt(Usuario usuario);
        void SaveApoyoConacyt(ApoyoConacyt apoyoConacyt);
	    ApoyoConacyt[] GetAllApoyosConacyt(Usuario usuario);
    }
}
