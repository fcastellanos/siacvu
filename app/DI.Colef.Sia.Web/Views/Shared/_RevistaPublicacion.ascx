<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<BaseForm>" %>
<%@ Import Namespace="DecisionesInteligentes.Colef.Sia.Web.Controllers.Models" %>
<%@ Import Namespace="DecisionesInteligentes.Colef.Sia.Web.Controllers.Catalogos" %>
<h4>Datos de la revista</h4>
<p>
    <label>Nombre de la revista</label>
    <%=Html.TextBox("RevistaPublicacionTitulo", Model.RevistaPublicacionTitulo,
                new { @class = "autocomplete buscar-requerido", url = Html.BuildUrlFromExpressionForAreas<RevistaPublicacionController>(x => x.Search("")), maxlength = 100 })%>
    <%=Html.Hidden("RevistaPublicacionId", Model.RevistaPublicacionId, new { rel = "#RevistaPublicacionTitulo", url = Url.Action("ChangeRevista") })%>
    <span class="cvu"></span>
    <%if (!Model.RevistaPublicacionExists && !String.IsNullOrEmpty(Model.RevistaPublicacionTitulo)) { %>
        <span class="field-alert">Esta revista no esta registrada en el cat&aacute;logo</span>
    <% } %>
</p>

<div id="institucionrevista">
    <p>
        <label>Instituci&oacute;n</label>
        <span id="span_institucionrevista" class="valor"><%=Html.Encode(Model.RevistaPublicacionInstitucionNombre)%>&nbsp;</span>
    </p>
</div>
<div id="paisrevista">
    <p>
        <label>Pa&iacute;s</label>
        <span id="span_paisrevista" class="valor"><%=Html.Encode(Model.RevistaPublicacionPaisNombre)%>&nbsp;</span>
    </p>
</div>
<div id="indice1revista">
    <p>
        <label>Clasificaci&oacute;n 1</label>
        <span id="span_indice1revista" class="valor"><%=Html.Encode(Model.RevistaPublicacionIndice1Nombre)%>&nbsp;</span>
    </p>
</div>
<div id="indice2revista">
    <p>
        <label>Clasificaci&oacute;n 2</label>
        <span id="span_indice2revista" class="valor"><%=Html.Encode(Model.RevistaPublicacionIndice2Nombre)%>&nbsp;</span>
    </p>
</div>
<div id="indice3revista">
    <p>
        <label>Clasificaci&oacute;n 3</label>
        <span id="span_indice3revista" class="valor"><%=Html.Encode(Model.RevistaPublicacionIndice3Nombre)%>&nbsp;</span>
    </p>
</div>
