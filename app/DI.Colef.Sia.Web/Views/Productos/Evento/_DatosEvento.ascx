﻿<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<EventoForm>" %>
<%@ Import Namespace="DecisionesInteligentes.Colef.Sia.Web.Controllers.Models" %>
<%@ Import Namespace="DecisionesInteligentes.Colef.Sia.Web.Extensions"%>

<p>
    <label>Nombre del evento</label>
    <%=Html.TextBox("Nombre", Model.Nombre, new { @class = "input420-bold-requerido", maxlength = 100 })%>
    <span class="cvu"></span>
    <%=Html.ValidationMessage("Nombre")%>
</p>

<p>
    <label>Tipo de evento</label>
    <%=Html.DropDownList("TipoEvento", Model.TiposEventos.CreateSelectList<TipoEventoForm>("Id", "Nombre"),
                "Seleccione ...", new { @class = "requerido" })%>
    <%=Html.ValidationMessage("TipoEvento") %>
</p>

<p class="OtroTipo">
    <label>Tipo</label>
    <%=Html.TextBox("OtroTipoEvento", Model.OtroTipoEvento, new { @class = "input420-requerido", maxlength = 100 })%>
    <%=Html.ValidationMessage("OtroTipoEvento")%>
</p>

<p>
    <label>Tipo de participaci&oacute;n</label>
    <%=Html.DropDownList("TipoParticipacion", Model.TiposParticipaciones.CreateSelectList<TipoParticipacionForm>("Id", "Nombre"),
                "Seleccione ...", new { @class = "requerido" })%>
    <span class="cvu"></span>
    <%=Html.ValidationMessage("TipoParticipacion")%>
</p>

<p>                     
    <label>Fechas del evento</label>
    <%=Html.TextBox("FechaInicial", Model.FechaInicial, new { @class = "datetime input100-requerido", maxlength = 10 })%> a 
    <%=Html.TextBox("FechaFinal", Model.FechaFinal, new { @class = "datetime input100-requerido", maxlength = 10 })%>
    <span>(Formato dd/mm/aaaa)</span>
    <span class="cvu"></span>
    <%=Html.ValidationMessage("FechaInicial")%>
    <%=Html.ValidationMessage("FechaFinal")%>
</p>

<p>
    <label>&Aacute;mbito</label>
    <%=Html.DropDownList("Ambito", Model.Ambitos.CreateSelectList<AmbitoForm>("Id", "Nombre"),
        "Seleccione ...", new { @class = "requerido" })%>
    <span class="cvu"></span>
    <%=Html.ValidationMessage("Ambito") %>
</p>

<% Html.RenderPartial("_EditInstitucion", Model ); %>
