namespace DecisionesInteligentes.Colef.Sia.Web.Controllers.Models
{
    public class ObraTraducidaForm : BaseForm
    {
		public string Nombre { get; set; }
		public string NombreTraductor { get; set; }
		public string ApellidoPaterno { get; set; }
		public string ApellidoMaterno { get; set; }
		public string NombreObraTraducida { get; set; }
		public int TipoObraTraducida { get; set; }
		public string PalabraClave1 { get; set; }
		public string PalabraClave2 { get; set; }
		public string PalabraClave3 { get; set; }
		public int EstadoProducto { get; set; }
		public string FechaAceptacion { get; set; }
		public string FechaPublicacion { get; set; }
		public string Volumen { get; set; }
        public int Edicion { get; set; }
        public string Numero { get; set; }
		public int PaginaInicial { get; set; }
		public int PaginaFinal { get; set; }
		public string NombreLibro { get; set; }
		public int TipoLibro { get; set; }
		public string Resumen { get; set; }
        public int NoCitas { get; set; }
		public string ISBN { get; set; }
		public int Reimpresion { get; set; }
		public int Tiraje { get; set; }
        public string DepartamentoNombre { get; set; }
        public string SedeNombre { get; set; }
		public bool Activo { get; set; }
		public string Modificacion { get; set; }
        public bool CoautorSeOrdenaAlfabeticamente { get; set; }
        public bool AutorSeOrdenaAlfabeticamente { get; set; }
        public int PosicionAutor { get; set; }
        public int NoPaginas { get; set; }

        public int Pais { get; set; }
        public int PaisId { get; set; }
        public string PaisNombre { get; set; }

        public int Idioma { get; set; }
        public int IdiomaId { get; set; }
        public string IdiomaNombre { get; set; }

        public int TotalCoautores
        {
            get
            {
                return (CoautorExternoObraTraducidas == null ? 0 : CoautorExternoObraTraducidas.Length) +
                    (CoautorInternoObraTraducidas == null ? 0 : CoautorInternoObraTraducidas.Length) + 1;
            }
        }

        public int TotalAutores
        {
            get
            {
                return (AutorExternoObraTraducidas == null ? 0 : AutorExternoObraTraducidas.Length) +
                       (AutorInternoObraTraducidas == null ? 0 : AutorInternoObraTraducidas.Length) + 1;
            }
        }

        public string NombreCompleto
        {
            get
            {
                return string.Format("{0} {1} {2}", NombreTraductor, ApellidoPaterno, ApellidoMaterno);
            }
        }

        public EditorialProductoForm[] EditorialObraTraducidas { get; set; }
        public override EditorialProductoForm[] Editoriales
        {
            get { return EditorialObraTraducidas; }
        }

        public CoautorExternoProductoForm[] CoautorExternoObraTraducidas { get; set; }
        public CoautorInternoProductoForm[] CoautorInternoObraTraducidas { get; set; }
        public AutorInternoProductoForm[] AutorInternoObraTraducidas { get; set; }
        public AutorExternoProductoForm[] AutorExternoObraTraducidas { get; set; }
        public ArchivoForm[] ArchivosObraTraducida { get; set; }

        public override ArchivoForm[] Archivos
        {
            get { return ArchivosObraTraducida; }
        }
        /* New */
        public CoautorExternoProductoForm CoautorExternoProducto { get; set; }
        public CoautorInternoProductoForm CoautorInternoProducto { get; set; }
        public AutorInternoProductoForm AutorInternoProducto { get; set; }
        public AutorExternoProductoForm AutorExternoProducto { get; set; }
        public EditorialProductoForm EditorialProducto { get; set; }

        /* Show */
        public ShowFieldsForm ShowFields { get; set; }
        public AreaTematicaForm AreaTematica { get; set; }
        public RevistaPublicacionForm RevistaPublicacion { get; set; }

        /* Catalogos */
        public CustomSelectForm[] TiposObraTraducidas { get; set; }
        public CustomSelectForm[] EstadosProductos { get; set; }
        public CustomSelectForm[] TiposLibro { get; set; }
		public IdiomaForm[] Idiomas { get; set; }
        public CustomSelectForm[] Reimpresiones { get; set; }
        public CustomSelectForm[] Ediciones { get; set; }
        public PaisForm[] Paises { get; set; }
    }
}