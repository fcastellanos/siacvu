﻿<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<EstanciaAcademicaExternaForm>" %>
<%@ Import Namespace="DecisionesInteligentes.Colef.Sia.Web.Extensions"%>
<%@ Import Namespace="DecisionesInteligentes.Colef.Sia.Web.Controllers.Models"%>
<h4>Datos de la estancia</h4>
<p>
    <label>Nombre del investigador externo</label>
    <%=Html.TextBox("InvestigadorExterno", Model.InvestigadorExterno, new { @class = "input420-requerido", maxlength = 40 })%>
    <%=Html.ValidationMessage("InvestigadorExterno")%>
</p>
<p>
    <label>Grado acad&eacute;mico</label>
    <%=Html.DropDownList("GradoAcademico", Model.GradosAcademicos.CreateSelectList<GradoAcademicoForm>("Id", "Nombre"),
		"Seleccione ...", new { @class = "requerido" })%>
    <%=Html.ValidationMessage("GradoAcademico")%>
</p>
<p>
    <label>Tipo de estancia</label>
    <%=Html.DropDownList("TipoEstancia", Model.TiposEstancias.CreateSelectList<TipoEstanciaForm>("Id", "Nombre"),
		"Seleccione ...", new { @class = "requerido" })%>
    <%=Html.ValidationMessage("TipoEstancia") %>
</p>

<% Html.RenderPartial("_ShowInstitucionLong", Model); %>

<p>
	<label>L&iacute;neas de investigaci&oacute;n</label>
	<%=Html.TextBox("LineasInvestigacion", Model.LineasInvestigacion, new { @class = "input420-requerido", maxlength = 100 })%>
	<%=Html.ValidationMessage("LineasInvestigacion")%>
</p>
<p>
    <label>Departamento de adscripci&oacute;n</label>
    <%=Html.DropDownList("Departamento", Model.Departamentos.CreateSelectList<DepartamentoForm>("Id", "Nombre"),
        "Seleccione ...", new { @class = "requerido" })%>
    <span class="cvu"></span>
    <%=Html.ValidationMessage("Departamento")%>
</p>
<p>
    <label>Sede</label>
    <%=Html.DropDownList("Sede", Model.Sedes.CreateSelectList<SedeForm>("Id", "Nombre"),
        "Seleccione ...", new { @class = "requerido cascade", rel = Url.Action("ChangeSede") })%>
    <span class="cvu"></span>
    <%=Html.ValidationMessage("Sede")%>
</p>
<p>
    <label>Adscripci&oacute;n regional:</label>
    <span id="span_direccionregional" class="valor"><%=Html.Encode(Model.SedeDireccionRegionalNombre)%>&nbsp;</span>
</p>
<p>
	<label>Fecha de inicio</label>
	<%=Html.TextBox("FechaInicial", Model.FechaInicial, new { @class="datetime input100", maxlength = 10 })%>
	<span>(Formato dd/mm/aaaa)</span>
	<%=Html.ValidationMessage("FechaInicial")%>
</p>
<p>
	<label>Fecha de conclusi&oacute;n</label>
	<%=Html.TextBox("FechaFinal", Model.FechaFinal, new { @class="datetime input100", maxlength = 10 })%>
	<span>(Formato dd/mm/aaaa)</span>
	<%=Html.ValidationMessage("FechaFinal")%>
</p>
<p>
	<label>Actividades acad&eacute;micas previstas</label>
	<%=Html.TextArea("Actividades", Model.Actividades, 5, 35, new { @class = "input420-requerido", maxlength = 100 })%>
	<%=Html.ValidationMessage("Actividades")%>
</p>
<p>
	<label>Principales logros</label>
	<%=Html.TextArea("Logros", Model.Logros, 5, 35, new { @class = "input420-requerido", maxlength = 100 })%>
	<%=Html.ValidationMessage("Logros")%>
</p>