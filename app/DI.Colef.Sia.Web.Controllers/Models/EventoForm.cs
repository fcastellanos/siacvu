namespace DecisionesInteligentes.Colef.Sia.Web.Controllers.Models
{
    public class EventoForm : BaseForm
    {
        public string Nombre { get; set; }
        public string TituloTrabajo { get; set; }
        public bool Invitacion { get; set; }
        public string FechaInicial { get; set; }
        public string FechaFinal { get; set; }
        public string FechaEvento { get; set; }
        public string PalabraClave1 { get; set; }
        public string PalabraClave2 { get; set; }
        public string PalabraClave3 { get; set; }
        public bool Activo { get; set; }
        public string Modificacion { get; set; }
        public string DepartamentoNombre { get; set; }
        public string SedeNombre { get; set; }
        public string ObjetivoEvento { get; set; }
        public int FinanciamientoInterno { get; set; }
        public int FinanciamientoExterno { get; set; }
        public int SesionesTrabajo { get; set; }
        public bool CoautorSeOrdenaAlfabeticamente { get; set; }

        public string OtroTipoEvento{ get; set;}

        public int TipoEvento { get; set; }
        public int TipoEventoId { get; set; }
        public string TipoEventoNombre { get; set; }

        public int Ambito { get; set; }
        public int AmbitoId { get; set; }
        public string AmbitoNombre { get; set; }

        public int TipoParticipacion { get; set; }
        public int TipoParticipacionId { get; set; }
        public string TipoParticipacionNombre { get; set; }

        public int TotalCoautores
        {
            get
            {
                return (CoautorExternoEventos == null ? 0 : CoautorExternoEventos.Length) +
                    (CoautorInternoEventos == null ? 0 : CoautorInternoEventos.Length) + 1;
            }
        }

        public CoautorExternoProductoForm[] CoautorExternoEventos { get; set; }
        public CoautorInternoProductoForm[] CoautorInternoEventos { get; set; }
        public InstitucionProductoForm[] InstitucionEventos { get; set; }
        public SesionEventoForm[] SesionEventos { get; set; }
        public ArchivoForm[] ArchivosEvento { get; set; }

        public override InstitucionProductoForm[] Instituciones
        {
            get { return InstitucionEventos; }
        }

        public override ArchivoForm[] Archivos
        {
            get { return ArchivosEvento; }
        }

        /* New */
        public CoautorExternoProductoForm CoautorExternoProducto { get; set; }
        public CoautorInternoProductoForm CoautorInternoProducto { get; set; }
        public InstitucionProductoForm InstitucionProducto { get; set; }
        public SesionEventoForm SesionEvento { get; set; }

        /*Show*/
        public ShowFieldsForm ShowFields { get; set; }
        public AreaTematicaForm AreaTematica { get; set; }

        /* Catalogos */
        public AmbitoForm[] Ambitos { get; set; }
        public TipoEventoForm[] TiposEventos { get; set; }
        public TipoParticipacionForm[] TiposParticipaciones { get; set; }
    }
}
