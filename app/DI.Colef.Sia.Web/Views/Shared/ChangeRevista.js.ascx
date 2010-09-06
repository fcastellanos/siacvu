<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<ShowFieldsForm>" %>
<%@ Import Namespace="DecisionesInteligentes.Colef.Sia.Web.Controllers.Models"%>

$('#institucionrevista').slideUp();
$('#indice1revista').slideUp();
$('#indice2revista').slideUp();
$('#indice3revista').slideUp();
$('#paisrevista').slideUp();

<% if(Model.RevistaPublicacionId != 0){ %>

    <% if(!String.IsNullOrEmpty(Model.RevistaPublicacionInstitucionNombre)){ %>
        $('#institucionrevista').slideDown();
        $('#span_institucionrevista').html('<%=Html.Encode(Model.RevistaPublicacionInstitucionNombre)%>&nbsp;');
    <% } %>

    <% if(!String.IsNullOrEmpty(Model.RevistaPublicacionPaisNombre)){ %>
        $('#paisrevista').slideDown(); 
        $('#span_paisrevista').html('<%=Html.Encode(Model.RevistaPublicacionPaisNombre)%>&nbsp;');
    <% } %>

    <% if(!String.IsNullOrEmpty(Model.RevistaPublicacionIndice1Nombre)){ %> 
        $('#indice1revista').slideDown();
        $('#span_indice1revista').html('<%=Html.Encode(Model.RevistaPublicacionIndice1Nombre)%>&nbsp;');
    <% } %>
    
    <% if(!String.IsNullOrEmpty(Model.RevistaPublicacionIndice2Nombre)){ %>
        $('#indice2revista').slideDown(); 
        $('#span_indice2revista').html('<%=Html.Encode(Model.RevistaPublicacionIndice2Nombre)%>&nbsp;');
    <% } %>

    <% if(!String.IsNullOrEmpty(Model.RevistaPublicacionIndice3Nombre)){ %>
        $('#indice3revista').slideDown(); 
        $('#span_indice3revista').html('<%=Html.Encode(Model.RevistaPublicacionIndice3Nombre)%>&nbsp;');
    <% } %>

<% } else { %>

    $('#span_institucionrevista').html('&nbsp;');
    $('#span_indice1revista').html('&nbsp;');
    $('#span_indice2revista').html('&nbsp;');
    $('#span_indice3revista').html('&nbsp;');

<% } %>