<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="true" 
    Inherits="System.Web.Mvc.ViewPage<GenericViewData<ExperienciaProfesionalForm>>" %>
<%@ Import Namespace="DecisionesInteligentes.Colef.Sia.Web.Controllers"%>
<%@ Import Namespace="DecisionesInteligentes.Colef.Sia.Web.Controllers.ViewData"%>
<%@ Import Namespace="DecisionesInteligentes.Colef.Sia.Web.Controllers.Models"%>
<%@ Import Namespace="DI.Colef.Sia.Web.Controllers" %>
<%@ Import Namespace="DecisionesInteligentes.Colef.Sia.Web.Controllers.Helpers" %>
<%@ Import Namespace="DecisionesInteligentes.Colef.Sia.Core" %>

<asp:Content ID="titleContent" ContentPlaceHolderID="TituloPlaceHolder" runat="server">
    <h2> <%=Html.ProductoListTitle(TipoProductoEnum.ExperienciaProfesional) %></h2>
</asp:Content>

<asp:Content ID="introductionContent" ContentPlaceHolderID="IntroduccionPlaceHolder" runat="server">
    <div id="subcontenido">
        <h3> <%=Html.ProductoListSubTitle(TipoProductoEnum.ExperienciaProfesional) %></h3>
        <p>
            <%=Html.ProductoListMessage(TipoProductoEnum.ExperienciaProfesional) %>
		</p>
        <div class="botonzon">
            <span><%=Html.ActionLink<ExperienciaProfesionalController>(x => x.New(), Model.GetObjectNameNewButton())%></span>
        </div>
    </div>
</asp:Content>

<asp:Content ID="sidebarContent" ContentPlaceHolderID="SidebarContentPlaceHolder" runat="server">
    <div id="barra">
        <div id="asistente">
            <h3>Asistente de secci&oacute;n</h3>
            <p>Lista de experiencia profesional registrada en el sistema.</p>
            <% Html.RenderPartial("_ListSidebar"); %>
        </div><!--end asistente-->
    </div><!--end barra-->
</asp:Content>

<asp:Content ID="indexContent" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
<div id="textos">
	
	<% Html.RenderPartial("_Message"); %>
	<% Html.RenderPartial("_Search"); %>
	
	<div id="lista">
		<h4>Experiencias profesionales</h4>
            
		<% if (Model.List == null || Model.List.Length == 0) { %>
			<div class="elementolista">
				<div class="elementodescripcion">
					<h5><span>No hay experiencias profesionales registradas</span></h5>
				</div><!--end elementodescripcion-->

			</div><!--end elementolista-->
		<% } else { %>
			<% foreach (var experienciaProfesional in Model.List) { %>
				<div class="elementolista" id="accion_<%=Html.Encode(experienciaProfesional.Id) %>_0">
					<div class="elementodescripcion">
					    <h5><span><%=Html.Encode(experienciaProfesional.Nombramiento)%></span></h5>
						<h6>Modificado el <%=Html.Encode(experienciaProfesional.Modificacion) %></h6>
					</div><!--end elementodescripcion-->

					<div class="elementobotones">
						<p>
							<span><%=Html.ActionLink<ExperienciaProfesionalController>(x => x.Edit(experienciaProfesional.Id), "Editar") %></span>
	                        <span><%=Html.ActionLink<ExperienciaProfesionalController>(x => x.Deactivate(experienciaProfesional.Id), "Borrar", new { @class = "button-delete" })%></span>
	                   	</p>
					</div><!--end elementobotones-->
		
               </div><!--end elementolista-->
			<% } %>
		<% } %>
            
	</div><!--end lista-->

</div><!--end textos-->

<script type="text/javascript">
    $(document).ready(function() {
        setupDocument();
    });
</script>

</asp:Content>