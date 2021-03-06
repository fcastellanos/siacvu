﻿<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<InvestigadorForm>" %>
<%@ Import Namespace="DecisionesInteligentes.Colef.Sia.Web.Controllers"%>
<%@ Import Namespace="DecisionesInteligentes.Colef.Sia.Web.Extensions"%>
<%@ Import Namespace="DecisionesInteligentes.Colef.Sia.Web.Controllers.Models"%>
<div id="sniList" class="minilista">
	<h5>SNI</h5>
	<% foreach(var sni in Model.SNIsInvestigador) { %>
        <div id="estado_<%=Html.Encode(sni.Id) %>" class="sublista">
            <h6><a href="<%=Url.Action("DeleteSni", null, new{id = sni.Id, investigadorId = Model.Id}) %>" class="remote delete"><img src="<%=ResolveUrl("~/Content/Images/eliminar-icon.png") %>" /></a>
    	    <%=Html.Encode(sni.SNINombre)%> <%if(sni.SNINombre != "No pertence") { %> <span><%=Html.Encode(sni.FechaInicial) %> <% if(sni.SNINombre != "Emerito") { %> a <%=Html.Encode(sni.FechaFinal) %><% } %></span> <% } %>
    	    <%if(!String.IsNullOrEmpty(sni.ComprobanteNombre)) { %> 
    	    Comprobante: <%=Html.ActionLink<ArchivoController>(x => x.Show(sni.ComprobanteId), sni.ComprobanteNombre, new {target="_blank"})%> <% } %></h6>
        </div><!--end estadolista-->
    <% } %>
    
    <div id="sniNew" class="minilistaboton">
		<p><span><%=Html.ActionLink("+ Nuevo SNI", "NewSNI", new { Id = Model.Id }, new { @class = "remote get" })%></span></p>
	</div><!--end minilistaboton-->	
          
</div><!--end minilista-->

<div id="sniForm" class="display:hidden;"></div>