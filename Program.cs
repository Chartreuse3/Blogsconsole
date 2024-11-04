using System.Xml.Serialization;
using NLog;
string path = Directory.GetCurrentDirectory() + "//nlog.config";

// create instance of Logger
var logger = LogManager.Setup().LoadConfigurationFromFile(path).GetCurrentClassLogger();

logger.Info("Program started");

// new option picker

while (true)
{

  Console.WriteLine("\nChoose an option:");
  Console.WriteLine("1. Display all blogs");
  Console.WriteLine("2. Add Blog");
  var choice = Console.ReadLine();

  if (choice == "1")
  {

    // Create and save a new Blog
    Console.Write("Enter a name for a new Blog: ");
    var name = Console.ReadLine();
    var blog = new Blog { Name = name };
    var db = new DataContext();
    db.AddBlog(blog);
    logger.Info("Blog added - {name}", name);
  }

  else if (choice == "2")
  {

    // Display all Blogs from the database
    var query = db.Blogs.OrderBy(b => b.Name);
    Console.WriteLine("All blogs in the database:");
    foreach (var item in query)
    {
      Console.WriteLine(item.Name);
    }
  }

  else if (choice == "3")
  {

  }

  else if (choice == "4") 
  {

  }
}

logger.Info("Program ended");