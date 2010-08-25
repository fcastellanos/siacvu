<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<BaseForm>" %>
<%@ Import Namespace="DecisionesInteligentes.Colef.Sia.Web.Controllers.Models"%>

$('#accion_<%=Html.Encode(Model.Id)%>_<%=Html.Encode(Model.TipoProducto)%>').slideUp();