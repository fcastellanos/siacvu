<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<DictamenForm>" %>
<%@ Import Namespace="DecisionesInteligentes.Colef.Sia.Web.Controllers.Models"%>

<% if (Model.RevistaPublicacionId != 0){ %>
    $('#institucionrevista').html('<%=Html.Encode(Model.RevistaPublicacionInstitucionNombre)%>&nbsp;');
<% } else { %>
    $('#institucionrevista').html('&nbsp;');
<% } %>