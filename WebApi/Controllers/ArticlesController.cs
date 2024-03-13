using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Common;
using WebApi.Models.Entities;
using WebApi.Persistence;
using WebApi.Services;
using WebApi.ViewModels;
using WebApi.ViewModels.Filters;

namespace WebApi.Controllers
{
    public class ArticlesController : ApiControllerBase
    {
        private readonly ArticleService _service;
        private readonly IValidator<ArticleVm> _validateArticle;

        public ArticlesController(
            IValidator<ArticleVm> validateArticle,
            ArticleService service)
        {
            _validateArticle = validateArticle;
            _service = service;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Index(int id, bool forEdit)
        {
            var articles = await _service.GetArtical(id, forEdit);
            return Ok(articles);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Save(ArticleVm article)
        {
            _validateArticle.ValidateAndThrow(article);
            var art = await _service.Save(article);
            return Ok(new {art.Id, art.UpdatedDate, art.Title });
        }

        [Authorize]
        [HttpGet("{id}/change-status")]
        public async Task<IActionResult> ChangeStatus(int id, bool active = false)
        {
            var article = await _service.ChangeArticleStatus(id, active);
            return Ok(article);
        }

        [HttpGet]
        public async Task<IActionResult> Filter([FromQuery] FilterBase filter)
        {
            var data = await _service.Filter(filter);
            return Ok(data);
        }
    }
}