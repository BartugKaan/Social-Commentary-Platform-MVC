using EntityLayer.Concrete;
using System.Collections.Generic;

namespace BusinessLayer.Abstract
{
    /// <summary>
    /// Service interface for public showcase operations
    /// Follows Interface Segregation Principle by separating public concerns
    /// </summary>
    public interface IVitrinService
    {
        /// <summary>
        /// Retrieves active content for public display
        /// </summary>
        /// <param name="headingId">Optional heading filter</param>
        /// <param name="take">Number of items to return</param>
        /// <returns>List of active content</returns>
        List<Content> GetPublicContents(int? headingId = null, int take = 20);
        
        /// <summary>
        /// Retrieves recent active headings for sidebar
        /// </summary>
        /// <param name="take">Number of items to return</param>
        /// <returns>List of recent headings</returns>
        List<Heading> GetRecentHeadings(int take = 10);
        
        /// <summary>
        /// Retrieves active categories for navigation
        /// </summary>
        /// <returns>List of active categories</returns>
        List<Category> GetActiveCategories();
        
        /// <summary>
        /// Retrieves single content item for detail view
        /// </summary>
        /// <param name="contentId">Content identifier</param>
        /// <returns>Content item if found and active</returns>
        Content GetPublicContent(int contentId);
        
        /// <summary>
        /// Retrieves heading with related active content
        /// </summary>
        /// <param name="headingId">Heading identifier</param>
        /// <returns>Heading if found and active</returns>
        Heading GetPublicHeading(int headingId);
    }
}