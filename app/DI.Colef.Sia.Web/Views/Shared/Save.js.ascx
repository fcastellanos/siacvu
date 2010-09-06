<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl" %>

$('#Id').val('<%=Model %>');

$('#mensaje-error').removeClass('mensaje-error');
$('#mensaje-error').text('');

$('span.field-validation-error').remove();
$('input').removeClass('input-validation-error');
$('textarea').removeClass('input-validation-error');
$('select').removeClass('input-validation-error');



if($('.fileUpload').length > 0) {
    UploadMulti.upload();
} else {
    window.location.href = $('#regresar').attr('href');
}