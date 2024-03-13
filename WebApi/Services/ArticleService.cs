using WebApi.Common;
using WebApi.Common.Exceptions;
using WebApi.Common.Models;
using WebApi.Models.Entities;
using WebApi.Persistence;
using WebApi.Services.Common;
using WebApi.ViewModels;
using WebApi.ViewModels.Filters;

namespace WebApi.Services
{
    public class ArticleService
    {
        private readonly UnitOfWork _uow;
        private readonly LoggedInUser _user;
        private readonly TokenService _tokenService;
        private readonly UploadService _upload;

        public ArticleService(UnitOfWork uow,
            LoggedInUser user,
            TokenService tokenService,
            UploadService upload)
        {
            _uow = uow;
            _user = user;
            _tokenService = tokenService;
            _upload = upload;
        }

        public async Task<Article> Save(ArticleVm article)
        {
            var existing = article.Id.HasValue && article.Id > 0 ? await _uow.Articles.GetDetails(article.Id.Value) : null;

            var artNew = Map(article, existing ?? new Article());
            if (existing is null)
            {
                artNew.CreatedDate = artNew.UpdatedDate;
                artNew = await _uow.Articles.Add(artNew);
            }

            await _uow.CompleteAsync();

            return artNew;
        }

        internal async Task<ArticleVm> GetArtical(int artId ,bool forEdit=false)
        {
            var article = await _uow.Articles.GetDetails(artId);

            return article is null
                ? throw new Exception($"Article with Id:{artId} not found")
                : Map(article, new ArticleVm(), forEdit);
        }
        public async Task<DataPageModel<ArticleVm>> Filter(FilterBase filter)
        {
            var data = await _uow.Articles.Filter(filter);

            return new DataPageModel<ArticleVm>
            {
                Data = data.Data.ToList().Select(x => Map(x, new ArticleVm())),
                Count = data.Count,
                PageCount = data.PageCount,
            };
        }

        private Article Map(ArticleVm art, Article existing)
        {
            var curTime = DateTime.UtcNow;

            existing.Title = art.Title;
            existing.SubTitle = art.SubTitle;
            existing.ParagraphOne = art.ParagraphOne;
            existing.ParagraphTwo = art.ParagraphTwo;
            existing.Language = art.Language;
            existing.ImagePath = _upload.GetFullPath(art.ImagePath);

            existing.UpdatedDate = curTime;

            existing.Images.Clear();

            foreach (var img in art.Images)
            {
                var file = _tokenService.ParseToken<FileModel>(img.Token ?? "");
                if (file is not null)
                {
                    existing.Images.Add(new ArticleImage
                    {
                        MimeType = file.MimeType,
                        Name = file.Name,
                        Path = file.RelativePath,
                    });
                }
            }

            if (existing.Images.Count > 0)
            {
                existing.ImagePath = existing.Images.FirstOrDefault()?.Path;
            }

            return existing;
        }

        private ArticleVm Map(Article source, ArticleVm dest, bool forEdit = false)
        {
            dest.Id = source.Id;
            dest.Title = source.Title;
            dest.SubTitle = source.SubTitle;
            dest.ParagraphOne = source.ParagraphOne;
            dest.ParagraphTwo = source.ParagraphTwo;
            dest.Language = source.Language;
            dest.ImagePath = _upload.GetFullPath(source.ImagePath);
            dest.Active = source.Active;
            dest.UpdatedDate = source.UpdatedDate;
            dest.CreatedDate = source.CreatedDate;

            var images = new List<FileToken>();
            foreach (var img in source.Images)
            {
                var token = !forEdit ? null: _tokenService.CreateToken(new FileModel
                {
                    MimeType = img.MimeType,
                    Name = img.Name,
                    RelativePath = img.Path,
                });
                images.Add(new FileToken
                {
                    Token = token,
                    Name = img.Name,
                    RelativePath= img.Path,
                    FullPath = _upload.GetFullPath(img.Path),
                });
            }
            dest.Images = images;
            return dest;
        }
        internal async Task<ArticleVm> ChangeArticleStatus(int artId, bool active)
        {
            var art = await _uow.Articles.FindOne(x => x.Id == artId)
                ?? throw new AppBadRequestException($"Article with id:{artId} for logged in user not found.");

            if (art.Active != active)
            {
                art.Active = active;
                await _uow.CompleteAsync();
            }
            return Map(art, new ArticleVm());
        }
    }
}