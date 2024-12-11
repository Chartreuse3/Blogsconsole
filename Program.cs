// Required using statements
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NLog;

// Main Program
class Program
{
    private static readonly ILogger logger = LogManager.GetCurrentClassLogger();

    static void Main(string[] args)
    {
        logger.Info("Program started");

        var db = new DataContext();

        while (true)
        {
            Console.Clear();
            Console.WriteLine("Choose an option:");
            Console.WriteLine("1. Display all products");
            Console.WriteLine("2. Add new product");
            Console.WriteLine("3. Edit a product");
            Console.WriteLine("4. Delete a product");
            Console.WriteLine("5. Display a specific product");
            Console.WriteLine("6. Display all categories");
            Console.WriteLine("7. Add new category");
            Console.WriteLine("8. Edit a category");
            Console.WriteLine("9. Delete a category");
            Console.WriteLine("10. Display category with active products");
            Console.WriteLine("11. Exit");
            Console.Write("Enter your choice: ");

            switch (Console.ReadLine())
            {
                case "1":
                    DisplayProducts(db);
                    break;
                case "2":
                    AddProduct(db);
                    break;
                case "3":
                    EditProduct(db);
                    break;
                case "4":
                    DeleteProduct(db);
                    break;
                case "5":
                    DisplaySpecificProduct(db);
                    break;
                case "6":
                    DisplayCategories(db);
                    break;
                case "7":
                    AddCategory(db);
                    break;
                case "8":
                    EditCategory(db);
                    break;
                case "9":
                    DeleteCategory(db);
                    break;
                case "10":
                    DisplayCategoryWithProducts(db);
                    break;
                case "11":
                    logger.Info("Program ended by user");
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
        }
    }

    private static void DisplayProducts(DataContext db)
    {
        Console.WriteLine("Choose product filter:");
        Console.WriteLine("1. All products");
        Console.WriteLine("2. Active products");
        Console.WriteLine("3. Discontinued products");
        Console.Write("Enter your choice: ");
        string filter = Console.ReadLine();

        IQueryable<Product> query = db.Products.Include(p => p.Category);

        switch (filter)
        {
            case "1":
                break; // No filter
            case "2":
                query = query.Where(p => !p.Discontinued);
                break;
            case "3":
                query = query.Where(p => p.Discontinued);
                break;
            default:
                Console.WriteLine("Invalid choice. Showing all products.");
                break;
        }

        var products = query.ToList();
        foreach (var product in products)
        {
            Console.WriteLine($"{product.ProductId}: {product.ProductName} - {(product.Discontinued ? "Discontinued" : "Active")}");
        }
        logger.Info("Displayed products with filter {Filter}", filter);
    }

    private static void DeleteProduct(DataContext db)
{
    Console.WriteLine("Choose a product to delete:");

    var products = db.Products.ToList();
    if (products.Any())
    {
        foreach (var product in products)
        {
            Console.WriteLine($"{product.ProductId}: {product.ProductName}");
        }
        
        Console.Write("Enter Product ID to delete: ");
        if (int.TryParse(Console.ReadLine(), out int productId))
        {
            var product = db.Products.Find(productId);
            if (product != null)
            {
                db.Products.Remove(product);
                db.SaveChanges();
                logger.Info("Deleted product with ID {ProductId}", productId);
                Console.WriteLine("Product deleted successfully.");
            }
            else
            {
                Console.WriteLine("Product not found.");
                logger.Warn("Product with ID {ProductId} not found", productId);
            }
        }
        else
        {
            Console.WriteLine("Invalid input for Product ID.");
            logger.Warn("Invalid input for Product ID");
        }
    }
    else
    {
        Console.WriteLine("No products available to delete.");
    }
}

private static void AddProduct(DataContext db)
{
    Console.WriteLine("Choose a category for the new product:");

    var categories = db.Categories.ToList();
    if (categories.Any())
    {
        foreach (var category in categories)
        {
            Console.WriteLine($"{category.CategoryId}: {category.CategoryName}");
        }

        Console.Write("Enter Category ID: ");
        if (int.TryParse(Console.ReadLine(), out int categoryId))
        {
            var category = db.Categories.Find(categoryId);
            if (category != null)
            {
                Console.Write("Enter Product Name: ");
                var name = Console.ReadLine();
                var product = new Product { ProductName = name, CategoryId = categoryId, Discontinued = false };
                db.Products.Add(product);
                db.SaveChanges();
                logger.Info("Added new product: {ProductName}", product.ProductName);
                Console.WriteLine("Product added successfully.");
            }
            else
            {
                Console.WriteLine("Invalid Category ID.");
                logger.Warn("Category ID {CategoryId} not found", categoryId);
            }
        }
        else
        {
            Console.WriteLine("Invalid input for Category ID.");
            logger.Warn("Invalid input for Category ID");
        }
    }
    else
    {
        Console.WriteLine("No categories available to choose from.");
    }
}

private static void EditProduct(DataContext db)
{
    Console.WriteLine("Choose a product to edit:");

    var products = db.Products.ToList();
    if (products.Any())
    {
        foreach (var product in products)
        {
            Console.WriteLine($"{product.ProductId}: {product.ProductName}");
        }

        Console.Write("Enter Product ID to edit: ");
        if (int.TryParse(Console.ReadLine(), out int productId))
        {
            var product = db.Products.Find(productId);
            if (product != null)
            {
                Console.Write("Enter new Product Name: ");
                product.ProductName = Console.ReadLine();

                Console.WriteLine("Choose a new category:");
                var categories = db.Categories.ToList();
                if (categories.Any())
                {
                    foreach (var category in categories)
                    {
                        Console.WriteLine($"{category.CategoryId}: {category.CategoryName}");
                    }

                    Console.Write("Enter new Category ID: ");
                    if (int.TryParse(Console.ReadLine(), out int categoryId))
                    {
                        var category = db.Categories.Find(categoryId);
                        if (category != null)
                        {
                            product.CategoryId = categoryId;
                            db.SaveChanges();
                            logger.Info("Edited product with ID {ProductId}", productId);
                            Console.WriteLine("Product updated successfully.");
                        }
                        else
                        {
                            Console.WriteLine("Invalid Category ID.");
                            logger.Warn("Category ID {CategoryId} not found", categoryId);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid input for Category ID.");
                        logger.Warn("Invalid input for Category ID");
                    }
                }
                else
                {
                    Console.WriteLine("No categories available to choose from.");
                }
            }
            else
            {
                Console.WriteLine("Product not found.");
                logger.Warn("Product with ID {ProductId} not found", productId);
            }
        }
        else
        {
            Console.WriteLine("Invalid input for Product ID.");
            logger.Warn("Invalid input for Product ID");
        }
    }
    else
    {
        Console.WriteLine("No products available to edit.");
    }
}



    private static void DisplaySpecificProduct(DataContext db)
    {
        Console.Write("Enter Product ID to view: ");
        if (int.TryParse(Console.ReadLine(), out int productId))
        {
            var product = db.Products.Include(p => p.Category).FirstOrDefault(p => p.ProductId == productId);
            if (product != null)
            {
                Console.WriteLine($"Product ID: {product.ProductId}");
                Console.WriteLine($"Name: {product.ProductName}");
                Console.WriteLine($"Category: {product.Category.CategoryName}");
                Console.WriteLine($"Discontinued: {product.Discontinued}");
                logger.Info("Displayed specific product with ID {ProductId}", productId);
            }
            else
            {
                Console.WriteLine("Product not found.");
                logger.Warn("Product with ID {ProductId} not found", productId);
            }
        }
        else
        {
            Console.WriteLine("Invalid input for Product ID.");
            logger.Warn("Invalid input for Product ID");
        }
    }

    private static void DisplayCategories(DataContext db)
    {
        var categories = db.Categories.ToList();
        Console.WriteLine("All Categories:");
        foreach (var category in categories)
        {
            Console.WriteLine($"Category ID: {category.CategoryId}, Name: {category.CategoryName}, Description: {category.Description}");
        }
        logger.Info("Displayed all categories");
    }

    private static void AddCategory(DataContext db)
    {
        Console.Write("Enter Category Name: ");
        var name = Console.ReadLine();
        Console.Write("Enter Description: ");
        var description = Console.ReadLine();

        var category = new Category { CategoryName = name, Description = description };
        db.Categories.Add(category);
        db.SaveChanges();
        logger.Info("Added new category: {CategoryName}", name);
        Console.WriteLine("Category added successfully.");
    }

    private static void EditCategory(DataContext db)
    {
        Console.Write("Enter Category ID to edit: ");
        if (int.TryParse(Console.ReadLine(), out int categoryId))
        {
            var category = db.Categories.Find(categoryId);
            if (category != null)
            {
                Console.Write("Enter new Category Name: ");
                category.CategoryName = Console.ReadLine();
                Console.Write("Enter new Description: ");
                category.Description = Console.ReadLine();
                db.SaveChanges();
                logger.Info("Edited category with ID {CategoryId}", categoryId);
                Console.WriteLine("Category updated successfully.");
            }
            else
            {
                Console.WriteLine("Category not found.");
                logger.Warn("Category with ID {CategoryId} not found", categoryId);
            }
        }
        else
        {
            Console.WriteLine("Invalid input for Category ID.");
            logger.Warn("Invalid input for Category ID");
        }
    }

    private static void DeleteCategory(DataContext db)
    {
        Console.Write("Enter Category ID to delete: ");
        if (int.TryParse(Console.ReadLine(), out int categoryId))
        {
            var category = db.Categories.Include(c => c.Products).FirstOrDefault(c => c.CategoryId == categoryId);
            if (category != null)
            {
                if (!category.Products.Any())
                {
                    db.Categories.Remove(category);
                    db.SaveChanges();
                    logger.Info("Deleted category with ID {CategoryId}", categoryId);
                    Console.WriteLine("Category deleted successfully.");
                }
                else
                {
                    Console.WriteLine("Category cannot be deleted because it has associated products.");
                    logger.Warn("Attempted to delete category with ID {CategoryId} which has products", categoryId);
                }
            }
            else
            {
                Console.WriteLine("Category not found.");
                logger.Warn("Category with ID {CategoryId} not found", categoryId);
            }
        }
        else
        {
            Console.WriteLine("Invalid input for Category ID.");
            logger.Warn("Invalid input for Category ID");
        }
    }

    private static void DisplayCategoryWithProducts(DataContext db)
    {
        Console.Write("Enter Category ID to display: ");
        if (int.TryParse(Console.ReadLine(), out int categoryId))
        {
            var category = db.Categories.Include(c => c.Products).FirstOrDefault(c => c.CategoryId == categoryId);
            if (category != null)
            {
                Console.WriteLine($"Category ID: {category.CategoryId}");
                Console.WriteLine($"Name: {category.CategoryName}");
                Console.WriteLine($"Description: {category.Description}");
                Console.WriteLine("Products in this category:");
                foreach (var product in category.Products)
                {
                    Console.WriteLine($"- {product.ProductName} (Discontinued: {product.Discontinued})");
                }
                logger.Info("Displayed category with products for Category ID {CategoryId}", categoryId);
            }
            else
            {
                Console.WriteLine("Category not found.");
                logger.Warn("Category with ID {CategoryId} not found", categoryId);
            }
        }
        else
        {
            Console.WriteLine("Invalid input for Category ID.");
            logger.Warn("Invalid input for Category ID");
        }
    }
}
