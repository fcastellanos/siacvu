﻿using System;

namespace DecisionesInteligentes.Colef.Sia.Core.DataInterfaces
{
    public class ProductoDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int TipoProducto { get; set; }
        public DateTime CreadoEl { get; set; }
        //public int TipoPublicacion { get; set; }
        //public int EstatusProducto { get; set; }
        //public RevistaPublicacion RevistaPublicacion { get; set; }
        //public Institucion Institucion { get; set; }
        //public TipoDictamen TipoDictamen { get; set; }
        //public TipoOrgano TipoOrgano { get; set; }
        //public TipoEvento TipoEvento { get; set; }
        //public TipoParticipacion TipoParticipacion { get; set; }
        //public Firma Firma { get; set; }
        //public Usuario Usuario { get; set; }
        public int Aceptacion2 { get; set; }
        public int Aceptacion1 { get; set; }
        //public int GuidNumber { get; set; }

        public int Estatus { get; set; }
        public string RevistaNombre { get; set; }
        public string InstitucionNombre { get; set; }
        public int Tipo { get; set; }
        public string TipoNombre { get; set; }

        public string UsuarioApellidoMaterno { get; set; }
        public string UsuarioApellidoPaterno { get; set; }
        public string UsuarioNombre { get; set; }

        public int FirmaAceptacion1 { get; set; }
        public int FirmaAceptacion2 { get; set; }

        public string InvestigadorNombre
        {
            get
            {
                return string.Format("{0} {1} {2}", UsuarioApellidoPaterno, UsuarioApellidoMaterno, UsuarioNombre);
            }
        }

        public string FechaCreacion
        {
            get { return CreadoEl.ToString("dd MMM, yyyy"); }
        }

        public bool IsFirmed()
        {
            return FirmaAceptacion1 == 1 & FirmaAceptacion2 != 1;
        }

        public bool IsValidated()
        {
            return FirmaAceptacion2 == 1;
        }

        public bool IsRejected()
        {
            return FirmaAceptacion2 == 2;
        }
    }
}