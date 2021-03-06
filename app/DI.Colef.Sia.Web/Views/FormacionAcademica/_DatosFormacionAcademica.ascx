﻿<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<FormacionAcademicaForm>" %>
<%@ Import Namespace="DecisionesInteligentes.Colef.Sia.Web.Extensions"%>
<%@ Import Namespace="DecisionesInteligentes.Colef.Sia.Web.Controllers.Models"%>
<p>
    <label>Nivel/Grado acad&eacute;mico</label>
    <%=Html.DropDownList("NivelEstudio", Model.NivelesEstudios.CreateSelectList<NivelEstudioForm>("Id", "Nombre"),
		"Seleccione ...", new { @class = "requerido" })%>
    <span class="cvu"></span>
    <%=Html.ValidationMessage("NivelEstudio") %>
</p>
<p>
    <label>N&uacute;mero de c&eacute;dula profesional</label>
    <%=Html.TextBox("NumeroCedula", Model.NumeroCedula, new { @class = "input250", maxlength = 20 })%>
    <span class="cvu"></span>
    <%=Html.ValidationMessage("NumeroCedula")%>
</p>
<p>
    <label>T&iacute;tulo de Nivel/Grado acad&eacute;mico</label>
    <%=Html.TextBox("TituloGrado", Model.TituloGrado, new { @class = "input420-bold-requerido", maxlength = 100 })%>
    <%=Html.ValidationMessage("TituloGrado")%>
</p>
<p>
    <label>Fecha de obtenci&oacute;n</label>
    <%=Html.TextBox("FechaObtencion", Model.FechaObtencion, new { @class = "datetime input100-requerido", maxlength = 10 })%>
    <span>(Formato dd/mm/aaaa)</span>
    <span class="cvu"></span>
    <%=Html.ValidationMessage("FechaObtencion")%>
</p>

<% Html.RenderPartial("_ShowInstitucionLong", Model); %>

<p>
    <label>T&iacute;tulo de tesis</label>
    <%=Html.TextBox("TituloTesis", Model.TituloTesis, new { @class = "input420-requerido", maxlength = 100 })%>
    <span class="cvu"></span>
    <%=Html.ValidationMessage("TituloTesis")%>
</p>

<p>
    <label>Estatus</label>
    <%=Html.DropDownList("Estatus", Model.EstatusFormacionAcademicas.CreateSelectList<EstatusFormacionAcademicaForm>("Id", "Nombre"),
        "Seleccione ...", new { @class = "requerido" })%>
    <span class="cvu"></span>
    <%=Html.ValidationMessage("Estatus")%>
</p>

<p>
    <label>Pa&iacute;s</label>
    <%=Html.DropDownList("Pais", Model.Paises.CreateSelectList<PaisForm>("Id", "Nombre"),
        "Seleccione ...", new { @class = "requerido cascade", rel = Url.Action("ChangePais") })%>
    <span class="cvu"></span>
    <%=Html.ValidationMessage("Pais") %>
</p>
<p>
    <label>Estado</label>
    <%=Html.DropDownList("EstadoPais", Model.EstadosPaises.CreateSelectList<EstadoPaisForm>("Id", "Nombre"),
        "Seleccione ...", new { @class = "requerido" })%>
    <span class="cvu"></span>
    <%=Html.ValidationMessage("EstadoPais") %>
</p>
<p>
    <label>Ciudad</label>
    <%=Html.TextBox("Ciudad", Model.Ciudad, new { @class = "input420-requerido", maxlength = 100 })%>
    <%=Html.ValidationMessage("Ciudad")%>
</p>

<% Html.RenderPartial("_Show2doNivel", new ShowFieldsForm { Sectores = Model.Sectores, Organizaciones = Model.Organizaciones, Niveles = Model.Niveles, IsShowForm = false }); %>
<% Html.RenderPartial("_ShowSubdisciplina", new ShowFieldsForm { Areas = Model.Areas, Disciplinas = Model.Disciplinas, Subdisciplinas = Model.Subdisciplinas, IsShowForm = false }); %>