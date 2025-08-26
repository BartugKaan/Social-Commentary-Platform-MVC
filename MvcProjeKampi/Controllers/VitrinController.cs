using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MvcProjeKampi.Controllers
{
    /// <summary>
    /// Handles public showcase functionality displaying articles and headings
    /// Follows SRP by only managing public content display
    /// </summary>
    public class VitrinController : Controller
    {
        private readonly IContentService _contentService;
        private readonly IHeadingService _headingService;
        private readonly ICategoryService _categoryService;
        
        public VitrinController()
        {
            // Using existing business layer managers for consistent data access
            _contentService = new ContentManager(new EFContentDal());
            _headingService = new HeadingManager(new EFHeadingDal());
            _categoryService = new CategoryManager(new EFCategoryDal());
        }
        
        /// <summary>
        /// Displays main showcase page with latest articles and sidebar headings
        /// </summary>
        /// <param name="headingId">Optional heading filter</param>
        /// <returns>Main showcase view</returns>
        public ActionResult Index(int? headingId)
        {
            try
            {
                var contents = GetPublicContents(headingId);
                var recentHeadings = GetRecentHeadings();
                var categories = GetActiveCategories();
                
                var model = new VitrinViewModel
                {
                    RecentHeadings = recentHeadings,
                    Categories = categories,
                    Contents = contents,
                    SelectedHeadingId = headingId
                };
                
                return View(model);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Content could not be loaded at this time.";
                return View(new VitrinViewModel());
            }
        }
        
        /// <summary>
        /// Displays articles for specific heading
        /// </summary>
        /// <param name="id">Heading identifier</param>
        /// <returns>Articles view for specified heading</returns>
        public ActionResult HeadingContents(int id)
        {
            try
            {
                var heading = _headingService.GetById(id);
                if (heading == null || !heading.HeadingStatus)
                {
                    TempData["ErrorMessage"] = "Heading not found or not available.";
                    return RedirectToAction("Index");
                }
                
                var contents = _contentService.GetListById(id)
                    .Where(c => c.ContentStatus)
                    .OrderByDescending(c => c.ContentDate)
                    .ToList();
                
                ViewBag.HeadingName = heading.HeadingName;
                ViewBag.RecentHeadings = GetRecentHeadings();
                
                return View(contents);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Content could not be loaded.";
                return RedirectToAction("Index");
            }
        }
        
        /// <summary>
        /// Displays full article content
        /// </summary>
        /// <param name="id">Content identifier</param>
        /// <returns>Article detail view</returns>
        public ActionResult ArticleDetail(int id)
        {
            try
            {
                var content = _contentService.GetById(id);
                if (content == null || !content.ContentStatus || !content.Heading.HeadingStatus)
                {
                    TempData["ErrorMessage"] = "Article not found or not available.";
                    return RedirectToAction("Index");
                }
                
                ViewBag.RecentHeadings = GetRecentHeadings();
                
                return View(content);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Article could not be loaded.";
                return RedirectToAction("Index");
            }
        }
        
        #region Private Helper Methods
        
        /// <summary>
        /// Retrieves public content with filtering options
        /// </summary>
        private List<Content> GetPublicContents(int? headingId = null, int take = 20)
        {
            var allContents = _contentService.GetAll()
                .Where(c => c.ContentStatus && c.Heading.HeadingStatus);
            
            if (headingId.HasValue)
            {
                allContents = allContents.Where(c => c.HeadingId == headingId.Value);
            }
            
            return allContents
                .OrderByDescending(c => c.ContentDate)
                .Take(take)
                .ToList();
        }
        
        /// <summary>
        /// Retrieves recent active headings for sidebar display
        /// </summary>
        private List<Heading> GetRecentHeadings(int take = 10)
        {
            return _headingService.GetAll()
                .Where(h => h.HeadingStatus)
                .OrderByDescending(h => h.HeadingDate)
                .Take(take)
                .ToList();
        }
        
        /// <summary>
        /// Retrieves active categories for navigation
        /// </summary>
        private List<Category> GetActiveCategories()
        {
            return _categoryService.GetAll()
                .Where(c => c.CategoryStatus)
                .OrderBy(c => c.CategoryName)
                .ToList();
        }
        
        #endregion
    }
    
    /// <summary>
    /// ViewModel for showcase page following separation of concerns
    /// Contains strongly typed properties for better maintainability
    /// </summary>
    public class VitrinViewModel
    {
        public List<Heading> RecentHeadings { get; set; } = new List<Heading>();
        public List<Category> Categories { get; set; } = new List<Category>();
        public List<Content> Contents { get; set; } = new List<Content>();
        public int? SelectedHeadingId { get; set; }
    }
}