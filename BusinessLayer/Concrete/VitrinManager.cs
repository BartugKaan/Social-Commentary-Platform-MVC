using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLayer.Concrete
{
    /// <summary>
    /// Service implementation for public showcase operations
    /// Follows Single Responsibility Principle by handling only public content display
    /// </summary>
    public class VitrinManager : IVitrinService
    {
        private readonly IContentDal _contentDal;
        private readonly IHeadingDal _headingDal;
        private readonly ICategoryDal _categoryDal;
        
        public VitrinManager(IContentDal contentDal, IHeadingDal headingDal, ICategoryDal categoryDal)
        {
            _contentDal = contentDal;
            _headingDal = headingDal;
            _categoryDal = categoryDal;
        }
        
        /// <summary>
        /// Retrieves active content for public display with filtering options
        /// </summary>
        public List<Content> GetPublicContents(int? headingId = null, int take = 20)
        {
            var query = _contentDal.List(c => c.ContentStatus && c.Heading.HeadingStatus);
            
            if (headingId.HasValue)
            {
                query = query.Where(c => c.HeadingId == headingId.Value).ToList();
            }
            
            return query.OrderByDescending(c => c.ContentDate)
                       .Take(take)
                       .ToList();
        }
        
        /// <summary>
        /// Retrieves recent active headings for sidebar display
        /// </summary>
        public List<Heading> GetRecentHeadings(int take = 10)
        {
            return _headingDal.List(h => h.HeadingStatus)
                             .OrderByDescending(h => h.HeadingDate)
                             .Take(take)
                             .ToList();
        }
        
        /// <summary>
        /// Retrieves active categories for navigation
        /// </summary>
        public List<Category> GetActiveCategories()
        {
            return _categoryDal.List(c => c.CategoryStatus)
                              .OrderBy(c => c.CategoryName)
                              .ToList();
        }
        
        /// <summary>
        /// Retrieves single active content item for detail view
        /// </summary>
        public Content GetPublicContent(int contentId)
        {
            return _contentDal.List(c => c.ContentId == contentId && 
                                        c.ContentStatus && 
                                        c.Heading.HeadingStatus)
                             .FirstOrDefault();
        }
        
        /// <summary>
        /// Retrieves active heading for content display
        /// </summary>
        public Heading GetPublicHeading(int headingId)
        {
            return _headingDal.List(h => h.HeadingId == headingId && h.HeadingStatus)
                             .FirstOrDefault();
        }
    }
}