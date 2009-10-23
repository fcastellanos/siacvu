﻿<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="true"
    Inherits="System.Web.Mvc.ViewPage<GenericViewData<ArticuloForm>>" %>
<%@ Import Namespace="DecisionesInteligentes.Colef.Sia.Web.Controllers.Helpers"%>
<%@ Import Namespace="DecisionesInteligentes.Colef.Sia.Web.Controllers.Productos"%>
<%@ Import Namespace="DecisionesInteligentes.Colef.Sia.Web.Controllers.ViewData" %>
<%@ Import Namespace="DecisionesInteligentes.Colef.Sia.Web.Controllers.Models" %>
<%@ Import Namespace="DecisionesInteligentes.Colef.Sia.Web.Extensions" %>
<%@ Import Namespace="DI.Colef.Sia.Web.Controllers" %>

<asp:Content ID="titleContent" ContentPlaceHolderID="TituloPlaceHolder" runat="server">
    <h2>
        <%=Html.Encode(Model.Title) %>
    </h2>
</asp:Content>

<asp:Content ID="introductionContent" ContentPlaceHolderID="IntroduccionPlaceHolder" runat="server">
    <div id="introduccion">
        <p>
            Aqu&iacute; se muestra la informaci&oacute;n detallada del art&iacute;culo como est&aacute; en el sistema.
		</p>
    </div><!--end introduccion-->
</asp:Content>

<asp:Content ID="sidebarContent" ContentPlaceHolderID="SidebarContentPlaceHolder" runat="server">
    <div id="barra">
        <div id="asistente">
            <h3>Asistente de secci&oacute;n</h3>
            <% Html.RenderPartial("_ShowSidebar"); %>
        </div><!--end asistente-->

    </div><!--end barra-->
</asp:Content>

