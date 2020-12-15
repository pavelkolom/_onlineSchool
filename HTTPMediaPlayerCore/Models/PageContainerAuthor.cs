using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HTTPMediaPlayerCore.Services;

namespace HTTPMediaPlayerCore.Models
{
  public class PageContainerAuthor
  {
    public Author Author { get; private set; }
    public string AuthorPageHTML { get; private set; }
    public List<Category> Categories { get; private set; }

    public PageContainerAuthor(Author author, List<Category> categories, string authorPageHTML)
    {
      AuthorPageHTML = authorPageHTML;
      Author = author;
      Categories = categories;
    }
  }
}
