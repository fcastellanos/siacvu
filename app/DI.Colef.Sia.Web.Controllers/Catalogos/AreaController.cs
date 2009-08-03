using System;
using System.Web.Mvc;
using DecisionesInteligentes.Colef.Sia.ApplicationServices;
using DecisionesInteligentes.Colef.Sia.Core;
using DecisionesInteligentes.Colef.Sia.Web.Controllers.Mappers;
using DecisionesInteligentes.Colef.Sia.Web.Controllers.Models;
using SharpArch.Web.NHibernate;

namespace DecisionesInteligentes.Colef.Sia.Web.Controllers
{
    [HandleError]
    public class AreaController : BaseController<Area, AreaForm>
    {
		readonly ICatalogoService catalogoService;
        readonly IAreaMapper areaMapper;

        public AreaController(IUsuarioService usuarioService, ICatalogoService catalogoService, IAreaMapper areaMapper)
            : base(usuarioService)
        {
            this.catalogoService = catalogoService;
            this.areaMapper = areaMapper;
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Index() 
        {
			var data = CreateViewDataWithTitle(Title.Index);

            var areas = catalogoService.GetAllAreas();
            data.List = areaMapper.Map(areas);

            return View(data);
        }
        
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult New()
        {			
			var data = CreateViewDataWithTitle(Title.New);
            data.Form = new AreaForm();
			
			return View(data);
        }
        
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Edit(int id)
        {
            var data = CreateViewDataWithTitle(Title.Edit);

            var area = catalogoService.GetAreaById(id);
            data.Form = areaMapper.Map(area);

			ViewData.Model = data;
            return View();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Show(int id)
        {
            var data = CreateViewDataWithTitle(Title.Show);

            var area = catalogoService.GetAreaById(id);
            data.Form = areaMapper.Map(area);
            
            ViewData.Model = data;
            return View();
        }
        
        [Transaction]
        [ValidateAntiForgeryToken]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(AreaForm form)
        {
        
            var area = areaMapper.Map(form);

            if(!IsValidateModel(area, form, Title.New))
                return ViewNew();

            catalogoService.SaveArea(area);

            return RedirectToIndex(String.Format("{0} ha sido creada", area.Nombre));
        }
        
        [Transaction]
        [ValidateAntiForgeryToken]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Update(AreaForm form)
        {
        
            var area = areaMapper.Map(form);

            if (!IsValidateModel(area, form, Title.Edit))
                return ViewEdit();

            catalogoService.SaveArea(area);

            return RedirectToIndex(String.Format("{0} ha sido modificada", area.Nombre));
        }
        
        [Transaction]
        [AcceptVerbs(HttpVerbs.Put)]
        public ActionResult Activate(int id)
        {
            var area = catalogoService.GetAreaById(id);
            area.Activo = true;
            catalogoService.SaveArea(area);

            var form = areaMapper.Map(area);
            
            return Rjs(form);
        }
        
        [Transaction]
        [AcceptVerbs(HttpVerbs.Put)]
        public ActionResult Deactivate(int id)
        {
            var area = catalogoService.GetAreaById(id);
            area.Activo = false;
            catalogoService.SaveArea(area);

            var form = areaMapper.Map(area);
            
            return Rjs("Activate", form);
        }
    }
}