<asp:Content ID="indexContent" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <div id="textos">

        <div id="forma">
            <div id="campos">
                <h4>Datos del investigador</h4>
	            <p>
                    <label>Departamento</label>
                    <strong><%=Html.Encode(Model.Form.DepartamentoNombre)%>&nbsp;</strong>
                </p>
                <p>
                    <label>Sede</label>
                    <strong><%=Html.Encode(Model.Form.SedeNombre)%>&nbsp;</strong>
                </p>
            
    <!--DATOS PUBLICACION-->
                <h4>Datos de la publicaci&oacute;n</h4>
                <p>
                    <label>Nombre del art&iacute;culo</label>
                    <strong><%=Html.Encode(Model.Form.Titulo)%>&nbsp;</strong>
                </p>
                <p>
                    <label>Tipo de art&iacute;culo</label>
                    <strong><%=Html.Encode(Model.Form.TipoArticuloNombre)%>&nbsp;</strong>
                </p>
                
                <h4>Coautores</h4>
				<%
				    Html.RenderPartial("_ShowCoautorInterno",
				                       new CoautorForm
				                           {CoautoresInternos = Model.Form.CoautorInternoArticulos, ModelId = Model.Form.Id});%>
	            <%
				    Html.RenderPartial("_ShowCoautorExterno",
				                       new CoautorForm
				                           {CoautoresExternos = Model.Form.CoautorExternoArticulos, ModelId = Model.Form.Id});%>
	            <p>
	                <label>Autores</label>
	                <span id="totalcoautores" class="valor"><%=Html.Encode(Model.Form.TotalAutores)%></span>	          
	            </p>
	            
	            <p>
	                <label>Posici&oacute;n del autor</label>
                    <strong><%=Html.Encode(Model.Form.PosicionAutor)%>&nbsp;</strong>
                </p>
                <% if(Model.Form.RevistaPublicacionTitulo != ""){ %>
                    <p>
                        <label>Nombre de la revista</label>
                        <strong><%=Html.Encode(Model.Form.RevistaPublicacionTitulo)%>&nbsp;</strong>
                    </p>
                    
                    <% Html.RenderPartial("_ShowRevista", Model.Form.ShowFields); %>
                    
                    <p>
                        <label>&Iacute;ndice 1</label>
                        <strong><%=Html.Encode(Model.Form.RevistaPublicacionIndice1Nombre)%>&nbsp;</strong>
                    </p>
                    <p>
                        <label>&Iacute;ndice 2</label>
                        <strong><%=Html.Encode(Model.Form.RevistaPublicacionIndice2Nombre)%>&nbsp;</strong>
                    </p>
                    <p>
                        <label>&Iacute;ndice 3</label>
                        <strong><%=Html.Encode(Model.Form.RevistaPublicacionIndice3Nombre)%>&nbsp;</strong>
                    </p>
                <% } %>
                <p>
                    <label></label>
                    Tiene proyecto de investigaci&oacute;n de referencia?
                    <strong><%= HumanizeHelper.Boolean(Model.Form.TieneProyecto) %>&nbsp;</strong>
                </p>
                <% if (Model.Form.TieneProyecto) { %>
                    <p>
                        <label>Nombre del proyecto de investigaci&oacute;n</label>
                        <strong><%= Html.Encode(Model.Form.ProyectoNombre)%>&nbsp;</strong>
                    </p>
                    
                    <% Html.RenderPartial("_ShowProyecto", Model.Form.ShowFields); %>
                <% } else { %>
                
                    <% Html.RenderPartial("_ShowAreaTematica", Model.Form.ShowFields); %>
                <% } %>

                <p>
                    <label>Palabra clave 1</label>
                    <strong><%= Html.Encode(Model.Form.PalabraClave1)%>&nbsp;</strong>
                </p>
                <p>
                    <label>Palabra clave 2</label>
                    <strong><%= Html.Encode(Model.Form.PalabraClave2)%>&nbsp;</strong>
                </p>
                <p>
                    <label>Palabra clave 3</label>
                    <strong><%= Html.Encode(Model.Form.PalabraClave3)%>&nbsp;</strong>
                </p>
                <p>
                    <label>Estatus de la publicaci&oacute;n</label>
                    <strong><%= HumanizeHelper.EstadoProducto(Model.Form.EstadoProducto)%>&nbsp;</strong>
                </p>
                <% if (Model.Form.EstadoProducto == 1) { %>
                    <p>
                        <label>Fecha de aceptaci&oacute;n</label>
                        <strong><%= Html.Encode(Model.Form.FechaAceptacion)%>&nbsp;</strong><span>Formato (dd/mm/yyyy)</span>
                    </p>
                <% } if (Model.Form.EstadoProducto == 2){ %>
                    <p>
                        <label>Fecha de publicaci&oacute;n</label>
                        <strong><%= Html.Encode(Model.Form.FechaPublicacion)%>&nbsp;</strong><span>Formato (dd/mm/yyyy)</span>
                    </p>
                    
        <!--DATOS REFERENCIA BIBLIOGRAFICA-->
                    <h4>Referencia bibliogr&aacute;fica</h4>
                    <p>
                        <label>Volumen</label>
                        <strong><%= HumanizeHelper.Volumen(Model.Form.Volumen)%>&nbsp;</strong>
                    </p>
                    <p>
                        <label>N&uacute;mero</label>
                        <strong><%= Html.Encode(Model.Form.Numero)%>&nbsp;</strong>
                    </p>
                    <p>
                        <label>P&aacute;gina inicial</label> 
                        <strong><%= Html.Encode(Model.Form.PaginaInicial)%>&nbsp;</strong>
                    </p>
                    <p>    
                        <label>P&aacute;gina final</label>
                        <strong><%= Html.Encode(Model.Form.PaginaFinal)%>&nbsp;</strong>
                    </p>
                    <p>
                        <label>A&ntilde;o de publicaci&oacute;n</label>
                        <strong><%= Html.Encode(Model.Form.AnioPublicacion)%>&nbsp;</strong><span>Formato (yyyy)</span>
                    </p>
                    <p>
                        <label>A&ntilde;o de edici&oacute;n</label>    
                        <strong><%= Html.Encode(Model.Form.FechaEdicion)%>&nbsp;</strong><span>Formato (yyyy)</span>
                    </p>
                    <p>
                        <label></label>
                        Art&iacute;culo traducido a otro idioma? <strong><%= HumanizeHelper.Boolean(Model.Form.ArticuloTraducido)%>&nbsp;</strong>
                    </p>
                    <% if (Model.Form.ArticuloTraducido) { %>
                        <p>
                            <label>Idioma al que se tradujo</label>
                            <strong><%= Html.Encode(Model.Form.IdiomaNombre)%>&nbsp;</strong>
                        </p>
                    <% } %>
                <% } %>
                
                <p class="submit">                    
                    <%= Html.ActionLink<ArticuloController>(x => x.Index(), "Regresar") %>
                </p>
             </div><!--end campos-->
        </div><!--end lista-->

    </div><!--end textos-->
    
<script type="text/javascript">
    $(document).ready(function() {
        setupDocument();
    });
</script>
</asp:Content>
