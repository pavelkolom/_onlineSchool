using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HTTPMediaPlayerCore.Services;

namespace HTTPMediaPlayerCore.Models
{
  public class CourceCategoryContainer
  {
    public Author Author { get; private set; }
    public string AuthorPageHTML { get; private set; }
    public List<Category> Categories { get; private set; }

    public List<Author> AuthorsSection { get; private set; }

    public CourceCategoryContainer(Author author, List<Category> categories, string authorPageHTML, List<Author> authorsSection)
    {
      AuthorPageHTML = authorPageHTML;
      Author = author;
      Categories = categories;
      AuthorsSection = authorsSection;
    }
  }
}
